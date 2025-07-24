using MoneyManagement_Api.Contract;
using MoneyManagement_Api.Models.Transactions;
using MoneyManagement_Data;

namespace MoneyManagement_Api.Interfaces;

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