using System.ComponentModel.DataAnnotations;
using MoneyManagement_Web.Data.BankAccount;

namespace MoneyManagement_Web.Data;

public class CardMasterData : BaseEntity
{
    [Key] public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Currency { get; set; }
    public string CardType { get; set; }
    public string Network { get; set; }
    public string Pan { get; set; } //TODO CRIPT
    public string Secret { get; set; } //TODO PIN, CVV2 ETC

    //public AccountMasterData AccountMasterData { get; set; }
}