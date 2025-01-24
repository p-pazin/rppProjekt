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
        private readonly OfferRepository _offerRepository;
        private readonly CompanyServices _companyServices;
        private readonly OfferVehicleRepository _offerVehicleRepository;
        private readonly AdRepository _adRepository;
        private readonly IMapper _mapper;

        public VehicleServices(VehicleRepository vehicleRepository, AdRepository adRepository, OfferVehicleRepository offerVehicleRepository, IMapper mapper, CompanyRepository companyRepository, OfferRepository offerRepository, CompanyServices companyServices)
        {
            _vehicleRepository = vehicleRepository;
            _mapper = mapper;
            _companyRepository = companyRepository;
            _companyServices = companyServices;
            _offerRepository = offerRepository;
            _offerVehicleRepository = offerVehicleRepository;
            _adRepository = adRepository;
        }

        public ICollection<VehicleDto> GetAllCatalog(string email)
        {
            int companyId = _companyServices.GetCompanyId(email);
            var vehicles = _vehicleRepository.GetAllCatalog(companyId);
            return _mapper.Map<ICollection<VehicleDto>>(vehicles);
        }

        public ICollection<VehicleDto> GetAll(string email)
        {
            int companyId = _companyServices.GetCompanyId(email);
            var vehicles = _vehicleRepository.GetAll(companyId);
            return _mapper.Map<ICollection<VehicleDto>>(vehicles);
        }

        public ICollection<VehicleDto> GetAllSale(string email)
        {
            int companyId = _companyServices.GetCompanyId(email);
            var vehicles = _vehicleRepository.GetAllSale(companyId);
            return _mapper.Map<ICollection<VehicleDto>>(vehicles);
        }

        public ICollection<VehicleDto> GetAllRent(string email)
        {
            int companyId = _companyServices.GetCompanyId(email);
            var vehicles = _vehicleRepository.GetAllRent(companyId);
            return _mapper.Map<ICollection<VehicleDto>>(vehicles);
        }

        public VehicleDto GetVehicleById(int id, string email)
        {
            int companyId = _companyServices.GetCompanyId(email);
            var vehicle = _vehicleRepository.GetVehicleById(id, companyId);
            return _mapper.Map<VehicleDto>(vehicle);
        }

        public VehicleDto  GetVehicleIdByRegistration(string reg, string email)
        {
            int companyId = _companyServices.GetCompanyId(email);
            var vehicle = _vehicleRepository.GetVehicleIdByRegistration(reg, companyId);
            return _mapper.Map<VehicleDto>(vehicle);
        }

        public ICollection<VehicleDto> GetVehiclesByModel(string model, string email)
        {
            int companyId = _companyServices.GetCompanyId(email);
            var vehicles = _vehicleRepository.GetVehiclesByModel(model, companyId);
            return _mapper.Map<ICollection<VehicleDto>>(vehicles);
        }

        public ICollection<VehicleDto> GetVehiclesByRegistration(string reg, string email)
        {
            int companyId = _companyServices.GetCompanyId(email);
            var vehicles = _vehicleRepository.GetVehiclesByRegistration(reg, companyId);
            return _mapper.Map<ICollection<VehicleDto>>(vehicles);
        }

        public ICollection<VehicleDto> GetVehiclesByType(string type, string email)
        {
            int companyId = _companyServices.GetCompanyId(email);
            var vehicles = _vehicleRepository.GetVehiclesByType(type, companyId);
            return _mapper.Map<ICollection<VehicleDto>>(vehicles);
        }

        public ICollection<VehicleDto> GetVehiclesByColor(string color, string email)
        {
            int companyId = _companyServices.GetCompanyId(email);
            var vehicles = _vehicleRepository.GetVehiclesByColor(color, companyId);
            return _mapper.Map<ICollection<VehicleDto>>(vehicles);
        }

        public ICollection<VehicleDto> GetVehiclesByMileage(int minMileage, int maxMileage, string email)
        {
            int companyId = _companyServices.GetCompanyId(email);
            var vehicles = _vehicleRepository.GetVehiclesByMileage(minMileage, maxMileage, companyId);
            return _mapper.Map<ICollection<VehicleDto>>(vehicles);
        }

        public ICollection<VehicleDto> GetVehiclesByTransType(string transmissionType, string email)
        {
            int companyId = _companyServices.GetCompanyId(email);
            var vehicles = _vehicleRepository.GetVehiclesByTransType(transmissionType, companyId);
            return _mapper.Map<ICollection<VehicleDto>>(vehicles);
        }

        public ICollection<VehicleDto> GetVehiclesByPrice(double minPrice, double maxPrice, string email)
        {
            int companyId = _companyServices.GetCompanyId(email);
            var vehicles = _vehicleRepository.GetVehiclesByPrice(minPrice, maxPrice, companyId);
            return _mapper.Map<ICollection<VehicleDto>>(vehicles);
        }

        public ICollection<VehicleDto> GetVehiclesByCondition(string condition, string email)
        {
            int companyId = _companyServices.GetCompanyId(email);
            var vehicles = _vehicleRepository.GetVehiclesByCondition(condition, companyId);
            return _mapper.Map<ICollection<VehicleDto>>(vehicles);
        }
        
        public ICollection<VehicleDto> GetVehiclesByProdYear(int minYear, int maxYear, string email)
        {
            int companyId = _companyServices.GetCompanyId(email);
            var vehicles = _vehicleRepository.GetVehiclesByProdYear(minYear, maxYear, companyId);
            return _mapper.Map<ICollection<VehicleDto>>(vehicles);
        }

        public ICollection<VehicleDto> GetVehiclesByEngPower(int minPower, int maxPower, string email)
        {
            int companyId = _companyServices.GetCompanyId(email);
            var vehicles = _vehicleRepository.GetVehiclesByEngPower(minPower, maxPower, companyId);
            return _mapper.Map<ICollection<VehicleDto>>(vehicles);
        }

        public ICollection<VehicleDto> GetVehiclesByCubicCapacity(double minCapacity, double maxCapacity, string email)
        {
            int companyId = _companyServices.GetCompanyId(email);
            var vehicles = _vehicleRepository.GetVehiclesByCubCapacity(minCapacity, maxCapacity, companyId);
            return _mapper.Map<ICollection<VehicleDto>>(vehicles);
        }

        public ICollection<VehicleDto> GetVehiclesByEngine(string engine, string email)
        {
            int companyId = _companyServices.GetCompanyId(email);
            var vehicles = _vehicleRepository.GetVehiclesByEngine(engine, companyId);
            return _mapper.Map<ICollection<VehicleDto>>(vehicles);
        }

        public ICollection<VehicleDto> GetVehicleByState(int state, string email)
        {
            int companyId = _companyServices.GetCompanyId(email);
            var vehicles = _vehicleRepository.GetVehiclesByState(state, companyId);
            return _mapper.Map<ICollection<VehicleDto>>(vehicles);
        }

        public ICollection<VehicleDto> GetVehiclesByOffer(string email, int offerId)
        {
            var companyId = _companyServices.GetCompanyId(email);
            var offer = _offerRepository.GetOfferById(offerId, companyId);
            var vehicles = _vehicleRepository.GetVehiclesByOffer(offer);
            return _mapper.Map<ICollection<VehicleDto>>(vehicles);
        }

        public bool AddVehicle(VehicleDto vehicleDto, string email)
        {
            int companyId = _companyServices.GetCompanyId(email);
            var vehicle = _mapper.Map<Vehicle>(vehicleDto);
            vehicle.Company = _companyRepository.GetCompanies().Where(c => c.Id == companyId).FirstOrDefault();
            return _vehicleRepository.AddVehicle(vehicle);
        }

        public bool UpdateVehicle(VehicleDto vehicleDto, string email)
        {
            int companyId = _companyServices.GetCompanyId(email);
            var vehicle = _mapper.Map<Vehicle>(vehicleDto);
            vehicle.Company = _companyRepository.GetCompanies().Where(c => c.Id == companyId).FirstOrDefault();
            return _vehicleRepository.UpdateVehicle(vehicle);
        }

        public bool DeleteVehicle(int id, string email)
        {
            var vehicleDto = GetVehicleById(id, email);

            var vehicle = _mapper.Map<Vehicle>(vehicleDto);
            return _vehicleRepository.DeleteVehicle(vehicle);
        }

        public bool CheckIfVehicleExists(int id)
        {
            return _vehicleRepository.CheckIfVehicleExists(id);
        }

        public bool ConnectVehicleToPhoto(int vehicleId, string photoUrl)
        {
            return _vehicleRepository.ConnectVehicleToPhoto(vehicleId, photoUrl);
        }

        public ICollection<VehiclePhotoDto> GetVehiclePhotos(int vehicleId, string email)
        {
            var companyId = _companyServices.GetCompanyId(email);
            var photos = _vehicleRepository.GetVehiclePhotos(vehicleId);
            return _mapper.Map<ICollection<VehiclePhotoDto>>(photos);
        }

        public bool DeleteVehiclePhoto(int photoId)
        {
            return _vehicleRepository.DeleteVehiclePhoto(photoId);
        }
    }
}
