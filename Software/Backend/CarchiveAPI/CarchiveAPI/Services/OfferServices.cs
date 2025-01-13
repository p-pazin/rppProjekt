﻿using AutoMapper;
using CarchiveAPI.Dto;
using CarchiveAPI.Models;
using CarchiveAPI.Repositories;

namespace CarchiveAPI.Services
{
    public class OfferServices
    {
        private readonly OfferRepository _offerRepository;
        private readonly ContactRepository _contactRepository;
        private readonly VehicleRepository _vehicleRepository;
        private readonly CompanyRepository _companyRepository;
        private readonly UserRepository _userRepository;
        private readonly CompanyServices _companyServices;
        private readonly OfferVehicleRepository _offerVehicleRepository;
        private readonly IMapper _mapper;
        public OfferServices(OfferRepository offerRepository, IMapper mapper, VehicleRepository vehicleRepository, 
            ContactRepository contactRepository, CompanyRepository companyRepository, UserRepository userRepository, 
            OfferVehicleRepository offerVehicleRepository, CompanyServices companyServices)
        {
            _offerRepository = offerRepository;
            _contactRepository = contactRepository;
            _companyRepository = companyRepository;
            _userRepository = userRepository;
            _vehicleRepository = vehicleRepository;
            _offerVehicleRepository = offerVehicleRepository;
            _companyServices = companyServices;
            _mapper = mapper;
        }
        public ICollection<OfferDto> GetOffers(string email)
        {
            int companyId = _companyServices.GetCompanyId(email);
            var offers = _offerRepository.GetAll(companyId);
            return _mapper.Map<ICollection<OfferDto>>(offers);
        }
        public ICollection<OfferDto> GetOffersByContact(int contactId, string email)
        {
            int companyId = _companyServices.GetCompanyId(email);
            var contact = _contactRepository.GetContact(contactId, companyId);
            var offers = _offerRepository.GetOffersByContact(contact);
            return _mapper.Map<ICollection<OfferDto>>(offers);
        }

        public OfferDto GetOfferById(int id, string email)
        {
            int companyId = _companyServices.GetCompanyId(email);
            var offer = _offerRepository.GetOfferById(id, companyId);
            return _mapper.Map<OfferDto>(offer);
        }

        public bool AddOffer(OfferDto offerDto, int userId, int contactId, List<int> vehiclesId, string email)
        {
            var offer = _mapper.Map<Offer>(offerDto);
            int companyId = _companyServices.GetCompanyId(email);
            offer.User = _userRepository.GetAll().Where(s => s.Id == userId).FirstOrDefault();
            offer.Contact = _contactRepository.GetContact(contactId, companyId);
            _offerRepository.Add(offer);
            _offerRepository.Save();
            bool result = false;
            
            foreach (var vehicleId in vehiclesId)
            {
                var vehicle = _vehicleRepository.GetVehicleById(vehicleId, companyId).ToList();
                OfferVehicle offerVehicle = new OfferVehicle
                {
                    OfferId = offer.Id,
                    VehicleId = vehicleId
                };
                result = _offerVehicleRepository.Add(offerVehicle);
            }

            return result;
        }

        public bool UpdateOffer(OfferDto offerDto, int userId, int contactId, List<int> vehiclesId, string email)
        {
            var offer = _mapper.Map<Offer>(offerDto);
            int companyId = _companyServices.GetCompanyId(email);
            offer.User = _userRepository.GetAll().Where(s => s.Id == userId).FirstOrDefault();
            offer.Contact = _contactRepository.GetContact(contactId, companyId);
            
            _offerRepository.Update(offer);
            bool result = false;

            foreach (var vehicleId in vehiclesId)
            {
                _offerVehicleRepository.Delete(offer.Id);
                var vehicle = _vehicleRepository.GetVehicleById(vehicleId, companyId).ToList();
                OfferVehicle offerVehicle = new OfferVehicle
                {
                    OfferId = offer.Id,
                    VehicleId = vehicleId
                };
                result = _offerVehicleRepository.Add(offerVehicle);
            }

            return result;
        }

        public bool DeleteOffer(int id, string email)
        {
            int companyId = _companyServices.GetCompanyId(email);
            var offer = _offerRepository.GetOfferById(id, companyId);
            var offerVehicles = _offerVehicleRepository.GetAllByOfferId(offer.Id);
            foreach (var offerVehicle in offerVehicles)
            {
                _offerVehicleRepository.Delete(offerVehicle.OfferId);
            }
            return _offerRepository.Delete(offer);
        }

    }
}
