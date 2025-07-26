using MoneyManagement_Api.Contract;
using MoneyManagement_Data;
using MoneyManagement_Data.DTOs;

namespace MoneyManagement_Api.Interfaces;

public interface IIdentityServiceV1
{
    Task<ApiResponse<string>> Signup(SignupModelDto model);
    Task<ApiResponse<TokenModelDto>> Login(LoginModelDto model);
    Task<ApiResponse<string>> RefreshToken(TokenModelDto model);
    Task<ApiResponse<string>> RevokeToken(string username);
}