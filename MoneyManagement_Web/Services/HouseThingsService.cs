using System.Diagnostics;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using MoneyManagement_Web.Data.HouseThings;
using MoneyManagement_Web.Interfaces;

namespace MoneyManagement_Web.Services
{
    public class HouseThingsService : IHouseThingsService
    {
        HttpClient _httpClient;
        JsonSerializerOptions _serializerOptions;
        private readonly IUtilityServices _utilityServices;
        IAccessServices _accessService;

        public HouseThingsService(IUtilityServices utilityService, IAccessServices accessServices)
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

        #region House Things        

        public async Task<List<HouseThings>> GetActiveHouseThingsList()
        {
            Uri uri = new Uri(string.Format(_utilityServices.GetRestUrl() + $"api/HouseThings/GetHouseThingsList", string.Empty));
            var dataResponse = new List<HouseThings>();

            try
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessService._apiToken);
                HttpResponseMessage response = await _httpClient.GetAsync(uri);

                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return null;
                }

                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    dataResponse = JsonSerializer.Deserialize<List<HouseThings>>(content, _serializerOptions);
                }

                return dataResponse;

            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"\tERROR {0}", ex.Message);
                return null;
            }
        }

        public async Task<List<HouseThings>> GetActiveHouseThingsListByRoom(int id)
        {
            Uri uri = new Uri(string.Format(_utilityServices.GetRestUrl() + $"api/HouseThings/GetHouseThingsListByRoom/{id}", string.Empty));
            var dataResponse = new List<HouseThings>();

            try
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessService._apiToken);
                HttpResponseMessage response = await _httpClient.GetAsync(uri);

                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return null;
                }

                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    dataResponse = JsonSerializer.Deserialize<List<HouseThings>>(content, _serializerOptions);
                }

                return dataResponse;

            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"\tERROR {0}", ex.Message);
                return null;
            }
        }

        public async Task<List<HouseThings>> GetHistoryHouseThingsList(int historyId)
        {
            Uri uri = new Uri(string.Format(_utilityServices.GetRestUrl() + $"api/HouseThings/GetHistoryHouseThingsList/{historyId}", string.Empty));
            var dataResponse = new List<HouseThings>();

            try
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessService._apiToken);
                HttpResponseMessage response = await _httpClient.GetAsync(uri);

                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return null;
                }

                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    dataResponse = JsonSerializer.Deserialize<List<HouseThings>>(content, _serializerOptions);
                }

                return dataResponse;

            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"\tERROR {0}", ex.Message);
                return null;
            }
        }
        
        public async Task<HouseThings> GetHouseThings(int houseThingsId)
        {
            Uri uri = new Uri(string.Format(_utilityServices.GetRestUrl() + $"api/HouseThings/GetHouseThings/{houseThingsId}", string.Empty));
            var dataResponse = new HouseThings();

            try
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessService._apiToken);
                HttpResponseMessage response = await _httpClient.GetAsync(uri);

                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    dataResponse = JsonSerializer.Deserialize<HouseThings>(content, _serializerOptions);
                }

                return dataResponse;

            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"\tERROR {0}", ex.Message);
                return null;
            }
        }

        public async Task<HouseThings> AddHouseThings(HouseThings houseThings)
        {
            Uri uri = new Uri(string.Format(_utilityServices.GetRestUrl() + $"api/HouseThings/AddHouseThings", string.Empty));

            try
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessService._apiToken);
                HttpResponseMessage response = await _httpClient.PostAsJsonAsync(uri, houseThings);

                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    var dataResponse = JsonSerializer.Deserialize<HouseThings>(content, _serializerOptions);
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
        
        public async Task<HouseThings> RenewHouseThings(HouseThings houseThings)
        {
            Uri uri = new Uri(string.Format(_utilityServices.GetRestUrl() + $"api/HouseThings/RenewHouseThings", string.Empty));

            try
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessService._apiToken);
                HttpResponseMessage response = await _httpClient.PostAsJsonAsync(uri, houseThings);

                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    var dataResponse = JsonSerializer.Deserialize<HouseThings>(content, _serializerOptions);
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

        public async Task<HouseThings> UpdateHouseThings(HouseThings houseThings)
        {
            Uri uri = new Uri(string.Format(_utilityServices.GetRestUrl() + $"api/HouseThings/UpdateHouseThings", string.Empty));

            try
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessService._apiToken);
                HttpResponseMessage response = await _httpClient.PutAsJsonAsync(uri, houseThings);

                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    var dataResponse = JsonSerializer.Deserialize<HouseThings>(content, _serializerOptions);
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

        public async Task<HouseThings> DeleteHouseThings(HouseThings houseThings)
        {
            Uri uri = new Uri(string.Format(_utilityServices.GetRestUrl() + $"api/HouseThings/DeleteHouseThings", string.Empty));

            try
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessService._apiToken);
                HttpResponseMessage response = await _httpClient.PutAsJsonAsync(uri, houseThings);

                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    var dataResponse = JsonSerializer.Deserialize<HouseThings>(content, _serializerOptions);
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

        #region House Things Room

        public async Task<List<HouseThingsRooms>> GetActiveHouseThingsRoomsList()
        {
            Uri uri = new Uri(string.Format(_utilityServices.GetRestUrl() + $"api/HouseThingsRooms/GetHouseThingsRoomsList", string.Empty));
            var dataResponse = new List<HouseThingsRooms>();

            try
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessService._apiToken);
                HttpResponseMessage response = await _httpClient.GetAsync(uri);

                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return null;
                }

                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    dataResponse = JsonSerializer.Deserialize<List<HouseThingsRooms>>(content, _serializerOptions);
                }

                return dataResponse.OrderBy(x => x.Name).ToList();

            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"\tERROR {0}", ex.Message);
                return null;
            }
        }

        public async Task<HouseThingsRooms> GetHouseThingsRooms(int houseThingsRoomsId)
        {
            Uri uri = new Uri(string.Format(_utilityServices.GetRestUrl() + $"api/HouseThingsRooms/GetHouseThingsRooms/{houseThingsRoomsId}", string.Empty));
            var dataResponse = new HouseThingsRooms();

            try
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessService._apiToken);
                HttpResponseMessage response = await _httpClient.GetAsync(uri);

                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    dataResponse = JsonSerializer.Deserialize<HouseThingsRooms>(content, _serializerOptions);
                }

                return dataResponse;

            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"\tERROR {0}", ex.Message);
                return null;
            }
        }

        public async Task<HouseThingsRooms> AddHouseThingsRooms(HouseThingsRooms houseThingsRooms)
        {
            Uri uri = new Uri(string.Format(_utilityServices.GetRestUrl() + $"api/HouseThingsRooms/AddHouseThingsRooms", string.Empty));

            try
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessService._apiToken);
                HttpResponseMessage response = await _httpClient.PostAsJsonAsync(uri, houseThingsRooms);

                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    var dataResponse = JsonSerializer.Deserialize<HouseThingsRooms>(content, _serializerOptions);
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

        public async Task<HouseThingsRooms> UpdateHouseThingsRooms(HouseThingsRooms houseThingsRooms)
        {
            Uri uri = new Uri(string.Format(_utilityServices.GetRestUrl() + $"api/HouseThingsRooms/UpdateHouseThingsRooms", string.Empty));

            try
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessService._apiToken);
                HttpResponseMessage response = await _httpClient.PutAsJsonAsync(uri, houseThingsRooms);

                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    var dataResponse = JsonSerializer.Deserialize<HouseThingsRooms>(content, _serializerOptions);
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

        public async Task<HouseThingsRooms> DeleteHouseThingsRooms(HouseThingsRooms houseThingsRooms)
        {
            Uri uri = new Uri(string.Format(_utilityServices.GetRestUrl() + $"api/HouseThingsRooms/DeleteHouseThingsRooms", string.Empty));

            try
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessService._apiToken);
                HttpResponseMessage response = await _httpClient.PutAsJsonAsync(uri, houseThingsRooms);

                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    var dataResponse = JsonSerializer.Deserialize<HouseThingsRooms>(content, _serializerOptions);
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
