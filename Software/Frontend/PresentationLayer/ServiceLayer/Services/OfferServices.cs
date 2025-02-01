using CarchiveAPI.Dto;
using ServiceLayer.Network.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ServiceLayer.Services
{
    public class OfferServices
    {
        private readonly HttpClient _httpClient;
        private readonly TokenManager _tokenManager;
        private const string BASE_URL = "https://carchive.online/api/";

        public OfferServices()
        {
            _httpClient = new HttpClient { BaseAddress = new Uri(BASE_URL) };
            _tokenManager = new TokenManager();
        }

        public async Task<List<OfferDto>> GetOffers()
        {
            string token = _tokenManager.GetToken();
            if (string.IsNullOrEmpty(token))
                throw new Exception("User is not logged in.");

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage response = await _httpClient.GetAsync("Offer");

            if (!response.IsSuccessStatusCode)
                throw new Exception("Failed to fetch offer data.");

            string json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<OfferDto>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        public async Task<List<VehicleDto>> GetVehiclesForOffer(int offerId)
        {
            string token = _tokenManager.GetToken();
            if (string.IsNullOrEmpty(token))
                throw new Exception("User is not logged in.");

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage response = await _httpClient.GetAsync($"Vehicle/offer/{offerId}");

            if (!response.IsSuccessStatusCode)
                throw new Exception("Failed to fetch offer data.");
            string json = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<List<VehicleDto>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        public async Task<ContactDto> GetContactForOffer(int offerId)
        {
            string token = _tokenManager.GetToken();
            if (string.IsNullOrEmpty(token))
                throw new Exception("User is not logged in.");

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage response = await _httpClient.GetAsync($"Contact/contacts/{offerId}");

            if (!response.IsSuccessStatusCode)
                throw new Exception("Failed to fetch offer data.");

            string json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<ContactDto>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        public async Task PostOffer(OfferDto offer, List<VehicleDto> vehicles, ContactDto contact)
        {
            string token = _tokenManager.GetToken();
            if (string.IsNullOrEmpty(token))
                throw new Exception("User is not logged in.");

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            string json = JsonSerializer.Serialize(offer);

            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

            List<int> vehicleIds = vehicles.Select(v => v.Id).ToList();

            string requestUri = $"Offer?contactId={contact.Id}&{string.Join("&", vehicleIds.Select(id => $"vehiclesId={id}"))}";

            HttpResponseMessage response = await _httpClient.PostAsync(requestUri, content);

            if (!response.IsSuccessStatusCode)
                throw new Exception("Failed to add offer data.");
        }


        public async Task PutOffer(OfferDto offer, List<VehicleDto> vehicles, ContactDto contact)
        {
            string token = _tokenManager.GetToken();
            if (string.IsNullOrEmpty(token))
                throw new Exception("User is not logged in.");

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            string json = JsonSerializer.Serialize(offer);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

            List<int> vehicleIds = vehicles.Select(v => v.Id).ToList();

            string queryString = $"?contactId={contact.Id}&vehiclesId={string.Join("&vehiclesId=", vehicleIds)}";

            HttpResponseMessage response = await _httpClient.PutAsync($"Offer{queryString}", content);

            if (!response.IsSuccessStatusCode)
                throw new Exception("Failed to update offer data.");
        }

        public async void DeleteOffer(int offerId)
        {
            string token = _tokenManager.GetToken();
            if (string.IsNullOrEmpty(token))
                throw new Exception("User is not logged in.");

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage response = await _httpClient.DeleteAsync($"Offer/{offerId}");

            if (!response.IsSuccessStatusCode)
                throw new Exception("Failed to delete offer data.");
        }
    }
}
