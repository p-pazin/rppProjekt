
using AutoMapper;
using CarchiveAPI.Data;
using CarchiveAPI.Dto;
using CarchiveAPI.Models;
using CarchiveAPI.Repositories;

namespace CarchiveAPI.Services
{
    public class AdServices
    {
        private DataContext _context;
        private AdRepository _adRepository;
        private CompanyServices _companyServices;
        private UserRepository _userRepository;
        private VehicleRepository _vehicleRepository;
        private readonly IMapper _mapper;
        public AdServices(DataContext context, AdRepository adRepository, CompanyServices companyServices,
            UserRepository userRepository, VehicleRepository vehicleRepository, IMapper mapper)
        {
            this._context = context;
            this._adRepository = adRepository;
            this._companyServices = companyServices;
            this._userRepository = userRepository;
            this._vehicleRepository = vehicleRepository;
            this._mapper = mapper;
        }
        public ICollection<AdDto> GetAds(string email)
        {
            int companyId = _companyServices.GetCompanyId(email);
            var ads = _adRepository.GetAll(companyId);
            return _mapper.Map<List<AdDto>>(ads);
        }
        public ICollection<IndexAdDto> GetIndexAds(int id)
        {
            var ads = _adRepository.GetAll(id);
            return _mapper.Map<List<IndexAdDto>>(ads);
        }

        public AdDto GetAd(int id, string email)
        {
            int companyId = _companyServices.GetCompanyId(email);
            var ad = _adRepository.Get(id, companyId);
            return _mapper.Map<AdDto>(ad);
        }

        public bool AddAd(AdDto newAdDto, string email, int id)
        {
            int companyId = _companyServices.GetCompanyId(email);
            var user = _userRepository.GetUserAndCompanyByEmail(email);
            var vehicle = _vehicleRepository.GetOneVehicleById(id, companyId);
            if (vehicle == null)
            {
                return false;
            }
            Ad ad = new Ad
            {
                Title = newAdDto.Title,
                Description = newAdDto.Description,
                PaymentMethod = newAdDto.PaymentMethod,
                DateOfPublishment = newAdDto.DateOfPublishment,
                User = user,
                Vehicle = vehicle
            };
            return _adRepository.AddAd(ad, companyId);
        }

        public bool UpdateAd(AdDto adDto, string email)
        {
            int companyId = _companyServices.GetCompanyId(email);
            var ad = _adRepository.Get(adDto.Id, companyId);
            ad.Title = adDto.Title;
            ad.Description = adDto.Description;
            ad.PaymentMethod = adDto.PaymentMethod;
            return _adRepository.UpdateAd(ad);
        }

        public bool DeleteAd(int adId, string email)
        {
            int companyId = _companyServices.GetCompanyId(email);
            var ad = _adRepository.Get(adId, companyId);
            if (ad == null)
            {
                return false;
            }
            if (ad.User.Company.Id != companyId)
            {
                return false;
            }
            return _adRepository.DeleteAd(ad);
        }



    }
}
