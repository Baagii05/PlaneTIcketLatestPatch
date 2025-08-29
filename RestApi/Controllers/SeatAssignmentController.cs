using Microsoft.AspNetCore.Mvc;
using BussinessLogic.Interfaces;
using ModelAndDto.Dtos;

namespace RestApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SeatAssignmentController : ControllerBase
    {
        private readonly ISeatAssignmentManager _seatAssignmentManager;

        public SeatAssignmentController(ISeatAssignmentManager seatAssignmentManager)
        {
            _seatAssignmentManager = seatAssignmentManager;
        }

        /// <summary>
        /// Assign a seat to a passenger
        /// </summary>
        [HttpPost("assign")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AssignSeat([FromBody] AssignSeatRequest request)
        {
            if (request == null)
                return BadRequest("Missing seat assignment data.");

            try
            {
                var (success, message) = await _seatAssignmentManager.AssignSeatAsync(
                    request.PassengerId,
                    request.FlightId,
                    request.SeatNumber);

                if (success)
                    return Ok(message);
                else
                    return BadRequest(message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Check if a seat is available
        /// </summary>
        [HttpGet("available")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> IsSeatAvailable([FromQuery] int flightId, [FromQuery] int seatNumber)
        {
            try
            {
                var available = await _seatAssignmentManager.IsSeatAvailableAsync(flightId, seatNumber);
                return Ok(available);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}