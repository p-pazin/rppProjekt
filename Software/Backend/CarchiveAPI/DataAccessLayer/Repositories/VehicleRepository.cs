using CarchiveAPI.Data;
using CarchiveAPI.Models;

namespace CarchiveAPI.Repositories
{
    public class VehicleRepository
    {
        private readonly DataContext _context;

        public VehicleRepository(DataContext context)
        {
            _context = context;
        }

        public ICollection<Vehicle> GetAllCatalog(int companyId)
        {
            var a = _context.Vehicles.Where(v => v.Company.Id == companyId && v.State != 4).ToList();
            return a;
        }

        public ICollection<Vehicle> GetAll(int companyId)
        {
            var a = _context.Vehicles.Where(v => v.Company.Id == companyId).ToList();
            return a;
        }

        public ICollection<Vehicle> GetAllSale(int companyId)
        {
            var a = _context.Vehicles.Where(v => v.Company.Id == companyId && v.State == 1 && v.Usage == 1).ToList();
            return a;
        }

        public ICollection<Vehicle> GetAllRent(int companyId)
        {
            var a = _context.Vehicles.Where(v => v.Company.Id == companyId && v.State == 1 && v.Usage == 2).ToList();
            return a;
        }

        public Vehicle GetOneVehicleById(int id, int companyId)
        {
            return _context.Vehicles.Where(v => v.Id == id && v.Company.Id == companyId).FirstOrDefault();
        }

        public Vehicle GetVehicleById(int id, int companyId)
        {
            return _context.Vehicles.Where(v => v.Id == id && v.Company.Id == companyId).FirstOrDefault();
        }

        public Vehicle GetVehicleIdByRegistration(string reg, int companyId)
        {
            return _context.Vehicles.Where(v => v.Registration == reg && v.Company.Id == companyId).FirstOrDefault();
        }

        public ICollection<Vehicle> GetVehiclesByModel(string model, int companyId)
        {
            return _context.Vehicles.Where(v => v.Model == model && v.Company.Id == companyId).ToList();
        }

        public ICollection<Vehicle> GetVehiclesByRegistration(string reg, int companyId)
        {
            return _context.Vehicles.Where(v => v.Registration == reg && v.Company.Id == companyId).ToList();
        }

        public ICollection<Vehicle> GetVehiclesByType(string type, int companyId)
        {
            return _context.Vehicles.Where(v => v.Type == type && v.Company.Id == companyId).ToList();
        }

        public ICollection<Vehicle> GetVehiclesByColor(string color, int companyId)
        {
            return _context.Vehicles.Where(v => v.Color == color && v.Company.Id == companyId).ToList();
        }

        public ICollection<Vehicle> GetVehiclesByMileage(int minMileage, int maxMileage, int companyId)
        {
            return _context.Vehicles.Where(v => v.Mileage >= minMileage && v.Mileage <= maxMileage && v.Company.Id == companyId).ToList();
        }

        public ICollection<Vehicle> GetVehiclesByTransType(string transmissionType, int companyId)
        {
            return _context.Vehicles.Where(v => v.TransmissionType == transmissionType && v.Company.Id == companyId).ToList();
        }

        public ICollection<Vehicle> GetVehiclesByPrice(double minPrice, double maxPrice, int companyId)
        {
            return _context.Vehicles.Where(v => v.Price >= minPrice && v.Price <= maxPrice && v.Company.Id == companyId).ToList();
        }

        public ICollection<Vehicle> GetVehiclesByCondition(string condition, int companyId)
        {
            return _context.Vehicles.Where(v => v.Condition == condition && v.Company.Id == companyId).ToList();
        }

        public ICollection<Vehicle> GetVehiclesByProdYear(int minYear, int maxYear, int companyId)
        {
            return _context.Vehicles.Where(v => v.ProductionYear >= minYear && v.ProductionYear <= maxYear && v.Company.Id == companyId).ToList();
        }

        public ICollection<Vehicle> GetVehiclesByEngPower(int minPower, int maxPower, int companyId)
        {
            return _context.Vehicles.Where(v => v.EnginePower >= minPower && v.EnginePower <= maxPower && v.Company.Id == companyId).ToList();
        }

        public ICollection<Vehicle> GetVehiclesByCubCapacity(double minCap, double maxCap, int companyId)
        {
            return _context.Vehicles.Where(v => v.CubicCapacity >= minCap && v.CubicCapacity <= maxCap && v.Company.Id == companyId).ToList();
        }

        public ICollection<Vehicle> GetVehiclesByEngine(string engine, int companyId)
        {
            return _context.Vehicles.Where(v => v.Engine == engine && v.Company.Id == companyId).ToList();
        }

        public ICollection<Vehicle> GetVehiclesByState(int state, int companyId)
        {
            return _context.Vehicles.Where(v => v.State == state && v.Company.Id == companyId).ToList();
        }

        public ICollection<Vehicle> GetVehiclesByOffer(Offer offer)
        {
            var vehicles = new List<Vehicle>();
            var foundOfferVehicles = _context.OffersVehicles.Where(v => v.OfferId == offer.Id).ToList();
            foreach(var foundOfferVehicle in foundOfferVehicles)
            {
                var foundVehicle = _context.Vehicles.FirstOrDefault(v => v.Id == foundOfferVehicle.VehicleId);
                if (foundVehicle != null)
                {
                    vehicles.Add(foundVehicle);
                }
            }
            return vehicles;
        }

        public bool AddVehicle(Vehicle vehicle)
        {
            if (VehicleExists(vehicle.Registration))
            {
                return false;
            }
            _context.Vehicles.Add(vehicle);
            return Save();
        }

        public bool UpdateVehicle(Vehicle vehicle)
        {
            _context.Vehicles.Update(vehicle);
            return Save();
        }

        public bool DeleteVehicle(Vehicle vehicle)
        {
            var foundVehicle = _context.Vehicles.FirstOrDefault(v => v.Id == vehicle.Id);
            foundVehicle.State = 4;
            if(foundVehicle != null)
            {
                _context.Vehicles.Update(foundVehicle);
                return Save();
            }
            return false;
        }

        bool Save()
        {
            return _context.SaveChanges() > 0;
        }
        bool VehicleExists(string registration)
        {
            return _context.Vehicles.Any(v => v.Registration == registration);
        }

        public bool CheckIfVehicleExists(int id)
        {
            return _context.Vehicles.Any(v => v.Id == id);
        }

        public bool ConnectVehicleToPhoto(int vehicleId, string photoUrl)
        {
            var vehicle = _context.Vehicles.FirstOrDefault(v => v.Id == vehicleId);
            VehiclePhoto photo = new VehiclePhoto()
            {
                Link = photoUrl,
                Description = "0",
                Vehicle = vehicle
            };

            _context.VehiclePhotos.Add(photo);
            if (vehicle != null)
            {
                vehicle.VehiclePhotos.Add(photo);
                return Save();
            }
            return false;
        }

        public IEnumerable<object> GetVehiclePhotos(int vehicleId)
        {
            return _context.VehiclePhotos
                .Where(vp => vp.Vehicle != null && vp.Vehicle.Id == vehicleId)
                .ToList();
        }

        public bool DeleteVehiclePhoto(int photoId)
        {
            var photo = _context.VehiclePhotos.FirstOrDefault(vp => vp.Id == photoId);
            if (photo != null)
            {
                _context.VehiclePhotos.Remove(photo);
                return Save();
            }
            return false;
        }
    }
}
