using System.Text.Json.Serialization;

namespace CarchiveAPI.Models
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public Role? Role { get; set; }
        public Company Company { get; set; }
        public ICollection<Offer> Offers { get; set; }
        public ICollection<Contract> Contracts { get; set; }
        public ICollection<Ad> Ads { get; set; }
        public ICollection<Reservation> Reservations { get; set; }
    }
}
