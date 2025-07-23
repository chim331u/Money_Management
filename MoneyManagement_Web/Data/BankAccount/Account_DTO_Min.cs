using System.ComponentModel.DataAnnotations;

namespace MoneyManagement_Web.Data.BankAccount
{
    public class Account_DTO_Min
    {

            [Key]
            public int Id { get; set; }
            public string? Name { get; set; }
    }
}
