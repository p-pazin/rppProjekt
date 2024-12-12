﻿using CarchiveAPI.Data;
using CarchiveAPI.Models;

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
        public bool AddCompany(User user)
        {
            Company company = user.Company;
            if (!CompanyExists(company.Pin) || !AdminExists(user.Email)){
                _context.Companies.Add(company);
                user.Company = company;
                _context.Users.Add(user);
                return Save();
            }
            return false;

        }
        public bool CompanyExists(string pin)
        {
            return _context.Companies.Any(c => c.Pin == pin);
        }
        public bool AdminExists(string email)
        {
            return _context.Users.Any(u => u.Email == email);
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }
    }
}
