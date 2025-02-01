﻿using System;
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
    public class ContractService
    {
        private readonly HttpClient _httpClient;
        private readonly TokenManager _tokenManager;

        public ContractService()
        {
            _httpClient = new HttpClient { BaseAddress = new Uri(Environment.BASE_URL) };
            _tokenManager = new TokenManager();
        }

        public async Task<List<ContractDto>> GetContractsAsync()
        {
            string token = _tokenManager.GetToken();
            if (string.IsNullOrEmpty(token))
                throw new Exception("User is not logged in.");

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage response = await _httpClient.GetAsync("Contract");
            if (!response.IsSuccessStatusCode)
                throw new Exception("Failed to fetch contracts data.");

            string json = await response.Content.ReadAsStringAsync();
            return System.Text.Json.JsonSerializer.Deserialize<List<ContractDto>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
        public async Task PostContractSaleAsync(ContractDto newContract, int? contactId, int? vehicleId, int? offerId)
        {
            string token = _tokenManager.GetToken();
            if (string.IsNullOrEmpty(token))
                throw new Exception("User is not logged in.");

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            string jsonContent = JsonConvert.SerializeObject(newContract);
            HttpContent content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            string url = $"Contract/sell?contactId={contactId}&vehicleId={vehicleId}&offerId={offerId}";

            HttpResponseMessage response = await _httpClient.PostAsync(url, content);

            if (!response.IsSuccessStatusCode)
                throw new Exception("Failed to add contract.");
        }
        public async Task PostContractRentAsync(ContractDto newContract, int? reservationId, int? insuranceId)
        {
            string token = _tokenManager.GetToken();
            if (string.IsNullOrEmpty(token))
                throw new Exception("User is not logged in.");

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            string jsonContent = JsonConvert.SerializeObject(newContract);
            HttpContent content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            string url = $"Contract/rent?reservationId={reservationId}&insuranceId={insuranceId}";

            HttpResponseMessage response = await _httpClient.PostAsync(url, content);

            if (!response.IsSuccessStatusCode)
                throw new Exception("Failed to add contract.");
        }
        public async Task DeleteContractAsync(int contractId)
        {
            string token = _tokenManager.GetToken();
            if (string.IsNullOrEmpty(token))
                throw new Exception("User is not logged in.");

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage response = await _httpClient.DeleteAsync($"Contract/{contractId}");

            if (!response.IsSuccessStatusCode)
                throw new Exception("Failed to delete contract.");
        }
    }
}
