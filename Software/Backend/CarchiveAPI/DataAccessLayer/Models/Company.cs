namespace CarchiveAPI.Models
{
    public class Company
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public string Pin { get; set; }
        public int Approved { get; set; }
        public ICollection<Vehicle> Vehicles { get; set; }
        public ICollection<User> Users { get; set; }
        public ICollection<Contact> Contacts { get; set; }
        public ICollection<Contract> Contracts { get; set; }
    }
}
