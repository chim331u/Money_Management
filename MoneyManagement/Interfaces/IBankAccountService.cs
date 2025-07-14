using MoneyManagement.Contract;
using MoneyManagement.Models.BankAccount;

namespace MoneyManagement.Interfaces
{
    public interface IBankAccountService
    {

        Task<ApiResponse<ICollection<AccountMasterData>>> GetActiveAccountList();
        Task<ApiResponse<AccountMasterData>> GetAccount(int accountId);
        Task<ApiResponse<AccountMasterData>> AddAccount(AccountMasterData account);
        Task<ApiResponse<AccountMasterData>> UpdateAccount(AccountMasterData account);
        Task<ApiResponse<bool>> DeleteAccount(int id);

        //Task<ICollection<BankMasterData>> GetActiveBankList();
        Task<ApiResponse<ICollection<BankMasterData>>> GetActiveBankList();
        Task<ApiResponse<BankMasterData>> GetBank(int bankId);
        Task<ApiResponse<BankMasterData>> AddBank(BankMasterData bank);
        Task<ApiResponse<BankMasterData>> UpdateBank(BankMasterData bank);
        Task<ApiResponse<bool>> DeleteBank(int id);
    }
}
