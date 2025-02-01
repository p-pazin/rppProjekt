using Esri.ArcGISRuntime;
using Esri.ArcGISRuntime.Mapping;
using Esri.ArcGISRuntime.UI.Controls;
using System;
using ServiceLayer.ViewModels;
using System.Windows.Controls;
using Esri.ArcGISRuntime.Geometry;

namespace PresentationLayer.UserControls
{
    public partial class UCVehicleLocation : UserControl
    {
        public UCVehicleLocation()
        {
            InitializeComponent();
            MapPoint mapCenterPoint = new MapPoint(-118.805, 34.027, SpatialReferences.Wgs84);
            MainMapView.SetViewpoint(new Viewpoint(mapCenterPoint, 100000));
        }
    }
}
