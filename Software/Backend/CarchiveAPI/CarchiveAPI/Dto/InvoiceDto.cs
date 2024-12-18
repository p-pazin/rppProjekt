﻿namespace CarchiveAPI.Dto
{
    public class InvoiceDto
    {
        public int Id { get; set; }
        public DateOnly DateOfCreation { get; set; }
        public double Vat { get; set; }
        public string PaymentMethod { get; set; }
        public double TotalCost { get; set; }
        public int Mileage { get; set; }
    }
}
