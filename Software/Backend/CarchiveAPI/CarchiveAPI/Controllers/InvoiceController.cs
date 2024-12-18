using CarchiveAPI.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CarchiveAPI.Services;
using System.Security.Claims;

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

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin, User")]
        public IActionResult DeleteInvoice(int id)
        {
            var email = User.FindFirst(ClaimTypes.Name)?.Value;
            var result = _invoiceServices.DeleteInvoice(email, id);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (!result)
            {
                return BadRequest("Failed to delete invoice");
            }
            return Ok("Invoice deleted!");
        }
    }
}
