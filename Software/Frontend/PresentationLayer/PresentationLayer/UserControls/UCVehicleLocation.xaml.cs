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
using NetTopologySuite.Geometries;
using System.Globalization;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.IO;
using System.Windows.Threading;
using System;

namespace PresentationLayer.UserControls
{
    public partial class UCVehicleLocation : UserControl
    {
        private readonly LocationService _locationService = new LocationService();
        private DispatcherTimer _timer;
        private List<Coord> _routeCoordinates;
        private int _currentRouteIndex;
        private ILayer _markerLayer;

        public UCVehicleLocation()
        {
            InitializeComponent();
            InitializeMap();
        }

        private async void InitializeMap()
        {
            MyMap.Map.Layers.Add(new TileLayer(KnownTileSources.Create()));

            double varazdinLat = 46.3042;
            double varazdinLon = 16.3378;

            var routeData = await _locationService.LoadGpxRouteDataAsync("files/route.gpx");
            _routeCoordinates = routeData.Coordinates;

            if (_routeCoordinates.Count > 0)
            {
                var firstCoord = _routeCoordinates[0];
                (double x, double y) sphericalMercator = SphericalMercator.FromLonLat(firstCoord.Longitude, firstCoord.Latitude);
                var mapCenter = new MPoint(sphericalMercator.x, sphericalMercator.y);
                MyMap.Map.Home = n => n.CenterOnAndZoomTo(mapCenter, 2);
            }

            _markerLayer = CreateMarkerLayer(_routeCoordinates[0]);
            MyMap.Map.Layers.Add(_markerLayer);

            StartAnimation();
        }

        private void StartAnimation()
        {
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromMilliseconds(2000);
            _timer.Tick += OnTimerTick;
            _timer.Start();
        }

        private void OnTimerTick(object sender, EventArgs e)
        {
            if (_currentRouteIndex < _routeCoordinates.Count - 1)
            {
                _currentRouteIndex++;
                UpdateMarkerPosition(_routeCoordinates[_currentRouteIndex]);
            }
            else
            {
                _timer.Stop();
            }
        }

        private void UpdateMarkerPosition(Coord coord)
        {
            MyMap.Map.Layers.Remove(_markerLayer);

            _markerLayer = CreateMarkerLayer(coord);
            MyMap.Map.Layers.Add(_markerLayer);

            MyMap.Refresh();
        }

        private ILayer CreateMarkerLayer(Coord coord)
        {
            (double x, double y) sphericalMercator = SphericalMercator.FromLonLat(coord.Longitude, coord.Latitude);
            var point = new MPoint(sphericalMercator.x, sphericalMercator.y);

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
                    Text = $"Lat: {coord.Latitude}\nLon: {coord.Longitude}",
                    ForeColor = Color.Black,
                    BackColor = new Brush(Color.White),
                    Offset = new Offset(0, -20),
                    Font = new Mapsui.Styles.Font { Size = 12 },
                    Halo = new Pen(Color.White, 2)
                }
            }
            };

            var memoryProvider = new MemoryProvider(new List<IFeature> { feature });
            return new Layer("Marker Layer") { DataSource = memoryProvider };
        }
    }
}
