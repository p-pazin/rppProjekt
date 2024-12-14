namespace CarchiveAPI.Dto
{
    public class OfferDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public double Price { get; set; }
        public DateOnly DateOfCreation { get; set; }
    }
}
