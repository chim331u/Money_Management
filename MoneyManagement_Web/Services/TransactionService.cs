using System.Diagnostics;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using MoneyManagement_Data.DTOs;
using MoneyManagement_Web.Interfaces;

namespace MoneyManagement_Web.Services
{
    public class TransactionService: ITransactionService
    {
        HttpClient _httpClient;
        JsonSerializerOptions _serializerOptions;
        private readonly IUtilityServices _utilityServices;
        IAccessServices _accessService;

        public TransactionService(IUtilityServices utilityService, IAccessServices accessService)
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

        #region TransactionDto

        public async Task<List<TransactionDto>> GetActiveTransactionList()
        {
            Uri uri = new Uri(string.Format(_utilityServices.GetRestUrl() + $"api/TransactionDto/GetTransactionDtoList", string.Empty));
            var dataResponse = new List<TransactionDto>();

            try
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessService._apiToken);
                HttpResponseMessage response = await _httpClient.GetAsync(uri);

                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    dataResponse = JsonSerializer.Deserialize<List<TransactionDto>>(content, _serializerOptions);
                }

                return dataResponse;

            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"\tERROR {0}", ex.Message);
                return null;
            }
        }

        public async Task<TransactionDto> GetTransaction(int id)
        {
            Uri uri = new Uri(string.Format(_utilityServices.GetRestUrl() + $"api/TransactionDto/GetTransactionDto/{id}", string.Empty));
            var dataResponse = new TransactionDto();

            try
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessService._apiToken);
                HttpResponseMessage response = await _httpClient.GetAsync(uri);

                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    dataResponse = JsonSerializer.Deserialize<TransactionDto>(content, _serializerOptions);
                }

                return dataResponse;

            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"\tERROR {0}", ex.Message);
                return null;
            }
        }

        public async Task<TransactionDto> UpdateTransaction(TransactionDto item)
        {
            Uri uri = new Uri(string.Format(_utilityServices.GetRestUrl() + $"api/TransactionDto/UpdateTransactionDto", string.Empty));

            try
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessService._apiToken);
                HttpResponseMessage response = await _httpClient.PutAsJsonAsync(uri, item);

                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    var dataResponse = JsonSerializer.Deserialize<TransactionDto>(content, _serializerOptions);
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
        
        public async Task<TransactionDto> CategoryConfirmed(TransactionDto item)
        {
            Uri uri = new Uri(string.Format(_utilityServices.GetRestUrl() + $"api/TransactionDto/CategoryConfirmed", string.Empty));

            try
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessService._apiToken);
                HttpResponseMessage response = await _httpClient.PutAsJsonAsync(uri, item);

                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    var dataResponse = JsonSerializer.Deserialize<TransactionDto>(content, _serializerOptions);
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

        public async Task<TransactionDto> AddTransaction(TransactionDto item)
        {
            Uri uri = new Uri(string.Format(_utilityServices.GetRestUrl() + $"api/TransactionDto/AddTransactionDto", string.Empty));

            try
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessService._apiToken);
                HttpResponseMessage response = await _httpClient.PostAsJsonAsync(uri, item);

                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    var dataResponse = JsonSerializer.Deserialize<TransactionDto>(content, _serializerOptions);
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
        
        public async Task<string> UploadCsv(IList<TransactionDto> TransactionDtos)
        {
            Uri uri = new Uri(string.Format(_utilityServices.GetRestUrl() + $"api/TransactionDto/UploadCsv", string.Empty));

            try
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessService._apiToken);
                HttpResponseMessage response = await _httpClient.PostAsJsonAsync(uri, TransactionDtos);

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

        public async Task<TransactionDto> DeleteTransaction(TransactionDto item)
        {

            Uri uri = new Uri(string.Format(_utilityServices.GetRestUrl() + $"api/TransactionDto/DeleteTransactionDto", string.Empty));

            try
            {

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessService._apiToken);
                HttpResponseMessage response = await _httpClient.PutAsJsonAsync(uri, item);

                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    var dataResponse = JsonSerializer.Deserialize<TransactionDto>(content, _serializerOptions);
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

        #region ML

        public async Task<string> TrainModel()
        {
            Uri uri = new Uri(string.Format(_utilityServices.GetRestUrl() + $"api/TransactionDto/TrainModelTransactionDto", string.Empty));

            try
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessService._apiToken);
                HttpResponseMessage response = await _httpClient.PutAsJsonAsync(uri, string.Empty);

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
        public async Task<string> CategorizeAllTransaction()
        {
            Uri uri = new Uri(string.Format(_utilityServices.GetRestUrl() + $"api/TransactionDto/CategorizeAllTransactionDto", string.Empty));

            try
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessService._apiToken);
                HttpResponseMessage response = await _httpClient.PutAsJsonAsync(uri, string.Empty);

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


        #endregion
    }
}
