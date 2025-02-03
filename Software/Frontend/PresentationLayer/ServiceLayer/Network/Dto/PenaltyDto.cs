using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Network.Dto
{
    public class PenaltyDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Cost { get; set; }

        public string DisplayText => $"{Name} - {Cost}";
    }
}
