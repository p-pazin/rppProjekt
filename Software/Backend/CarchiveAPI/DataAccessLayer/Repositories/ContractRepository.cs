using AutoMapper;
using CarchiveAPI.Data;
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
            return _context.Contracts.Include(c => c.Company)
                .Include(c => c.Contact)
                .Include(c => c.Vehicle)
                .Include(c => c.Offer)
                .Include(c => c.User)
                .Include(c => c.Reservation)
                .Include(c => c.Insurance)
                .Where(c => c.Company.Id == companyId).ToList();
        }

        public ICollection<Contract> GetUnsignedContracts(int companyId)
        {
            return _context.Contracts.Include(c => c.Company)
                .Include(c => c.Contact)
                .Include(c => c.Vehicle)
                .Include(c => c.Offer)
                .Include(c => c.User)
                .Include(c => c.Reservation)
                .Include(c => c.Insurance)
                .Where(c => c.Company.Id == companyId && c.Signed == 0).ToList();
        }
        public Contract GetContractsRent(int contractId, int companyId)
        {
            return _context.Contracts
                .Include(c => c.Company)
                .Include(c => c.User)
                .Include(c => c.Reservation)
                .Include(c => c.Insurance)
                .Where(c => c.Id == contractId && c.Company.Id == companyId).FirstOrDefault();
        }
        public Contract GetContract(int contractId, int companyId)
        {
            return _context.Contracts.Include(c => c.Contact).Include(c => c.Vehicle)
                .Where(c => c.Id == contractId && c.Company.Id == companyId).FirstOrDefault();
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

        public bool AddContract(Contract contract)
        {
            _context.Contracts.Add(contract);
            return Save();
        }
    }
}
