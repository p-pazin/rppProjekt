using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using CarchiveAPI.Dto;
using Newtonsoft.Json;

namespace ServiceLayer.Services
{
    public class ReservationService
    {
        private readonly HttpClient _httpClient;
        private readonly TokenManager _tokenManager;

        public ReservationService()
        {
            _httpClient = new HttpClient { BaseAddress = new Uri(Environment.BASE_URL) };
            _tokenManager = new TokenManager();
        }

        public async Task<List<ReservationDto>> GetReservationsAsync()
        {
            string token = _tokenManager.GetToken();
            if (string.IsNullOrEmpty(token))
                throw new Exception("User is not logged in.");

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage response = await _httpClient.GetAsync("Reservation");
            if (!response.IsSuccessStatusCode)
                throw new Exception("Failed to fetch reservations data.");

            string json = await response.Content.ReadAsStringAsync();
            return System.Text.Json.JsonSerializer.Deserialize<List<ReservationDto>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
    }
}
