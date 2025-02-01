namespace CarchiveAPI.Dto
{
    public class InvoiceDto
    {
        public int Id { get; set; }
        public string DateOfCreation { get; set; }
        public double Vat { get; set; }
        public string PaymentMethod { get; set; }
        public double TotalCost { get; set; }
        public int Mileage { get; set; }
        public int ContractId { get; set; }
    }
}
