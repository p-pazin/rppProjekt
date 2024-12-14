using AutoMapper;
using CarchiveAPI.Dto;
using CarchiveAPI.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace CarchiveAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactController : Controller
    {
        private readonly ContactRepository _contactRepository;
        private readonly IMapper _mapper;

        public ContactController(ContactRepository contactRepository, IMapper mapper)
        {
            this._contactRepository = contactRepository;
            this._mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<ContactDto>))]

        public IActionResult GetContacts() {
            var contacts = _mapper.Map<List<ContactDto>>(_contactRepository.GetContacts());

            if (!ModelState.IsValid) { 
                return BadRequest(ModelState);
            }
            return Ok(contacts);
        }

        [HttpGet("{contactId}")]
        [ProducesResponseType(200, Type = typeof(ContactDto))]
        [ProducesResponseType(400)]

        public IActionResult GetContact(int contactId) {
            if (!_contactRepository.ContactExists(contactId)) { 
                return NotFound();
            }

            var contact = _mapper.Map<ContactDto>(_contactRepository.GetContact(contactId));

            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(contact);
        }

        [HttpGet("company/{contactId}")]
        [ProducesResponseType(200, Type = typeof(CompanyDto))]
        [ProducesResponseType(400)]

        public IActionResult GetCompanyByContactId(int contactId)
        {
            if (!_contactRepository.ContactExists(contactId))
            {
                return NotFound();
            }

            var company = _mapper.Map<CompanyDto>(_contactRepository.GetCompanyByContact(contactId));

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(company);
        }
    }
}
