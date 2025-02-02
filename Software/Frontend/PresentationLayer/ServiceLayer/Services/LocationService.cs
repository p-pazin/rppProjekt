using ServiceLayer.Network.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ServiceLayer.Services
{
    public class LocationService
    {
        private readonly HttpClient _httpClient;
        private readonly TokenManager _tokenManager;
        private const string BASE_URL = "https://carchive.online/api/";

        public LocationService()
        {
            _httpClient = new HttpClient { BaseAddress = new Uri(BASE_URL) };
            _tokenManager = new TokenManager();
        }

        public async Task<List<LocationDto>> GetLocations()
        {
            string token = _tokenManager.GetToken();
            if (string.IsNullOrEmpty(token))
                throw new Exception("User is not logged in.");

            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage response = await _httpClient.GetAsync("Location");

            if (!response.IsSuccessStatusCode)
                throw new Exception("Failed to fetch location data.");

            string json = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<List<LocationDto>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

    }
}
