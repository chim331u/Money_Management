using System.Diagnostics;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using MoneyManagement_Data;
using MoneyManagement_Web.Data;
using MoneyManagement_Web.Interfaces;

namespace MoneyManagement_Web.Services;

public class ConnectionService : IConnectionService
{
    public bool IsAuthorized { get => isAuthorized; set { isAuthorized = value; NotifyStateChanged(); } }
    public string _apiToken { get; set; }

    private bool isAuthorized;

    public event Action? OnChange;

    private void NotifyStateChanged() => OnChange?.Invoke();
    HttpClient _httpClient;
    JsonSerializerOptions _serializerOptions;
    private readonly IConfiguration _config;
    private string _baseUrl;
    // private readonly IUtilityServices _utilityServices;
  
    public ConnectionService(HttpClient httpClient, IConfiguration config)
    {
        _httpClient = httpClient;

        _serializerOptions = new JsonSerializerOptions
        {
            // PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true,
            NumberHandling =
                JsonNumberHandling.AllowReadingFromString |
                JsonNumberHandling.WriteAsString,
            ReadCommentHandling = JsonCommentHandling.Skip
        };
        
        _config = config;
        
        _baseUrl = _config.GetSection("Uri").Value;
    }
    
    public async Task<bool> IsConnectedAsync()
    {
        Uri uri = new Uri(string.Format(_baseUrl + $"api/Balance/GetBalanceList", string.Empty));
        //var dataResponse = new List<Balance>();

        try
        {
            //_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiToken);
            HttpResponseMessage response = await _httpClient.GetAsync(uri);

            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                return false;
            }

            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
               // dataResponse = JsonSerializer.Deserialize<List<Balance>>(content, _serializerOptions);
               response.EnsureSuccessStatusCode();
                return true;
            }

            return false;

        }
        catch (Exception ex)
        {
            Debug.WriteLine(@"\tERROR {0}", ex.Message);
            return false;
        }
    }

    public async Task<ApiResponse<T>> GetAsync<T>(string endpoint, string parameters)
    {
        Uri uri = new Uri($"{_baseUrl}{endpoint}{parameters}");
        //var dataResponse = new List<Balance>();

        try
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiToken);
            HttpResponseMessage response = await _httpClient.GetAsync(uri);

            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                return new ApiResponse<T>(default, "Unauthorized access");
            }

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
               var dataResponse = JsonSerializer.Deserialize<ApiResponse<T>>(content, _serializerOptions);
                response.EnsureSuccessStatusCode();
                return dataResponse;
            }

            return new ApiResponse<T>(default, response.ReasonPhrase);

        }
        catch (Exception ex)
        {
            Debug.WriteLine(@"\tERROR {0}", ex.Message);
            return new ApiResponse<T>(default, ex.Message);
        }
    }

    public async Task<ApiResponse<T>> PostAsync<T>(string endpoint, object data)
    {
        Uri uri = new Uri($"{_baseUrl}{endpoint}");
        
        try
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiToken);
            var response = await _httpClient.PostAsJsonAsync(uri, data);

            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                return new ApiResponse<T>(default, "Unauthorized access");
            }

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var dataResponse = JsonSerializer.Deserialize<ApiResponse<T>>(content, _serializerOptions);
                response.EnsureSuccessStatusCode();
                return dataResponse;
            }

            return new ApiResponse<T>(default, response.ReasonPhrase);

        }
        catch (Exception ex)
        {
            Debug.WriteLine(@"\tERROR {0}", ex.Message);
            return new ApiResponse<T>(default, $"Error: {ex.Message}");
        }
    }

    public async Task<ApiResponse<T>> PutAsync<T>(string endpoint, T data)
    {
        Uri uri = new Uri($"{_baseUrl}{endpoint}");
        
        try
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiToken);
            var response = await _httpClient.PutAsJsonAsync(uri, data);

            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                return new ApiResponse<T>(default, "Unauthorized access");
            }

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var dataResponse = JsonSerializer.Deserialize<ApiResponse<T>>(content, _serializerOptions);
                response.EnsureSuccessStatusCode();
                return dataResponse;
            }

            return new ApiResponse<T>(default, response.ReasonPhrase);

        }
        catch (Exception ex)
        {
            Debug.WriteLine(@"\tERROR {0}", ex.Message);
            return new ApiResponse<T>(default, $"Error: {ex.Message}");
        }
    }

    public async Task<ApiResponse<T>> DeleteAsync<T>(string endpoint, string parameters)
    {
        Uri uri = new Uri($"{_baseUrl}{endpoint}{parameters}");
        
        try
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiToken);
            var response = await _httpClient.DeleteAsync(uri);

            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                return new ApiResponse<T>(default, "Unauthorized access");
            }

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var dataResponse = JsonSerializer.Deserialize<ApiResponse<T>>(content, _serializerOptions);
                response.EnsureSuccessStatusCode();
                return dataResponse;
            }

            return new ApiResponse<T>(default, response.ReasonPhrase);

        }
        catch (Exception ex)
        {
            Debug.WriteLine(@"\tERROR {0}", ex.Message);
            return new ApiResponse<T>(default, $"Error: {ex.Message}");
        }
    }
}