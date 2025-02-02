using NetTopologySuite.Geometries;
using ServiceLayer.Network.Dto;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ServiceLayer.Services
{
    public class LocationService
    {
        private readonly HttpClient _httpClient;
        private readonly TokenManager _tokenManager;
        private const string BASE_URL = "https://carchive.online/api/";

        public LocationService()
        {
            _httpClient = new HttpClient { BaseAddress = new Uri(BASE_URL) };
            _tokenManager = new TokenManager();
        }

        public async Task<List<LocationDto>> GetLocations()
        {
            string token = _tokenManager.GetToken();
            if (string.IsNullOrEmpty(token))
                throw new Exception("User is not logged in.");

            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage response = await _httpClient.GetAsync("Location");

            if (!response.IsSuccessStatusCode)
                throw new Exception("Failed to fetch location data.");

            string json = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<List<LocationDto>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        public async Task<RouteData> LoadGpxRouteDataAsync(string filePath)
        {
            string gpxContent = await Task.Run(() => File.ReadAllText(filePath));

            XDocument gpxDoc = XDocument.Parse(gpxContent);

            XNamespace ns = gpxDoc.Root.GetDefaultNamespace();

            RouteData routeData = new RouteData();

            foreach (var trkpt in gpxDoc.Descendants(ns + "trkpt"))
            {
                double lat = double.Parse(trkpt.Attribute("lat")?.Value ?? "0", CultureInfo.InvariantCulture);
                double lon = double.Parse(trkpt.Attribute("lon")?.Value ?? "0", CultureInfo.InvariantCulture);

                routeData.Coordinates.Add(new Coord { Latitude = lat, Longitude = lon });
            }

            return routeData;
        }
    }
}
