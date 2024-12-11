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
    }
}
