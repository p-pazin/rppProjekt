using CarchiveAPI.Dto;
using CarchiveAPI.Models;
using CarchiveAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CarchiveAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehicleController : Controller
    {
        private readonly VehicleServices _vehicleServices;
        private readonly OfferServices _offerServices;

        public VehicleController(VehicleServices vehicleServices)
        {
            _vehicleServices = vehicleServices;
        }

        [HttpGet]
        [Authorize(Roles = "Admin, User")]
        [ProducesResponseType(200, Type = typeof(List<VehicleDto>))]

        public IActionResult GetVehicles()
        {
            var email = User.FindFirst(ClaimTypes.Name)?.Value;
            var vehicles = _vehicleServices.GetAll(email);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(vehicles);
        }

        [HttpGet("{vehicleId}")]
        [Authorize(Roles = "Admin, User")]
        [ProducesResponseType(200, Type = typeof(VehicleDto))]
        [ProducesResponseType(400)]

        public IActionResult GetVehicleById(string vehicleId)
        {
            int Id = int.Parse(vehicleId);
            var email = User.FindFirst(ClaimTypes.Name)?.Value;
            var vehicle = _vehicleServices.GetVehicleById(Id, email);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(vehicle);
        }

        [HttpGet("model/{model}")]
        [Authorize(Roles = "Admin, User")]
        [ProducesResponseType(200, Type = typeof(List<VehicleDto>))]
        [ProducesResponseType(400)]

        public IActionResult GetVehiclesByModel(string model)
        {
            var email = User.FindFirst(ClaimTypes.Name)?.Value;
            var vehicles = _vehicleServices.GetVehiclesByModel(model, email);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(vehicles);
        }

        [HttpGet("registration/{reg}")]
        [Authorize(Roles = "Admin, User")]
        [ProducesResponseType(200, Type = typeof(List<VehicleDto>))]
        [ProducesResponseType(400)]

        public IActionResult GetVehiclesByRegistration(string reg)
        {
            var email = User.FindFirst(ClaimTypes.Name)?.Value;
            var vehicles = _vehicleServices.GetVehiclesByRegistration(reg, email);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(vehicles);
        }

        [HttpGet("type/{type}")]
        [Authorize(Roles = "Admin, User")]
        [ProducesResponseType(200, Type = typeof(List<VehicleDto>))]
        [ProducesResponseType(400)]

        public IActionResult GetVehiclesByType(string type)
        {
            var email = User.FindFirst(ClaimTypes.Name)?.Value;
            var vehicles = _vehicleServices.GetVehiclesByType(type, email);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(vehicles);
        }

        [HttpGet("color/{color}")]
        [Authorize(Roles = "Admin, User")]
        [ProducesResponseType(200, Type = typeof(List<VehicleDto>))]
        [ProducesResponseType(400)]

        public IActionResult GetVehiclesByColor(string color)
        {
            var email = User.FindFirst(ClaimTypes.Name)?.Value;
            var vehicles = _vehicleServices.GetVehiclesByColor(color, email);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(vehicles);
        }


        [HttpGet("mileage/{minMileage}/{maxMileage}")]
        [Authorize(Roles = "Admin, User")]
        [ProducesResponseType(200, Type = typeof(List<VehicleDto>))]
        [ProducesResponseType(400)]

        public IActionResult GetVehiclesByMileage(int minMileage, int maxMileage)
        {
            var email = User.FindFirst(ClaimTypes.Name)?.Value;
            var vehicles = _vehicleServices.GetVehiclesByMileage(minMileage, maxMileage, email);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(vehicles);
        }

        [HttpGet("transmissiontype/{transmissionType}")]
        [Authorize(Roles = "Admin, User")]
        [ProducesResponseType(200, Type = typeof(List<VehicleDto>))]
        [ProducesResponseType(400)]

        public IActionResult GetVehiclesByTransType(string transmissionType)
        {
            var email = User.FindFirst(ClaimTypes.Name)?.Value;
            var vehicles = _vehicleServices.GetVehiclesByTransType(transmissionType, email);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(vehicles);
        }

        [HttpGet("price/{minPrice}/{maxPrice}")]
        [Authorize(Roles = "Admin, User")]
        [ProducesResponseType(200, Type = typeof(List<VehicleDto>))]
        [ProducesResponseType(400)]

        public IActionResult GetVehiclesByPrice(double minPrice, double maxPrice)
        {
            var email = User.FindFirst(ClaimTypes.Name)?.Value;
            var vehicles = _vehicleServices.GetVehiclesByPrice(minPrice, maxPrice, email);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(vehicles);
        }

        [HttpGet("condition/{condition}")]
        [Authorize(Roles = "Admin, User")]
        [ProducesResponseType(200, Type = typeof(List<VehicleDto>))]
        [ProducesResponseType(400)]

        public IActionResult GetVehiclesByCondition(string condition)
        {
            var email = User.FindFirst(ClaimTypes.Name)?.Value;
            var vehicles = _vehicleServices.GetVehiclesByCondition(condition, email);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(vehicles);
        }

        [HttpGet("productionYear/{minYear}/{maxYear}")]
        [Authorize(Roles = "Admin, User")]
        [ProducesResponseType(200, Type = typeof(List<VehicleDto>))]
        [ProducesResponseType(400)]

        public IActionResult GetVehiclesByProdYear(int minYear, int maxYear)
        {
            var email = User.FindFirst(ClaimTypes.Name)?.Value;
            var vehicles = _vehicleServices.GetVehiclesByProdYear(minYear, maxYear, email);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(vehicles);
        }

        [HttpGet("enginePower/{minPower}/{maxPower}")]
        [Authorize(Roles = "Admin, User")]
        [ProducesResponseType(200, Type = typeof(List<VehicleDto>))]
        [ProducesResponseType(400)]

        public IActionResult GetVehiclesByEngPower(int minPower, int maxPower)
        {
            var email = User.FindFirst(ClaimTypes.Name)?.Value;
            var vehicles = _vehicleServices.GetVehiclesByEngPower(minPower, maxPower, email);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(vehicles);
        }

        [HttpGet("cubicCapacity/{minCapacity}/{maxCapacity}")]
        [Authorize(Roles = "Admin, User")]
        [ProducesResponseType(200, Type = typeof(List<VehicleDto>))]
        [ProducesResponseType(400)]

        public IActionResult GetVehiclesByCubicCapacity(double minCapacity, double maxCapacity)
        {
            var email = User.FindFirst(ClaimTypes.Name)?.Value;
            var vehicles = _vehicleServices.GetVehiclesByCubicCapacity(minCapacity, maxCapacity, email);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(vehicles);
        }

        [HttpGet("engine/{engine}")]
        [Authorize(Roles = "Admin, User")]
        [ProducesResponseType(200, Type = typeof(List<VehicleDto>))]
        [ProducesResponseType(400)]

        public IActionResult GetVehiclesByEngine(string engine)
        {
            var email = User.FindFirst(ClaimTypes.Name)?.Value;
            var vehicles = _vehicleServices.GetVehiclesByEngine(engine, email);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(vehicles);
        }

        [HttpPost]
        [Authorize(Roles = "Admin, User")]
        [ProducesResponseType(201, Type = typeof(VehicleDto))]
        [ProducesResponseType(400)]

        public IActionResult AddVehicle([FromBody] VehicleDto vehicle)
        {
            if(vehicle == null)
            {
                return BadRequest(ModelState);
            }

            var email = User.FindFirst(ClaimTypes.Name)?.Value;
            var findVehicle = _vehicleServices.GetVehiclesByRegistration(vehicle.Registration, email);

            if (findVehicle.Count != 0)
            {
                ModelState.AddModelError("", "Vehicle already exists");
                return StatusCode(422, ModelState);
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!_vehicleServices.AddVehicle(vehicle, email))
            {
                ModelState.AddModelError("", "Something went wrong when saving the vehicle.");
                return StatusCode(500, ModelState);
            }
            return Ok("Successfully created vehicle!");
        }

        [HttpPut("{vehicleId}")]
        [Authorize(Roles = "Admin, User")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]

        public IActionResult UpdateVehicle(int vehicleId, [FromBody] VehicleDto vehicle)
        {
            if (vehicle == null)
            {
                return BadRequest(ModelState);
            }

            if (vehicleId != vehicle.Id)
            {
                return BadRequest(ModelState);
            }

            if (!_vehicleServices.CheckIfVehicleExists(vehicleId))
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var email = User.FindFirst(ClaimTypes.Name)?.Value;
            if (!_vehicleServices.UpdateVehicle(vehicle, email))
            {
                ModelState.AddModelError("", "Something went wrong when updating the vehicle.");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully updated vehicle!");
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin, User")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]

        public IActionResult DeleteVehicle(int id)
        {
            if (!_vehicleServices.CheckIfVehicleExists(id))
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var email = User.FindFirst(ClaimTypes.Name)?.Value;
            if (_vehicleServices.DeleteVehicle(id, email))
            {
                ModelState.AddModelError("", "Something went wrong when deleting contact.");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully deleted vehicle!");
        }

        [HttpGet("/offer/{offerId}")]
        [Authorize(Roles = "Admin, User")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult GetVehiclesByOffer(int offerId)
        {
            var email = User.FindFirst(ClaimTypes.Name)?.Value;
            var vehicle = _vehicleServices.GetVehiclesByOffer(email, offerId);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(vehicle);
        }
    }
}
