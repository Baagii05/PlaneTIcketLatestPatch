using System;
using System.Collections.Generic;
using ModelAndDto.Models;
using ModelAndDto.Dtos;

namespace BussinessLogic.Interfaces
{
    /// <summary>
    /// Service interface for managing passengers and their related flight information.
    /// </summary>
    public interface IPassengerService
    {
        /// <summary>
        /// Gets a passenger by their passport number.
        /// </summary>
        /// <param name="passportNumber">The passport number of the passenger.</param>
        /// <returns>The passenger if found; otherwise, null.</returns>
        Passenger? GetByPassportNumber(string passportNumber);

        /// <summary>
        /// Gets passenger and flight information by passport number.
        /// </summary>
        /// <param name="passportNumber">The passport number of the passenger.</param>
        /// <param name="flightService">The flight service to retrieve flight details.</param>
        /// <returns>Combined passenger and flight information if found; otherwise, null.</returns>
        PassengerFlightInfo? GetPassengerWithFlight(string passportNumber, IFlightService flightService);

        /// <summary>
        /// Registers a new passenger for a flight.
        /// </summary>
        /// <param name="passport">The passport number of the passenger.</param>
        /// <param name="name">The name of the passenger.</param>
        /// <param name="flightId">The flight ID to register the passenger to.</param>
        void RegisterPassenger(string passport, string name, int flightId);

        /// <summary>
        /// Gets a passenger by their unique ID.
        /// </summary>
        /// <param name="id">The passenger ID.</param>
        /// <returns>The passenger if found; otherwise, null.</returns>
        Passenger? GetPassenger(int id);

        /// <summary>
        /// Gets all passengers.
        /// </summary>
        /// <returns>An enumerable collection of all passengers.</returns>
        IEnumerable<Passenger> GetAllPassengers();

        /// <summary>
        /// Updates the details of an existing passenger.
        /// </summary>
        /// <param name="passenger">The passenger object with updated information.</param>
        void UpdatePassenger(Passenger passenger);

        /// <summary>
        /// Deletes a passenger by their unique ID.
        /// </summary>
        /// <param name="id">The passenger ID.</param>
        void DeletePassenger(int id);
    }
}