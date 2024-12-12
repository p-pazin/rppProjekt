namespace CarchiveAPI.Models
{
    public class Invoice
    {
        public int Id { get; set; }
        public DateOnly DateOfCreation { get; set; }
        public double Vat {  get; set; }
        public string PaymentMethod { get; set; }
        public int ContractId { get; set; }
        public Contract Contract { get; set; }
    }
}
