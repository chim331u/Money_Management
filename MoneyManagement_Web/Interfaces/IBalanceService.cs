using MoneyManagement_Web.Data.Balance;

namespace MoneyManagement_Web.Interfaces
{
    public interface IBalanceService
    {
        Task<List<Balance>> GetActiveBalanceList();
        Task<Balance> GetBalance(int bankId);
        Task<Balance> AddBalance(Balance bank);
        Task<Balance> UpdateBalance(Balance bank);
        Task<Balance> DeleteBalance(Balance bank);
    }
}
