using System.Security.Claims;
using CarchiveAPI.Dto;
using CarchiveAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarchiveAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OfferController : Controller
    {
        private readonly OfferServices _offerServices;

        public OfferController(OfferServices offerServices)
        {
            this._offerServices = offerServices;
        }

        [HttpGet]
        [Authorize(Roles = "Admin, User")]
        [ProducesResponseType(200, Type = typeof(List<OfferDto>))]

        public IActionResult GetOffers()
        {
            var email = User.FindFirst(ClaimTypes.Name)?.Value;
            var offers = _offerServices.GetOffers(email);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(offers);
        }

        [HttpGet("{offerId}")]
        [Authorize(Roles = "Admin, User")]
        [ProducesResponseType(200, Type = typeof(OfferDto))]
        [ProducesResponseType(400)]

        public IActionResult GetOfferById(string offerId)
        {
            int id = int.Parse(offerId);
            var email = User.FindFirst(ClaimTypes.Name)?.Value;
            var offer = _offerServices.GetOfferById(id, email);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(offer);
        }

        [HttpGet("contact/{contactId}")]
        [Authorize(Roles = "Admin, User")]
        [ProducesResponseType(200, Type = typeof(List<OfferDto>))]
        [ProducesResponseType(400)]

        public IActionResult GetOffersByContact(int contactId)
        {
            var email = User.FindFirst(ClaimTypes.Name)?.Value;
            var offers = _offerServices.GetOffersByContact(contactId, email);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(offers);
        }

        [HttpPost]
        [Authorize(Roles = "Admin, User")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]

        public IActionResult AddOffer([FromBody] OfferDto offerDto, int contactId, [FromQuery] List<int> vehiclesId)
        {
            var email = User.FindFirst(ClaimTypes.Name)?.Value;
            var result = _offerServices.AddOffer(offerDto, contactId, vehiclesId, email);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(result);
        }

        [HttpPut]
        [Authorize(Roles = "Admin, User")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]

        public IActionResult UpdateOffer([FromBody] OfferDto offerDto, int contactId, [FromQuery] List<int> vehiclesId)
        {
            var email = User.FindFirst(ClaimTypes.Name)?.Value;
            _offerServices.UpdateOffer(offerDto, contactId, vehiclesId, email);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin, User")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]

        public IActionResult DeleteOffer(int id)
        {
            var email = User.FindFirst(ClaimTypes.Name)?.Value;
            if (_offerServices.DeleteOffer(id, email) == false)
            {
                return NotFound(new { message = "Postoji ugovor vezan uz ponudu." });
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return NoContent();
        }
    }
}
