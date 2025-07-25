using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using MoneyManagement_Api.AppContext;
using MoneyManagement_Api.Contract;
using MoneyManagement_Api.Interfaces;
using MoneyManagement_Api.Models.Transactions;
using MoneyManagement_Data;
using MoneyManagement_Data.DTOs;

namespace MoneyManagement_Api.Services;

public class TransactionService : ITransactionService
{
    private readonly ApplicationContext _context;
    private readonly IUtilityService _utilityService;
    private readonly IMlTransactionService _mlService;
    private readonly ILogger<TransactionService> _logger;

    public TransactionService(ILogger<TransactionService> logger, ApplicationContext context,
        IUtilityService utilityService, IMlTransactionService mlservice)
    {
        _context = context;
        _utilityService = utilityService;
        _mlService = mlservice;
        _logger = logger;
    }

    public async Task<ApiResponse<ICollection<TransactionDto>>> GetActiveTransactionList()
    {
        try
        {
            var result = await _context.Transaction.Include(c => c.Account)
                .Include(c => c.Account.Currency)
                .Where(x => x.IsActive).OrderByDescending(x => x.TxnDate).ToListAsync();


            return new
                ApiResponse<ICollection<TransactionDto>>(
                    result.Select(x => AutoMapper.MapTransactionToDto(x)).ToList(),
                    $"Active transactions retrieved successfully. Total: {result.Count}");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error retrieving active transactions: {ex.Message}");
            return new ApiResponse<ICollection<TransactionDto>>(null, "Error retrieving active transactions");
        }
    }

    public async Task<ApiResponse<TransactionDto>> GetTransaction(int transactionId)
    {
        try
        {
            var result = await _context.Transaction.Include(c => c.Account)
                .Include(c => c.Account.Currency)
                .Where(x => x.Id == transactionId).FirstOrDefaultAsync();
            return new ApiResponse<TransactionDto>(AutoMapper.MapTransactionToDto(result),
                $"Transaction with ID {transactionId} retrieved successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error retrieving transaction with ID {transactionId}: {ex.Message}");
            return new ApiResponse<TransactionDto>(null, $"Error retrieving transaction with ID {transactionId}");
        }
    }

    public async Task<ApiResponse<TransactionDto>> UpdateTransaction(TransactionDto item)
    {
        try
        {
            var existingTransaction = await _context.Transaction
                .Include(c => c.Account)
                .Include(c => c.Account.Currency)
                .Where(x => x.Id == item.Id)
                .FirstOrDefaultAsync();

            if (existingTransaction == null)
            {
                _logger.LogWarning($"Transaction with ID {item.Id} not found.");
                return new ApiResponse<TransactionDto>(null, $"Transaction with ID {item.Id} not found.");
            }

            // Update the properties of the existing transaction
            existingTransaction.TxnDate = item.TxnDate;
            existingTransaction.TxnAmount = item.TxnAmount;
            existingTransaction.Description = item.Description;
            existingTransaction.IsCatConfirmed = item.IsCatConfirmed;
            existingTransaction.Note = item.Note;

            existingTransaction.LastUpdatedDate = DateTime.Now;

            if (item.Area != null) item.Area = item.Area.ToUpper();

            existingTransaction.Area = item.Area;

            _context.Transaction.Update(existingTransaction);
            await _context.SaveChangesAsync();

            return new ApiResponse<TransactionDto>(AutoMapper.MapTransactionToDto(existingTransaction),
                $"Transaction with ID {item.Id} updated successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error updating transaction with ID {item.Id}: {ex.Message}");
            return new ApiResponse<TransactionDto>(null, $"Error updating transaction with ID {item.Id}");
        }
    }

    public async Task<ApiResponse<TransactionDto>> CategoryConfirmed(TransactionDto item)
    {
        if (item.IsCatConfirmed)
            //Confirmed Category
            //add train data
            _mlService.AddToTrain(item);

        try
        {
            return new ApiResponse<TransactionDto>(UpdateTransaction(item).Result.Data,
                $"Category confirmation for transaction with ID {item.Id} updated successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error confirming category for transaction with ID {item.Id}: {ex.Message}");
            return new ApiResponse<TransactionDto>(null,
                $"Error confirming category for transaction with ID {item.Id}");
        }
    }

    public async Task<ApiResponse<TransactionDto>> CategorizeTransaction(TransactionDto item)
    {
        try
        {
            item.Area = _mlService.PredictCategory(item.Description).ToUpper();

            return new ApiResponse<TransactionDto>(UpdateTransaction(item).Result.Data,
                $"Transaction with ID {item.Id} categorized successfully in {item.Area}.");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error categorizing transaction with ID {item.Id}: {ex.Message}");
            return new ApiResponse<TransactionDto>(null, $"Error categorizing transaction with ID {item.Id}");
        }
    }

    public async Task<ApiResponse<string>> CategorizeAllTransaction()
    {
        var transactionsToCat =
            await _context.Transaction.Where(x => x.IsActive && x.IsCatConfirmed == false).ToListAsync();

        var ok = 0;
        var ko = 0;

        foreach (var item in transactionsToCat)
            try
            {
                // item.LastUpdatedDate = DateTime.Now;
                item.Area = _mlService.PredictCategory(item.Description);

                //todo manage massive update
                await UpdateTransaction(AutoMapper.MapTransactionToDto(item));

                ok++;
            }
            catch (Exception ex)
            {
                ko++;
                _logger.LogError($"Error categorizing transaction with ID {item.Id}: {ex.Message}");
            }

        return new ApiResponse<string>($"File categorized: {ok}, with error: {ko} (check the log)",
            $"Categorization completed. Total transactions processed: {transactionsToCat.Count}");
    }

    public async Task<ApiResponse<string>> TrainModelTransaction()
    {
        try
        {
            var result = _mlService.TrainAndSaveModel();


            return !string.IsNullOrEmpty(result)
                ? new ApiResponse<string>(result, $"Model Trained successfully")
                : new ApiResponse<string>(string.Empty, "Error in train model: check the log");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error training model: {ex.Message}");
            return new ApiResponse<string>(string.Empty, $"Error training model: {ex.Message}");
        }
    }

    public async Task<ApiResponse<TransactionDto>> AddTransaction(TransactionDto item)
    {
        var account = await _context.AccountMasterData
            .Include(c => c.Currency)
            .Where(x => x.Id == item.AccountId)
            .FirstOrDefaultAsync();

        try
        {
            var newTransaction = new Transaction
            {
                TxnDate = item.TxnDate,
                TxnAmount = item.TxnAmount,
                Description = item.Description,
                IsCatConfirmed = item.IsCatConfirmed,
                Note = item.Note,
                Area = item.Area?.ToUpper(),
                Account = account,
                LastUpdatedDate = DateTime.Now,
                CreatedDate = DateTime.Now,
                IsActive = true,
                UniqueKey = MD5UniqueKey(item.AccountName, item.TxnDate.ToString(CultureInfo.InvariantCulture),
                    item.TxnAmount.ToString(CultureInfo.InvariantCulture), item.Description)
            };

            newTransaction.Account = account;

            if (_context.Transaction.Any(x => x.UniqueKey == newTransaction.UniqueKey))
            {
                //record already present: Duplicate Record
                item.Id = 0;
                return new ApiResponse<TransactionDto>(null, "Transaction already exists with the same unique key.");
            }

            await _context.Transaction.AddAsync(newTransaction);
            await _context.SaveChangesAsync();

            return new ApiResponse<TransactionDto>(AutoMapper.MapTransactionToDto(newTransaction),
                $"Transaction with ID {item.Id} added successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error adding transaction: {ex.Message}");
            return new ApiResponse<TransactionDto>(null, $"Error adding transaction: {ex.Message}");
        }
    }

    public async Task<ApiResponse<bool>> DeleteTransaction(int id)
    {
        try
        {
            var existingTransaction = await _context.Transaction.Include(c => c.Account)
                .Include(c => c.Account.Currency)
                .Where(x => x.Id == id).FirstOrDefaultAsync();

            if (existingTransaction == null)
            {
                _logger.LogWarning($"Transaction with ID {id} not found for deletion.");
                return new ApiResponse<bool>(false, $"Transaction with ID {id} not found for deletion.");
            }

            existingTransaction.LastUpdatedDate = DateTime.Now;
            existingTransaction.IsActive = false;

            _context.Transaction.Update(existingTransaction);
            await _context.SaveChangesAsync();

            return new ApiResponse<bool>(true, $"Transaction with ID {id} deleted successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error deleting transaction with ID {id}: {ex.Message}");
            return new ApiResponse<bool>(false, $"Error deleting transaction with ID {id}: {ex.Message}");
        }
    }

    public async Task<ApiResponse<string>> UploadCsv(IList<TransactionDto> transactions)
    {
        if (transactions.Count == 0)
        {
            _logger.LogWarning("No transactions to upload.");
            return new ApiResponse<string>(string.Empty, $"No transactions to upload.");
        }

        var loaded = 0;
        var notLoaded = 0;
        var duplicated = 0;

        foreach (var item in transactions)
        {
            var result = await AddTransaction(item);

            if (result.Data is null)
                //return BadRequest("Error adding Transaction");
                notLoaded++;

            if (result.Data.Id < 1) duplicated++;

            if (result.Data.Id > 0) loaded++;
        }

        return new ApiResponse<string>(
            $"Loaded: {loaded} - Not Loaded: {notLoaded} - Duplicated: {duplicated}, ${transactions.Count} transactions processed.",
            $"Transactions upload completed. Total: {transactions.Count}");
    }

    private string MD5UniqueKey(string accountName, string txnDate, string txnAmount, string txnDescription)
    {
        using var md5Hash = MD5.Create();
        var hash = GetMd5Hash(md5Hash, string.Concat(accountName, txnDate, txnAmount, txnDescription));

        return hash;
    }

    private static string GetMd5Hash(MD5 md5Hash, string input)
    {
        // Convert the input string to a byte array and compute the hash.
        var data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

        // Create a new Stringbuilder to collect the bytes
        // and create a string.
        var sBuilder = new StringBuilder();

        // Loop through each byte of the hashed data 
        // and format each one as a hexadecimal string.
        foreach (var t in data) sBuilder.Append(t.ToString("x2"));

        // Return the hexadecimal string.
        return sBuilder.ToString();
    }
}