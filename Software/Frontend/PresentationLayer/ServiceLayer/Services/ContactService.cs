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
    public class ContactService
    {
        private readonly HttpClient _httpClient;
        private readonly TokenManager _tokenManager;

        public ContactService()
        {
            _httpClient = new HttpClient { BaseAddress = new Uri(Environment.BASE_URL) };
            _tokenManager = new TokenManager();
        }

        public async Task<List<ContactDto>> GetContactsAsync()
        {
            string token = _tokenManager.GetToken();
            if(string.IsNullOrEmpty(token))
                throw new Exception("User is not logged in.");

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage response = await _httpClient.GetAsync("Contact");
            if(!response.IsSuccessStatusCode)
                throw new Exception("Failed to fetch contacts data.");

            string json = await response.Content.ReadAsStringAsync();
            return System.Text.Json.JsonSerializer.Deserialize<List<ContactDto>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
        public async Task PostContactAsync(ContactDto newContact)
        {
            string token = _tokenManager.GetToken();
            if(string.IsNullOrEmpty(token))
                throw new Exception("User is not logged in.");

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            string jsonContent = JsonConvert.SerializeObject(newContact);
            HttpContent content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PostAsync("Contact", content);

            if(!response.IsSuccessStatusCode)
                throw new Exception("Failed to add contact.");
        }
        public async Task PutContactAsync(ContactDto contactToUpdate)
        {
            string token = _tokenManager.GetToken();
            if (string.IsNullOrEmpty(token))
                throw new Exception("User is not logged in.");

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            string jsonContent = JsonConvert.SerializeObject(contactToUpdate);
            HttpContent content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PutAsync($"Contact/{contactToUpdate.Id}", content);

            if (!response.IsSuccessStatusCode)
                throw new Exception("Failed to update contact.");
        }
        public async Task DeleteContactAsync(int contactId)
        {
            string token = _tokenManager.GetToken();
            if (string.IsNullOrEmpty(token))
                throw new Exception("User is not logged in.");

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage response = await _httpClient.DeleteAsync($"Contact/{contactId}");

            if (!response.IsSuccessStatusCode)
                throw new Exception("Failed to delete contact.");
        }
    }
}
