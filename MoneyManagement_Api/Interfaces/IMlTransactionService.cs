using MoneyManagement_Api.Models.Transactions;

namespace MoneyManagement_Api.Interfaces;

public interface IMlTransactionService
{
    string PredictCategory(string fileNameToPredict);
    string TrainAndSaveModel();
    void AddToTrain(Transaction transaction);
}