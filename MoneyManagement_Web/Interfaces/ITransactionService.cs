using MoneyManagement_Data.DTOs;

namespace MoneyManagement_Web.Interfaces;

public interface ITransactionService
{
    Task<List<TransactionDto>> GetActiveTransactionList();
    Task<TransactionDto> GetTransaction(int transactionId);
    Task<TransactionDto> AddTransaction(TransactionDto transaction);
    Task<TransactionDto> UpdateTransaction(TransactionDto transaction);
    Task<TransactionDto> CategoryConfirmed(TransactionDto transaction);
    Task<TransactionDto> DeleteTransaction(TransactionDto transaction);
    Task<string> UploadCsv(IList<TransactionDto> transactions);

    Task<string> TrainModel();
    Task<string> CategorizeAllTransaction();
}