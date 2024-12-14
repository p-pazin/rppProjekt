using System.Diagnostics;
using CarchiveAPI.Models;
using CarchiveAPI.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace CarchiveAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : Controller
    {
        private readonly CompanyRepository _companyRepository;
        public CompanyController(CompanyRepository companyRepository)
        {
            this._companyRepository = companyRepository;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(ICollection<Company>))]
        public IActionResult GetCompanies()
        {
            var companies = _companyRepository.GetCompanies();

            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }
            return Ok(companies);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        public IActionResult AddCompany([FromBody] User user)
        {
            if (user == null)
            {
                return BadRequest("User object is null.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = _companyRepository.AddCompany(user);
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
