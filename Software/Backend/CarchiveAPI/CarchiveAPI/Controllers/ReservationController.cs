using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using CarchiveAPI.Dto;
using CarchiveAPI.Services;
using CarchiveAPI.Models;

namespace CarchiveAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationController : Controller
    {
        private readonly ReservationServices _reservationServices;
        public ReservationController(ReservationServices reservationServices)
        {
            this._reservationServices = reservationServices;
        }

        [HttpGet]
        [Authorize(Roles = "Admin, User")]
        [ProducesResponseType(200, Type = typeof(ICollection<ReservationDto>))]
        public IActionResult GetReservations()
        {
            var email = User.FindFirst(ClaimTypes.Name)?.Value;
            var reservations = _reservationServices.GetReservations(email);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(reservations);

        }
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin, User")]
        [ProducesResponseType(200, Type = typeof(ICollection<ReservationDto>))]
        public IActionResult GetOneReservation(int id)
        {
            var email = User.FindFirst(ClaimTypes.Name)?.Value;
            var reservation = _reservationServices.GetOneReservation(email, id);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(reservation);
        }

        [HttpPost]
        [Authorize(Roles = "Admin, User")]
        [ProducesResponseType(200, Type = typeof(bool))]
        public IActionResult AddReservation([FromBody] ReservationDto reservationDto,[FromQuery] int contactid, [FromQuery] int vehicleId)
        {
            var email = User.FindFirst(ClaimTypes.Name)?.Value;
            var result = _reservationServices.AddReservation(email, reservationDto, vehicleId, contactid);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if(!result)
            {
                return BadRequest("Reservation not added");
            }
            return Ok(result);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin, User")]
        [ProducesResponseType(200, Type = typeof(bool))]
        public IActionResult UpdateReservation([FromBody] ReservationDto reservationDto, int id, [FromQuery] int vehicleId)
        {
            var email = User.FindFirst(ClaimTypes.Name)?.Value;
            var result = _reservationServices.UpdateReservation(email, reservationDto, id, vehicleId);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(result);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin, User")]
        [ProducesResponseType(200, Type = typeof(bool))]
        public IActionResult DeleteReservation(int id)
        {
            var email = User.FindFirst(ClaimTypes.Name)?.Value;
            var result = _reservationServices.DeleteReservation(email, id);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(result);
        }
    }
}
