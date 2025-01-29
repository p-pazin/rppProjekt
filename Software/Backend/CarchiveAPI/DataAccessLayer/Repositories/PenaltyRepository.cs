using System.ComponentModel.Design;
using CarchiveAPI.Data;
using CarchiveAPI.Models;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace CarchiveAPI.Repositories
{
    public class PenaltyRepository
    {
        private readonly DataContext _context;
        public PenaltyRepository(DataContext context)
        {
            _context = context;
        }

        public ICollection<Penalty> GetAll()
        {
            return _context.Penalties
                .ToList();
        }

        public Penalty Get(int id)
        {
            var p = _context.Penalties.First(p => p.Id == id) as Penalty;
            return p;
        }

        public ICollection<InvoicePenalty> GetList(int id)
        {
            var p = _context.InvoicesPenalties.Where(p => p.InvoiceId == id).ToList();
            return p;
        }

        public bool Delete(InvoicePenalty penalty)
        {
            _context.InvoicesPenalties.Remove(penalty);
            return Save();
        }

        public bool AddPenltyInvoice(InvoicePenalty penalty)
        {
            if (penalty == null)
            {
                return false;
            }
            _context.InvoicesPenalties.Add(penalty);
            return Save();
        }

        public bool Save()
        {
            return _context.SaveChanges() > 0;
        }
    }
}
