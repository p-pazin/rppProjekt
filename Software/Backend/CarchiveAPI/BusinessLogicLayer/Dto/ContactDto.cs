namespace CarchiveAPI.Dto
{
    public class ContactDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Pin { get; set; }
        public DateOnly DateOfCreation { get; set; }
        public int State { get; set; }
        public string? Country { get; set; }
        public string? City { get; set; }
        public string? Address { get; set; }
        public string? TelephoneNumber { get; set; }
        public string? MobileNumber { get; set; }
        public string? Email { get; set; }
        public string? Description { get; set; }
    }
}
