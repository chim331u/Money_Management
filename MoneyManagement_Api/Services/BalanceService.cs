using Microsoft.EntityFrameworkCore;
using MoneyManagement_Api.AppContext;
using MoneyManagement_Api.Contract;
using MoneyManagement_Api.Interfaces;
using MoneyManagement_Api.Models.Balance;

namespace MoneyManagement_Api.Services;

public class BalanceService : IBalanceService
{
    private readonly ApplicationContext _context;
    private readonly ILogger<BalanceService> _logger;

    public BalanceService(ApplicationContext context, ILogger<BalanceService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<ApiResponse<ICollection<Balance>>> GetActiveBalanceList()
    {
        try
        {
            var result = await _context.Balance
                .Include(c => c.Account)
                .Include(c => c.Account.Currency)
                .Where(x => x.IsActive)
                .OrderByDescending(x => x.DateBalance).ToListAsync();

            return new ApiResponse<ICollection<Balance>>(result,
                $"Balance list retrieved successfully with {result.Count} items.");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error retrieving Active balance list: {ex.Message}");
            return new ApiResponse<ICollection<Balance>>(null, $"Error retrieving Active balance list: {ex.Message}");
        }
    }

    public async Task<ApiResponse<Balance>> GetBalance(int balanceId)
    {
        try
        {
            var result = await _context.Balance
                .Include(c => c.Account)
                .Include(c => c.Account.Currency)
                .Where(x => x.Id == balanceId).FirstOrDefaultAsync();

            return new ApiResponse<Balance>(result, $"Balance retrieved successfully with id {balanceId}");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error retrieving Balance: {ex.Message}");
            return new ApiResponse<Balance>(null, $"Error retrieving Balance: {ex.Message}");
        }
    }

    public async Task<ApiResponse<Balance>> UpdateBalance(Balance item)
    {
        try
        {
            var existingBalance = await _context.Balance
                .Include(c => c.Account)
                .Include(c => c.Account.Currency)
                .Where(x => x.Id == item.Id).FirstOrDefaultAsync();

            if (existingBalance == null)
            {
                _logger.LogError("Balance not found for update.");
                return new ApiResponse<Balance>(null, $"Balance not found for update.");
            }

            existingBalance.DateBalance = item.DateBalance;
            existingBalance.BalanceValue = item.BalanceValue;
            existingBalance.IsActive = item.IsActive;
            existingBalance.Note = item.Note;
            existingBalance.LastUpdatedDate = item.LastUpdatedDate;

            _context.Balance.Update(existingBalance);
            await _context.SaveChangesAsync();

            return new ApiResponse<Balance>(existingBalance,
                $"Balance updated successfully with id {existingBalance.Id}");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error update Balance: {ex.Message}");
            return new ApiResponse<Balance>(null, $"Error update Balance: {ex.Message}");
        }
    }

    public async Task<ApiResponse<Balance>> AddBalance(Balance item)
    {
        var account = await _context.AccountMasterData
            .Include(x => x.Currency)
            .Where(x => x.Id == item.Account.Id)
            .FirstOrDefaultAsync();

        try
        {
            item.CreatedDate = DateTime.Now;
            item.IsActive = true;
            item.Account = account;
            item.LastUpdatedDate = DateTime.Now;
            await _context.Balance.AddAsync(item);
            await _context.SaveChangesAsync();

            return new ApiResponse<Balance>(item, $"Balance added successfully with id {item.Id}");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error adding Balance: {ex.Message}");
            return new ApiResponse<Balance>(null, $"Error adding Balance: {ex.Message}");
        }
    }

    public async Task<ApiResponse<bool>> DeleteBalance(int id)
    {
        try
        {
            var existingBalance = await _context.Balance.Include(c => c.Account)
                .Include(c => c.Account.Currency)
                .Where(x => x.Id == id).FirstOrDefaultAsync();

            if (existingBalance == null)
            {
                _logger.LogWarning("Balance not found for delete.");
                return new ApiResponse<bool>(false, $"Balance not found for delete.");
            }

            existingBalance.LastUpdatedDate = DateTime.Now;
            existingBalance.IsActive = false;

            _context.Balance.Update(existingBalance);

            await _context.SaveChangesAsync();

            return new ApiResponse<bool>(true, $"Balance deleted successfully with id {id}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return new ApiResponse<bool>(false, $"Error deleting Balance: {ex.Message}");
        }
    }
}