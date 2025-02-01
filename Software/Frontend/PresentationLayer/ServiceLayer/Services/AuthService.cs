using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using ServiceLayer.Network.Dto;

namespace ServiceLayer.Services
{
    public class AuthService
    {
        private readonly HttpClient _httpClient;
        private readonly TokenManager _tokenManager = new TokenManager();

        public AuthService()
        {
            _httpClient = new HttpClient { BaseAddress = new Uri(Environment.BASE_URL) }; ;
        }

        public async Task<string> LoginAsync(string email, string password)
        {
            var url = "User/login";
            var loginData = new { email, password };
            var json = JsonSerializer.Serialize(loginData, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(url, content);

            if (response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                var responseJson = JsonSerializer.Deserialize<LoginResponse>(responseBody);
                return responseJson?.token ?? string.Empty;
            }
            return string.Empty;
        }

        public async Task<bool> RegisterAsync(NewCompanyDto newCompany)
        {
            var url = "Company";
            var json = JsonSerializer.Serialize(newCompany, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(url, content);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> ChangePasswordAsync(NewPasswordDto passwordsData)
        {
            string token = _tokenManager.GetToken();
            if (string.IsNullOrEmpty(token))
                throw new Exception("User is not logged in.");

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var url = "User/newpassword";

            string jsonContent = JsonSerializer.Serialize(passwordsData);
            HttpContent content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PutAsync(url, content);

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
                throw new Exception("Failed to change password.");
            }
            return true;
        }

    }

    internal class LoginResponse
    {
        public string token { get; set; }
    }
}

