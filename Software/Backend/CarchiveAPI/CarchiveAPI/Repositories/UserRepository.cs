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
        public User GetUserByIdAndCheckCompany(int userId, int companyId)
        {
            return _context.Users.Where(u => u.Id == userId && u.Company.Id == companyId).FirstOrDefault();
        }
        public User GetUserByAdminRoleAndCheckCompany(int companyId)
        {
            return _context.Users.Where(u => u.Role.Id == 1 && u.Company.Id == companyId).FirstOrDefault();
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
        public Role getInactiveRole()
        {
            return _context.Roles.FirstOrDefault(r => r.Name == "Inactive");
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
        public bool UpdateUser(User user)
        {
            _context.Users.Update(user);
            return Save();
        }
        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0;
        }
    }
}