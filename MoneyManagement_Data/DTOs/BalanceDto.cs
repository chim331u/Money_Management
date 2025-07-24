namespace MoneyManagement_Data.DTOs;

public class BalanceDto
{
    public int Id { get; set; }
    public double BalanceValue { get; set; }
    public DateTime DateBalance { get; set; }
    public string? Note { get; set; }
    public int AccountId { get; set; }
    public string? AccountName { get; set; }
    public string? CurrencyName { get; set; }
    public int CurrencyId { get; set; }
}