using System.ComponentModel.DataAnnotations;

namespace MoneyManagement_Data.DTOs;

public class TokenModelDto
{
    [Required] public string AccessToken { get; set; } = string.Empty;

    [Required] public string RefreshToken { get; set; } = string.Empty;
}