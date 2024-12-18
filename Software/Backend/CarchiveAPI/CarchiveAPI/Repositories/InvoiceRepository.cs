using CarchiveAPI.Data;
using CarchiveAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CarchiveAPI.Repositories

{
    public class InvoiceRepository
    {
        private readonly DataContext _context;
        public InvoiceRepository(DataContext context)
        {
            _context = context;
        }

        public ICollection<Invoice> GetAll(int companyId)
        {
            return _context.Invoices
                .Include(i => i.Contract.Company)
                .Where(i => i.Contract.Company.Id == companyId)
                .ToList();
        }

        public Invoice Get(int id, int companyId)
        {
            return _context.Invoices
                .Include(i => i.Contract.Company)
                .Where(i => i.Contract.Company.Id == companyId && i.Id == id)
                .FirstOrDefault();
        }

        public bool AddInvoice(Invoice invoice, int companyId)
        {
            if (invoice.Contract.Company.Id != companyId)
            {
                return false;
            }
            _context.Invoices.Add(invoice);
            return Save();
        }
        public bool DeleteInvoice(Invoice invoice)
        {
            _context.Invoices.Remove(invoice);
            return Save();
        }

        bool Save()
        {
            return _context.SaveChanges() > 0;
        }
    }
}
