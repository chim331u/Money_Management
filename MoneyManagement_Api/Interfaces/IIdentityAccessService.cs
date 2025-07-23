using MoneyManagement_Api.Contract;
using MoneyManagement_Api.Models.IdentityAccess;

namespace MoneyManagement_Api.Interfaces;

public interface IIdentityAccessService
{
    Task<ApiResponse<ICollection<ISA_Accounts>>> GetActiveIdentityAccountList();
    Task<ApiResponse<ISA_Accounts>> GetIdentityAccount(int identityAccountId);
    Task<ApiResponse<ISA_Accounts>> AddIdentityAccount(ISA_Accounts identityAccount);
    Task<ApiResponse<ISA_Accounts>> UpdateIdentityAccount(ISA_Accounts identityAccount);
    Task<ApiResponse<bool>> DeleteIdentityAccount(int id);

    Task<ApiResponse<ICollection<ISA_PasswordsOld>>> ListAllOldPasswords(int accountId);
    Task<ApiResponse<string>> PasswordChange(ISA_Accounts item);
    Task<ApiResponse<string>> GetCleanPsw(int isa_account_id);
}