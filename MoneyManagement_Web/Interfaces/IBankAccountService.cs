using MoneyManagement_Web.Data.BankAccount;

namespace MoneyManagement_Web.Interfaces
{
    public interface IBankAccountService
    {
        Task<List<AccountMasterData>> GetActiveAccountListOriginal();
        Task<List<AccountMasterData>> GetActiveAccountList();
        Task<AccountMasterData> GetAccount(int accountId);
        Task<AccountMasterData> AddAccount(AccountMasterData account);
        Task<AccountMasterData> UpdateAccount(AccountMasterData account);
        Task<AccountMasterData> DeleteAccount(AccountMasterData account);

        Task<List<BankMasterData>> GetActiveBankList();
        Task<BankMasterData> GetBank(int bankId);
        Task<BankMasterData> AddBank(BankMasterData bank);
        Task<BankMasterData> UpdateBank(BankMasterData bank);
        Task<BankMasterData> DeleteBank(BankMasterData bank);
    }
}
