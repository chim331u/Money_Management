using MoneyManagement_Data.DTOs;
using MoneyManagement_Web.Data.BankAccount;

namespace MoneyManagement_Web.Interfaces;

public interface IBankAccountService
{
    Task<List<AccountDto>> GetActiveAccountListOriginal();
    Task<List<AccountDto>> GetActiveAccountList();
    Task<AccountDto> GetAccount(int accountId);
    Task<AccountDto> AddAccount(AccountDto account);
    Task<AccountDto> UpdateAccount(AccountDto account);
    Task<AccountDto> DeleteAccount(AccountDto account);

    Task<List<BankMasterData>> GetActiveBankList();
    Task<BankMasterData> GetBank(int bankId);
    Task<BankMasterData> AddBank(BankMasterData bank);
    Task<BankMasterData> UpdateBank(BankMasterData bank);
    Task<BankMasterData> DeleteBank(BankMasterData bank);
}