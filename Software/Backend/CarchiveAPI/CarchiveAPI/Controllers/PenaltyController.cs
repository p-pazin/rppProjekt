using CarchiveAPI.Dto;
using CarchiveAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarchiveAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PenaltyController : Controller
    {
        private readonly PenaltyServices _penaltyServices;
        public PenaltyController(PenaltyServices penaltyServices)
        {
            this._penaltyServices = penaltyServices;
        }

        [HttpGet]
        [Authorize(Roles = "Admin, User")]
        [ProducesResponseType(200, Type = typeof(ICollection<PenaltyDto>))]
        public IActionResult GetPenalties()
        {
            var penalties = _penaltyServices.GetPenalties();
            return Ok(penalties);
        }
    }
}
