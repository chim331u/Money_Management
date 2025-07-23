using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using MoneyManagement_Web.Data.Salary;
using MoneyManagement_Web.Interfaces;

namespace MoneyManagement_Web.Services
{
    public class TestServices : ITestServices
    {
        HttpClient _httpClient;
        JsonSerializerOptions _serializerOptions;
        private readonly IConfiguration _config;
        private readonly IUtilityServices _utilityServices;

        IAccessServices _accessService;

        public TestServices(IConfiguration config, IAccessServices accessService, IUtilityServices utilityServices)
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

            _config = config;
            _accessService = accessService;
            _utilityServices = utilityServices;
        }

        public async Task<string> UploadFile(MultipartFormDataContent item)
        {

            try
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessService._apiToken);

                //var client = new HttpClient();
                var request = new HttpRequestMessage(HttpMethod.Post, string.Format(_utilityServices.GetRestUrl() + $"api/Test/PostFile"));

                request.Content = item;
                var _response = await _httpClient.SendAsync(request);
                _response.EnsureSuccessStatusCode();
                var result = await _response.Content.ReadAsStringAsync();
                return result;

            }
            catch (Exception ex)
            {
                Console.WriteLine(@"\tERROR {0}", ex.Message);

                return null;
            }
        }        
        
        public async Task<string> PostFileSalary(SalaryWithFile salaryFile)
        {
            Uri uri = new Uri(string.Format(_utilityServices.GetRestUrl() + $"api/Test/PostFileSalary", string.Empty));

            try
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessService._apiToken);
                HttpResponseMessage response = await _httpClient.PostAsJsonAsync(uri, salaryFile);

                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    
                    return content;
                }

                return null;

            }
            catch (Exception ex)
            {
                Console.WriteLine(@"\tERROR {0}", ex.Message);

                return null;
            }


            //try
            //{
            //    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessService._apiToken);

            //    //var client = new HttpClient();
            //    var request = new HttpRequestMessage(HttpMethod.Post, string.Format(_utilityServices.GetRestUrl() + $"api/Test/PostFileSalary"));

            //    request.Content = salaryFile;
            //    var _response = await _httpClient.SendAsync(request);
            //    _response.EnsureSuccessStatusCode();
            //    var result = await _response.Content.ReadAsStringAsync();
            //    return result;

            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine(@"\tERROR {0}", ex.Message);

            //    return null;
            //}
        }
    }
}
