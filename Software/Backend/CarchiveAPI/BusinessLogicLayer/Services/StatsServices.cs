using AutoMapper;
using CarchiveAPI.Dto;
using CarchiveAPI.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CarchiveAPI.Services
{
    public class StatsServices
    {
        private readonly StatsRepository _statsRepository;
        private readonly CompanyServices _companyServices;
        private readonly IMapper _mapper;

        public StatsServices(StatsRepository statsRepository, CompanyServices companyServices, IMapper mapper)
        {
            this._statsRepository = statsRepository;
            this._companyServices = companyServices;
            this._mapper = mapper;
        }

        public ContactStatusStatsDto GetContactStatusStats(string email)
        {
            int companyId = _companyServices.GetCompanyId(email);
            var stats = new ContactStatusStatsDto();
            stats.Total = _statsRepository.GetTotalContacts(companyId);
            stats.ActiveCount = _statsRepository.GetActiveContacts(companyId);
            stats.InactiveCount = _statsRepository.GetInactiveContacts(companyId);
            return stats;
        }
        public YearlyInfoDto GetContactCreationStats(string email)
        {
            int companyId = _companyServices.GetCompanyId(email);
            int activeYear = DateTime.Now.Year;

            var yearlyInfo = new YearlyInfoDto();
            yearlyInfo.Year = activeYear;
            yearlyInfo.Jan = _statsRepository.GetContactsCreatedInMonth(companyId, 1, activeYear);
            yearlyInfo.Feb = _statsRepository.GetContactsCreatedInMonth(companyId, 2, activeYear);
            yearlyInfo.Mar = _statsRepository.GetContactsCreatedInMonth(companyId, 3, activeYear);
            yearlyInfo.Apr = _statsRepository.GetContactsCreatedInMonth(companyId, 4, activeYear);
            yearlyInfo.May = _statsRepository.GetContactsCreatedInMonth(companyId, 5, activeYear);
            yearlyInfo.Jun = _statsRepository.GetContactsCreatedInMonth(companyId, 6, activeYear);
            yearlyInfo.Jul = _statsRepository.GetContactsCreatedInMonth(companyId, 7, activeYear);
            yearlyInfo.Aug = _statsRepository.GetContactsCreatedInMonth(companyId, 8, activeYear);
            yearlyInfo.Sep = _statsRepository.GetContactsCreatedInMonth(companyId, 9, activeYear);
            yearlyInfo.Oct = _statsRepository.GetContactsCreatedInMonth(companyId, 10, activeYear);
            yearlyInfo.Nov = _statsRepository.GetContactsCreatedInMonth(companyId, 11, activeYear);
            yearlyInfo.Dec = _statsRepository.GetContactsCreatedInMonth(companyId, 12, activeYear);
            return yearlyInfo;
        }

        public YearlyInfoDto GetInvoiceCreationStats(string email)
        {
            int companyId = _companyServices.GetCompanyId(email);
            int activeYear = DateTime.Now.Year;

            var yearlyInfo = new YearlyInfoDto();
            yearlyInfo.Year = activeYear;
            yearlyInfo.Jan = _statsRepository.GetInvoicesCreatedInMonth(companyId, 1, activeYear);
            yearlyInfo.Feb = _statsRepository.GetInvoicesCreatedInMonth(companyId, 2, activeYear);
            yearlyInfo.Mar = _statsRepository.GetInvoicesCreatedInMonth(companyId, 3, activeYear);
            yearlyInfo.Apr = _statsRepository.GetInvoicesCreatedInMonth(companyId, 4, activeYear);
            yearlyInfo.May = _statsRepository.GetInvoicesCreatedInMonth(companyId, 5, activeYear);
            yearlyInfo.Jun = _statsRepository.GetInvoicesCreatedInMonth(companyId, 6, activeYear);
            yearlyInfo.Jul = _statsRepository.GetInvoicesCreatedInMonth(companyId, 7, activeYear);
            yearlyInfo.Aug = _statsRepository.GetInvoicesCreatedInMonth(companyId, 8, activeYear);
            yearlyInfo.Sep = _statsRepository.GetInvoicesCreatedInMonth(companyId, 9, activeYear);
            yearlyInfo.Oct = _statsRepository.GetInvoicesCreatedInMonth(companyId, 10, activeYear);
            yearlyInfo.Nov = _statsRepository.GetInvoicesCreatedInMonth(companyId, 11, activeYear);
            yearlyInfo.Dec = _statsRepository.GetInvoicesCreatedInMonth(companyId, 12, activeYear);
            return yearlyInfo;
        }
    }
}
