using System.Security.Claims;
using CarchiveAPI.Dto;
using CarchiveAPI.Services;
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
        [ProducesResponseType(200, Type = typeof(List<OfferDto>))]

        public IActionResult GetOffers()
        {
            var offers = _offerServices.GetOffers();
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(offers);
        }

        [HttpGet("{offerId}")]
        [ProducesResponseType(200, Type = typeof(OfferDto))]
        [ProducesResponseType(400)]

        public IActionResult GetOfferById(string offerId)
        {
            int id = int.Parse(offerId);
            var offer = _offerServices.GetOfferById(id);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(offer);
        }

        [HttpGet("contact/{contactId}")]
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
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]

        public IActionResult AddOffer([FromBody] OfferDto offerDto, int userId, int contactId, [FromQuery] List<int> vehiclesId)
        {
            var email = User.FindFirst(ClaimTypes.Name)?.Value;
            var result = _offerServices.AddOffer(offerDto, userId, contactId, vehiclesId, email);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(result);
        }

        [HttpPut]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]

        public IActionResult UpdateOffer([FromBody] OfferDto offerDto, int userId, int contactId, [FromQuery] List<int> vehiclesId)
        {
            var email = User.FindFirst(ClaimTypes.Name)?.Value;
            _offerServices.UpdateOffer(offerDto, userId, contactId, vehiclesId, email);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]

        public IActionResult DeleteOffer(int id)
        {
            _offerServices.DeleteOffer(id);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return NoContent();
        }
    }
}
