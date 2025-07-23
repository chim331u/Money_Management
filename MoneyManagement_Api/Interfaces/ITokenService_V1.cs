using System.Security.Claims;

namespace MoneyManagement_Api.Interfaces;

public interface ITokenService_V1
{
    string GenerateAccessToken(IEnumerable<Claim> claims);
    string GenerateRefreshToken();
    ClaimsPrincipal GetPrincipalFromExpiredToken(string accessToken);
}