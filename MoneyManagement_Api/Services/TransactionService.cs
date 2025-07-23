using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using MoneyManagement_Api.AppContext;
using MoneyManagement_Api.Contract;
using MoneyManagement_Api.Interfaces;
using MoneyManagement_Api.Models.Transactions;

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

    public async Task<ApiResponse<ICollection<Transaction>>> GetActiveTransactionList()
    {
        try
        {
            var result = await _context.Transaction.Include(c => c.Account).Include(c => c.Account.Currency)
                .Where(x => x.IsActive).OrderByDescending(x => x.TxnDate).ToListAsync();


            return new ApiResponse<ICollection<Transaction>>(result,
                $"Active transactions retrieved successfully. Total: {result.Count}");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error retrieving active transactions: {ex.Message}");
            return new ApiResponse<ICollection<Transaction>>(null, "Error retrieving active transactions");
        }
    }

    public async Task<ApiResponse<Transaction>> GetTransaction(int balanceId)
    {
        try
        {
            var result = await _context.Transaction.Include(c => c.Account).Include(c => c.Account.Currency)
                .Where(x => x.Id == balanceId).FirstOrDefaultAsync();
            return new ApiResponse<Transaction>(result, $"Transaction with ID {balanceId} retrieved successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error retrieving transaction with ID {balanceId}: {ex.Message}");
            return new ApiResponse<Transaction>(null, $"Error retrieving transaction with ID {balanceId}");
        }
    }

    public async Task<ApiResponse<Transaction>> UpdateTransaction(Transaction item)
    {
        try
        {
            var existingTransaction = await _context.Transaction.Include(c => c.Account).Where(x => x.Id == item.Id)
                .FirstOrDefaultAsync();

            if (existingTransaction == null)
            {
                _logger.LogWarning($"Transaction with ID {item.Id} not found.");
                return new ApiResponse<Transaction>(null, $"Transaction with ID {item.Id} not found.");
            }

            // Update the properties of the existing transaction
            existingTransaction.TxnDate = item.TxnDate;
            existingTransaction.TxnAmount = item.TxnAmount;
            existingTransaction.Description = item.Description;
            existingTransaction.IsActive = item.IsActive;
            existingTransaction.IsCatConfirmed = item.IsCatConfirmed;
            existingTransaction.UniqueKey = item.UniqueKey;
            existingTransaction.Note = item.Note;

            existingTransaction.LastUpdatedDate = DateTime.Now;

            if (item.Area != null) item.Area = item.Area.ToUpper();

            existingTransaction.Area = item.Area;

            _context.Transaction.Update(existingTransaction);
            await _context.SaveChangesAsync();

            return new ApiResponse<Transaction>(existingTransaction,
                $"Transaction with ID {item.Id} updated successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error updating transaction with ID {item.Id}: {ex.Message}");
            return new ApiResponse<Transaction>(null, $"Error updating transaction with ID {item.Id}");
        }
    }

    public async Task<ApiResponse<Transaction>> CategoryConfirmed(Transaction item)
    {
        if (item.IsCatConfirmed)
            //Confirmed Category
            //add train data
            _mlService.AddToTrain(item);

        try
        {
            return new ApiResponse<Transaction>(UpdateTransaction(item).Result.Data,
                $"Category confirmation for transaction with ID {item.Id} updated successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error confirming category for transaction with ID {item.Id}: {ex.Message}");
            return new ApiResponse<Transaction>(null, $"Error confirming category for transaction with ID {item.Id}");
        }
    }

    public async Task<ApiResponse<Transaction>> CategorizeTransaction(Transaction item)
    {
        try
        {
            item.Area = _mlService.PredictCategory(item.Description).ToUpper();

            return new ApiResponse<Transaction>(UpdateTransaction(item).Result.Data,
                $"Transaction with ID {item.Id} categorized successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error categorizing transaction with ID {item.Id}: {ex.Message}");
            return new ApiResponse<Transaction>(null, $"Error categorizing transaction with ID {item.Id}");
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

                await UpdateTransaction(item);
                // var result = _context.Transaction.Update(item);
                // await _context.SaveChangesAsync();
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

    public async Task<ApiResponse<Transaction>> AddTransaction(Transaction item)
    {
        var account = await _context.AccountMasterData.Include(c => c.Currency)
            .Where(x => x.Id == item.Account.Id)
            .FirstOrDefaultAsync();

        try
        {
            item.LastUpdatedDate = DateTime.Now;
            if (item.Area != null) item.Area = item.Area.ToUpper();

            item.CreatedDate = DateTime.Now;
            item.IsActive = true;
            item.UniqueKey = MD5UniqueKey(item.Account.Name, item.TxnDate.ToString(CultureInfo.InvariantCulture),
                item.TxnAmount.ToString(CultureInfo.InvariantCulture), item.Description);
            item.Account = account;

            if (_context.Transaction.Any(x => x.UniqueKey == item.UniqueKey))
            {
                //record already present: Duplicate Record
                item.Id = 0;
                return new ApiResponse<Transaction>(null, "Transaction already exists with the same unique key.");
            }

            await _context.Transaction.AddAsync(item);
            await _context.SaveChangesAsync();

            return new ApiResponse<Transaction>(item, $"Transaction with ID {item.Id} added successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error adding transaction: {ex.Message}");
            return new ApiResponse<Transaction>(null, $"Error adding transaction: {ex.Message}");
        }
    }

    public async Task<ApiResponse<bool>> DeleteTransaction(int id)
    {
        try
        {
            var existingTransaction = await _context.Transaction.Include(c => c.Account)
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

    public async Task<ApiResponse<string>> UploadCsv(IList<Transaction> transactions)
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