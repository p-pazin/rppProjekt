﻿using AutoMapper;
using CarchiveAPI.Data;
using CarchiveAPI.Dto;
using CarchiveAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CarchiveAPI.Repositories
{
    public class ContractRepository
    {
        private DataContext _context;
        private IMapper _mapper;

        public ContractRepository(DataContext context, IMapper mapper)
        {
            this._context = context;
            _mapper = mapper;
        }

        public ICollection<Contract> GetContracts(int companyId)
        {
            return _context.Contracts
                .Where(c => c.Company.Id == companyId)
                .Include(c => c.Company)
                .Include(c => c.Vehicle)
                .Include(c => c.Contact)
                .Include(c => c.Offer)
                    .ThenInclude(o => o.Contact)
                .Include(c => c.User)
                .Include(c => c.Reservation)
                .Include(c => c.Insurance)
                .Include(c => c.Offer)
                    .ThenInclude(o => o.OfferVehicles)
                    .ThenInclude(ov => ov.Vehicle)
                .ToList();
        }
        public Contract GetSaleContract(int contractId, int companyId)
        {
            return _context.Contracts
                .Where(c => c.Company.Id == companyId && c.Id == contractId)
                .Include(c => c.Company)
                .Include(c => c.Vehicle)
                .Include(c => c.Contact)
                .Include(c => c.Offer)
                    .ThenInclude(o => o.Contact)
                .Include(c => c.User)
                .Include(c => c.Reservation)
                .Include(c => c.Insurance)
                .Include(c => c.Offer)
                    .ThenInclude(o => o.OfferVehicles)
                    .ThenInclude(ov => ov.Vehicle)
                .FirstOrDefault();
        }

        public SaleContractDto GetSaleContractDto(int contractId, int companyId)
        {

            var contract = _context.Contracts.FirstOrDefault(c => c.Company.Id == companyId && c.Id == contractId);

            var company = _context.Companies.FirstOrDefault(c => c.Id == companyId);

            var offer = _context.Offers.Include(o => o.User).Include(o => o.Contact).FirstOrDefault(o => o.Id == contract.OfferId);

            var contact = _context.Contacts.FirstOrDefault(c => c.Id == offer.Contact.Id);

            var user = _context.Users.FirstOrDefault(u => u.Id == offer.User.Id);

            var offerVehicles = _context.OffersVehicles.Where(ov => ov.OfferId == offer.Id).ToList();

            List<Vehicle> vehicles = new List<Vehicle>();

            foreach (var offerVehicle in offerVehicles)
            {
                var vehicle = _context.Vehicles.First(v => v.Id == offerVehicle.VehicleId);
                vehicles.Add(vehicle);
            }

            var mappedVehicles = _mapper.Map<List<VehicleDto>>(vehicles);

            var saleContractDto = new SaleContractDto
            {
                Id = contract.Id,
                Title = contract.Title,
                Place = contract.Place,
                DateOfCreation = contract.DateOfCreation,
                Type = contract.Type,
                Content = contract.Content,
                Signed = contract.Signed,
                CompanyName = company.Name,
                CompanyAddress = company.Address,
                CompanyPin = company.Pin,
                ContactName = contact.LastName + " " + contact.FirstName,
                ContactAddress = contact.Address,
                ContactPin = contact.Pin,
                UserName = user.FirstName + " " + user.LastName,
                Price = offer.Price,
                Vehicles = mappedVehicles
            };

            return saleContractDto;
        }
        public bool DeleteContract(Contract contract)
        {
            _context.Contracts.Remove(contract);
            return Save();
        }
        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }
        public bool ContractExists(int contractId)
        {
            return _context.Contracts.Any(c => c.Id == contractId);
        }
        public bool CreateContractForSale(Contract contract)
        {
            _context.Contracts.Add(contract);
            return Save();
        }
        public bool UpdateContract(Contract contract)
        {
            _context.Contracts.Update(contract);
            return Save();
        }
    }
}
