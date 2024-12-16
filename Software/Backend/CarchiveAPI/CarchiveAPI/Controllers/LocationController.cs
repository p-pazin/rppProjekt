using AutoMapper;
using CarchiveAPI.Dto;
using CarchiveAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace CarchiveAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class LocationController : Controller
    {
        private readonly LocationServices _locationServices;
        private readonly IMapper _mapper;

        public LocationController(LocationServices locationServices, IMapper mapper)
        {
            _locationServices = locationServices;
            _mapper = mapper;
        }

        [HttpGet("{vehicleId}")]
        [ProducesResponseType(200, Type = typeof(LocationDto))]
        [ProducesResponseType(400)]

        public IActionResult GetLocationForVehicle(int vehicleId)
        {
            var location = _locationServices.GetLocationForVehicle(vehicleId);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(location);
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<LocationDto>))]
        public IActionResult GetAll()
        {
            var locations = _locationServices.GetAll();
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(locations);
        }
    }
}
