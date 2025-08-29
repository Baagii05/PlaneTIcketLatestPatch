using Microsoft.AspNetCore.Mvc;
using BussinessLogic.Interfaces;
using ModelAndDto.Models;
using ModelAndDto.Dtos;

namespace RestApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PassengerController : ControllerBase
    {
        private readonly IPassengerService _passengerService;
        private readonly IFlightService _flightService;

        public PassengerController(IPassengerService passengerService, IFlightService flightService)
        {
            _passengerService = passengerService;
            _flightService = flightService;
        }

        /// <summary>
        /// Get all passengers
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Passenger>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<IEnumerable<Passenger>> GetAllPassengers()
        {
            try
            {
                var passengers = _passengerService.GetAllPassengers();
                return Ok(passengers);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Get passenger by ID
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Passenger), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<Passenger> GetPassenger(int id)
        {
            try
            {
                var passenger = _passengerService.GetPassenger(id);
                if (passenger == null)
                    return NotFound($"Passenger with ID {id} not found");
                return Ok(passenger);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Get passenger by passport number
        /// </summary>
        [HttpGet("by-passport/{passportNumber}")]
        [ProducesResponseType(typeof(Passenger), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<Passenger> GetByPassportNumber(string passportNumber)
        {
            try
            {
                var passenger = _passengerService.GetByPassportNumber(passportNumber);
                if (passenger == null)
                    return NotFound($"Passenger with passport number {passportNumber} not found");
                return Ok(passenger);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Get passenger with flight info by passport number
        /// </summary>
        [HttpGet("flight-info/{passportNumber}")]
        [ProducesResponseType(typeof(PassengerFlightInfo), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<PassengerFlightInfo> GetPassengerWithFlight(string passportNumber)
        {
            try
            {
                var info = _passengerService.GetPassengerWithFlight(passportNumber, _flightService);
                if (info == null)
                    return NotFound($"Passenger with passport number {passportNumber} or flight not found");
                return Ok(info);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Register a new passenger
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult RegisterPassenger([FromBody] RegisterPassengerRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                _passengerService.RegisterPassenger(request.PassportNumber, request.Name, request.FlightId);
                return Ok("Passenger registered successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Update passenger details
        /// </summary>
        [HttpPut("{id}")]
        public IActionResult UpdatePassenger(int id, [FromBody] Passenger passenger)
        {
            if (passenger == null || passenger.Id != id)
                return BadRequest("Passenger ID mismatch or missing passenger data.");

            try
            {
                _passengerService.UpdatePassenger(passenger);
                return Ok("Passenger updated successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Delete a passenger
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult DeletePassenger(int id)
        {
            try
            {
                _passengerService.DeletePassenger(id);
                return Ok("Passenger deleted successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}