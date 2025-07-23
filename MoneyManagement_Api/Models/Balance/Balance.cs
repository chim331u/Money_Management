using System.ComponentModel.DataAnnotations;
using MoneyManagement_Api.Models.BankAccount;
using MoneyManagement_Api.Models.Utility;

namespace MoneyManagement_Api.Models.Balance;

public class Balance : BaseEntity
{
    [Key] public int Id { get; set; }
    public double BalanceValue { get; set; }
    public DateTime DateBalance { get; set; }

    public AccountMasterData? Account { get; set; }
}