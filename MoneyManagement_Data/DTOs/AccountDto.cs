namespace MoneyManagement_Data.DTOs;

public class AccountDto
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Conto { get; set; } //NEW 
    public string? Description { get; set; }
    public string? Iban { get; set; }
    public string? Bic { get; set; }
    public string? AccountType { get; set; }
    public string? Note { get; set; }
    public int CurrencyId { get; set; }
    public string? CurrencyName { get; set; }
    public int BankId { get; set; }
    public string? BankName { get; set; }
}