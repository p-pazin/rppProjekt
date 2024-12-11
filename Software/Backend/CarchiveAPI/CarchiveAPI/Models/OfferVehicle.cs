namespace CarchiveAPI.Models
{
    public class OfferVehicle
    {
        public int IdOffer { get; set; }
        public int IdVehicle { get; set; }
        public Offer Offer { get; set; }
        public Vehicle Vehicle { get; set; }
    }
}
