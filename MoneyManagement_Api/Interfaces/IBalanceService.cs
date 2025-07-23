using MoneyManagement_Api.Contract;
using MoneyManagement_Api.Models.Balance;

namespace MoneyManagement_Api.Interfaces;

public interface IBalanceService
{
    Task<ApiResponse<ICollection<Balance>>> GetActiveBalanceList();
    Task<ApiResponse<Balance>> GetBalance(int balanceId);
    Task<ApiResponse<Balance>> AddBalance(Balance balance);
    Task<ApiResponse<Balance>> UpdateBalance(Balance balance);
    Task<ApiResponse<bool>> DeleteBalance(int id);
}