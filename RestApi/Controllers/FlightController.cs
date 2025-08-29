using Microsoft.AspNetCore.Mvc;
using BussinessLogic.Interfaces;
using ModelAndDto.Models;
using ModelAndDto.Dtos;

namespace RestApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FlightController : ControllerBase
    {
        private readonly IFlightService _flightService;

        public FlightController(IFlightService flightService)
        {
            _flightService = flightService;
        }

        /// <summary>
        /// Get all flights
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Flight>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<IEnumerable<Flight>> GetAllFlights()
        {
            try
            {
                var flights = _flightService.GetAllFlights();
                return Ok(flights);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Get flight by ID
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Flight), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<Flight> GetFlight(int id)
        {
            try
            {
                var flight = _flightService.GetFlight(id);
                if (flight == null)
                    return NotFound($"Flight with ID {id} not found");
                return Ok(flight);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Create a new flight
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult CreateFlight([FromBody] CreateFlightRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                _flightService.CreateFlight(
                    request.FlightNumber,
                    request.Departure,
                    request.Arrival);
                return Ok("Flight created successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Update flight status
        /// </summary>
        [HttpPut("{id}/status")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult UpdateFlightStatus(int id, [FromBody] UpdateFlightStatusRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                _flightService.ChangeStatus(id, request.Status);
                return Ok("Flight status updated successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Delete a flight
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult DeleteFlight(int id)
        {
            try
            {
                _flightService.DeleteFlight(id);
                return Ok("Flight deleted successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Update flight details
        /// </summary>
        [HttpPut("{id}")]
        public IActionResult UpdateFlight(int id, [FromBody] Flight flight)
        {
            if (flight == null || flight.FlightId != id)
                return BadRequest("Flight ID mismatch or missing flight data.");

            try
            {
                _flightService.UpdateFlight(flight);
                return Ok("Flight updated successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}