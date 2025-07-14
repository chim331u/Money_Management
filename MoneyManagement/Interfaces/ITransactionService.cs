using MoneyManagement.Contract;
using MoneyManagement.Models.Transactions;

namespace MoneyManagement.Interfaces
{
    public interface ITransactionService
    {
        Task<ApiResponse<ICollection<Transaction>>> GetActiveTransactionList();
        Task<ApiResponse<Transaction>> GetTransaction(int transactionId);
        Task<ApiResponse<Transaction>> AddTransaction(Transaction transaction);
        Task<ApiResponse<Transaction>> UpdateTransaction(Transaction transaction);
        Task<ApiResponse<Transaction>> CategoryConfirmed(Transaction transaction);
        Task<ApiResponse<Transaction>> CategorizeTransaction(Transaction transaction);
        Task<ApiResponse<string>> TrainModelTransaction();
        Task<ApiResponse<string>> CategorizeAllTransaction();
        Task<ApiResponse<bool>> DeleteTransaction(int id);
        Task<ApiResponse<string>> UploadCsv(IList<Transaction> transactions);
    }
}
