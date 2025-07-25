using System.Diagnostics;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using MoneyManagement_Web.Data.IdentityAccess;
using MoneyManagement_Web.Interfaces;

namespace MoneyManagement_Web.Services;

public class IdentityAccessService : IIdentityAccessService
{
    private HttpClient _httpClient;
    private JsonSerializerOptions _serializerOptions;
    private readonly IUtilityServices _utilityServices;
    private IAccessServices _accessService;

    public IdentityAccessService(IUtilityServices utilityService, IAccessServices accessService)
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
        _accessService = accessService;
    }

    public async Task<List<ISA_Accounts>> GetActiveIdentityAccountList()
    {
        var uri = new Uri(string.Format(_utilityServices.GetRestUrl() + $"api/IdentiyAccess/GetIdentityAccessList",
            string.Empty));
        var dataResponse = new List<ISA_Accounts>();

        try
        {
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", _accessService._apiToken);
            var response = await _httpClient.GetAsync(uri);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                dataResponse = JsonSerializer.Deserialize<List<ISA_Accounts>>(content, _serializerOptions);
            }

            return dataResponse;
        }
        catch (Exception ex)
        {
            Debug.WriteLine(@"\tERROR {0}", ex.Message);
            return null;
        }
    }

    public async Task<ISA_Accounts> GetIdentityAccount(int identityAccountId)
    {
        var uri = new Uri(string.Format(
            _utilityServices.GetRestUrl() + $"api/IdentiyAccess/GetIdentityAccess/{identityAccountId}", string.Empty));
        var dataResponse = new ISA_Accounts();

        try
        {
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", _accessService._apiToken);
            var response = await _httpClient.GetAsync(uri);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                dataResponse = JsonSerializer.Deserialize<ISA_Accounts>(content, _serializerOptions);
            }

            return dataResponse;
        }
        catch (Exception ex)
        {
            Debug.WriteLine(@"\tERROR {0}", ex.Message);
            return null;
        }
    }

    public async Task<ISA_Accounts> AddIdentityAccount(ISA_Accounts identityAccount)
    {
        var uri = new Uri(string.Format(_utilityServices.GetRestUrl() + $"api/IdentiyAccess/AddIdentityAccess",
            string.Empty));

        try
        {
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", _accessService._apiToken);
            var response = await _httpClient.PostAsJsonAsync(uri, identityAccount);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var dataResponse = JsonSerializer.Deserialize<ISA_Accounts>(content, _serializerOptions);
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

    public async Task<ISA_Accounts> UpdateIdentityAccount(ISA_Accounts identityAccount)
    {
        var uri = new Uri(string.Format(_utilityServices.GetRestUrl() + $"api/IdentiyAccess/UpdateIdentityAccess",
            string.Empty));

        try
        {
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", _accessService._apiToken);
            var response = await _httpClient.PutAsJsonAsync(uri, identityAccount);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var dataResponse = JsonSerializer.Deserialize<ISA_Accounts>(content, _serializerOptions);
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

    public async Task<ISA_Accounts> DeleteIdentityAccount(ISA_Accounts identityAccount)
    {
        var uri = new Uri(string.Format(_utilityServices.GetRestUrl() + $"api/IdentiyAccess/DeleteIdentityAccess",
            string.Empty));

        try
        {
            identityAccount.IsActive = false;
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", _accessService._apiToken);
            var response = await _httpClient.PutAsJsonAsync(uri, identityAccount);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var dataResponse = JsonSerializer.Deserialize<ISA_Accounts>(content, _serializerOptions);
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

    public async Task<ICollection<ISA_PasswordsOld>> ListAllOldPasswords(int accountId)
    {
        var uri = new Uri(string.Format(
            _utilityServices.GetRestUrl() + $"api/IdentiyAccess/GetOldPasswordsList/{accountId}", string.Empty));
        var dataResponse = new List<ISA_PasswordsOld>();

        try
        {
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", _accessService._apiToken);
            var response = await _httpClient.GetAsync(uri);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                dataResponse = JsonSerializer.Deserialize<List<ISA_PasswordsOld>>(content, _serializerOptions);
            }

            return dataResponse;
        }
        catch (Exception ex)
        {
            Debug.WriteLine(@"\tERROR {0}", ex.Message);
            return null;
        }
    }

    public async Task<string> PasswordChange(ISA_Accounts item)
    {
        var uri = new Uri(string.Format(_utilityServices.GetRestUrl() + $"api/IdentiyAccess/PasswordChange",
            string.Empty));

        try
        {
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", _accessService._apiToken);
            var response = await _httpClient.PutAsJsonAsync(uri, item);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();

                return content;
            }

            return null;
        }
        catch (Exception ex)
        {
            Console.WriteLine(@"\tERROR {0}", ex.Message);

            return null;
        }
    }

    public async Task<string> GetCleanPsw(int id)
    {
        var uri = new Uri(string.Format(_utilityServices.GetRestUrl() + $"api/IdentiyAccess/GetCleanPsw/{id}",
            string.Empty));

        try
        {
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", _accessService._apiToken);
            var response = await _httpClient.GetAsync(uri);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return content;
            }

            return null;
        }
        catch (Exception ex)
        {
            Debug.WriteLine(@"\tERROR {0}", ex.Message);
            return null;
        }
    }
}