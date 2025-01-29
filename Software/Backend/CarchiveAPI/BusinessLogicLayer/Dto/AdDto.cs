namespace CarchiveAPI.Dto
{
    public class AdDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string PaymentMethod { get; set; }
        public DateOnly DateOfPublishment { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public List<string> Links { get; set; }
    }
}
