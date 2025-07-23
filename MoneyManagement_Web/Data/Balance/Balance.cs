using System.ComponentModel.DataAnnotations;
using MoneyManagement_Web.Data.BankAccount;

namespace MoneyManagement_Web.Data.Balance
{
    public class Balance : BaseEntity
    {
        [Key]
        public int Id { get; set; }
        public double BalanceValue { get; set; }
        public DateTime DateBalance { get; set; }

        public AccountMasterData? Account { get; set; }
    }
}
