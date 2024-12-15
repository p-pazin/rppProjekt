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
        private readonly IMapper _mapper;


        public UserServices(DataContext context, IMapper mapper)
        {
            this._context = context;
            this._userRepository = new UserRepository(context);
            this._mapper = mapper;
        }

        public ICollection<UserDto> GetUsers()
        {
            var usersDto = _mapper.Map<List<UserDto>>(_userRepository.GetAll());
            return usersDto;
        }

        public UserDto GetUser(int userId)
        {
            var user = _userRepository.GetUserById(userId);
            if (user == null)
            {
                return null;
            }
            var usersDto = _mapper.Map<UserDto>(user);
            return usersDto;
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
    }
}
