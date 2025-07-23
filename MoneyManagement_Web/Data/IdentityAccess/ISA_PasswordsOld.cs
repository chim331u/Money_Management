using System.ComponentModel.DataAnnotations;

namespace MoneyManagement_Web.Data.IdentityAccess
{
    public class ISA_PasswordsOld : BaseEntity
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Password { get; set; }

        //public ICollection<ISA_Accounts> ISA_Accounts { get; set; }
        public ISA_Accounts ISA_Account { get; set; }
    }
}
