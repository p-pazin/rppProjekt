using CarchiveAPI.Data;
using CarchiveAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CarchiveAPI.Repositories
{
    public class ReservationRepository
    {
        private readonly DataContext _context;
        public ReservationRepository(DataContext context)
        {
            this._context = context;
        }
        public ICollection<Reservation> GetAll(int companyId)
        {
            return _context.Reservations.Include(c=> c.Contact).Include(c => c.Vehicle).Include(c => c.User).Where(r => r.Vehicle.Company.Id == companyId).ToList();
        }
        public Reservation Get(int id, int companyId)
        {
            return _context.Reservations.Include(r => r.Vehicle).Include(r => r.Contact).FirstOrDefault(r => r.Id == id && r.Vehicle.Company.Id == companyId);
        }
        public bool Add(Reservation reservation)
        {
            _context.Reservations.Add(reservation);
            return Save();
        }
        public bool Update(Reservation reservation)
        {
            _context.Reservations.Update(reservation);
            return Save();
        }
        public bool Delete(int id, int companyId) {
            var reservation = Get(id, companyId);
            _context.Reservations.Remove(reservation);
            return Save();
        }
        public bool Save()
        {
            return _context.SaveChanges() > 0;
        }
    }
}
