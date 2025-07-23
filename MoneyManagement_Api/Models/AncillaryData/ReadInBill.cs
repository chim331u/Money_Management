using System.ComponentModel.DataAnnotations;
using MoneyManagement_Api.Models.Utility;

namespace MoneyManagement_Api.Models.AncillaryData;

public class ReadInBill : BaseEntity
{
    [Key] public int Id { get; set; }
    public string? BillProperty { get; set; }
    public string? PropertyDataType { get; set; }
    public string? KeyWord { get; set; }
    public string? RegexString { get; set; }

    public Supplier? Supplier { get; set; }
}