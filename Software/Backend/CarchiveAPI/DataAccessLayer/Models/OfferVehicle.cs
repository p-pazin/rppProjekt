namespace CarchiveAPI.Models
{
    public class OfferVehicle
    {
        public int OfferId { get; set; }
        public int VehicleId { get; set; }
        public Offer Offer { get; set; }
        public Vehicle Vehicle { get; set; }
    }
}
