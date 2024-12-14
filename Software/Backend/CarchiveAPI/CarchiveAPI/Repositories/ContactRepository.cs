using CarchiveAPI.Data;
using CarchiveAPI.Models;

namespace CarchiveAPI.Repositories
{
    public class ContactRepository
    {
        private DataContext _context;

        public ContactRepository(DataContext context)
        {
            this._context = context;
        }

        public ICollection<Contact> GetContacts() { 
            return _context.Contacts.ToList();
        }
        public Contact GetContact(int contactId) {
            return _context.Contacts.Where(c => c.Id == contactId).FirstOrDefault();
        }
        public bool ContactExists(int contactId) { 
            return _context.Contacts.Any(c => c.Id == contactId);
        }
        public Company GetCompanyByContact(int contactId) {
            return _context.Contacts.Where(c => c.Id == contactId).Select(c => c.Company).FirstOrDefault();
        }
        public ICollection<Offer> GetOffersByContact(int contactId) { 
            return _context.Offers.Where(o => o.Contact.Id == contactId).ToList();
        }
        public ICollection<Contract> GetContractsByContact(int contactId)
        {
            return _context.Contracts.Where(c => c.Contact.Id == contactId).ToList();
        }
    }
}
