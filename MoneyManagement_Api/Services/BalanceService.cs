using Microsoft.EntityFrameworkCore;
using MoneyManagement_Api.AppContext;
using MoneyManagement_Api.Contract;
using MoneyManagement_Api.Interfaces;
using MoneyManagement_Api.Models.Balance;
using MoneyManagement_Data;
using MoneyManagement_Data.DTOs;

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

    public async Task<ApiResponse<ICollection<BalanceDto>>> GetActiveBalanceList()
    {
        try
        {
            var result = await _context.Balance
                .Include(c => c.Account)
                .Include(c => c.Account.Currency)
                .Where(x => x.IsActive)
                .OrderByDescending(x => x.DateBalance).ToListAsync();

            return new ApiResponse<ICollection<BalanceDto>>(result.Select(b => AutoMapper.MapBalanceToDto(b)).ToList(),
                $"Balance list retrieved successfully with {result.Count} items.");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error retrieving Active balance list: {ex.Message}");
            return new ApiResponse<ICollection<BalanceDto>>(null,
                $"Error retrieving Active balance list: {ex.Message}");
        }
    }

    public async Task<ApiResponse<BalanceDto>> GetBalance(int balanceId)
    {
        try
        {
            var result = await _context.Balance
                .Include(c => c.Account)
                .Include(c => c.Account.Currency)
                .Where(x => x.Id == balanceId).FirstOrDefaultAsync();

            return new ApiResponse<BalanceDto>(AutoMapper.MapBalanceToDto(result),
                $"Balance retrieved successfully with id {balanceId}");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error retrieving Balance: {ex.Message}");
            return new ApiResponse<BalanceDto>(null, $"Error retrieving Balance: {ex.Message}");
        }
    }

    public async Task<ApiResponse<BalanceDto>> UpdateBalance(BalanceDto item)
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
                return new ApiResponse<BalanceDto>(null, $"Balance not found for update.");
            }

            existingBalance.DateBalance = item.DateBalance;
            existingBalance.BalanceValue = item.BalanceValue;
            existingBalance.Note = item.Note;
            existingBalance.LastUpdatedDate = DateTime.Now;


            _context.Balance.Update(existingBalance);
            await _context.SaveChangesAsync();

            return new ApiResponse<BalanceDto>(AutoMapper.MapBalanceToDto(existingBalance),
                $"Balance updated successfully with id {existingBalance.Id}");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error update Balance: {ex.Message}");
            return new ApiResponse<BalanceDto>(null, $"Error update Balance: {ex.Message}");
        }
    }

    public async Task<ApiResponse<BalanceDto>> AddBalance(BalanceDto item)
    {
        var account = await _context.AccountMasterData
            .Include(x => x.Currency)
            .Where(x => x.Id == item.AccountId)
            .FirstOrDefaultAsync();

        try
        {
            var newBalance = new Balance
            {
                CreatedDate = DateTime.Now,
                DateBalance = item.DateBalance,
                BalanceValue = item.BalanceValue,
                Note = item.Note,
                Account = account,
                IsActive = true
            };

            await _context.Balance.AddAsync(newBalance);
            await _context.SaveChangesAsync();

            return new ApiResponse<BalanceDto>(AutoMapper.MapBalanceToDto(newBalance),
                $"Balance added successfully with id {newBalance.Id}");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error adding Balance: {ex.Message}");
            return new ApiResponse<BalanceDto>(null, $"Error adding Balance: {ex.Message}");
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