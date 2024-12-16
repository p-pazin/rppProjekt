using AutoMapper;
using CarchiveAPI.Dto;
using CarchiveAPI.Repositories;

namespace CarchiveAPI.Services
{
    public class StatsServices
    {
        private readonly StatsRepository _statsRepository;
        private readonly UserServices _userServices;
        private readonly IMapper _mapper;

        public StatsServices(StatsRepository statsRepository, UserServices userServices, IMapper mapper)
        {
            this._statsRepository = statsRepository;
            this._userServices = userServices;
            this._mapper = mapper;
        }

        public ContactStatusStatsDto GetContactStatusStats(string email)
        {
            int companyId = _userServices.GetCompanyId(email);
            return _statsRepository.GetContactStatusStats(companyId);
        }
        public YearlyInfoDto GetContactCreationStats(string email)
        {
            int companyId = _userServices.GetCompanyId(email);
            int activeYear = DateTime.Now.Year;
            return _statsRepository.GetContactCreationStats(companyId, activeYear);
        }

        public YearlyInfoDto GetInvoiceCreationStats(string email)
        {
            int companyId = _userServices.GetCompanyId(email);
            int activeYear = DateTime.Now.Year;
            return _statsRepository.GetInvoiceCreationStats(companyId, activeYear);
        }
    }
}
