using CarchiveAPI.Data;
using CarchiveAPI.Models;

namespace CarchiveAPI.Repositories
{
    public class LocationRepository
    {
        private readonly DataContext _context;

        public LocationRepository(DataContext context)
        {
            _context = context;
        }

        public ICollection<Location> GetAll()
        {
            return _context.Locations.ToList();
        }

        public Location GetLocationForVehicle(int vehicleId)
        {
            return _context.Locations.FirstOrDefault(l => l.VehicleId == vehicleId);
        }
    }
}
