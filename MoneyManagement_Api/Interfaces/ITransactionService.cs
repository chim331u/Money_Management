using MoneyManagement_Data;
using MoneyManagement_Data.DTOs;

namespace MoneyManagement_Api.Interfaces;

public interface ITransactionService
{
    Task<ApiResponse<ICollection<TransactionDto>>> GetActiveTransactionList();
    Task<ApiResponse<TransactionDto>> GetTransaction(int transactionId);
    Task<ApiResponse<TransactionDto>> AddTransaction(TransactionDto transaction);
    Task<ApiResponse<TransactionDto>> UpdateTransaction(TransactionDto transaction);
    Task<ApiResponse<TransactionDto>> CategoryConfirmed(TransactionDto transaction);
    Task<ApiResponse<TransactionDto>> CategorizeTransaction(TransactionDto transaction);
    Task<ApiResponse<string>> TrainModelTransaction();
    Task<ApiResponse<string>> CategorizeAllTransaction();
    Task<ApiResponse<bool>> DeleteTransaction(int id);
    Task<ApiResponse<string>> UploadCsv(IList<TransactionDto> transactions);
}