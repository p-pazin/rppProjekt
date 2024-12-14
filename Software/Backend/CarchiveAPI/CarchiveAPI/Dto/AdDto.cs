namespace CarchiveAPI.Dto
{
    public class AdDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string PaymentMethod { get; set; }
        public DateOnly DateOfPublishment { get; set; }
    }
}
