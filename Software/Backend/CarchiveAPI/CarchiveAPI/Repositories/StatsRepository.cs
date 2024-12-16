using CarchiveAPI.Data;
using CarchiveAPI.Dto;

namespace CarchiveAPI.Repositories
{
    public class StatsRepository
    {
        private DataContext _context;

        public StatsRepository(DataContext context)
        {
            this._context = context;
        }

        public ContactStatusStatsDto GetContactStatusStats(int companyId)
        {
            var stats = new ContactStatusStatsDto();
            stats.Total = _context.Contacts.Where(c => c.Company.Id == companyId).Count();
            stats.ActiveCount = _context.Contacts.Where(c => c.Company.Id == companyId && c.State == 1).Count();
            stats.InactiveCount = _context.Contacts.Where(c => c.Company.Id == companyId && c.State == 0).Count();
            return stats;
        }

        public YearlyInfoDto GetContactCreationStats(int companyId, int activeYear)
        {
            var yearlyInfo = new YearlyInfoDto();
            yearlyInfo.Year = activeYear;
            yearlyInfo.Jan = _context.Contacts.Where(c => c.Company.Id == companyId && c.DateOfCreation.Month == 1 && 
            c.DateOfCreation.Year == activeYear).Count();
            yearlyInfo.Feb = _context.Contacts.Where(c => c.Company.Id == companyId && c.DateOfCreation.Month == 2 &&
            c.DateOfCreation.Year == activeYear).Count();
            yearlyInfo.Mar = _context.Contacts.Where(c => c.Company.Id == companyId && c.DateOfCreation.Month == 3 &&
            c.DateOfCreation.Year == activeYear).Count();
            yearlyInfo.Apr = _context.Contacts.Where(c => c.Company.Id == companyId && c.DateOfCreation.Month == 4 &&
            c.DateOfCreation.Year == activeYear).Count();
            yearlyInfo.May = _context.Contacts.Where(c => c.Company.Id == companyId && c.DateOfCreation.Month == 5 &&
            c.DateOfCreation.Year == activeYear).Count();
            yearlyInfo.Jun = _context.Contacts.Where(c => c.Company.Id == companyId && c.DateOfCreation.Month == 6 &&
            c.DateOfCreation.Year == activeYear).Count();
            yearlyInfo.Jul = _context.Contacts.Where(c => c.Company.Id == companyId && c.DateOfCreation.Month == 7 &&
            c.DateOfCreation.Year == activeYear).Count();
            yearlyInfo.Aug = _context.Contacts.Where(c => c.Company.Id == companyId && c.DateOfCreation.Month == 8 &&
            c.DateOfCreation.Year == activeYear).Count();
            yearlyInfo.Sep = _context.Contacts.Where(c => c.Company.Id == companyId && c.DateOfCreation.Month == 9 &&
            c.DateOfCreation.Year == activeYear).Count();
            yearlyInfo.Oct = _context.Contacts.Where(c => c.Company.Id == companyId && c.DateOfCreation.Month == 10 &&
            c.DateOfCreation.Year == activeYear).Count();
            yearlyInfo.Nov = _context.Contacts.Where(c => c.Company.Id == companyId && c.DateOfCreation.Month == 11 &&
            c.DateOfCreation.Year == activeYear).Count();
            yearlyInfo.Dec = _context.Contacts.Where(c => c.Company.Id == companyId && c.DateOfCreation.Month == 12 &&
            c.DateOfCreation.Year == activeYear).Count();
            return yearlyInfo;
        }
        public YearlyInfoDto GetInvoiceCreationStats(int companyId, int activeYear)
        {
            var yearlyInfo = new YearlyInfoDto();
            yearlyInfo.Year = activeYear;
            yearlyInfo.Jan = _context.Invoices.Where(i => i.Contract.Company.Id == companyId && i.DateOfCreation.Month == 1 
            && i.DateOfCreation.Year == activeYear).Count();
            yearlyInfo.Feb = _context.Invoices.Where(i => i.Contract.Company.Id == companyId && i.DateOfCreation.Month == 2
            && i.DateOfCreation.Year == activeYear).Count();
            yearlyInfo.Mar = _context.Invoices.Where(i => i.Contract.Company.Id == companyId && i.DateOfCreation.Month == 3
            && i.DateOfCreation.Year == activeYear).Count();
            yearlyInfo.Apr = _context.Invoices.Where(i => i.Contract.Company.Id == companyId && i.DateOfCreation.Month == 4
            && i.DateOfCreation.Year == activeYear).Count();
            yearlyInfo.May = _context.Invoices.Where(i => i.Contract.Company.Id == companyId && i.DateOfCreation.Month == 5
            && i.DateOfCreation.Year == activeYear).Count();
            yearlyInfo.Jun = _context.Invoices.Where(i => i.Contract.Company.Id == companyId && i.DateOfCreation.Month == 6
            && i.DateOfCreation.Year == activeYear).Count();
            yearlyInfo.Jul = _context.Invoices.Where(i => i.Contract.Company.Id == companyId && i.DateOfCreation.Month == 7
            && i.DateOfCreation.Year == activeYear).Count();
            yearlyInfo.Aug = _context.Invoices.Where(i => i.Contract.Company.Id == companyId && i.DateOfCreation.Month == 8
            && i.DateOfCreation.Year == activeYear).Count();
            yearlyInfo.Sep = _context.Invoices.Where(i => i.Contract.Company.Id == companyId && i.DateOfCreation.Month == 9
            && i.DateOfCreation.Year == activeYear).Count();
            yearlyInfo.Oct = _context.Invoices.Where(i => i.Contract.Company.Id == companyId && i.DateOfCreation.Month == 10
            && i.DateOfCreation.Year == activeYear).Count();
            yearlyInfo.Nov = _context.Invoices.Where(i => i.Contract.Company.Id == companyId && i.DateOfCreation.Month == 11
            && i.DateOfCreation.Year == activeYear).Count();
            yearlyInfo.Dec = _context.Invoices.Where(i => i.Contract.Company.Id == companyId && i.DateOfCreation.Month == 12
            && i.DateOfCreation.Year == activeYear).Count();
            return yearlyInfo;
        }
    }
}
