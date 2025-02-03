using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using CarchiveAPI.Dto;

namespace ServiceLayer.Services
{
    public class InvoiceService
    {
        private readonly HttpClient _httpClient;
        private readonly TokenManager _tokenManager;

        public InvoiceService()
        {
            _httpClient = new HttpClient { BaseAddress = new Uri(Environment.BASE_URL) };
            _tokenManager = new TokenManager();
        }

        public async Task<List<InvoiceDto>> GetInvoicesAsync()
        {
            string token = _tokenManager.GetToken();
            if (string.IsNullOrEmpty(token))
                throw new Exception("User is not logged in.");

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage response = await _httpClient.GetAsync("Invoice");
            if (!response.IsSuccessStatusCode)
                throw new Exception("Failed to fetch invoices data.");

            string json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<InvoiceDto>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        public async Task PostInvoicesSellAsync(InvoiceDto newInvoice)
        {
            string token = _tokenManager.GetToken();
            if (string.IsNullOrEmpty(token))
                throw new Exception("User is not logged in.");

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            string url = $"Invoice/sell?contractId={newInvoice.ContractId}";

            string jsonContent = JsonSerializer.Serialize(newInvoice);
            HttpContent content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PostAsync(url, content);

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
                throw new Exception("Failed to add new invoice.");
            }
        }

        public async Task PostInvoicesRentStartAsync(InvoiceDto newInvoice)
        {
            string token = _tokenManager.GetToken();
            if (string.IsNullOrEmpty(token))
                throw new Exception("User is not logged in.");

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            string url = $"Invoice/rent/start?contractId={newInvoice.ContractId}";

            string jsonContent = JsonSerializer.Serialize(newInvoice);
            HttpContent content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PostAsync(url, content);

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
                throw new Exception("Failed to add new invoice.");
            }
        }

        public async Task PostInvoicesRentEndAsync(InvoiceDto newInvoice, List<int> penaltyIds)
        {
            string token = _tokenManager.GetToken();
            if (string.IsNullOrEmpty(token))
                throw new Exception("User is not logged in.");

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            string penaltyQuery = string.Join("&penaltyIds=", penaltyIds);
            string url = $"Invoice/rent/final?contractId={newInvoice.ContractId}&penaltyIds={penaltyQuery}";

            string jsonContent = JsonSerializer.Serialize(newInvoice);
            HttpContent content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PostAsync(url, content);

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
                throw new Exception("Failed to add new invoice.");
            }
        }

    }
}
