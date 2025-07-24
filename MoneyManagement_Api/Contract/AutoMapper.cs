using MoneyManagement_Api.Models.Transactions;
using MoneyManagement_Data.DTOs;
using Serilog;

namespace MoneyManagement_Api.Contract;

public static class AutoMapper 
{
   
    public static TransactionDto MapTransactionToDto(Transaction transaction)
    {
        //Log.Debug($"Mapping transaction with ID: {transaction?.Id}");
        
        if (transaction == null)
        {
            Log.Logger.Warning("Transaction is null, cannot map to DTO.");
            return null;
        }
        
        if (transaction.Account == null)
        {
            Log.Logger.Warning("Transaction Account is null, cannot map to DTO.");
            return null;
        }
        
        if (transaction.Account.Currency == null)
        {
            Log.Logger.Warning("Transaction Account Currency is null, cannot map to DTO.");
            return null;
        }

        
        return new TransactionDto
        {
            Id = transaction.Id,
            Description = transaction.Description,
            Area = transaction.Area,
            IsCatConfirmed = transaction.IsCatConfirmed,
            Note = transaction.Note,
            TxnAmount = transaction.TxnAmount,
            TxnDate = transaction.TxnDate,
            AccountName = transaction.Account.Name,
            AccountId = transaction.Account.Id,
            AccountCurrencyCodeALF3 = transaction.Account.Currency.CurrencyCodeALF3
           
        };
    }
    
   
}