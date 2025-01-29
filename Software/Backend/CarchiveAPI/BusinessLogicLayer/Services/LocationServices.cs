using AutoMapper;
using CarchiveAPI.Dto;
using CarchiveAPI.Models;
using CarchiveAPI.Repositories;

namespace CarchiveAPI.Services
{
    public class LocationServices
    {
        private readonly LocationRepository _locationRepository;
        private readonly CompanyServices _companyServices;
        private readonly VehicleRepository _vehicleRepository;
        private readonly IMapper _mapper;

        public LocationServices(LocationRepository locationRepository, IMapper mapper, CompanyServices companyServices, VehicleRepository vehicleRepository)
        {
            _locationRepository = locationRepository;
            _vehicleRepository = vehicleRepository;
            _companyServices = companyServices;
            _mapper = mapper;
        }

        public List<LocationDto> GetAll(string email)
        {
            var companyId = _companyServices.GetCompanyId(email);
            var vehiclesIds = _vehicleRepository.GetAll(companyId).Select(v => v.Id);
            var locations = new List<Location>();
            foreach (var vehicleId in vehiclesIds)
            {
                var location = _locationRepository.GetLocationForVehicle(vehicleId);
                if(location != null)
                {
                    locations.Add(location);
                }
            }
            return _mapper.Map<List<LocationDto>>(locations);
        }

        public LocationDto GetLocationForVehicle(int vehicleId)
        {
            var location = _locationRepository.GetLocationForVehicle(vehicleId);
            return _mapper.Map<LocationDto>(location);
        }
    }
}
