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

        public async Task<List<VehicleDto>> GetVehicles()
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
    }
}
