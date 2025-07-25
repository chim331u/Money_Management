using System.Diagnostics;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using MoneyManagement_Data.DTOs;
using MoneyManagement_Web.Data.BankAccount;
using MoneyManagement_Web.Interfaces;

namespace MoneyManagement_Web.Services;

public class BankAccountService : IBankAccountService
{
    private HttpClient _httpClient;
    private JsonSerializerOptions _serializerOptions;
    private readonly IUtilityServices _utilityServices;
    private IAccessServices _accessService;

    public BankAccountService(IUtilityServices utilityService, IAccessServices accessServices)
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

    #region Bank

    public async Task<List<BankMasterData>> GetActiveBankList()
    {
        var uri = new Uri(string.Format(_utilityServices.GetRestUrl() + $"api/Bank/GetBankList", string.Empty));
        var dataResponse = new List<BankMasterData>();

        try
        {
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", _accessService._apiToken);
            var response = await _httpClient.GetAsync(uri);

            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized) return null;

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                dataResponse = JsonSerializer.Deserialize<List<BankMasterData>>(content, _serializerOptions);
            }

            return dataResponse;
        }
        catch (Exception ex)
        {
            Debug.WriteLine(@"\tERROR {0}", ex.Message);
            return null;
        }
    }

    public async Task<BankMasterData> GetBank(int id)
    {
        var uri = new Uri(string.Format(_utilityServices.GetRestUrl() + $"api/Bank/GetBank/{id}", string.Empty));
        var dataResponse = new BankMasterData();

        try
        {
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", _accessService._apiToken);
            var response = await _httpClient.GetAsync(uri);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                dataResponse = JsonSerializer.Deserialize<BankMasterData>(content, _serializerOptions);
            }

            return dataResponse;
        }
        catch (Exception ex)
        {
            Debug.WriteLine(@"\tERROR {0}", ex.Message);
            return null;
        }
    }

    public async Task<BankMasterData> UpdateBank(BankMasterData item)
    {
        var uri = new Uri(string.Format(_utilityServices.GetRestUrl() + $"api/Bank/UpdateBank", string.Empty));

        try
        {
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", _accessService._apiToken);
            var response = await _httpClient.PutAsJsonAsync(uri, item);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var dataResponse = JsonSerializer.Deserialize<BankMasterData>(content, _serializerOptions);
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

    public async Task<BankMasterData> AddBank(BankMasterData item)
    {
        var uri = new Uri(string.Format(_utilityServices.GetRestUrl() + $"api/Bank/AddBank", string.Empty));

        try
        {
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", _accessService._apiToken);
            var response = await _httpClient.PostAsJsonAsync(uri, item);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var dataResponse = JsonSerializer.Deserialize<BankMasterData>(content, _serializerOptions);
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

    public async Task<BankMasterData> DeleteBank(BankMasterData item)
    {
        var uri = new Uri(string.Format(_utilityServices.GetRestUrl() + $"api/Bank/DeleteBank", string.Empty));

        try
        {
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", _accessService._apiToken);
            var response = await _httpClient.PutAsJsonAsync(uri, item);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var dataResponse = JsonSerializer.Deserialize<BankMasterData>(content, _serializerOptions);
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


    #region Account

    public async Task<List<AccountDto>> GetActiveAccountListOriginal()
    {
        var uri = new Uri(string.Format(_utilityServices.GetRestUrl() + $"api/Account/GetAccountListOriginal",
            string.Empty));
        var dataResponse = new List<AccountDto>();

        try
        {
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", _accessService._apiToken);
            var response = await _httpClient.GetAsync(uri);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                dataResponse = JsonSerializer.Deserialize<List<AccountDto>>(content, _serializerOptions);
            }

            return dataResponse;
        }
        catch (Exception ex)
        {
            Debug.WriteLine(@"\tERROR {0}", ex.Message);
            return null;
        }
    }

    public async Task<List<AccountDto>> GetActiveAccountList()
    {
        var uri = new Uri(string.Format(_utilityServices.GetRestUrl() + $"api/Account/GetAccountList", string.Empty));
        var dataResponse = new List<AccountDto>();

        try
        {
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", _accessService._apiToken);
            var response = await _httpClient.GetAsync(uri);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                dataResponse = JsonSerializer.Deserialize<List<AccountDto>>(content, _serializerOptions);
            }

            return dataResponse;
        }
        catch (Exception ex)
        {
            Debug.WriteLine(@"\tERROR {0}", ex.Message);
            return null;
        }
    }

    public async Task<AccountDto> GetAccount(int id)
    {
        var uri = new Uri(string.Format(_utilityServices.GetRestUrl() + $"api/Account/GetAccount/{id}", string.Empty));
        var dataResponse = new AccountDto();

        try
        {
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", _accessService._apiToken);

            var response = await _httpClient.GetAsync(uri);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                dataResponse = JsonSerializer.Deserialize<AccountDto>(content, _serializerOptions);
            }

            return dataResponse;
        }
        catch (Exception ex)
        {
            Debug.WriteLine(@"\tERROR {0}", ex.Message);
            return null;
        }
    }

    public async Task<AccountDto> UpdateAccount(AccountDto item)
    {
        var uri = new Uri(string.Format(_utilityServices.GetRestUrl() + $"api/Account/UpdateAccount", string.Empty));

        try
        {
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", _accessService._apiToken);

            var response = await _httpClient.PutAsJsonAsync(uri, item);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var dataResponse = JsonSerializer.Deserialize<AccountDto>(content, _serializerOptions);
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

    public async Task<AccountDto> AddAccount(AccountDto item)
    {
        var uri = new Uri(string.Format(_utilityServices.GetRestUrl() + $"api/Account/AddAccount", string.Empty));

        try
        {
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", _accessService._apiToken);

            var response = await _httpClient.PostAsJsonAsync(uri, item);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var dataResponse = JsonSerializer.Deserialize<AccountDto>(content, _serializerOptions);
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

    public async Task<AccountDto> DeleteAccount(AccountDto item)
    {
        var uri = new Uri(string.Format(_utilityServices.GetRestUrl() + $"api/Account/DeleteAccount", string.Empty));

        try
        {
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", _accessService._apiToken);

            var response = await _httpClient.PutAsJsonAsync(uri, item);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var dataResponse = JsonSerializer.Deserialize<AccountDto>(content, _serializerOptions);
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