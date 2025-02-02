using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using CarchiveAPI.Dto;

namespace ServiceLayer.Services
{
    public class StatsService
    {
        private readonly HttpClient _httpClient;
        private readonly TokenManager _tokenManager;

        public StatsService()
        {
            _httpClient = new HttpClient { BaseAddress = new Uri(Environment.BASE_URL) };
            _tokenManager = new TokenManager();
        }
        public async Task<ContactStatusStatsDto> GetContactStatusStatsAsync()
        {
            string token = _tokenManager.GetToken();
            if (string.IsNullOrEmpty(token))
                throw new Exception("User is not logged in.");

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage response = await _httpClient.GetAsync("Stats/ContactStatus");
            if (!response.IsSuccessStatusCode)
                throw new Exception("Failed to fetch contact status stats data.");

            string json = await response.Content.ReadAsStringAsync();
            return System.Text.Json.JsonSerializer.Deserialize<ContactStatusStatsDto>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
        public async Task<YearlyInfoDto> GetContactCreationStatsAsync()
        {
            string token = _tokenManager.GetToken();
            if (string.IsNullOrEmpty(token))
                throw new Exception("User is not logged in.");

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage response = await _httpClient.GetAsync("Stats/ContactCreation");
            if (!response.IsSuccessStatusCode)
                throw new Exception("Failed to fetch contact creation stats data.");

            string json = await response.Content.ReadAsStringAsync();
            return System.Text.Json.JsonSerializer.Deserialize<YearlyInfoDto>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
        public async Task<YearlyInfoDto> GetInvoiceCreationStatsAsync()
        {
            string token = _tokenManager.GetToken();
            if (string.IsNullOrEmpty(token))
                throw new Exception("User is not logged in.");

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage response = await _httpClient.GetAsync("Stats/InvoiceCreation");
            if (!response.IsSuccessStatusCode)
                throw new Exception("Failed to fetch invoice creation stats data.");

            string json = await response.Content.ReadAsStringAsync();
            return System.Text.Json.JsonSerializer.Deserialize<YearlyInfoDto>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
    }
}
