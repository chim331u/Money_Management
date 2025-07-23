using Microsoft.ML.Data;

namespace MoneyManagement_Api.Models.Transactions;

public class TransactionMlNamePrediction
{
    [ColumnName("PredictedLabel")] public string Area;

    [ColumnName("Score")] public float[] Score { get; set; }
}