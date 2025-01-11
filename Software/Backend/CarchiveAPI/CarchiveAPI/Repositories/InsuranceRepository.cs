using CarchiveAPI.Data;
using CarchiveAPI.Models;

namespace CarchiveAPI.Repositories
{
    public class InsuranceRepository
    {
        private readonly DataContext _context;
        public InsuranceRepository(DataContext context)
        {
            _context = context;
        }

        public ICollection<Insurance> GetAll()
        {
            return _context.Insurances.ToList();
        }

        public Insurance Get(int id)
        {
            return _context.Insurances.Find(id);
        }
    }
}
