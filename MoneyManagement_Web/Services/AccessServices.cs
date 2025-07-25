using System.Diagnostics;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using MoneyManagement_Web.Data;
using MoneyManagement_Web.Interfaces;

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

    public AccessServices(IUtilityServices utilityServices)
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
    }

    public async Task<AuthResponse> Login(AuthRequest authRequest)
    {
        var uri = new Uri(string.Format(_utilityServices.GetRestUrl() + $"api/access/login", string.Empty));

        try
        {
            var response = await _httpClient.PostAsJsonAsync(uri, authRequest);


            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var dataResponse = JsonSerializer.Deserialize<AuthResponse>(content, _serializerOptions);
                _apiToken = dataResponse.Token;
                LoggedInfo = new RegistrationRequest { Username = dataResponse.Username, Email = dataResponse.Email };
                IsAuthorized = true;

                await _utilityServices.WriteLog(string.Concat(response.StatusCode.ToString(), " - User: '",
                    dataResponse.Username, "' Access granted"));
                return dataResponse;
            }
            else
            {
                var content = await response.Content.ReadAsStringAsync();
                if (content == "Bad credential")
                    content = "Bad credential (user)";
                else if (content == "Bad credentials") content = "Bad credential (password)";

                await _utilityServices.WriteLog(string.Concat(response.StatusCode.ToString(), " - ", content));
                return null;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(@"\tERROR {0}", ex.Message);

            return null;
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