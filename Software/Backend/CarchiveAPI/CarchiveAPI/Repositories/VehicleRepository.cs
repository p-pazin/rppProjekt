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

        public ICollection<Vehicle> GetAll()
        {
            return _context.Vehicles.ToList();
        }

        public ICollection<Vehicle> GetVehicleById(int id)
        {
            return _context.Vehicles.Where(v => v.Id == id).ToList();
        }

        public ICollection<Vehicle> GetVehiclesByModel(string model)
        {
            return _context.Vehicles.Where(v => v.Model == model).ToList();
        }

        public ICollection<Vehicle> GetVehiclesByRegistration(string reg)
        {
            return _context.Vehicles.Where(v => v.Registration == reg).ToList();
        }

        public ICollection<Vehicle> GetVehiclesByType(string type)
        {
            return _context.Vehicles.Where(v => v.Type == type).ToList();
        }

        public ICollection<Vehicle> GetVehiclesByColor(string color)
        {
            return _context.Vehicles.Where(v => v.Color == color).ToList();
        }

        public ICollection<Vehicle> GetVehiclesByMileage(int minMileage, int maxMileage)
        {
            return _context.Vehicles.Where(v => v.Mileage >= minMileage && v.Mileage <= maxMileage).ToList();
        }

        public ICollection<Vehicle> GetVehiclesByTransType(string transmissionType)
        {
            return _context.Vehicles.Where(v => v.TransmissionType == transmissionType).ToList();
        }

        public ICollection<Vehicle> GetVehiclesByPrice(double minPrice, double maxPrice)
        {
            return _context.Vehicles.Where(v => v.Price >= minPrice && v.Price <= maxPrice).ToList();
        }

        public ICollection<Vehicle> GetVehiclesByCondition(string condition)
        {
            return _context.Vehicles.Where(v => v.Condition == condition).ToList();
        }

        public ICollection<Vehicle> GetVehiclesByProdYear(int minYear, int maxYear)
        {
            return _context.Vehicles.Where(v => v.ProductionYear >= minYear && v.ProductionYear <= maxYear).ToList();
        }

        public ICollection<Vehicle> GetVehiclesByEngPower(int minPower, int maxPower)
        {
            return _context.Vehicles.Where(v => v.EnginePower >= minPower && v.EnginePower <= maxPower).ToList();
        }

        public ICollection<Vehicle> GetVehiclesByCubCapacity(double minCap, double maxCap)
        {
            return _context.Vehicles.Where(v => v.CubicCapacity >= minCap && v.CubicCapacity <= maxCap).ToList();
        }

        public ICollection<Vehicle> GetVehiclesByEngine(string engine)
        {
            return _context.Vehicles.Where(v => v.Engine == engine).ToList();
        }

        bool Save()
        {
            return _context.SaveChanges() > 0;
        }

        bool VehicleExists(string registration)
        {
            return _context.Vehicles.Any(v => v.Registration == registration);
        }
    }
}
