using AutoMapper;
using CarchiveAPI.Dto;
using CarchiveAPI.Models;
using CarchiveAPI.Repositories;
using System.ComponentModel.Design;

namespace CarchiveAPI.Services
{
    public class VehicleServices
    {
        private readonly VehicleRepository _vehicleRepository;
        private readonly CompanyRepository _companyRepository;
        private readonly IMapper _mapper;

        public VehicleServices(VehicleRepository vehicleRepository, IMapper mapper, CompanyRepository companyRepository)
        {
            _vehicleRepository = vehicleRepository;
            _mapper = mapper;
            _companyRepository = companyRepository;
        }

        public ICollection<VehicleDto> GetAll()
        {
            var vehicles = _vehicleRepository.GetAll();
            return _mapper.Map<ICollection<VehicleDto>>(vehicles);
        }

        public ICollection<VehicleDto> GetVehicleById(int id)
        {
            var vehicle = _vehicleRepository.GetVehicleById(id);
            return _mapper.Map<ICollection<VehicleDto>>(vehicle);
        }

        public ICollection<VehicleDto> GetVehiclesByModel(string model)
        {
            var vehicles = _vehicleRepository.GetVehiclesByModel(model);
            return _mapper.Map<ICollection<VehicleDto>>(vehicles);
        }

        public ICollection<VehicleDto> GetVehiclesByRegistration(string reg)
        {
            var vehicles = _vehicleRepository.GetVehiclesByRegistration(reg);
            return _mapper.Map<ICollection<VehicleDto>>(vehicles);
        }

        public ICollection<VehicleDto> GetVehiclesByType(string type)
        {
            var vehicles = _vehicleRepository.GetVehiclesByType(type);
            return _mapper.Map<ICollection<VehicleDto>>(vehicles);
        }

        public ICollection<VehicleDto> GetVehiclesByColor(string color)
        {
            var vehicles = _vehicleRepository.GetVehiclesByColor(color);
            return _mapper.Map<ICollection<VehicleDto>>(vehicles);
        }

        public ICollection<VehicleDto> GetVehiclesByMileage(int minMileage, int maxMileage)
        {
            var vehicles = _vehicleRepository.GetVehiclesByMileage(minMileage, maxMileage);
            return _mapper.Map<ICollection<VehicleDto>>(vehicles);
        }

        public ICollection<VehicleDto> GetVehiclesByTransType(string transmissionType)
        {
            var vehicles = _vehicleRepository.GetVehiclesByTransType(transmissionType);
            return _mapper.Map<ICollection<VehicleDto>>(vehicles);
        }

        public ICollection<VehicleDto> GetVehiclesByPrice(double minPrice, double maxPrice)
        {
            var vehicles = _vehicleRepository.GetVehiclesByPrice(minPrice, maxPrice);
            return _mapper.Map<ICollection<VehicleDto>>(vehicles);
        }

        public ICollection<VehicleDto> GetVehiclesByCondition(string condition)
        {
            var vehicles = _vehicleRepository.GetVehiclesByCondition(condition);
            return _mapper.Map<ICollection<VehicleDto>>(vehicles);
        }
        
        public ICollection<VehicleDto> GetVehiclesByProdYear(int minYear, int maxYear)
        {
            var vehicles = _vehicleRepository.GetVehiclesByProdYear(minYear, maxYear);
            return _mapper.Map<ICollection<VehicleDto>>(vehicles);
        }

        public ICollection<VehicleDto> GetVehiclesByEngPower(int minPower, int maxPower)
        {
            var vehicles = _vehicleRepository.GetVehiclesByEngPower(minPower, maxPower);
            return _mapper.Map<ICollection<VehicleDto>>(vehicles);
        }

        public ICollection<VehicleDto> GetVehiclesByCubicCapacity(double minCapacity, double maxCapacity)
        {
            var vehicles = _vehicleRepository.GetVehiclesByCubCapacity(minCapacity, maxCapacity);
            return _mapper.Map<ICollection<VehicleDto>>(vehicles);
        }

        public ICollection<VehicleDto> GetVehiclesByEngine(string engine)
        {
            var vehicles = _vehicleRepository.GetVehiclesByEngine(engine);
            return _mapper.Map<ICollection<VehicleDto>>(vehicles);
        }

        public ICollection<VehicleDto> GetVehicleByState(int state)
        {
            var vehicles = _vehicleRepository.GetVehiclesByState(state);
            return _mapper.Map<ICollection<VehicleDto>>(vehicles);
        }

        public bool AddVehicle(VehicleDto vehicleDto, int companyId)
        {
            var vehicle = _mapper.Map<Vehicle>(vehicleDto);
            vehicle.Company = _companyRepository.GetCompanies().Where(c => c.Id == companyId).FirstOrDefault();
            return _vehicleRepository.AddVehicle(vehicle);
        }

        public bool UpdateVehicle(VehicleDto vehicleDto, int companyId)
        {
            var vehicle = _mapper.Map<Vehicle>(vehicleDto);
            vehicle.Company = _companyRepository.GetCompanies().Where(c => c.Id == companyId).FirstOrDefault();
            return _vehicleRepository.UpdateVehicle(vehicle);
        }

        public bool DeleteVehicle(int id)
        {
            var vehicleDto = GetVehicleById(id).FirstOrDefault();
            var vehicle = _mapper.Map<Vehicle>(vehicleDto);
            return _vehicleRepository.DeleteVehicle(vehicle);
        }
    }
}
