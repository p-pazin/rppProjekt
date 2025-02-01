using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Network.Dto
{
    public class VehicleDto
    {
        public int Id { get; set; }
        public string Registration { get; set; }
        public int State { get; set; }
        public string Brand { get; set; }
        public int Mileage { get; set; }
        public int ProductionYear { get; set; }
        public string Model { get; set; }
        public string Engine { get; set; }
        public double CubicCapacity { get; set; }
        public double EnginePower { get; set; }
        public string RegisteredTo { get; set; }
        public string Color { get; set; }
        public string DriveType { get; set; }
        public double? Price { get; set; }
        public string TransmissionType { get; set; }
        public string Type { get; set; }
        public string Condition { get; set; }
        public double? RentPrice { get; set; }
        public int Usage { get; set; }

        public string DisplayText => $"{Registration} - {Brand}, {Model}";
        public string UsageDisplay => Usage == 1 ? "U prodaji" : Usage == 2 ? "U najmu" : "Unknown";
        public string StateDisplay => State == 1 ? "Aktivno" : State == 2 ? "Prodano" : State == 3 ? "Najam" : State == 4 ? "Izbrisano" : "Unkown";

        public override bool Equals(object obj)
        {
            if (obj is VehicleDto other)
            {
                return this.Registration == other.Registration;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return Registration?.GetHashCode() ?? 0;
        }
    }

    public class VehiclePost
    {
        public string Registration { get; set; }
        public int State { get; set; }
        public string Brand { get; set; }
        public int Mileage { get; set; }
        public int ProductionYear { get; set; }
        public string Model { get; set; }
        public string Engine { get; set; }
        public double CubicCapacity { get; set; }
        public double EnginePower { get; set; }
        public string RegisteredTo { get; set; }
        public string Color { get; set; }
        public string DriveType { get; set; }
        public double? Price { get; set; }
        public string TransmissionType { get; set; }
        public string Type { get; set; }
        public string Condition { get; set; }
        public double? RentPrice { get; set; }
        public int Usage { get; set; }
    }
}
