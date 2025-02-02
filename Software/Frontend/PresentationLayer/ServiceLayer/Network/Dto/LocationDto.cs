using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Network.Dto
{
    public class LocationDto
    {
        public int Id { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }


    public class Coord
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }

    public class RouteData
    {
        public List<Coord> Coordinates { get; set; } = new List<Coord>();
    }
}
