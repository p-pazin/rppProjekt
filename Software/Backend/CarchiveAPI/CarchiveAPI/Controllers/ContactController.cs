using System.Security.Claims;
using AutoMapper;
using CarchiveAPI.Dto;
using CarchiveAPI.Models;
using CarchiveAPI.Repositories;
using CarchiveAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarchiveAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactController : Controller
    {
        private readonly ContactServices _contactService;
        private readonly OfferServices _offerService;

        public ContactController(ContactServices contactService, OfferServices offerServices)
        {
            this._contactService = contactService;
            this._offerService = offerServices;
        }

        [HttpGet]
        [Authorize(Roles = "Admin, User")]
        [ProducesResponseType(200, Type = typeof(List<ContactDto>))]

        public IActionResult GetContacts() {
            var email = User.FindFirst(ClaimTypes.Name)?.Value;
            var contacts = _contactService.GetContacts(email);

            if (!ModelState.IsValid) { 
                return BadRequest(ModelState);
            }
            return Ok(contacts);
        }

        [HttpGet("{contactId}")]
        [Authorize(Roles = "Admin, User")]
        [ProducesResponseType(200, Type = typeof(ContactDto))]
        [ProducesResponseType(400)]

        public IActionResult GetContact(int contactId) {
            if(!_contactService.CheckIfContactExists(contactId))
            {
                return NotFound();
            }

            var email = User.FindFirst(ClaimTypes.Name)?.Value;
            var contact = _contactService.GetContact(contactId, email);

            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(_contactService.MapContact(contact));
        }

        [HttpGet("company/{contactId}")]
        [Authorize(Roles = "Admin, User")]
        [ProducesResponseType(200, Type = typeof(CompanyDto))]
        [ProducesResponseType(400)]

        public IActionResult GetCompanyByContactId(int contactId)
        {
            if(!_contactService.CheckIfContactExists(contactId))
            {
                return NotFound();
            }

            var email = User.FindFirst(ClaimTypes.Name)?.Value;
            var company = _contactService.GetCompanyByContact(contactId, email);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(company);
        }

        [HttpGet("offers/{contactId}")]
        [Authorize(Roles = "Admin, User")]
        [ProducesResponseType(200, Type = typeof(List<OfferDto>))]
        [ProducesResponseType(400)]

        public IActionResult GetOffersByContactId(int contactId)
        {
            if (!_contactService.CheckIfContactExists(contactId))
            {
                return NotFound();
            }

            var email = User.FindFirst(ClaimTypes.Name)?.Value;
            var offers = _contactService.GetOffersByContact(contactId, email);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(offers);
        }

        [HttpGet("contacts/{offerId}")]
        [Authorize(Roles = "Admin, User")]
        [ProducesResponseType(200, Type = typeof(List<ContactDto>))]
        [ProducesResponseType(400)]

        public IActionResult GetContactsByOfferId(int offerId)
        {
            var email = User.FindFirst(ClaimTypes.Name)?.Value;

            if (_offerService.GetOfferById(offerId, email) == null)
            {
                return NotFound();
            }
            
            var contacts = _contactService.GetContactsByOffer(offerId, email);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(contacts);
        }

        [HttpGet("contracts/{contactId}")]
        [Authorize(Roles = "Admin, User")]
        [ProducesResponseType(200, Type = typeof(List<ContractDto>))]
        [ProducesResponseType(400)]

        public IActionResult GetContractsByContactId(int contactId)
        {
            if (!_contactService.CheckIfContactExists(contactId))
            {
                return NotFound();
            }

            var email = User.FindFirst(ClaimTypes.Name)?.Value;
            var contracts = _contactService.GetContractsByContact(contactId, email);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(contracts);
        }

        [HttpPost]
        [Authorize(Roles = "Admin, User")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]

        public IActionResult CreateContact([FromBody] ContactDto contactCreate)
        {
            if (contactCreate == null)
            {
                return BadRequest(ModelState);
            }

            var email = User.FindFirst(ClaimTypes.Name)?.Value;
            var contact = _contactService.GetContactByPin(contactCreate.Pin, email);

            if (contact != null)
            {
                ModelState.AddModelError("", "Contact already exists");
                return StatusCode(422, ModelState);
            }
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if(!_contactService.CreateContact(contactCreate, email))
            {
                ModelState.AddModelError("", "Something went wrong when saving contact.");
                return StatusCode(500, ModelState);
            }
            return Ok("Successfully created contact!");
        }

        [HttpPut("{contactId}")]
        [Authorize(Roles = "Admin, User")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]

        public IActionResult UpdateContact(int contactId, [FromBody] ContactDto contactUpdate)
        {
            if (contactUpdate == null)
            {
                return BadRequest(ModelState);
            }
            if(contactId != contactUpdate.Id)
            {
                return BadRequest(ModelState);
            }
            if (!_contactService.CheckIfContactExists(contactId))
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var email = User.FindFirst(ClaimTypes.Name)?.Value;

            if (!_contactService.UpdateContact(contactUpdate, email))
            {
                ModelState.AddModelError("", "Something went wrong when updating contact.");
                return StatusCode(500, ModelState);
            }
            return Ok("Successfully updated contact!");
        }

        [HttpDelete("{contactId}")]
        [Authorize(Roles = "Admin, User")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]

        public IActionResult DeleteContact(int contactId)
        {
            if (!_contactService.CheckIfContactExists(contactId))
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var email = User.FindFirst(ClaimTypes.Name)?.Value;
            var contactToDelete = _contactService.GetContact(contactId, email);

            if (!_contactService.DeleteContact(contactToDelete))
            {
                ModelState.AddModelError("", "Something went wrong when deleting contact.");
                return StatusCode(500, ModelState);
            }
            return Ok("Successfully deleted contact!");
        }
    }
}
