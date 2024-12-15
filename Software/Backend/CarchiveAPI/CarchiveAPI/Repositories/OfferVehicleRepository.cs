using CarchiveAPI.Data;
using CarchiveAPI.Models;

namespace CarchiveAPI.Repositories
{
    public class OfferVehicleRepository
    {
        private DataContext _context;

        public OfferVehicleRepository(DataContext context)
        {
            this._context = context;
        }

        public bool Add(OfferVehicle offerVehicle)
        {
            _context.OffersVehicles.Add(offerVehicle);
            return Save();
        }

        public bool Update(OfferVehicle offerVehicle)
        {
            _context.OffersVehicles.Update(offerVehicle);
            return Save();
        }

        public bool Delete(int id)
        {
            var offerVehicle = _context.OffersVehicles.Where(o => o.OfferId == id).ToList()[0];
            _context.OffersVehicles.Remove(offerVehicle);
            return Save();
        }

        public bool Save()
        {
            return _context.SaveChanges() >= 0;
        }
    }
}
