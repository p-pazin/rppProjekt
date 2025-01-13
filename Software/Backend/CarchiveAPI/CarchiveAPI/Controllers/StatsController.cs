using System.Security.Claims;
using CarchiveAPI.Dto;
using CarchiveAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarchiveAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatsController : Controller
    {
        private readonly StatsServices _statsServices;

        public StatsController(StatsServices statsServices)
        {
            this._statsServices = statsServices;
        }

        [HttpGet("ContactStatus")]
        [Authorize(Roles = "Admin, User")]
        [ProducesResponseType(200, Type = typeof(ContactStatusStatsDto))]
        
        public IActionResult GetContactStatusStats()
        {
            var email = User.FindFirst(ClaimTypes.Name)?.Value;
            var contactStatusStats = _statsServices.GetContactStatusStats(email);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(contactStatusStats);
        }

        [HttpGet("ContactCreation")]
        [Authorize(Roles = "Admin, User")]
        [ProducesResponseType(200, Type = typeof(YearlyInfoDto))]

        public IActionResult GetContactCreationStats()
        {
            var email = User.FindFirst(ClaimTypes.Name)?.Value;
            var contactCreationStats = _statsServices.GetContactCreationStats(email);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(contactCreationStats);
        }

        [HttpGet("InvoiceCreation")]
        [Authorize(Roles = "Admin, User")]
        [ProducesResponseType(200, Type = typeof(YearlyInfoDto))]

        public IActionResult GetInvoiceCreationStats()
        {
            var email = User.FindFirst(ClaimTypes.Name)?.Value;
            var invoiceCreationStats = _statsServices.GetInvoiceCreationStats(email);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(invoiceCreationStats);
        }
    }
}
