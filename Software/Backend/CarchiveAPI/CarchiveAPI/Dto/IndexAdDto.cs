namespace CarchiveAPI.Dto
{
    public class IndexAdDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string PaymentMethod { get; set; }
        public DateOnly DateOfPublishment { get; set; }
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
        public string Link { get; set; }
    }
}

