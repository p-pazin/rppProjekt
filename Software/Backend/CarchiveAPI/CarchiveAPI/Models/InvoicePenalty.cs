namespace CarchiveAPI.Models
{
    public class InvoicePenalty
    {
        public int InvoiceId { get; set; }
        public int PenaltyId { get; set; }
        public Invoice Invoice { get; set; }
        public Penalty Penalty { get; set; }
    }
}
