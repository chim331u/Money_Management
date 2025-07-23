using System.ComponentModel.DataAnnotations;

namespace MoneyManagement_Web.Data.AncillaryData
{
    public class CurrencyConversionRate : BaseEntity
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public decimal RateValue { get; set; }
        [Required]
        public string CurrencyCodeALF3 { get; set; }

        public DateTime ReferringDate { get; set; }

        public string? UniqueKey { get; set; } //concat currencyCodeAlf3+rateValue+referringDate(only date, no time)


    }
}
