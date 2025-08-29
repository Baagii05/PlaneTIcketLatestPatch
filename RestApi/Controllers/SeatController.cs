using Microsoft.AspNetCore.Mvc;
using BussinessLogic.Interfaces;
using ModelAndDto.Models;

namespace RestApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SeatController : ControllerBase
    {
        private readonly ISeatService _seatService;

        public SeatController(ISeatService seatService)
        {
            _seatService = seatService;
        }

        /// <summary>
        /// Get all seats
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Seat>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<IEnumerable<Seat>> GetAllSeats()
        {
            try
            {
                var seats = _seatService.GetAllSeats();
                return Ok(seats);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Get seat by ID
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Seat), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<Seat> GetSeat(int id)
        {
            try
            {
                var seat = _seatService.GetSeat(id);
                if (seat == null)
                    return NotFound($"Seat with ID {id} not found");
                return Ok(seat);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Add a new seat
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult AddSeat([FromBody] Seat seat)
        {
            if (seat == null)
                return BadRequest("Missing seat data.");

            try
            {
                _seatService.AddSeat(seat.FlightId, seat.SeatNumber);
                return Ok("Seat added successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Update seat details
        /// </summary>
        [HttpPut("{id}")]
        public IActionResult UpdateSeat(int id, [FromBody] Seat seat)
        {
            if (seat == null || seat.Id != id)
                return BadRequest("Seat ID mismatch or missing seat data.");

            try
            {
                _seatService.UpdateSeat(seat);
                return Ok("Seat updated successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Delete a seat
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult DeleteSeat(int id)
        {
            try
            {
                _seatService.DeleteSeat(id);
                return Ok("Seat deleted successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}