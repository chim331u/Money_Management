using MoneyManagement.Contract;
using MoneyManagement.Models.Balance;

namespace MoneyManagement.Interfaces
{
    public interface IBalanceService
    {
        Task<ApiResponse<ICollection<Balance>>> GetActiveBalanceList();
        Task<ApiResponse<Balance>> GetBalance(int balanceId);
        Task<ApiResponse<Balance>> AddBalance(Balance balance);
        Task<ApiResponse<Balance>> UpdateBalance(Balance balance);
        Task<ApiResponse<bool>> DeleteBalance(int id);
    }
}
