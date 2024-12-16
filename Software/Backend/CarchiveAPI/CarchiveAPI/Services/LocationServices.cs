﻿using AutoMapper;
using CarchiveAPI.Dto;
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
            var locations = _locationRepository.GetAll();
            foreach(var location in locations)
            {
                var vehicleId = location.VehicleId;
                var vehicle = _vehicleRepository.GetVehicleById(vehicleId, companyId);
                if(vehicle == null)
                {
                    locations.Remove(location);
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
