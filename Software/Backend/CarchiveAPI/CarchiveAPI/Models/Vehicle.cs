﻿namespace CarchiveAPI.Models
{
    public class Vehicle
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
        public DateOnly RegisteredTo { get; set; }
        public string Color { get; set; }
        public string DriveType { get; set; }
        public double Price { get; set; }
        public string TransmissionType { get; set; }
        public string Type { get; set; }
        public string Condition { get; set; }
        public double RentPrice { get; set; }
        public int Usage { get; set; }
        public Company Company { get; set; }
        public Location Location { get; set; }
        public ICollection<Ad> Ads { get; set; }
        public ICollection<Contract> Contracts { get; set; }
        public ICollection<VehiclePhoto> VehiclePhotos { get; set; }
        public ICollection<OfferVehicle> OfferVehicles { get; set; }
        public ICollection<Reservation> Reservations { get; set; }
    }
}
