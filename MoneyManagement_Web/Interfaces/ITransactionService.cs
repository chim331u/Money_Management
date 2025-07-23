using MoneyManagement_Web.Data.Transactions;

namespace MoneyManagement_Web.Interfaces
{
    public interface ITransactionService
    {
        Task<List<Transaction>> GetActiveTransactionList();
        Task<Transaction> GetTransaction(int transactionId);
        Task<Transaction> AddTransaction(Transaction transaction);
        Task<Transaction> UpdateTransaction(Transaction transaction);
        Task<Transaction> CategoryConfirmed(Transaction transaction);
        Task<Transaction> DeleteTransaction(Transaction transaction);
        Task<string> UploadCsv(IList<Transaction> transactions);

        Task<string> TrainModel();
        Task<string> CategorizeAllTransaction();
    }
}
