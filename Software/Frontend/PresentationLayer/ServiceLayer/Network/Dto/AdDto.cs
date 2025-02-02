using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Esri.ArcGISRuntime;

namespace ServiceLayer.Network.Dto
{
    public class AdDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string PaymentMethod { get; set; }
        public string DateOfPublishment { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public List<string> Links { get; set; }
    }
}
