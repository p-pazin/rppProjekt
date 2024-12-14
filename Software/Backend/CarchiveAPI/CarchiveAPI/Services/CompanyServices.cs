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
        private readonly IMapper _mapper;

        public CompanyServices(DataContext context, IMapper mapper)
        {
            this._context = context;
            this._companyRepository = new CompanyRepository(context);
            this._mapper = mapper;
        }

        public ICollection<CompanyDto> GetCompanies()
        {
            var companiesDto = _mapper.Map<List<CompanyDto>>(_companyRepository.GetCompanies());
            return companiesDto;
        }

        public CompanyDto GetCompany(int companyId)
        {
            var company = _companyRepository.GetCompanyById(companyId);
            if (company == null)
            {
                return null;
            }
            return new CompanyDto
            {
                Id = company.Id,
                Name = company.Name,
                City = company.City,
                Address = company.Address,
                Pin = company.Pin
            };
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
                Password = newCompanyDto.Password,
                Role = admin,
                Company = company
            };

            if (_companyRepository.newAdminExists(user.Email))
            {
                return false;
            }
            bool added = _companyRepository.AddCompany(company);
            if (added)
            {
                return true;
            }
            return false;


        }
    }
}
