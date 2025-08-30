using System;
using System.Collections.Generic;
using ModelAndDto.Models;

namespace BussinessLogic.Interfaces
{
    /// <summary>
    /// Manages seat assignment requests and seat availability for flights.
    /// </summary>
    public interface ISeatAssignmentManager : IDisposable
    {
        /// <summary>
        /// Enqueues a seat assignment request to be processed.
        /// </summary>
        /// <param name="request">The seat assignment request containing passenger, flight, and seat information.</param>
        void EnqueueRequest(SeatAssignmentRequest request);

        /// <summary>
        /// Attempts to assign a seat to a passenger for a specific flight asynchronously.
        /// </summary>
        /// <param name="passengerId">The ID of the passenger.</param>
        /// <param name="flightId">The ID of the flight.</param>
        /// <param name="seatNumber">The seat number to assign.</param>
        /// <returns>
        /// A tuple containing a success flag and a message describing the result.
        /// </returns>
        Task<(bool Success, string Message)> AssignSeatAsync(int passengerId, int flightId, int seatNumber);

        /// <summary>
        /// Checks asynchronously if a specific seat is available for a flight.
        /// </summary>
        /// <param name="flightId">The ID of the flight.</param>
        /// <param name="seatNumber">The seat number to check.</param>
        /// <returns>
        /// True if the seat is available; otherwise, false.
        /// </returns>
        Task<bool> IsSeatAvailableAsync(int flightId, int seatNumber);
    }
}