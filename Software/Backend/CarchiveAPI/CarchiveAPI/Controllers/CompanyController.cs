using System.Security.Claims;
using CarchiveAPI.Dto;
using CarchiveAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarchiveAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : Controller
    {

        private readonly CompanyServices _companyServices;
        public CompanyController(CompanyServices companyServices)
        {
            this._companyServices = companyServices;
        }

        [HttpGet]
        [Authorize(Roles = "Admin, User")]
        [ProducesResponseType(200, Type = typeof(CompanyDto))]
        public IActionResult GetCompany()
        {
            var email = User.FindFirst(ClaimTypes.Name)?.Value;
            var company = _companyServices.GetCompany(email);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(company);
        }

        [HttpGet("users")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(200, Type = typeof(ICollection<UserDto>))]
        public IActionResult GetWorkers()
        {
            var email = User.FindFirst(ClaimTypes.Name)?.Value;
            var users = _companyServices.GetCompanyWorkers(email);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(users);
        }

        [HttpPost]
        [ProducesResponseType(200)]
        public async Task<IActionResult> AddCompany([FromBody] NewCompanyDto newComapany)
        {
            if (newComapany == null)
            {
                return BadRequest("Company object is null.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = await _companyServices.AddCompanyAsync(newComapany);
                if (!result)
                {
                    return BadRequest("Company could not be added. It may already exist.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }

            return Ok("Kompanija dodana!");
        }

        [HttpGet("approve-company/{companyId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> ApproveCompany(int companyId)
        {
            try
            {
                var result = await _companyServices.ApproveCompanyAsync(companyId);
                if (!result)
                {
                    return NotFound("Company not found or could not be approved.");
                }

                return Ok("Company approved successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


    }
}
