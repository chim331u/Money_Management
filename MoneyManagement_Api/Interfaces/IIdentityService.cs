using MoneyManagement_Api.Models.Access;
using MoneyManagement_Api.Contract;

namespace MoneyManagement_Api.Interfaces;

public interface IIdentityService
{
    Task<AuthResponse> Login(AuthRequest model);
}