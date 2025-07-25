using System.Diagnostics;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using MoneyManagement_Data.DTOs;
using MoneyManagement_Web.Interfaces;

namespace MoneyManagement_Web.Services;

public class BalanceService : IBalanceService
{
    private HttpClient _httpClient;
    private JsonSerializerOptions _serializerOptions;
    private readonly IUtilityServices _utilityServices;
    private IAccessServices _accessService;

    public BalanceService(IUtilityServices utilityService, IAccessServices accessServices)
    {
        _httpClient = new HttpClient();

        _serializerOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true,
            NumberHandling =
                JsonNumberHandling.AllowReadingFromString |
                JsonNumberHandling.WriteAsString,
            ReadCommentHandling = JsonCommentHandling.Skip
        };

        _utilityServices = utilityService;
        _accessService = accessServices;
    }

    #region Balance

    public async Task<List<BalanceDto>> GetActiveBalanceList()
    {
        var uri = new Uri(string.Format(_utilityServices.GetRestUrl() + $"api/Balance/GetBalanceList", string.Empty));
        var dataResponse = new List<BalanceDto>();

        try
        {
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", _accessService._apiToken);
            var response = await _httpClient.GetAsync(uri);

            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized) return null;

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                dataResponse = JsonSerializer.Deserialize<List<BalanceDto>>(content, _serializerOptions);
            }

            return dataResponse;
        }
        catch (Exception ex)
        {
            Debug.WriteLine(@"\tERROR {0}", ex.Message);
            return null;
        }
    }

    public async Task<BalanceDto> GetBalance(int id)
    {
        var uri = new Uri(string.Format(_utilityServices.GetRestUrl() + $"api/Balance/GetBalance/{id}", string.Empty));
        var dataResponse = new BalanceDto();

        try
        {
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", _accessService._apiToken);
            var response = await _httpClient.GetAsync(uri);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                dataResponse = JsonSerializer.Deserialize<BalanceDto>(content, _serializerOptions);
            }

            return dataResponse;
        }
        catch (Exception ex)
        {
            Debug.WriteLine(@"\tERROR {0}", ex.Message);
            return null;
        }
    }

    public async Task<BalanceDto> UpdateBalance(BalanceDto item)
    {
        var uri = new Uri(string.Format(_utilityServices.GetRestUrl() + $"api/Balance/UpdateBalance", string.Empty));

        try
        {
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", _accessService._apiToken);
            var response = await _httpClient.PutAsJsonAsync(uri, item);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var dataResponse = JsonSerializer.Deserialize<BalanceDto>(content, _serializerOptions);
                return dataResponse;
            }

            return null;
        }
        catch (Exception ex)
        {
            Console.WriteLine(@"\tERROR {0}", ex.Message);

            return null;
        }
    }

    public async Task<BalanceDto> AddBalance(BalanceDto item)
    {
        var uri = new Uri(string.Format(_utilityServices.GetRestUrl() + $"api/Balance/AddBalance", string.Empty));

        try
        {
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", _accessService._apiToken);
            var response = await _httpClient.PostAsJsonAsync(uri, item);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var dataResponse = JsonSerializer.Deserialize<BalanceDto>(content, _serializerOptions);
                return dataResponse;
            }

            return null;
        }
        catch (Exception ex)
        {
            Console.WriteLine(@"\tERROR {0}", ex.Message);

            return null;
        }
    }

    public async Task<BalanceDto> DeleteBalance(BalanceDto item)
    {
        var uri = new Uri(string.Format(_utilityServices.GetRestUrl() + $"api/Balance/DeleteBalance", string.Empty));

        try
        {
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", _accessService._apiToken);
            var response = await _httpClient.PutAsJsonAsync(uri, item);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var dataResponse = JsonSerializer.Deserialize<BalanceDto>(content, _serializerOptions);
                return dataResponse;
            }

            return null;
        }
        catch (Exception ex)
        {
            Console.WriteLine(@"\tERROR {0}", ex.Message);

            return null;
        }
    }

    #endregion
}