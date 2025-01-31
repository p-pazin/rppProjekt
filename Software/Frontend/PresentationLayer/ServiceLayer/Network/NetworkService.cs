using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using ServiceLayer.Services;

namespace ServiceLayer.Network
{
    public class NetworkService
    {
        private static HttpClient _httpClient;

        public static HttpClient GetHttpClient(TokenManager tokenManager)
        {
            if (_httpClient == null)
            {
                var handler = new HttpClientHandler();
                _httpClient = new HttpClient(handler)
                {
                    BaseAddress = new Uri(Environment.BASE_URL)
                };
                _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenManager.GetToken());
            }

            return _httpClient;
        }
    }
}
