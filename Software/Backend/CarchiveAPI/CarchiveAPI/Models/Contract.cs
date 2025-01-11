namespace CarchiveAPI.Models
{
    public class Contract
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Place { get; set; }
        public DateOnly DateOfCreation { get; set; }
        public int Type { get; set; }
        public string Content { get; set; }
        public int Signed { get; set; }
        public Company Company { get; set; }
        public int? ContactId { get; set; }
        public Contact? Contact { get; set; }
        public int? VehicleId { get; set; }
        public Vehicle? Vehicle { get; set; }
        public int? OfferId { get; set; }
        public Offer? Offer { get; set; }
        public User User { get; set; }
        public ICollection<Invoice> Invoices { get; set; }
        public int? ReservationId { get; set; }
        public Reservation? Reservation { get; set; }
        public Insurance? Insurance { get; set; }
    }
}
