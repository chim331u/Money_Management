using System.Diagnostics;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using MoneyManagement_Web.Data.Bill;
using MoneyManagement_Web.Interfaces;

namespace MoneyManagement_Web.Services
{
    public class BillService : IBillService
    {
        HttpClient _httpClient;
        JsonSerializerOptions _serializerOptions;
        private readonly IUtilityServices _utilityServices;
        IAccessServices _accessService;

        public BillService(IUtilityServices utilityService, IAccessServices accessService)
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

        #region Bill

        public async Task<List<Bill>> GetActiveBillList()
        {
            Uri uri = new Uri(string.Format(_utilityServices.GetRestUrl() + $"api/Bill/GetBillList", string.Empty));
            var dataResponse = new List<Bill>();

            try
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessService._apiToken);
                HttpResponseMessage response = await _httpClient.GetAsync(uri);

                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    dataResponse = JsonSerializer.Deserialize<List<Bill>>(content, _serializerOptions);
                }

                return dataResponse;

            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"\tERROR {0}", ex.Message);
                return null;
            }
        }

        public async Task<Bill> GetBill(int id)
        {
            Uri uri = new Uri(string.Format(_utilityServices.GetRestUrl() + $"api/Bill/GetBill/{id}", string.Empty));
            var dataResponse = new Bill();

            try
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessService._apiToken);
                HttpResponseMessage response = await _httpClient.GetAsync(uri);

                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    dataResponse = JsonSerializer.Deserialize<Bill>(content, _serializerOptions);
                }

                return dataResponse;

            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"\tERROR {0}", ex.Message);
                return null;
            }
        }

        public async Task<Bill> UpdateBill(Bill item)
        {
            Uri uri = new Uri(string.Format(_utilityServices.GetRestUrl() + $"api/Bill/UpdateBill", string.Empty));

            try
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessService._apiToken);
                HttpResponseMessage response = await _httpClient.PutAsJsonAsync(uri, item);

                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    var dataResponse = JsonSerializer.Deserialize<Bill>(content, _serializerOptions);
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

        public async Task<Bill> CategoryConfirmed(Bill item)
        {
            Uri uri = new Uri(string.Format(_utilityServices.GetRestUrl() + $"api/Bill/CategoryConfirmed", string.Empty));

            try
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessService._apiToken);
                HttpResponseMessage response = await _httpClient.PutAsJsonAsync(uri, item);

                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    var dataResponse = JsonSerializer.Deserialize<Bill>(content, _serializerOptions);
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

        public async Task<Bill> AddBill(Bill item)
        {
            Uri uri = new Uri(string.Format(_utilityServices.GetRestUrl() + $"api/Bill/AddBill", string.Empty));

            try
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessService._apiToken);
                HttpResponseMessage response = await _httpClient.PostAsJsonAsync(uri, item);

                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    var dataResponse = JsonSerializer.Deserialize<Bill>(content, _serializerOptions);
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

        public async Task<string> UploadCsv(IList<Bill> bills)
        {
            Uri uri = new Uri(string.Format(_utilityServices.GetRestUrl() + $"api/Bill/UploadCsv", string.Empty));

            try
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessService._apiToken);
                HttpResponseMessage response = await _httpClient.PostAsJsonAsync(uri, bills);

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
        }

        public async Task<Bill> DeleteBill(Bill item)
        {

            Uri uri = new Uri(string.Format(_utilityServices.GetRestUrl() + $"api/Bill/DeleteBill", string.Empty));

            try
            {

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessService._apiToken);
                HttpResponseMessage response = await _httpClient.PutAsJsonAsync(uri, item);

                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    var dataResponse = JsonSerializer.Deserialize<Bill>(content, _serializerOptions);
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

        public async Task<string> UploadFile(MultipartFormDataContent item, int id)
        {

            try
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessService._apiToken);

                //var client = new HttpClient();
                var request = new HttpRequestMessage(HttpMethod.Post, string.Format(_utilityServices.GetRestUrl() + $"api/Bill/UploadBill/{id}"));

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

        #endregion
    }
}
