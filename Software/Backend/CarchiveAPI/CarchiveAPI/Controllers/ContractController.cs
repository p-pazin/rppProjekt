using CarchiveAPI.Dto;
using System.Security.Claims;
using CarchiveAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CarchiveAPI.Models;
using System.Diagnostics.Contracts;

namespace CarchiveAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContractController : Controller
    {
        private readonly ContractServices _contractService;

        public ContractController(ContractServices contractService)
        {
            this._contractService = contractService;
        }

        [HttpGet]
        [Authorize(Roles = "Admin, User")]
        [ProducesResponseType(200, Type = typeof(List<ContractDto>))]

        public IActionResult GetContracts()
        {
            var email = User.FindFirst(ClaimTypes.Name)?.Value;
            var contracts = _contractService.GetContracts(email);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(contracts);
        }

        [HttpGet("{contractId}")]
        [Authorize(Roles = "Admin, User")]
        [ProducesResponseType(200, Type = typeof(ContractDto))]
        [ProducesResponseType(400)]

        public IActionResult GetContract(int contractId)
        {
            if (!_contractService.CheckIfContractExists(contractId))
            {
                return NotFound();
            }

            var email = User.FindFirst(ClaimTypes.Name)?.Value;
            var contract = _contractService.GetContract(contractId, email);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(_contractService.MapContract(contract));
        }

        [HttpDelete("{contractId}")]
        [Authorize(Roles = "Admin, User")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]

        public IActionResult DeleteContract(int contractId)
        {
            if (!_contractService.CheckIfContractExists(contractId))
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var email = User.FindFirst(ClaimTypes.Name)?.Value;
            var contractToDelete = _contractService.GetContract(contractId, email);

            if (!_contractService.DeleteContract(contractToDelete))
            {
                ModelState.AddModelError("", "Something went wrong when deleting contract.");
                return StatusCode(500, ModelState);
            }
            return Ok("Successfully deleted contract!");
        }

        [HttpGet("rent/{contractId}")]
        [Authorize(Roles = "Admin, User")]
        [ProducesResponseType(200, Type = typeof(RentContractDto))]
        [ProducesResponseType(400)]

        public IActionResult GetRentContract(int contractId)
        {
            if (!_contractService.CheckIfContractExists(contractId))
            {
                return NotFound();
            }

            var email = User.FindFirst(ClaimTypes.Name)?.Value;
            var contract = _contractService.GetRentContract(contractId, email);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(contract);
        }

        [HttpPost("rent")]
        [Authorize(Roles = "Admin, User")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public IActionResult AddRentContract([FromBody] ContractDto newContract, [FromQuery] int contactId, [FromQuery] int vehicleId, [FromQuery] int reservationId, [FromQuery]int insuranceId)
        {
            if (newContract == null)
            {
                return BadRequest("Contract object is null.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var email = User.FindFirst(ClaimTypes.Name)?.Value;
            var result = _contractService.AddRentContract(newContract, email, contactId, vehicleId, reservationId, insuranceId);
            if (!result)
            {
                return BadRequest("Something went wrong when adding contract.");
            }
            return Ok("Successfully added contract!");
        }

        [HttpPut("rent")]
        [Authorize(Roles = "Admin, User")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public IActionResult UpdateRentContract([FromBody] ContractDto newContract, [FromQuery] int contactId, [FromQuery] int vehicleId, [FromQuery] int reservationId, [FromQuery] int insuranceId)
        {
            if (newContract == null)
            {
                return BadRequest("Contract object is null.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var email = User.FindFirst(ClaimTypes.Name)?.Value;
            var result = _contractService.UpdateRentContract(newContract, email, contactId, vehicleId, reservationId, insuranceId);
            if (!result)
            {
                return BadRequest("Something went wrong when adding contract.");
            }
            return Ok("Successfully added contract!");
        }



    }
}
