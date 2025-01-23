using System.Security.Claims;
using CarchiveAPI.Dto;
using CarchiveAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarchiveAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly UserServices _userServices;
        public UserController(UserServices userServices)
        {
            this._userServices = userServices;
        }

        [HttpGet]
        [Authorize(Roles = "Admin, User")]
        [ProducesResponseType(200, Type = typeof(ICollection<UserDto>))]
        public IActionResult GetUserInfo()
        {
            var email = User.FindFirst(ClaimTypes.Name)?.Value;
            var users = _userServices.GetUserInfo(email);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(users);
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginDto loginDto)
        {
            var token = _userServices.Login(loginDto.Email, loginDto.Password);
            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized(new { message = "Invalid credentials" });
            }

            return Ok(new { token });
        }

        [HttpPost("new")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult AddUser([FromBody] RegisterUserDto newUserDto)
        {
            var adminEmail = User.FindFirst(ClaimTypes.Name)?.Value;
            bool success = _userServices.AddNewUser(newUserDto, adminEmail);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!success)
            {
                return BadRequest("Failed to add user. Ensure the email is unique and the admin has a valid company.");
            }

            return Ok();
        }

        [HttpPut("update")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult ChangeUserInfo([FromBody] UserDto UserDto)
        {
            var adminEmail = User.FindFirst(ClaimTypes.Name)?.Value;
            bool success = _userServices.ChangeUserInfo(UserDto, adminEmail);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!success)
            {
                return BadRequest("Failed to edit user.");
            }

            return Ok();
        }
        [HttpPut("newpassword")]
        [Authorize(Roles = "Admin,User")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult ChangeUserPassword([FromBody] NewPasswordDto newPass)
        {
            bool success = _userServices.ChangeUserPassword(newPass);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!success)
            {
                return BadRequest("Failed to change password.");
            }

            return Ok("Password changed");
        }

        [HttpDelete("delete/{userId}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult DeleteUser(int userId)
        {
            var adminEmail = User.FindFirst(ClaimTypes.Name)?.Value;
            bool success = _userServices.DeleteUser(userId, adminEmail);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!success)
            {
                return BadRequest("Failed to delete user.");
            }

            return Ok();
        }
    }
}
