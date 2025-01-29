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

        public ICollection<OfferVehicle> GetAllByOfferId(int offerId)
        {
            return _context.OffersVehicles.Where(o => o.OfferId == offerId).ToList();
        }

        public ICollection<OfferVehicle> GetAllByVehicleId(int id)
        {
            return _context.OffersVehicles.Where(o => o.VehicleId == id).ToList();
        }

        public bool Add(OfferVehicle offerVehicle)
        {
            var existingEntity = _context.OffersVehicles.Find(offerVehicle.OfferId, offerVehicle.VehicleId);
            if (existingEntity == null)
            {
                _context.OffersVehicles.Add(offerVehicle);
            }
            else
            {
                _context.Entry(existingEntity).CurrentValues.SetValues(offerVehicle);
            }

            return Save();
        }

        public bool Update(OfferVehicle offerVehicle)
        {
            _context.OffersVehicles.Update(offerVehicle);
            return Save();
        }

        public bool DeleteById(int id)
        {
            var offerVehicles = _context.OffersVehicles.Where(ov => ov.OfferId == id).ToList();
            _context.OffersVehicles.RemoveRange(offerVehicles);
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
