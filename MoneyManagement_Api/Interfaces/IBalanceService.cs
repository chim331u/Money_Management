using MoneyManagement_Data;
using MoneyManagement_Data.DTOs;

namespace MoneyManagement_Api.Interfaces;

public interface IBalanceService
{
    Task<ApiResponse<ICollection<BalanceDto>>> GetActiveBalanceList();
    Task<ApiResponse<BalanceDto>> GetBalance(int balanceId);
    Task<ApiResponse<BalanceDto>> AddBalance(BalanceDto balance);
    Task<ApiResponse<BalanceDto>> UpdateBalance(BalanceDto balance);
    Task<ApiResponse<bool>> DeleteBalance(int id);
}