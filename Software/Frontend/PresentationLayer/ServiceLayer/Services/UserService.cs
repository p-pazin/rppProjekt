using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using ServiceLayer.Network.Dto;

namespace ServiceLayer.Services
{
    public class UserService
    {
        private readonly HttpClient _httpClient;
        private readonly TokenManager _tokenManager;
        private const string BASE_URL = "https://carchive.online/api/";

        public UserService()
        {
            _httpClient = new HttpClient { BaseAddress = new Uri(BASE_URL) };
            _tokenManager = new TokenManager();
        }

        public async Task<UserDto> GetCurrentUserAsync()
        {
            string token = _tokenManager.GetToken();
            if (string.IsNullOrEmpty(token))
                throw new Exception("User is not logged in.");

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage response = await _httpClient.GetAsync("User");
            if (!response.IsSuccessStatusCode)
                throw new Exception("Failed to fetch user data.");

            string json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<UserDto>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
    }
}
