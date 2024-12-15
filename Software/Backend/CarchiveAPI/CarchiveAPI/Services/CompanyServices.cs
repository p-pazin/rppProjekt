using AutoMapper;
using CarchiveAPI.Data;
using CarchiveAPI.Dto;
using CarchiveAPI.Models;
using CarchiveAPI.Repositories;

namespace CarchiveAPI.Services
{
    public class CompanyServices
    {
        private DataContext _context;
        private CompanyRepository _companyRepository;
        private UserRepository _userRepository;
        private readonly IMapper _mapper;

        public CompanyServices(DataContext context, IMapper mapper, UserRepository userRepository)
        {
            this._context = context;
            this._companyRepository = new CompanyRepository(context);
            this._mapper = mapper;
            _userRepository = userRepository;
        }

        public ICollection<CompanyDto> GetCompanies()
        {
            var companiesDto = _mapper.Map<List<CompanyDto>>(_companyRepository.GetCompanies());
            return companiesDto;
        }

        public CompanyDto GetCompany(string email)
        {
            var user = _userRepository.GetUserAndCompanyByEmail(email);
            var contacts = user.Company.Contacts;
            var companyDto = _mapper.Map<CompanyDto>(user.Company);
            return companyDto;
        }

        public bool CompanyExists(string companyPin)
        {
            return _companyRepository.CompanyExists(companyPin);
        }

        public bool AddCompany(NewCompanyDto newCompanyDto)
        {
            Company company = new Company
            {
                Name = newCompanyDto.Name,
                City = newCompanyDto.City,
                Address = newCompanyDto.Address,
                Pin = newCompanyDto.Pin
            };
            if (CompanyExists(company.Pin))
            {
                return false;
            }

            var admin = _companyRepository.getAdminRole();

            User user = new User
            {
                FirstName = newCompanyDto.FirstName,
                LastName = newCompanyDto.LastName,
                Email = newCompanyDto.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(newCompanyDto.Password),
                Role = admin,
                Company = company
            };

            if (_companyRepository.newAdminExists(user.Email))
            {
                return false;
            }
            bool addedCompany = _companyRepository.AddCompany(company);
            bool addedUser = _userRepository.AddUser(user);
            if (addedCompany && addedUser)
            {
                return true;
            }
            return true;
        }
    }
}
