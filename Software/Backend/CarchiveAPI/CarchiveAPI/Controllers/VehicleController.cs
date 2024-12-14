using CarchiveAPI.Dto;
using CarchiveAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace CarchiveAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehicleController : Controller
    {
        private readonly VehicleServices _vehicleServices;

        public VehicleController(VehicleServices vehicleServices)
        {
            _vehicleServices = vehicleServices;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<VehicleDto>))]

        public IActionResult GetVehicles()
        {
            var vehicles = _vehicleServices.GetAll();
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(vehicles);
        }

        [HttpGet("{vehicleId}")]
        [ProducesResponseType(200, Type = typeof(VehicleDto))]
        [ProducesResponseType(400)]

        public IActionResult GetVehicleById(int id)
        {
            var vehicle = _vehicleServices.GetVehicleById(id);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(vehicle);
        }

        [HttpGet("model/{model}")]
        [ProducesResponseType(200, Type = typeof(List<VehicleDto>))]
        [ProducesResponseType(400)]

        public IActionResult GetVehiclesByModel(string model)
        {
            var vehicles = _vehicleServices.GetVehiclesByModel(model);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(vehicles);
        }

        [HttpGet("registration/{reg}")]
        [ProducesResponseType(200, Type = typeof(List<VehicleDto>))]
        [ProducesResponseType(400)]

        public IActionResult GetVehiclesByRegistration(string reg)
        {
            var vehicles = _vehicleServices.GetVehiclesByRegistration(reg);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(vehicles);
        }

        [HttpGet("type/{type}")]
        [ProducesResponseType(200, Type = typeof(List<VehicleDto>))]
        [ProducesResponseType(400)]

        public IActionResult GetVehiclesByType(string type)
        {
            var vehicles = _vehicleServices.GetVehiclesByType(type);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(vehicles);
        }

        [HttpGet("color/{color}")]
        [ProducesResponseType(200, Type = typeof(List<VehicleDto>))]
        [ProducesResponseType(400)]

        public IActionResult GetVehiclesByColor(string color)
        {
            var vehicles = _vehicleServices.GetVehiclesByColor(color);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(vehicles);
        }


        [HttpGet("mileage/{minMileage}/{maxMileage}")]
        [ProducesResponseType(200, Type = typeof(List<VehicleDto>))]
        [ProducesResponseType(400)]

        public IActionResult GetVehiclesByMileage(int minMileage, int maxMileage)
        {
            var vehicles = _vehicleServices.GetVehiclesByMileage(minMileage, maxMileage);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(vehicles);
        }

        [HttpGet("transmissiontype/{transmissionType}")]
        [ProducesResponseType(200, Type = typeof(List<VehicleDto>))]
        [ProducesResponseType(400)]

        public IActionResult GetVehiclesByTransType(string transmissionType)
        {
            var vehicles = _vehicleServices.GetVehiclesByTransType(transmissionType);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(vehicles);
        }

        [HttpGet("price/{minPrice}/{maxPrice}")]
        [ProducesResponseType(200, Type = typeof(List<VehicleDto>))]
        [ProducesResponseType(400)]

        public IActionResult GetVehiclesByPrice(double minPrice, double maxPrice)
        {
            var vehicles = _vehicleServices.GetVehiclesByPrice(minPrice, maxPrice);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(vehicles);
        }

        [HttpGet("condition/{condition}")]
        [ProducesResponseType(200, Type = typeof(List<VehicleDto>))]
        [ProducesResponseType(400)]

        public IActionResult GetVehiclesByCondition(string condition)
        {
            var vehicles = _vehicleServices.GetVehiclesByCondition(condition);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(vehicles);
        }

        [HttpGet("productionYear/{minYear}/{maxYear}")]
        [ProducesResponseType(200, Type = typeof(List<VehicleDto>))]
        [ProducesResponseType(400)]

        public IActionResult GetVehiclesByProdYear(int minYear, int maxYear)
        {
            var vehicles = _vehicleServices.GetVehiclesByProdYear(minYear, maxYear);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(vehicles);
        }

        [HttpGet("enginePower/{minPower}/{maxPower}")]
        [ProducesResponseType(200, Type = typeof(List<VehicleDto>))]
        [ProducesResponseType(400)]

        public IActionResult GetVehiclesByEngPower(int minPower, int maxPower)
        {
            var vehicles = _vehicleServices.GetVehiclesByEngPower(minPower, maxPower);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(vehicles);
        }

        [HttpGet("cubicCapacity/{minCapacity}/{maxCapacity}")]
        [ProducesResponseType(200, Type = typeof(List<VehicleDto>))]
        [ProducesResponseType(400)]

        public IActionResult GetVehiclesByCubicCapacity(double minCapacity, double maxCapacity)
        {
            var vehicles = _vehicleServices.GetVehiclesByCubicCapacity(minCapacity, maxCapacity);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(vehicles);
        }

        [HttpGet("engine/{engine}")]
        [ProducesResponseType(200, Type = typeof(List<VehicleDto>))]
        [ProducesResponseType(400)]

        public IActionResult GetVehiclesByEngine(string engine)
        {
            var vehicles = _vehicleServices.GetVehiclesByEngine(engine);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(vehicles);
        }
    }
}
