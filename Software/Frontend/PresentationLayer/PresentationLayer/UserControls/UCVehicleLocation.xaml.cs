using Mapsui;
using Mapsui.Providers;
using Mapsui.Tiling.Layers;
using Mapsui.UI.Wpf;
using Mapsui.Utilities;
using System.Collections.Generic;
using System.Windows.Controls;
using BruTile.Predefined;
using Mapsui.Projections;
using Mapsui.Layers;
using Mapsui.Styles;
using System.Diagnostics;
using ServiceLayer.Network.Dto;
using ServiceLayer.Services;

namespace PresentationLayer.UserControls
{
    public partial class UCVehicleLocation : UserControl
    {
        private readonly LocationService _locationService = new LocationService();

        public UCVehicleLocation()
        {
            InitializeComponent();
            InitializeMap();
        }

        private async void InitializeMap()
        {
            MyMap.Map.Layers.Add(new TileLayer(KnownTileSources.Create()));

            var locations = await _locationService.GetLocations();
            Debug.WriteLine($"Locations Count: {locations?.Count}");
            if (locations.Count > 0)
            {
                Debug.WriteLine($"First Location: {locations[0].Latitude}, {locations[0].Longitude}");

                LocationDto firstLocation = locations[0];
                (double x, double y) sphericalMercator = SphericalMercator.FromLonLat(firstLocation.Longitude, firstLocation.Latitude);
                var mapCenter = new MPoint(sphericalMercator.x, sphericalMercator.y);
                MyMap.Map.Home = n => n.CenterOnAndZoomTo(mapCenter, 5000);
            }

            var markerLayer = CreateMarkerLayer(locations);
            MyMap.Map.Layers.Add(markerLayer);
        }

        private ILayer CreateMarkerLayer(List<LocationDto> locations)
        {
            var features = new List<IFeature>();

            foreach (var location in locations)
            {
                (double x, double y) sphericalMercator = SphericalMercator.FromLonLat(location.Longitude, location.Latitude);
                var point = new MPoint(sphericalMercator.x, sphericalMercator.y);

                string labelText = location.Id == 9
                        ? "2" : location.Id == 10 ? "1"
                        : $"ID: {location.Id}\nLat: {location.Latitude}\nLon: {location.Longitude}";

                var feature = new PointFeature(point)
                {
                    Styles = new List<IStyle>
            {
                new SymbolStyle
                {
                    SymbolScale = 1,
                    Fill = new Brush(Color.Red)
                },
                new LabelStyle
                {
                    Text = labelText,
                    ForeColor = Color.Black,
                    BackColor = new Brush(Color.White),
                    Offset = new Offset(0, -20),
                    Font = new Mapsui.Styles.Font { Size = 12 },
                    Halo = new Pen(Color.White, 2)
                }
            }
                };

                features.Add(feature);
            }

            var memoryProvider = new MemoryProvider(features);
            return new Layer("Marker Layer") { DataSource = memoryProvider };
        }

    }
}
