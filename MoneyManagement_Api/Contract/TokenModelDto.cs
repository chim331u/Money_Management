using System.ComponentModel.DataAnnotations;

namespace MoneyManagement_Api.Contract;

public class TokenModelDto
{
    [Required] public string AccessToken { get; set; } = string.Empty;

    [Required] public string RefreshToken { get; set; } = string.Empty;
}