using MoneyManagement_Data.DTOs;


namespace MoneyManagement_Web.Interfaces;

public interface IBalanceService
{
    Task<List<BalanceDto>> GetActiveBalanceList();
    Task<BalanceDto> GetBalance(int bankId);
    Task<BalanceDto> AddBalance(BalanceDto bank);
    Task<BalanceDto> UpdateBalance(BalanceDto bank);
    Task<BalanceDto> DeleteBalance(BalanceDto bank);
}