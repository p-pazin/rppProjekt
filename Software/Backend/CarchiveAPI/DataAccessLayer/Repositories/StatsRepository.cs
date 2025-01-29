using CarchiveAPI.Data;

namespace CarchiveAPI.Repositories
{
    public class StatsRepository
    {
        private readonly DataContext _context;

        public StatsRepository(DataContext context)
        {
            _context = context;
        }
        public int GetTotalContacts(int companyId)
        {
            return _context.Contacts.Where(c => c.Company.Id == companyId).Count();
        }
        public int GetActiveContacts(int companyId)
        {
            return _context.Contacts.Where(c => c.Company.Id == companyId && c.State == 1).Count();
        }
        public int GetInactiveContacts(int companyId)
        {
            return _context.Contacts.Where(c => c.Company.Id == companyId && c.State == 0).Count();
        }
        public int GetContactsCreatedInMonth(int companyId, int month, int year)
        {
            return _context.Contacts.Count(c => c.Company.Id == companyId && c.DateOfCreation.Month == month && c.DateOfCreation.Year == year);
        }
        public int GetInvoicesCreatedInMonth(int companyId, int month, int year)
        {
            return _context.Invoices.Count(i => i.Contract.Company.Id == companyId && i.DateOfCreation.Month == month && i.DateOfCreation.Year == year);
        }
    }
}
