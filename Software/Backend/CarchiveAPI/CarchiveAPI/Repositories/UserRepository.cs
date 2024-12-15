using CarchiveAPI.Data;
using CarchiveAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CarchiveAPI.Repositories
{
    public class UserRepository
    {
        private readonly DataContext _context;
        public UserRepository(DataContext context)
        {
            _context = context;
        }
        public ICollection<User> GetAll()
        {
            return _context.Users.OrderBy(u => u.Id).ToList();
        }
        public User GetUserById(int id)
        {
            return _context.Users.Find(id);
        }
        public User GetByEmail(string email)
        {
            return _context.Users.FirstOrDefault(u => u.Email == email);
        }

        public User GetUserRoleByEmail(string email)
        {
            return _context.Users.Include(u => u.Role).FirstOrDefault(u => u.Email == email);
        }


        public User GetUserAndCompanyByEmail(string email)
        {
            return _context.Users.Include(u => u.Company).FirstOrDefault(u => u.Email == email);
        }
        public bool UserExists(string email)
        {
            return _context.Users.Any(u => u.Email == email);
        }
        public Role getUserRole() { 
            return _context.Roles.FirstOrDefault(r => r.Name == "User"); 
        }
        public bool AddUser(User user)
        {
            if (UserExists(user.Email))
            {
                return false;
            }
            _context.Users.Add(user);
            return Save();
        }
        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0;
        }
    }
}