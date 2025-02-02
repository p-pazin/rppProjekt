using System.Collections.Generic;
using ServiceLayer.Network.Dto;

namespace CarchiveAPI.Dto
{
    public class SaleContractDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Place { get; set; }
        public string DateOfCreation { get; set; }
        public int Type { get; set; }
        public string Content { get; set; }
        public int Signed { get; set; }
        public string CompanyName { get; set; }
        public string CompanyPin { get; set; }
        public string CompanyAddress { get; set; }
        public string ContactName { get; set; }
        public string ContactPin { get; set; }
        public string ContactAddress { get; set; }
        public VehicleDto Vehicle { get; set; }
        public double Price { get; set; }
        public string UserName { get; set; }
        public int? OfferId { get; set; }
        public List<VehicleDto> Vehicles { get; set; }
    }
}
