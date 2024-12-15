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

        [HttpGet("companies")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(200, Type = typeof(ICollection<CompanyDto>))]
        public IActionResult GetCompanies()
        {
            var companies = _companyServices.GetCompanies();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(companies);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        public IActionResult AddCompany([FromBody] NewCompanyDto newComapany)
        {
            if (newComapany == null)
            {
                return BadRequest("User object is null.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = _companyServices.AddCompany(newComapany);
                if (!result)
                {
                    return BadRequest("Company could not be added. It may already exist.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }

            return NoContent();
        }
    }
}
