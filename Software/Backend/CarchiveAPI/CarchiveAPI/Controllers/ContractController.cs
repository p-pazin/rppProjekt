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

        [HttpGet("unsigned")]
        [Authorize(Roles = "Admin, User")]
        [ProducesResponseType(200, Type = typeof(List<ContractDto>))]

        public IActionResult GetUnsignedContracts()
        {
            var email = User.FindFirst(ClaimTypes.Name)?.Value;
            var contracts = _contractService.GetUnsignedContracts(email);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(contracts);
        }


        [HttpGet("sell/{contractId}")]
        [Authorize(Roles = "Admin, User")]
        [ProducesResponseType(200, Type = typeof(List<SaleContractDto>))]

        public IActionResult GetSaleContract(int contractId)
        {
            if (!_contractService.CheckIfContractExists(contractId))
            {
                return NotFound();
            }
            var email = User.FindFirst(ClaimTypes.Name)?.Value;
            if (_contractService.GetSaleContract(contractId, email) == null)
            {
                ModelState.AddModelError("", "Contract not found.");
                return StatusCode(404, ModelState);
            }
            var contract = _contractService.GetSaleContract(contractId, email);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(contract);
        }

        [HttpGet("{contractId}")]
        [Authorize(Roles = "Admin, User")]
        [ProducesResponseType(200, Type = typeof(SaleContractDto))]
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

        [HttpPost("sell")]
        [Authorize(Roles = "Admin, User")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]

        public IActionResult CreateContractForSale([FromQuery] int? contactId, [FromQuery] int? vehicleId,
            [FromQuery] int? offerId, [FromBody] ContractDto contractCreate)
        {
            if (contractCreate == null)
            {
                return BadRequest(ModelState);
            }

            var email = User.FindFirst(ClaimTypes.Name)?.Value;

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if ((offerId.HasValue && (contactId.HasValue || vehicleId.HasValue)) ||
                (!offerId.HasValue && (!contactId.HasValue || !vehicleId.HasValue)))
            {
                ModelState.AddModelError("", "Only OfferId or both ContactId and VehicleId must be provided, not a mix of these.");
                return BadRequest(ModelState);
            }
            if (!_contractService.CreateContractForSale(contractCreate, contactId, vehicleId, offerId, email))
            {
                ModelState.AddModelError("", "Something went wrong when saving contract.");
                return StatusCode(500, ModelState);
            }
            return Ok("Successfully created contract!");
        }

        [HttpPut("sell")]
        [Authorize(Roles = "Admin, User")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]

        public IActionResult UpdateContractForSale([FromQuery] int? contactId, [FromQuery] int? vehicleId,
            [FromQuery] int? offerId, [FromBody] ContractDto contractUpdate)
        {
            if (contractUpdate == null)
            {
                return BadRequest(ModelState);
            }
            if (!_contractService.CheckIfContractExists(contractUpdate.Id))
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if ((offerId.HasValue && (contactId.HasValue || vehicleId.HasValue)) || 
                (!offerId.HasValue && (!contactId.HasValue || !vehicleId.HasValue)))
            {
                ModelState.AddModelError("", "Only OfferId or both ContactId and VehicleId must be provided, not a mix of these.");
                return BadRequest(ModelState);
            }

            var email = User.FindFirst(ClaimTypes.Name)?.Value;

            if (!_contractService.UpdateContractForSale(contractUpdate, contactId, vehicleId, offerId, email))
            {
                ModelState.AddModelError("", "Something went wrong when updating contract.");
                return StatusCode(500, ModelState);
            }
            return Ok("Successfully updated contract!");
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
            if (_contractService.GetRentContract(contractId, email) == null)
            {
                ModelState.AddModelError("", "Contract not found.");
                return StatusCode(404, ModelState);
            }
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
        public IActionResult AddRentContract([FromBody] ContractDto newContract, [FromQuery] int reservationId, [FromQuery]int insuranceId)
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
            var result = _contractService.AddRentContract(newContract, email, reservationId, insuranceId);
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
        public IActionResult UpdateRentContract([FromBody] ContractDto newContract, [FromQuery] int reservationId, [FromQuery] int insuranceId)
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
            var result = _contractService.UpdateRentContract(newContract, email, reservationId, insuranceId);
            if (!result)
            {
                return BadRequest("Something went wrong when adding contract.");
            }
            return Ok("Successfully updated contract!");
        }



    }
}
