using MoneyManagement.Interfaces;
using MoneyManagement.Models.Transactions;

namespace MoneyManagement.Endpoints;

public static class TransactionEndpoint
{
    public static IEndpointRouteBuilder MapTransactionEndPoint(this IEndpointRouteBuilder app)
    {
        // Define the endpoints
        app.MapGet("/GetTransactionList", async (ITransactionService service) =>
        {
            var result = await service.GetActiveTransactionList();
            return result != null ? Results.Ok(result) : Results.NotFound();
        });
        
        app.MapGet("/GetTransaction/{id}", async (int id, ITransactionService service) =>
        {
            var result = await service.GetTransaction(id);
            return result != null ? Results.Ok(result) : Results.NotFound();
        });
        
        app.MapPut("/UpdateTransaction", async (Transaction item, ITransactionService service) =>
        {
            var result = await service.UpdateTransaction(item);
            return result != null ? Results.Ok(result) : Results.NotFound();
        });
        
        app.MapPost("/AddTransaction", async (Transaction item, ITransactionService service) =>
        {
            var result = await service.AddTransaction(item);
            return result != null ? Results.Ok(result) : Results.BadRequest("Failed to add transaction");
        });
        
        app.MapPost("/UploadCsv", async (List<Transaction> item, ITransactionService service) =>
        {
            var result = await service.UploadCsv(item);
            return result!=null  ? Results.BadRequest("Transaction list is null or empty") : Results.Ok(result) ;
        });
        
        app.MapDelete("/DeleteTransaction/{id}", async (int id, ITransactionService service) =>
        {
            var result = await service.DeleteTransaction(id);
            return result != null ? Results.Ok(result) : Results.NotFound();
        });
        
        app.MapPut("/CategoryConfirmed", async (Transaction item, ITransactionService service) =>
        {
            var result = await service.CategoryConfirmed(item);
            return result != null ? Results.Ok(result) : Results.NotFound();
        });        
        
        app.MapPut("/CategorizeTransaction", async (Transaction item, ITransactionService service) =>
        {
            var result = await service.CategorizeTransaction(item);
            return result != null ? Results.Ok(result) : Results.NotFound();
        });
        
        app.MapPut("/CategorizeAllTransaction", async (ITransactionService service) =>
        {
            var result = await service.CategorizeAllTransaction();
            return result != null ? Results.Ok(result) : Results.NotFound();
        });
        
        app.MapPut("/TrainModelTransaction", async (ITransactionService service) =>
        {
            var result = await service.TrainModelTransaction();
            return result != null ? Results.Ok(result) : Results.NotFound();
        });
        
        
        
        return app;
    }
    
}