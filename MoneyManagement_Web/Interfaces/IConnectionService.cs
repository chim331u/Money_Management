using MoneyManagement_Data;
using MoneyManagement_Web.Data;

namespace MoneyManagement_Web.Interfaces;

public interface IConnectionService
{
    bool IsAuthorized{get;set;}
    string _apiToken { get; set; }
    event Action? OnChange;
    
    Task<bool> IsConnectedAsync();
    
    Task<ApiResponse<T>> GetAsync<T>(string endpoint, string parameters);
    
    Task<ApiResponse<T>> PostAsync<T>(string endpoint, object data);
    Task<ApiResponse<T>> PutAsync<T>(string endpoint, T data);
    Task<ApiResponse<T>> DeleteAsync<T>(string endpoint, string parameters);
}