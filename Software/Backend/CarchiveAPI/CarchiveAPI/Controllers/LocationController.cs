using AutoMapper;
using CarchiveAPI.Dto;
using CarchiveAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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
        [Authorize(Roles = "Admin, User")]
        [ProducesResponseType(200, Type = typeof(LocationDto))]
        [ProducesResponseType(400)]

        public IActionResult GetLocationForVehicle(int vehicleId)
        {
            var email = User.FindFirst(ClaimTypes.Name)?.Value;
            var location = _locationServices.GetLocationForVehicle(vehicleId);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(location);
        }

        [HttpGet]
        [Authorize(Roles = "Admin, User")]
        [ProducesResponseType(200, Type = typeof(List<LocationDto>))]
        public IActionResult GetAll()
        {
            var email = User.FindFirst(ClaimTypes.Name)?.Value;
            var locations = _locationServices.GetAll(email);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(locations);
        }
    }
}
