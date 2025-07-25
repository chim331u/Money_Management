using MoneyManagement_Web.Data.IdentityAccess;

namespace MoneyManagement_Web.Interfaces;

public interface IIdentityAccessService
{
    Task<List<ISA_Accounts>> GetActiveIdentityAccountList();
    Task<ISA_Accounts> GetIdentityAccount(int identityAccountId);
    Task<ISA_Accounts> AddIdentityAccount(ISA_Accounts identityAccount);
    Task<ISA_Accounts> UpdateIdentityAccount(ISA_Accounts identityAccount);
    Task<ISA_Accounts> DeleteIdentityAccount(ISA_Accounts identityAccount);

    Task<ICollection<ISA_PasswordsOld>> ListAllOldPasswords(int accountId);
    Task<string> PasswordChange(ISA_Accounts item);
    Task<string> GetCleanPsw(int isa_account_id);
}