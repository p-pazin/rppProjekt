using AutoMapper;
using CarchiveAPI.Dto;
using CarchiveAPI.Models;
using CarchiveAPI.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace CarchiveAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactController : Controller
    {
        private readonly ContactRepository _contactRepository;
        private readonly CompanyRepository _companyRepository;
        private readonly IMapper _mapper;

        public ContactController(ContactRepository contactRepository, CompanyRepository companyRepository, IMapper mapper)
        {
            this._contactRepository = contactRepository;
            this._companyRepository = companyRepository;
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

        [HttpGet("offers/{contactId}")]
        [ProducesResponseType(200, Type = typeof(List<OfferDto>))]
        [ProducesResponseType(400)]

        public IActionResult GetOffersByContactId(int contactId)
        {
            if (!_contactRepository.ContactExists(contactId))
            {
                return NotFound();
            }

            var offers = _mapper.Map<List<OfferDto>>(_contactRepository.GetOffersByContact(contactId));

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
            if (!_contactRepository.ContactExists(contactId))
            {
                return NotFound();
            }

            var contracts = _mapper.Map<List<ContractDto>>(_contactRepository.GetContractsByContact(contactId));

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
            
            var contact = _contactRepository.GetContacts().Where(c => c.Pin == contactCreate.Pin).FirstOrDefault();

            if(contact != null)
            {
                ModelState.AddModelError("", "Contact already exists");
                return StatusCode(422, ModelState);
            }

            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var contactMap = _mapper.Map<Contact>(contactCreate);
            contactMap.Company = _companyRepository.GetCompanies().Where(c => c.Id == companyId).FirstOrDefault();

            if(!_contactRepository.CreateContact(contactMap))
            {
                ModelState.AddModelError("", "Something went wrong when saving contact.");
                return StatusCode(500, ModelState);
            }
            return Ok("Successfully created contact!");
        }
    }
}
