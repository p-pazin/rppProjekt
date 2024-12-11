namespace CarchiveAPI.Models
{
    public class Offer
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public double Price { get; set; }
        public DateOnly DateOfCreation { get; set; }
        public User User { get; set; }
        public Contact Contact { get; set; }
        public ICollection<OfferVehicle> OfferVehicles { get; set; }
    }
}
