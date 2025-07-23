using System.Diagnostics;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using MoneyManagement_Web.Data.Salary;
using MoneyManagement_Web.Interfaces;

namespace MoneyManagement_Web.Services
{
    public class SalaryService : ISalaryService
    {

        HttpClient _httpClient;
        JsonSerializerOptions _serializerOptions;
        private readonly IUtilityServices _utilityServices;
        IAccessServices _accessService;

        public SalaryService(IUtilityServices utilityService, IAccessServices accessService)
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

        #region Salary

        public async Task<List<Salary>> GetActiveSalaryList()
        {
            Uri uri = new Uri(string.Format(_utilityServices.GetRestUrl() + $"api/Salary/GetSalaryList", string.Empty));
            var dataResponse = new List<Salary>();

            try
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessService._apiToken);
                HttpResponseMessage response = await _httpClient.GetAsync(uri);

                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    dataResponse = JsonSerializer.Deserialize<List<Salary>>(content, _serializerOptions);
                }

                return dataResponse;

            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"\tERROR {0}", ex.Message);
                return null;
            }
        }

        public async Task<Salary> GetSalary(int id)
        {
            Uri uri = new Uri(string.Format(_utilityServices.GetRestUrl() + $"api/Salary/GetSalary/{id}", string.Empty));
            var dataResponse = new Salary();

            try
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessService._apiToken);
                HttpResponseMessage response = await _httpClient.GetAsync(uri);

                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    dataResponse = JsonSerializer.Deserialize<Salary>(content, _serializerOptions);
                }

                return dataResponse;

            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"\tERROR {0}", ex.Message);
                return null;
            }
        }

        public async Task<Salary> UpdateSalary(Salary item)
        {
            Uri uri = new Uri(string.Format(_utilityServices.GetRestUrl() + $"api/Salary/UpdateSalary", string.Empty));

            try
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessService._apiToken);
                HttpResponseMessage response = await _httpClient.PutAsJsonAsync(uri, item);

                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    var dataResponse = JsonSerializer.Deserialize<Salary>(content, _serializerOptions);
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

        public async Task<Salary> AddSalary(Salary item)
        {
            Uri uri = new Uri(string.Format(_utilityServices.GetRestUrl() + $"api/Salary/AddSalary", string.Empty));

            try
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessService._apiToken);
                HttpResponseMessage response = await _httpClient.PostAsJsonAsync(uri, item);

                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    var dataResponse = JsonSerializer.Deserialize<Salary>(content, _serializerOptions);
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

        public async Task<Salary> DeleteSalary(Salary item)
        {

            Uri uri = new Uri(string.Format(_utilityServices.GetRestUrl() + $"api/Salary/DeleteSalary", string.Empty));

            try
            {
                item.IsActive = false;
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessService._apiToken);
                HttpResponseMessage response = await _httpClient.PutAsJsonAsync(uri, item);

                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    var dataResponse = JsonSerializer.Deserialize<Salary>(content, _serializerOptions);
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
}