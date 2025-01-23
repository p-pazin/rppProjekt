using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using CarchiveAPI.Data;
using CarchiveAPI.Dto;
using CarchiveAPI.Models;
using CarchiveAPI.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace CarchiveAPI.Services
{
    public class UserServices
    {
        private DataContext _context;
        private UserRepository _userRepository;
        private CompanyServices _companyServices;
        private readonly IMapper _mapper;


        public UserServices(DataContext context, CompanyServices companyServices, UserRepository userRepository, IMapper mapper)
        {
            this._context = context;
            this._userRepository = userRepository;
            this._companyServices = companyServices;
            this._mapper = mapper;
        }

        public UserDto GetUserInfo(string email)
        {
            var user = _userRepository.GetUserAndCompanyByEmail(email);
            var userDto = _mapper.Map<UserDto>(user);
            return userDto;
        }

        public bool UserExists(string userEmail)
        {
            return _userRepository.UserExists(userEmail);
        }

        public bool AddNewUser(RegisterUserDto newUserDto , string adminEmail)
        {
            var admin = _userRepository.GetUserAndCompanyByEmail(adminEmail);
            var userRole = _userRepository.getUserRole();
            if (admin == null || admin.Company == null)
            {
                return false;
            }
            User user = new User
            {
                FirstName = newUserDto.FirstName,
                LastName = newUserDto.LastName,
                Email = newUserDto.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(newUserDto.Password),
                Role = userRole,
                Company = admin.Company
            };
            if (UserExists(user.Email))
            {
                return false;
            }
            return _userRepository.AddUser(user);
        }

        public string Login(string email, string password)
        {
            var user = _context.Users.Include(u => u.Role).FirstOrDefault(u => u.Email == email);
            var company = _companyServices.GetCompany(email);
            if(user == null || user.Role.Name == "Inactive" || company.Approved == 0)
            {
                return null;
            }
            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.Password))
            {
                return null;
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("3nVhe7YYN5KAl06W5qLEeQRMATqfqpZ5K305e8");

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                    new[]
                    {
                    new Claim(ClaimTypes.Name, user.Email),
                    new Claim(ClaimTypes.Role, user.Role.Name)
                    }
                ),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature
                )
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public bool ChangeUserInfo(UserDto UserInfo, string adminEmail)
        {
            var admin = _userRepository.GetUserAndCompanyByEmail(adminEmail);
            var user = _userRepository.GetUserById(UserInfo.Id);
            if (admin == null || admin.Company == null || user == null)
            {
                return false;
            }
            user.FirstName = UserInfo.FirstName;
            user.LastName = UserInfo.LastName;
            user.Email = UserInfo.Email;
            return _userRepository.UpdateUser(user);
        }

        public bool ChangeUserPassword(NewPasswordDto newPass)
        {
            var user = _userRepository.GetUserAndCompanyByEmail(newPass.Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(newPass.Password, user.Password))
            {
                return false;
            }
            user.Password = BCrypt.Net.BCrypt.HashPassword(newPass.NewPassword);
            return _userRepository.UpdateUser(user);
        }

        public bool DeleteUser(int userId, string adminEmail)
        {
            var admin = _userRepository.GetUserAndCompanyByEmail(adminEmail);
            var user = _userRepository.GetUserByIdAndCheckCompany(userId, admin.Company.Id);
            if (admin == null || admin.Company == null || user == null)
            {
                return false;
            }
            user.Role = _userRepository.getInactiveRole();
            return _userRepository.UpdateUser(user); ;
        }
    }
}
