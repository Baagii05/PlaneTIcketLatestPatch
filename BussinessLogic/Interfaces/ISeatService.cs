using System;
using System.Collections.Generic;
using ModelAndDto.Models;

namespace BussinessLogic.Interfaces
{
    /// <summary>
    /// Service interface for managing seats in flights.
    /// </summary>
    public interface ISeatService
    {
        /// <summary>
        /// Adds a new seat to a specific flight.
        /// </summary>
        /// <param name="flightId">The ID of the flight to add the seat to.</param>
        /// <param name="seatNumber">The seat number to add.</param>
        void AddSeat(int flightId, int seatNumber);

        /// <summary>
        /// Gets a seat by its unique ID.
        /// </summary>
        /// <param name="id">The seat ID.</param>
        /// <returns>The seat if found; otherwise, null.</returns>
        Seat? GetSeat(int id);

        /// <summary>
        /// Gets all seats.
        /// </summary>
        /// <returns>An enumerable collection of all seats.</returns>
        IEnumerable<Seat> GetAllSeats();

        /// <summary>
        /// Updates the details of an existing seat.
        /// </summary>
        /// <param name="seat">The seat object with updated information.</param>
        void UpdateSeat(Seat seat);

        /// <summary>
        /// Deletes a seat by its unique ID.
        /// </summary>
        /// <param name="id">The seat ID.</param>
        void DeleteSeat(int id);
    }
}