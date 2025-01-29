namespace CarchiveAPI.Models
{
    public class Insurance
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Cost { get; set; }
        public string Description { get; set; }
        public ICollection<Contract> Contracts { get; set; }
    }
}
