using CarchiveAPI.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CarchiveAPI.Services;
using System.Security.Claims;
using CarchiveAPI.Models;

namespace CarchiveAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoiceController : Controller
    {
        private readonly InvoiceServices _invoiceServices;
        public InvoiceController(InvoiceServices invoiceServices)
        {
            this._invoiceServices = invoiceServices;
        }

        [HttpGet]
        [Authorize(Roles = "Admin, User")]
        [ProducesResponseType(200, Type = typeof(ICollection<InvoiceDto>))]
        public IActionResult GetInvoices()
        {
            var email = User.FindFirst(ClaimTypes.Name)?.Value;
            var invoices = _invoiceServices.GetInvoices(email);
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(invoices);

        }
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin, User")]
        [ProducesResponseType(200, Type = typeof(InvoiceDto))]
        public IActionResult GetOneInvoice(int id)
        {
            var email = User.FindFirst(ClaimTypes.Name)?.Value;
            var invoice = _invoiceServices.GetOneInvoice(email, id);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(invoice);
        }

        [HttpPost("sell")]
        [Authorize(Roles = "Admin, User")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]

        public IActionResult CreateInvoiceForSale([FromQuery] int contractId, [FromBody] InvoiceDto invoiceCreate)
        {
            if (invoiceCreate == null)
            {
                return BadRequest(ModelState);
            }

            var email = User.FindFirst(ClaimTypes.Name)?.Value;

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (!_invoiceServices.CreateInvoiceForSale(invoiceCreate, contractId, email))
            {
                ModelState.AddModelError("", "Something went wrong when saving invoice.");
                return StatusCode(500, ModelState);
            }
            return Ok("Successfully created invoice!");
        }

        [HttpPost("rent/start")]
        [Authorize(Roles = "Admin, User")]
        public IActionResult PostStartInvoiceRent([FromQuery] int contractId, [FromBody] InvoiceDto invoiceCreate)
        {
            if(invoiceCreate == null)
            {
                return BadRequest(ModelState);
            }

            var email = User.FindFirst(ClaimTypes.Name)?.Value;
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (!_invoiceServices.PostStartInvoiceRent(invoiceCreate, contractId, email))
            {
                ModelState.AddModelError("Invoice", "Failed to add invoice");
                return StatusCode(500, "Failed to add start invoice");
            }
            return Ok("Invoice for rent made!");
        }

        [HttpPost("rent/final")]
        [Authorize(Roles = "Admin, User")]
        public IActionResult PostFinalInvoiceRent([FromQuery] int contractId, [FromQuery] List<int> penaltyIds, [FromBody] InvoiceDto invoiceCreate)
        {
            if (invoiceCreate == null)
            {
                return BadRequest(ModelState);
            }

            var email = User.FindFirst(ClaimTypes.Name)?.Value;
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (!_invoiceServices.PostFinalInvoiceRent(invoiceCreate, contractId, email, penaltyIds))
            {
                ModelState.AddModelError("Invoice", "Failed to add invoice");
                return StatusCode(500, "Failed to add final invoice");
            }
            return Ok("Invoice for rent made!");
        }
    }
}
