using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace CarchiveAPI.Dto
{
    public class ContractDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Place { get; set; }
        public DateOnly DateOfCreation { get; set; }
        public int Type { get; set; }
        public string Content { get; set; }
        public int Signed { get; set; }
        public string? ContactName { get; set; }
    }
}
