using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using CarchiveAPI.Dto;
using ServiceLayer.Network.Dto;

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
            return JsonSerializer.Deserialize<List<ReservationDto>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        public async Task PostReservationsAsync(ReservationDto newReservation)
        {
            string token = _tokenManager.GetToken();
            if (string.IsNullOrEmpty(token))
                throw new Exception("User is not logged in.");

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            string url = $"Reservation?contactid={newReservation.ContactId}&vehicleId={newReservation.VehicleId}";

            string jsonContent = JsonSerializer.Serialize(newReservation);
            HttpContent content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PostAsync(url, content);

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
                throw new Exception("Failed to add new reservation.");
            }
        }

        public async Task PutReservationsAsync(ReservationDto newReservationInfo)
        {
            string token = _tokenManager.GetToken();
            if (string.IsNullOrEmpty(token))
                throw new Exception("User is not logged in.");

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            string url = $"Reservation/{newReservationInfo.Id}?vehicleId={newReservationInfo.VehicleId}";

            string jsonContent = JsonSerializer.Serialize(newReservationInfo);
            HttpContent content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PutAsync(url, content);

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
                throw new Exception("Failed to add new reservation.");
            }
        }

        public async Task DeleteReservationsAsync(int id)
        {
            string token = _tokenManager.GetToken();
            if (string.IsNullOrEmpty(token))
                throw new Exception("User is not logged in.");

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage response = await _httpClient.DeleteAsync($"Reservation/{id}");

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
                throw new Exception("Failed to delete reservation.");
            }
        }
    }
}
