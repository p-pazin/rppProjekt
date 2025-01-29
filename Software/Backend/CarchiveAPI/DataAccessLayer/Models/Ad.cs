namespace CarchiveAPI.Models
{
    public class Ad
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string PaymentMethod { get; set; }
        public DateOnly DateOfPublishment { get; set; }
        public User User { get; set; }
        public Vehicle Vehicle { get; set; }
    }
}
