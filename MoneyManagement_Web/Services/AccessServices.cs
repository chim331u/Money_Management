using System.Diagnostics;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using MoneyManagement_Data.DTOs;
using MoneyManagement_Web.Data;
using MoneyManagement_Web.Interfaces;
using LoginModelDto = MoneyManagement_Web.Data.LoginModelDto;

namespace MoneyManagement_Web.Services;

public class AccessServices : IAccessServices
{
    public bool IsAuthorized
    {
        get => isAuthorized;
        set
        {
            isAuthorized = value;
            NotifyStateChanged();
        }
    }

    public RegistrationRequest LoggedInfo { get; set; }
    private IConnectionService _connectionService;

    private bool isAuthorized;

    public event Action? OnChange;

    private void NotifyStateChanged()
    {
        OnChange?.Invoke();
    }

    private HttpClient _httpClient;
    private JsonSerializerOptions _serializerOptions;
    private readonly IUtilityServices _utilityServices;

    public string _apiToken { get; set; }

    public AccessServices(IUtilityServices utilityServices, IConnectionService connectionService)
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

        _utilityServices = utilityServices;
        _connectionService = connectionService;
    }

    public async Task<bool> Login(LoginModelDto authRequest)
    {
        
       // var uri = new Uri(string.Format(_utilityServices.GetRestUrl() + $"api/access/login", string.Empty));

        try
        {
            //var response = await _httpClient.PostAsJsonAsync(uri, authRequest);

            var loginResponse = await _connectionService.PostAsync<TokenModelDto>("api/v1/Login", authRequest);

            var dataResponse = loginResponse.Data;

            if (dataResponse != null)
            {
                
                _apiToken = dataResponse.AccessToken;
                LoggedInfo = new RegistrationRequest { Email = authRequest.Username };
                IsAuthorized = true;

                // await _utilityServices.WriteLog(string.Concat(" User: '",
                //     authRequest.Username, "' Access granted"));
                return true;
            }
            else
            {
                
                await _utilityServices.WriteLog("Failed to login, no data returned");
                return false;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(@"\tERROR {0}", ex.Message);

            return false;
        }
    }

    public async Task<string> Register(RegistrationRequest registrationRequest)
    {
        var uri = new Uri(string.Format(_utilityServices.GetRestUrl() + $"api/Access/register", string.Empty));

        try
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiToken);
            var response = await _httpClient.PostAsJsonAsync(uri, registrationRequest);

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

            return ex.Message;
        }
    }

    public async Task<string> AccessTest()
    {
        var uri = new Uri(string.Format(_utilityServices.GetRestUrl() + $"api/Access", string.Empty));


        try
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiToken);
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
            return ex.Message;
        }
    }
}