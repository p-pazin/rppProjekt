using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using ServiceLayer.Network.Dto;

namespace ServiceLayer.Services
{
    public class CompanyService
    {
        private readonly HttpClient _httpClient;
        private readonly TokenManager _tokenManager;

        public CompanyService()
        {
            _httpClient = new HttpClient { BaseAddress = new Uri(Environment.BASE_URL) };
            _tokenManager = new TokenManager();
        }

        public async Task<CompanyDto> GetCurrentCompanyAsync()
        {
            string token = _tokenManager.GetToken();
            if (string.IsNullOrEmpty(token))
                throw new Exception("User is not logged in.");

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage response = await _httpClient.GetAsync("Company");
            if (!response.IsSuccessStatusCode)
                throw new Exception("Failed to fetch company data.");

            string json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<CompanyDto>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
    }
}
