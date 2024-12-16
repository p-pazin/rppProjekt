using AutoMapper;
using CarchiveAPI.Dto;
using CarchiveAPI.Repositories;

namespace CarchiveAPI.Services
{
    public class LocationServices
    {
        private readonly LocationRepository _locationRepository;
        private readonly IMapper _mapper;

        public LocationServices(LocationRepository locationRepository, IMapper mapper)
        {
            _locationRepository = locationRepository;
            _mapper = mapper;
        }

        public List<LocationDto> GetAll()
        {
            var locations = _locationRepository.GetAll();
            return _mapper.Map<List<LocationDto>>(locations);
        }

        public LocationDto GetLocationForVehicle(int vehicleId)
        {
            var location = _locationRepository.GetLocationForVehicle(vehicleId);
            return _mapper.Map<LocationDto>(location);
        }
    }
}
