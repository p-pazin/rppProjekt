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
        private EmailService _emailService;
        private readonly IMapper _mapper;

        public CompanyServices(DataContext context, IMapper mapper, UserRepository userRepository, EmailService emailService)
        {
            this._context = context;
            this._companyRepository = new CompanyRepository(context);
            this._mapper = mapper;
            _userRepository = userRepository;
            _emailService = emailService;
        }

        public ICollection<CompanyDto> GetCompanies()
        {
            var companiesDto = _mapper.Map<List<CompanyDto>>(_companyRepository.GetCompanies());
            return companiesDto;
        }

        public CompanyDto GetCompany(string email)
        {
            var user = _userRepository.GetUserAndCompanyByEmail(email);
            var companyDto = _mapper.Map<CompanyDto>(user.Company);
            return companyDto;
        }

        public bool CompanyExists(string name)
        {
            return _companyRepository.CompanyExists(name);
        }

        public bool CompanyRegistered(string companyPin)
        {
            return _companyRepository.CompanyRegistered(companyPin);
        }

        public int GetCompanyId(string email)
        {
            var user = _userRepository.GetUserAndCompanyByEmail(email);
            var companyId = user.Company.Id;
            return companyId;
        }

        public ICollection<UserDto> GetCompanyWorkers(string email)
        {
            var admin = _userRepository.GetUserAndCompanyByEmail(email);
            Company company = admin.Company;
            admin.Company.Users = _companyRepository.GetCompanyWorkers(company);
            var usersDto = _mapper.Map<ICollection<UserDto>>(admin.Company.Users);
            return usersDto;
        }

        public bool AddCompany(NewCompanyDto newCompanyDto)
        {
            Company company = new Company
            {
                Name = newCompanyDto.Name,
                City = newCompanyDto.City,
                Address = newCompanyDto.Address,
                Pin = newCompanyDto.Pin,
                Approved = 0
            };
            if (CompanyRegistered(company.Pin))
            {
                return false;
            }
            if (CompanyExists(company.Name))
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

        public async Task<bool> AddCompanyAsync(NewCompanyDto newCompanyDto)
        {
            Company company = new Company
            {
                Name = newCompanyDto.Name,
                City = newCompanyDto.City,
                Address = newCompanyDto.Address,
                Pin = newCompanyDto.Pin,
                Approved = 0
            };

            if (CompanyRegistered(company.Pin) || CompanyExists(company.Name))
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
                var approvalLink = $"https://carchive.online/api/Company/approve-company/{company.Id}";

                var emailBody = $@"
                <h1>Novi zahtjev za registraciju</h1>
                <p><strong>Ime firme:</strong> {company.Name}</p>
                <p><strong>Grad:</strong> {company.City}</p>
                <p><strong>Adresa:</strong> {company.Address}</p>
                <p><strong>Pin:</strong> {company.Pin}</p>
                <p><strong>Kontakt osoba:</strong> {user.FirstName} {user.LastName}</p>
                <p><strong>Email:</strong> {user.Email}</p>
                <a href='{approvalLink}'> Odobri firmu </a> ";

                await _emailService.SendEmailAsync("dvucina22@carchive.online", "Novi zahtjev za registraciju", emailBody);

                return true;
            }

            return false;
        }

        public async Task<bool> ApproveCompanyAsync(int companyId)
        {
            var company = _companyRepository.GetCompanyById(companyId);
            if (company == null)
            {
                return false;
            }

            company.Approved = 1;
            var result = _companyRepository.UpdateCompany(company);
            if (!result)
            {
                return false;
            }

            var adminUser = _userRepository.GetUserByAdminRoleAndCheckCompany(companyId);
            if (adminUser != null)
            {
                var emailSubject = "Vaša firma je odobrena!";
                var emailBody = $@"
            <h1>Čestitamo!</h1>
            <p>Vaša firma <strong>{company.Name}</strong> je uspješno odobrena.</p>
            <p>Sada možete pristupiti svim značajkama naše platforme.</p>
            <p>Za dodatne informacije slobodno nas kontaktirajte.</p>";

                await _emailService.SendEmailAsync(adminUser.Email, emailSubject, emailBody);
            }

            return true;
        }



    }
}
