using CarchiveAPI.Data;
using CarchiveAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CarchiveAPI.Repositories
{
    public class ContractRepository
    {
        private DataContext _context;

        public ContractRepository(DataContext context)
        {
            this._context = context;
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
        public Contract GetContractsRent(int contractId, int companyId)
        {
            return _context.Contracts
                .Include(c => c.Company)
                .Include(c => c.Contact)
                .Include(c => c.Vehicle)
                .Include(c => c.Offer)
                .Include(c => c.User)
                .Include(c => c.Reservation)
                .Include(c => c.Insurance)
                .Where(c => c.Id == contractId && c.Company.Id == companyId).FirstOrDefault();
        }
        public Contract GetContract(int contractId, int companyId)
        {
            return _context.Contracts.Where(c => c.Id == contractId && c.Company.Id == companyId).FirstOrDefault();
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

        public bool AddContract(Contract contract)
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
