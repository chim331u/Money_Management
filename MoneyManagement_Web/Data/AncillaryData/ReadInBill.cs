using System.ComponentModel.DataAnnotations;

namespace MoneyManagement_Web.Data.AncillaryData
{
    public class ReadInBill : BaseEntity
    {
        [Key]
        public int Id { get; set; }
        public string? BillProperty { get; set; }
        public string? PropertyDataType { get; set; }
        public string? KeyWord { get; set; }
        public string? RegexString { get; set; }

        public Supplier? Supplier { get; set; }
    }
}
