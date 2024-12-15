using CarchiveAPI.Data;
using CarchiveAPI.Models;

namespace CarchiveAPI.Repositories
{
    public class OfferRepository
    {
        private DataContext _context;

        public OfferRepository(DataContext context)
        {
            this._context = context;
        }

        public ICollection<Offer> GetAll()
        {
            return _context.Offers.ToList();
        }

        public Offer GetOfferById(int id)
        {
            return _context.Offers.Where(o => o.Id == id).FirstOrDefault();
        }

        public Offer GetLastOffer()
        {
            return _context.Offers.OrderByDescending(o => o.Id).FirstOrDefault();
        }

        public ICollection<Offer> GetOffersByContact(Contact contact)
        {
            return _context.Offers.Where(o => o.Contact == contact).ToList();
        }

        public ICollection<Offer> GetOffersByTitle(string title)
        {
            return _context.Offers.Where(o => o.Title == title).ToList();
        }


        public bool Add(Offer offer)
        {
            _context.Offers.Add(offer);
            return Save();
        }

        public bool Update(Offer offer)
        {
            _context.Offers.Update(offer);
            return Save();
        }

        public bool Delete(Offer offer)
        {
            _context.Offers.Remove(offer);
            return Save();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }
    }
}
