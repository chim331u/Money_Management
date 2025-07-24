using MoneyManagement_Api.Models.Balance;
using MoneyManagement_Api.Models.BankAccount;
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
    
   public static AccountDto MapAccountToDto(AccountMasterData account)
    {
        
        if (account == null)
        {
            Log.Logger.Warning("Account is null, cannot map to DTO.");
            return null;
        }
        
        if (account.Currency == null)
        {
            Log.Logger.Warning("Account Currency is null, cannot map to DTO.");
            return null;
        }

        return new AccountDto
        {
            Id = account.Id,
            Name = account.Name,
            Conto = account.Conto,
            Description = account.Description,
            Iban = account.Iban,
            Bic = account.Bic,
            AccountType = account.AccountType,
            Note = account.Note,
            CurrencyId = account.Currency.Id,
            CurrencyName = account.Currency.Name,
            BankId = account.BankMasterData?.Id ?? 0,
            BankName = account.BankMasterData?.Name
        };
    }
   
   public static BalanceDto MapBalanceToDto(Balance balance)
   { 
        if (balance == null)
        {
            Log.Logger.Warning("Balance is null, cannot map to DTO.");
            return null;
        }
        
        if (balance.Account == null)
        {
            Log.Logger.Warning("Balance Account is null, cannot map to DTO.");
            return null;
        }
        
        if (balance.Account.Currency == null)
        {
            Log.Logger.Warning("Balance Account Currency is null, cannot map to DTO.");
            return null;
        }

        return new BalanceDto
        {
            Id = balance.Id,
            AccountId = balance.Account.Id,
            AccountName = balance.Account.Name,
            CurrencyId = balance.Account.Currency.Id,
            CurrencyName = balance.Account.Currency.Name,
            DateBalance = balance.DateBalance,
            BalanceValue = balance.BalanceValue,
            Note = balance.Note
        };
     }
   
}