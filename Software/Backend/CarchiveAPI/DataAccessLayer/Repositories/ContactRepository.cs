using CarchiveAPI.Data;
using CarchiveAPI.Models;
using System;

namespace CarchiveAPI.Repositories
{
    public class ContactRepository
    {
        private DataContext _context;

        public ContactRepository(DataContext context)
        {
            this._context = context;
        }

        public ICollection<Contact> GetContacts(int companyId) { 
            return _context.Contacts.Where(c => c.Company.Id == companyId).ToList();
        }
        public Contact GetContact(int contactId, int companyId) {
            return _context.Contacts.Where(c => c.Id == contactId && c.Company.Id == companyId).FirstOrDefault();
        }
        public bool ContactExists(int contactId) { 
            return _context.Contacts.Any(c => c.Id == contactId);
        }
        public Company GetCompanyByContact(int contactId, int companyId) {
            return _context.Contacts.Where(c => c.Id == contactId && c.Company.Id == companyId).Select(c => c.Company).FirstOrDefault();
        }
        public ICollection<Offer> GetOffersByContact(int contactId, int companyId) { 
            return _context.Offers.Where(o => o.Contact.Id == contactId && o.User.Company.Id == companyId).ToList();
        }
        public ICollection<Contract> GetContractsByContact(int contactId, int companyId)
        {
            return _context.Contracts.Where(c => c.Contact.Id == contactId && c.Company.Id == companyId).ToList();
        }

        public Contact GetContactsByOffer(Offer offer)
        {
            return _context.Contacts.Where(c => c.Offers.Contains(offer)).FirstOrDefault();
        }

        public bool CreateContact(Contact contact)
        {
            _context.Contacts.Add(contact);
            return Save();
        }
        public bool UpdateContact(Contact contact)
        {
            _context.Contacts.Update(contact);
            return Save();
        }
        public bool DeleteContact(Contact contact)
        {
            _context.Contacts.Remove(contact);
            return Save();
        }
        public bool Save() {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }
    }
}
