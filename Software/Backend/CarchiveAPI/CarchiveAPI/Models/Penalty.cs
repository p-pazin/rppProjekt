namespace CarchiveAPI.Models
{
    public class Penalty
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Cost { get; set; }
        public ICollection<InvoicePenalty> InvoicePenalties { get; set; }
    }
}
