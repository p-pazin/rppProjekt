namespace CarchiveAPI.Dto
{
    public class RentContractDto
    { 
        public int Id { get; set; }
        public string Title { get; set; }
        public string Place { get; set; }
        public DateOnly DateOfCreation { get; set; }
        public int Type { get; set; }
        public string Content { get; set; }
        public int Signed { get; set; }

        // Company
        public string Name { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public string Pin { get; set; }

        // User
        public string FirstNameDirector { get; set; }
        public string LastNameDirector { get; set; }

        // Contact
        public string FirstNameContact { get; set; }
        public string LastNameContact { get; set; }
        public string PinContact { get; set; }
        public string? CountryContact { get; set; }
        public string? CityContact { get; set; }
        public string? AddressContact { get; set; }

        // Vehicle
        public string Brand { get; set; }
        public string Model { get; set; }
        public string Engine { get; set; }
        public string Registration { get; set; }
        public int Mileage { get; set; }

        // Reservation
        public int ReservationId { get; set; }
        public double Price { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public int MaxMileage { get; set; }

        // Insurance
        public string NameInsurance { get; set; }
        public double CostInsurance { get; set; }
    }
}
