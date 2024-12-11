﻿namespace CarchiveAPI.Models
{
    public class Location
    {
        public int Id { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public int IdVehicle { get; set; }
        public Vehicle Vehicle { get; set; }
    }
}
