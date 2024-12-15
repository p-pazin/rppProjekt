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
        private readonly IMapper _mapper;
        public ContactService(ContactRepository contactRepository, CompanyRepository companyRepository, IMapper mapper)
        {
            this._contactRepository = contactRepository;
            this._companyRepository = companyRepository;
            this._mapper = mapper;
        }

        public ICollection<ContactDto> GetContacts()
        {
            var contacts = _contactRepository.GetContacts();
            return _mapper.Map<List<ContactDto>>(contacts);
        }
        public bool CheckIfContactExists(int contactId)
        {
            return _contactRepository.ContactExists(contactId);
        }
        public Contact GetContact(int contactId)
        {
            return _contactRepository.GetContact(contactId);
        }
        public ContactDto MapContact(Contact contact)
        {
            return _mapper.Map<ContactDto>(contact);
        }
        public CompanyDto GetCompanyByContact(int contactId)
        {
            var company = _contactRepository.GetCompanyByContact(contactId);
            return _mapper.Map<CompanyDto>(company);
        }
        public ICollection<OfferDto> GetOffersByContact(int contactId)
        {
            var offers = _contactRepository.GetOffersByContact(contactId);
            return _mapper.Map<List<OfferDto>>(offers);
        }
        public ICollection<ContractDto> GetContractsByContact(int contactId)
        {
            var contracts = _contactRepository.GetContractsByContact(contactId);
            return _mapper.Map<List<ContractDto>>(contracts);
        }
        public Contact GetContactByPin(string contactPin)
        {
            return _contactRepository.GetContacts().Where(c => c.Pin == contactPin).FirstOrDefault();
        }
        public bool CreateContact(ContactDto contactDto, int companyId)
        {
            var contactMap = _mapper.Map<Contact>(contactDto);
            contactMap.Company = _companyRepository.GetCompanies().Where(c => c.Id == companyId).FirstOrDefault();

            return _contactRepository.CreateContact(contactMap);
        }
        public bool UpdateContact(ContactDto contactDto, int companyId)
        {
            var contactMap = _mapper.Map<Contact>(contactDto);
            contactMap.Company = _companyRepository.GetCompanies().Where(c => c.Id == companyId).FirstOrDefault();

            return _contactRepository.UpdateContact(contactMap);
        }
        public bool DeleteContact(Contact contact)
        {
            return _contactRepository.DeleteContact(contact);
        }
    }
}
