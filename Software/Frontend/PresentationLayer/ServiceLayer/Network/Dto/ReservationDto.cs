namespace CarchiveAPI.Dto
{
    public class ReservationDto
    {
        public int Id { get; set; }
        public int State { get; set; }
        public double Price { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string DateOfCreation { get; set; }
        public int MaxMileage { get; set; }
        public int VehicleId { get; set; }
        public int ContactId { get; set; }

    }
}
