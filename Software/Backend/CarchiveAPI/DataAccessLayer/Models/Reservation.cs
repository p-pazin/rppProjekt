namespace CarchiveAPI.Models
{
    public class Reservation
    {
        public int Id { get; set; }
        public int State { get; set; }
        public double Price { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public DateOnly DateOfCreation { get; set; }
        public int MaxMileage { get; set; }
        public Contact Contact { get; set; }
        public Vehicle Vehicle { get; set; }
        public User User { get; set; }
        public Contract? Contract { get; set; }
    }
}
