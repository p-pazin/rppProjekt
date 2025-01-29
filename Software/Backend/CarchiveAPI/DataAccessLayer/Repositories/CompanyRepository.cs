using System.Diagnostics;
using CarchiveAPI.Data;
using CarchiveAPI.Models;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace CarchiveAPI.Repositories
{
    public class CompanyRepository
    {
        private readonly DataContext _context;

        public CompanyRepository(DataContext context)
        {
            this._context = context;
        }
        public ICollection<Company> GetCompanies()
        {
            return _context.Companies.OrderBy(c => c.Id).ToList();
        }
        public bool newAdminExists(string email)
        {
            return _context.Users.Any(u => u.Email == email);
        }

        public Company GetCompanyById(int id)
        {
            return _context.Companies.Find(id);
        }

        public ICollection<User> GetCompanyWorkers(Company company)
        {
            return _context.Users.Where(u => u.Company == company).Where(u => u.Role.Id == 2).ToList();
        }

        public bool CompanyExists(string name)
        {
            return _context.Companies.Any(c => c.Name == name);
        }

        public bool CompanyRegistered(string pin)
        {
            return _context.Companies.Any(c => c.Pin == pin);
        }

        public Role getAdminRole()
        {
            return _context.Roles.FirstOrDefault(r => r.Name == "Admin");
        }

        public bool AddCompany(Company company)
        {
            string pin = company.Pin;
            if (CompanyRegistered(pin))
            {
                return false;
            }
            var exists = CompanyExists(company.Name);
            if (exists)
            {
                return false;
            }
            _context.Companies.Add(company);
            return Save();
        }
        public bool UpdateCompany(Company company)
        {
            _context.Companies.Update(company);
            return Save();
        }
        public bool DeleteCompany(Company company)
        {
            _context.Companies.Remove(company);
            return Save();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0;
        }
    }
}
