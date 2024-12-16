using AutoMapper;
using CarchiveAPI.Dto;
using CarchiveAPI.Repositories;

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
            return _statsRepository.GetContactStatusStats(companyId);
        }
        public YearlyInfoDto GetContactCreationStats(string email)
        {
            int companyId = _companyServices.GetCompanyId(email);
            int activeYear = DateTime.Now.Year;
            return _statsRepository.GetContactCreationStats(companyId, activeYear);
        }

        public YearlyInfoDto GetInvoiceCreationStats(string email)
        {
            int companyId = _companyServices.GetCompanyId(email);
            int activeYear = DateTime.Now.Year;
            return _statsRepository.GetInvoiceCreationStats(companyId, activeYear);
        }
    }
}
