using MoneyManagement_Api.Contract;
using MoneyManagement_Api.Models.BankAccount;
using MoneyManagement_Data;
using MoneyManagement_Data.DTOs;

namespace MoneyManagement_Api.Interfaces;

public interface IBankAccountService
{
    Task<ApiResponse<ICollection<AccountDto>>> GetActiveAccountList();
    Task<ApiResponse<AccountDto>> GetAccount(int accountId);
    Task<ApiResponse<AccountDto>> AddAccount(AccountDto account);
    Task<ApiResponse<AccountDto>> UpdateAccount(AccountDto account);
    Task<ApiResponse<bool>> DeleteAccount(int id);

    //Task<ICollection<BankMasterData>> GetActiveBankList();
    Task<ApiResponse<ICollection<BankMasterData>>> GetActiveBankList();
    Task<ApiResponse<BankMasterData>> GetBank(int bankId);
    Task<ApiResponse<BankMasterData>> AddBank(BankMasterData bank);
    Task<ApiResponse<BankMasterData>> UpdateBank(BankMasterData bank);
    Task<ApiResponse<bool>> DeleteBank(int id);
}