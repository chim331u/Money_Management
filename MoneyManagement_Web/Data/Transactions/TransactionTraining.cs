using System.ComponentModel.DataAnnotations;

namespace MoneyManagement_Web.Data.Transactions
{
    public class TransactionTraining
    {
        [Key]
        public int Id { get; set; }
        public string Description { get; set; }
        public string UniqueKey { get; set; }

        public string Area { get; set; } //category, tab

    }
}
