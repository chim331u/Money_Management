using MoneyManagement_Api.Models.Transactions;
using MoneyManagement_Data.DTOs;

namespace MoneyManagement_Api.Interfaces;

public interface IMlTransactionService
{
    string PredictCategory(string fileNameToPredict);
    string TrainAndSaveModel();
    void AddToTrain(TransactionDto transaction);
}