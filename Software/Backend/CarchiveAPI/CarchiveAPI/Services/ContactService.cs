using System.ComponentModel.Design;
using AutoMapper;
using CarchiveAPI.Dto;
using CarchiveAPI.Models;
using CarchiveAPI.Repositories;

namespace CarchiveAPI.Services
{
    public class ContactService
    {
        private readonly ContactRepository _contactRepository;
        private readonly CompanyRepository _companyRepository;
        private readonly UserRepository _userRepository;
        private readonly UserServices _userServices;
        private readonly IMapper _mapper;
        public ContactService(ContactRepository contactRepository, CompanyRepository companyRepository, 
            UserRepository userRepository, UserServices userServices, IMapper mapper)
        {
            this._contactRepository = contactRepository;
            this._companyRepository = companyRepository;
            this._userRepository = userRepository;
            this._userServices = userServices;
            this._mapper = mapper;
        }

        public ICollection<ContactDto> GetContacts(string email)
        {
            int companyId = _userServices.GetCompanyId(email);
            var contacts = _contactRepository.GetContacts(companyId);
            return _mapper.Map<List<ContactDto>>(contacts);
        }
        public bool CheckIfContactExists(int contactId)
        {
            return _contactRepository.ContactExists(contactId);
        }
        public Contact GetContact(int contactId, string email)
        {
            int companyId = _userServices.GetCompanyId(email);
            return _contactRepository.GetContact(contactId, companyId);
        }
        public ContactDto MapContact(Contact contact)
        {
            return _mapper.Map<ContactDto>(contact);
        }
        public CompanyDto GetCompanyByContact(int contactId, string email)
        {
            int companyId = _userServices.GetCompanyId(email);
            var company = _contactRepository.GetCompanyByContact(contactId, companyId);
            return _mapper.Map<CompanyDto>(company);
        }
        public ICollection<OfferDto> GetOffersByContact(int contactId, string email)
        {
            int companyId = _userServices.GetCompanyId(email);
            var offers = _contactRepository.GetOffersByContact(contactId, companyId);
            return _mapper.Map<List<OfferDto>>(offers);
        }
        public ICollection<ContractDto> GetContractsByContact(int contactId, string email)
        {
            int companyId = _userServices.GetCompanyId(email);
            var contracts = _contactRepository.GetContractsByContact(contactId, companyId);
            return _mapper.Map<List<ContractDto>>(contracts);
        }
        public Contact GetContactByPin(string contactPin, string email)
        {
            int companyId = _userServices.GetCompanyId(email);
            return _contactRepository.GetContacts(companyId).Where(c => c.Pin == contactPin).FirstOrDefault();
        }
        public bool CreateContact(ContactDto contactDto, string email)
        {
            int companyId = _userServices.GetCompanyId(email);
            var contactMap = _mapper.Map<Contact>(contactDto);
            contactMap.Company = _companyRepository.GetCompanies().Where(c => c.Id == companyId).FirstOrDefault();

            return _contactRepository.CreateContact(contactMap);
        }
        public bool UpdateContact(ContactDto contactDto, string email)
        {
            int companyId = _userServices.GetCompanyId(email);
            var contact = _contactRepository.GetContact(contactDto.Id, companyId);
            if(contact == null)
            {
                return false;
            }
            _mapper.Map(contactDto, contact);
            contact.Company = _companyRepository.GetCompanies().Where(c => c.Id == companyId).FirstOrDefault();

            return _contactRepository.UpdateContact(contact);
        }
        public bool DeleteContact(Contact contact)
        {
            if(contact == null)
            {
                return false;
            }
            return _contactRepository.DeleteContact(contact);
        }
    }
}
