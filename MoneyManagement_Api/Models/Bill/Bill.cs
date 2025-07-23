using System.ComponentModel.DataAnnotations;
using MoneyManagement_Api.Models.AncillaryData;
using MoneyManagement_Api.Models.Utility;

namespace MoneyManagement_Api.Models.Bill;

public class Bill : BaseEntity
{
    [Key] public int Id { get; set; }

    public double Amount { get; set; }
    public DateTime PaidDate { get; set; }
    public DateTime RefPeriodStart { get; set; }
    public DateTime RefPeriodEnd { get; set; }
    public string? FullPathFileName { get; set; }
    public double Consumption { get; set; }
    public string? BillNumber { get; set; }


    public Supplier? Supplier { get; set; }
}