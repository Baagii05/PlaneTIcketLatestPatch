using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BussinessLogic.Interfaces;
using ModelAndDto.Models;
using DataAccess.Repositories;
using ModelAndDto.Dtos;

namespace BussinessLogic.Services
{
    public class PassengerService : IPassengerService
    {
        private readonly PassengerRepository _passengerRepo;

        public PassengerService(PassengerRepository passengerRepo)
        {
            _passengerRepo = passengerRepo;
        }

        public Passenger? GetByPassportNumber(string passportNumber)
        {
            try
            {
                return _passengerRepo.GetByPassportNumber(passportNumber);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error getting passenger by passport number {passportNumber}: {ex.Message}");
                throw;
            }
        }

        public PassengerFlightInfo? GetPassengerWithFlight(string passportNumber, IFlightService flightService)
        {
            try
            {
                var passenger = GetByPassportNumber(passportNumber);
                if (passenger == null)
                    return null;

                var flight = flightService.GetFlight(passenger.FlightId);
                if (flight == null)
                    return null;

                return new PassengerFlightInfo
                {
                    PassengerId = passenger.Id,
                    PassengerName = passenger.Name,
                    PassportNumber = passenger.PassportNumber,
                    FlightNumber = flight.FlightNumber,
                    DepartingLocation = flight.DepartureLocation,
                    ArrivalLocation = flight.ArrivalLocation,

                };
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error getting passenger flight info for passport {passportNumber}: {ex.Message}");
                throw;
            }
        }

        public void RegisterPassenger(string passport, string name, int flightId)
        {
            try
            {
                var passenger = new Passenger
                {
                    PassportNumber = passport,
                    Name = name,
                    FlightId = flightId,
                    SeatNumber = null
                };
                _passengerRepo.Add(passenger);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error registering passenger with passport {passport}: {ex.Message}");
                throw;
            }
        }

        public Passenger? GetPassenger(int id)
        {
            try
            {
                return _passengerRepo.GetById(id);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error getting passenger with ID {id}: {ex.Message}");
                throw;
            }
        }

        public IEnumerable<Passenger> GetAllPassengers()
        {
            try
            {
                return _passengerRepo.GetAll();
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error getting all passengers: {ex.Message}");
                throw;
            }
        }

        public void UpdatePassenger(Passenger passenger)
        {
            try
            {
                _passengerRepo.Update(passenger);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error updating passenger with ID {passenger.Id}: {ex.Message}");
                throw;
            }
        }

        public void DeletePassenger(int id)
        {
            try
            {
                _passengerRepo.Delete(id);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error deleting passenger with ID {id}: {ex.Message}");
                throw;
            }
        }
    }
}
