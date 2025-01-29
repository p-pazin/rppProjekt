namespace CarchiveAPI.Models
{
    public class VehiclePhoto
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string Link { get; set; }
        public Vehicle Vehicle { get; set; }
    }
}
