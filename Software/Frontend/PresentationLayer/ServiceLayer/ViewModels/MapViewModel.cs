using Esri.ArcGISRuntime.Mapping;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.ViewModels
{
    public class MapViewModel : INotifyPropertyChanged
    {
        public MapViewModel()
        {
            SetupMap();
        }

        public event PropertyChangedEventHandler PropertyChanged; 

        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private Map _map;

        public Map Map
        {
            get { return _map; }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(value), "Map cannot be set to null.");
                }
                _map = value;
                OnPropertyChanged();
            }
        }

        private void SetupMap()
        {
            _map = new Map(BasemapStyle.ArcGISTopographic);
        }
    }
}
