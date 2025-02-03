using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using ServiceLayer.Network.Dto;

namespace ServiceLayer.Services
{
    public class PenaltyService
    {
        private readonly HttpClient _httpClient;
        private readonly TokenManager _tokenManager;

        public PenaltyService()
        {
            _httpClient = new HttpClient { BaseAddress = new Uri(Environment.BASE_URL) };
            _tokenManager = new TokenManager();
        }

        public async Task<List<PenaltyDto>> GetPenaltiesAsync()
        {
            string token = _tokenManager.GetToken();
            if (string.IsNullOrEmpty(token))
                throw new Exception("User is not logged in.");

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage response = await _httpClient.GetAsync("Penalty");
            if (!response.IsSuccessStatusCode)
                throw new Exception("Failed to fetch penalties.");

            string json = await response.Content.ReadAsStringAsync();
            return System.Text.Json.JsonSerializer.Deserialize<List<PenaltyDto>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
    }
}

