using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using ServiceLayer.Network.Dto;

namespace ServiceLayer.Services
{
    public class UserService
    {
        private readonly HttpClient _httpClient;
        private readonly TokenManager _tokenManager;

        public UserService()
        {
            _httpClient = new HttpClient { BaseAddress = new Uri(Environment.BASE_URL) };
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

        public async Task PutCompanyUser(UserDto companyUserInfo)
        {
            string token = _tokenManager.GetToken();
            if (string.IsNullOrEmpty(token))
                throw new Exception("User is not logged in.");

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var url = "User/update";

            string jsonContent = JsonSerializer.Serialize(companyUserInfo);
            HttpContent content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PutAsync(url, content);

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
                throw new Exception("Failed to add new company user.");
            }
        }

        public async Task DeleteCompanyUser(int userId)
        {
            string token = _tokenManager.GetToken();
            if (string.IsNullOrEmpty(token))
                throw new Exception("User is not logged in.");

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage response = await _httpClient.DeleteAsync($"User/delete/{userId}");

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
                throw new Exception("Failed to add new company user.");
            }
        }
    }
}
