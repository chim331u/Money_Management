using System.ComponentModel.DataAnnotations;
using MoneyManagement_Api.Models.Utility;

namespace MoneyManagement_Api.Models.IdentityAccess;

public class ISA_Tags : BaseEntity
{
    [Key] public int Id { get; set; }
    [Required] public string Name { get; set; }

    public ISA_Applications ISA_Applications { get; set; }
}