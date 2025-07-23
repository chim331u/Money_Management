using System.ComponentModel.DataAnnotations;

namespace MoneyManagement_Web.Data.AncillaryData
{
    public class Currency : BaseEntity
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }

        public string? Description { get; set; }
        [Required]
        public string CurrencyCodeALF3 { get; set; }

        public string? CurrencyCodeNum3 { get; set; }

        //public ICollection<AccountMasterData> AccountMasterData { get; set; }

    }
}
