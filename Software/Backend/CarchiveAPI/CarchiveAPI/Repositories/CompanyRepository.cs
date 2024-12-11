using CarchiveAPI.Data;
using CarchiveAPI.Models;

namespace CarchiveAPI.Repositories
{
    public class CompanyRepository
    {
        private readonly DataContext _context;

        public CompanyRepository(DataContext context)
        {
            this._context = context;
        }
        public ICollection<Company> GetCompanies()
        {
            return _context.Companies.OrderBy(c => c.Id).ToList();
        } 
    }
}
