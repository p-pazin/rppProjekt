using AutoMapper;
using CarchiveAPI.Dto;
using CarchiveAPI.Models;
using CarchiveAPI.Repositories;
using CarchiveAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace CarchiveAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactController : Controller
    {
        private readonly ContactService _contactService;

        public ContactController(ContactService contactService)
        {
            this._contactService = contactService;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<ContactDto>))]

        public IActionResult GetContacts() {
            var contacts = _contactService.GetContacts();

            if (!ModelState.IsValid) { 
                return BadRequest(ModelState);
            }
            return Ok(contacts);
        }

        [HttpGet("{contactId}")]
        [ProducesResponseType(200, Type = typeof(ContactDto))]
        [ProducesResponseType(400)]

        public IActionResult GetContact(int contactId) {
            if(!_contactService.CheckIfContactExists(contactId))
            {
                return NotFound();
            }

            var contact = _contactService.GetContact(contactId);

            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(_contactService.MapContact(contact));
        }

        [HttpGet("company/{contactId}")]
        [ProducesResponseType(200, Type = typeof(CompanyDto))]
        [ProducesResponseType(400)]

        public IActionResult GetCompanyByContactId(int contactId)
        {
            if(!_contactService.CheckIfContactExists(contactId))
            {
                return NotFound();
            }
            var company = _contactService.GetCompanyByContact(contactId);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(company);
        }

        [HttpGet("offers/{contactId}")]
        [ProducesResponseType(200, Type = typeof(List<OfferDto>))]
        [ProducesResponseType(400)]

        public IActionResult GetOffersByContactId(int contactId)
        {
            if (!_contactService.CheckIfContactExists(contactId))
            {
                return NotFound();
            }

            var offers = _contactService.GetOffersByContact(contactId);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(offers);
        }

        [HttpGet("contracts/{contactId}")]
        [ProducesResponseType(200, Type = typeof(List<ContractDto>))]
        [ProducesResponseType(400)]

        public IActionResult GetContractsByContactId(int contactId)
        {
            if (!_contactService.CheckIfContactExists(contactId))
            {
                return NotFound();
            }

            var contracts = _contactService.GetContractsByContact(contactId);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(contracts);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]

        public IActionResult CreateContact([FromQuery] int companyId, [FromBody] ContactDto contactCreate)
        {
            if (contactCreate == null)
            {
                return BadRequest(ModelState);
            }

            var contact = _contactService.GetContactByPin(contactCreate.Pin);

            if(contact != null)
            {
                ModelState.AddModelError("", "Contact already exists");
                return StatusCode(422, ModelState);
            }
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if(!_contactService.CreateContact(contactCreate, companyId))
            {
                ModelState.AddModelError("", "Something went wrong when saving contact.");
                return StatusCode(500, ModelState);
            }
            return Ok("Successfully created contact!");
        }

        [HttpPut("{contactId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]

        public IActionResult UpdateContact(int contactId, [FromQuery] int companyId, [FromBody] ContactDto contactUpdate)
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

            if(!_contactService.UpdateContact(contactUpdate, companyId))
            {
                ModelState.AddModelError("", "Something went wrong when updating contact.");
                return StatusCode(500, ModelState);
            }
            return Ok("Successfully updated contact!");
        }

        [HttpDelete("{contactId}")]
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

            var contactToDelete = _contactService.GetContact(contactId);

            if (!_contactService.DeleteContact(contactToDelete))
            {
                ModelState.AddModelError("", "Something went wrong when deleting contact.");
                return StatusCode(500, ModelState);
            }
            return Ok("Successfully deleted contact!");
        }
    }
}
