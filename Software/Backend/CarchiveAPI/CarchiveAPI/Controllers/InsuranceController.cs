using CarchiveAPI.Dto;
using CarchiveAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarchiveAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InsuranceController : Controller
    {

        private readonly InsuranceServices _insuranceServices;
        public InsuranceController(InsuranceServices insuranceServices)
        {
            this._insuranceServices = insuranceServices;
        }

        [HttpGet]
        [Authorize(Roles = "Admin, User")]
        [ProducesResponseType(200, Type = typeof(ICollection<InsuranceDto>))]
        public IActionResult GetInsurance()
        {
            var insurance = _insuranceServices.GetInsurances();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(insurance);
        }
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin, User")]
        [ProducesResponseType(200, Type = typeof(InsuranceDto))]
        public IActionResult GetOneInsurance(int id)
        {
            var insurances = _insuranceServices.GetInsurance(id);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(insurances);
        }
    }
}
