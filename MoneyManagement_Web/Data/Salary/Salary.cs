using System.ComponentModel.DataAnnotations;
using MoneyManagement_Web.Data.AncillaryData;

namespace MoneyManagement_Web.Data.Salary;

public class Salary : BaseEntity
{
    [Key] public int Id { get; set; }
    public double SalaryValue { get; set; }
    public double SalaryValueEur { get; set; }
    public DateTime SalaryDate { get; set; }
    public string? ReferYear { get; set; }
    public string? ReferMonth { get; set; }
    public string? FileName { get; set; }
    public decimal ExcengeRate { get; set; }
    public Currency? Currency { get; set; }

    public ServiceUser? User { get; set; }
}