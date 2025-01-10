namespace CarchiveAPI.Dto
{
    public class SaleContractDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Place { get; set; }
        public DateOnly DateOfCreation { get; set; }
        public int Type { get; set; }
        public string Content { get; set; }
        public int Signed { get; set; }
        public string CompanyName { get; set; }
        public string CompanyPin { get; set; }
        public string CompanyAddress { get; set; }
        public string ContactName { get; set; }
        public string ContactPin { get; set; }
        public string ContactAddress { get; set; }
        public string? VehicleBrand { get; set; }
        public string? VehicleModel { get; set; }
        public string? VehicleRegistration { get; set; }
        public string? VehicleCubicCapacity { get; set; }
        public string Price { get; set; }
        public string UserName { get; set; }
        public List<VehicleDto>? Vehicles { get; set; }
    }
}
