using ServiceLayer.Network.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ServiceLayer.Services
{
    public class VehicleService
    {
        private readonly HttpClient _httpClient;
        private readonly TokenManager _tokenManager;
        private const string BASE_URL = "https://carchive.online/api/";

        public VehicleService()
        {
            _httpClient = new HttpClient { BaseAddress = new Uri(BASE_URL) };
            _tokenManager = new TokenManager();
        }

        public async Task<List<VehicleDto>> GetNotDeletedVehicles()
        {
            string token = _tokenManager.GetToken();
            if (string.IsNullOrEmpty(token))
                throw new Exception("User is not logged in.");

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage response = await _httpClient.GetAsync("Vehicle/catalog");
            if (!response.IsSuccessStatusCode)
                throw new Exception("Failed to fetch vehicle data.");

            string json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<VehicleDto>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        public async Task<List<VehicleDto>> GetVehicles()
        {
            string token = _tokenManager.GetToken();
            if (string.IsNullOrEmpty(token))
                throw new Exception("User is not logged in.");

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage response = await _httpClient.GetAsync("Vehicle");
            if (!response.IsSuccessStatusCode)
                throw new Exception("Failed to fetch vehicle data.");

            string json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<VehicleDto>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        public async Task<List<VehicleDto>> GetVehiclesRent()
        {
            string token = _tokenManager.GetToken();
            if (string.IsNullOrEmpty(token))
                throw new Exception("User is not logged in.");

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage response = await _httpClient.GetAsync("Vehicle/rent");
            if (!response.IsSuccessStatusCode)
                throw new Exception("Failed to fetch vehicle data.");

            string json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<VehicleDto>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        public async Task<List<VehicleDto>> GetVehiclesSale()
        {
            string token = _tokenManager.GetToken();
            if (string.IsNullOrEmpty(token))
                throw new Exception("User is not logged in.");

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage response = await _httpClient.GetAsync("Vehicle/sale");
            if (!response.IsSuccessStatusCode)
                throw new Exception("Failed to fetch vehicle data.");

            string json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<VehicleDto>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        public async Task PostVehicle(VehiclePost newVehicle)
        {
            string token = _tokenManager.GetToken();
            if (string.IsNullOrEmpty(token))
                throw new Exception("User is not logged in.");

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            string jsonContent = JsonSerializer.Serialize(newVehicle);
            HttpContent content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PostAsync("Vehicle", content);

            if (!response.IsSuccessStatusCode)
                Console.WriteLine(response.StatusCode);
            throw new Exception("Failed to add vehicle.");
        }

        public async Task PutVehicle(VehicleDto vehicleToUpdate)
        {
            string token = _tokenManager.GetToken();
            if (string.IsNullOrEmpty(token))
                throw new Exception("User is not logged in.");

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            string jsonContent = JsonSerializer.Serialize(vehicleToUpdate);

            HttpContent content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var vehicleId = vehicleToUpdate.Id;

            HttpResponseMessage response = await _httpClient.PutAsync($"Vehicle/{vehicleId}", content);
            if (!response.IsSuccessStatusCode)
                throw new Exception("Failed to update vehicle.");
        }

        public async Task DeleteVehicle(int vehicleId)
        {
            string token = _tokenManager.GetToken();

            if (string.IsNullOrEmpty(token))
                throw new Exception("User is not logged in.");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage response = await _httpClient.DeleteAsync($"Vehicle/{vehicleId}");

            if (!response.IsSuccessStatusCode)
                throw new Exception("Failed to delete vehicle.");
        }

    }
}
