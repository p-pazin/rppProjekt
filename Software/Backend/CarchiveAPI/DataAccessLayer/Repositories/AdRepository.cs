using CarchiveAPI.Data;
using CarchiveAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CarchiveAPI.Repositories
{
    public class AdRepository
    {
        private readonly DataContext _context;
        public AdRepository(DataContext context)
        {
            _context = context;
        }

        public ICollection<Ad> GetAll(int companyId)
        {
            return _context.Ads
                .Include(a => a.User.Company)
                .Include(v => v.Vehicle)
                .Include(v => v.Vehicle.VehiclePhotos)
                .Where(a => a.User.Company.Id == companyId)
                .ToList();
        }


        public Ad Get(int id, int companyId) {
            return _context.Ads
                .Include(a => a.User.Company)
                .Include(v => v.Vehicle)
                .Include(v => v.Vehicle.VehiclePhotos)
                .Where(a => a.User.Company.Id == companyId && a.Id == id)
                .FirstOrDefault();
        }
        
        public ICollection<Ad> GetAdsByVehicleId(int vehicleId, int companyId)
        {
            return _context.Ads
                .Include(a => a.User.Company)
                .Include(v => v.Vehicle)
                .Include(v => v.Vehicle.VehiclePhotos)
                .Where(a => a.User.Company.Id == companyId && a.Vehicle.Id == vehicleId)
                .ToList();
        }

        public bool AddAd(Ad ad, int companyId)
        {
            if (ad.User.Company.Id != companyId)
            {
                return false;
            }
            _context.Ads.Add(ad);
            return Save();
        }

        public bool UpdateAd(Ad ad)
        {
            _context.Ads.Update(ad);
            return Save();
        }

        public bool DeleteAd(Ad ad)
        {
            _context.Ads.Remove(ad);
            return Save();
        }

        bool Save()
        {
            return _context.SaveChanges() > 0;
        }
    }
}
