using System;
using System.Collections.Generic;
using ModelAndDto.Models;

namespace BussinessLogic.Interfaces
{
    /// <summary>
    /// Service interface for managing flights and their statuses.
    /// </summary>
    public interface IFlightService
    {
        /// <summary>
        /// Creates a new flight with the specified flight number, departure, and arrival locations.
        /// </summary>
        /// <param name="flightNumber">Unique flight number.</param>
        /// <param name="departure">Departure location.</param>
        /// <param name="arrival">Arrival location.</param>
        void CreateFlight(string flightNumber, string departure, string arrival);

        /// <summary>
        /// Gets a flight by its unique ID.
        /// </summary>
        /// <param name="id">Flight ID.</param>
        /// <returns>The flight if found; otherwise, null.</returns>
        Flight? GetFlight(int id);

        /// <summary>
        /// Gets all flights.
        /// </summary>
        /// <returns>An enumerable collection of all flights.</returns>
        IEnumerable<Flight> GetAllFlights();

        /// <summary>
        /// Updates the details of an existing flight.
        /// </summary>
        /// <param name="flight">The flight object with updated information.</param>
        void UpdateFlight(Flight flight);

        /// <summary>
        /// Deletes a flight by its unique ID.
        /// </summary>
        /// <param name="id">Flight ID.</param>
        void DeleteFlight(int id);

        /// <summary>
        /// Changes the status of a flight.
        /// </summary>
        /// <param name="flightId">Flight ID.</param>
        /// <param name="status">New flight status.</param>
        void ChangeStatus(int flightId, FlightStatus status);
    }
}