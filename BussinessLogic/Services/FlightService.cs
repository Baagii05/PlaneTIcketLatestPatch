using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Repositories;
using BussinessLogic.Interfaces;
using ModelAndDto.Models;

namespace BussinessLogic.Services
{
    public class FlightService : IFlightService
    {
        private readonly FlightRepository _flightRepo;

        public FlightService(FlightRepository flightRepo)
        {
            _flightRepo = flightRepo;
        }

        public void CreateFlight(string flightNumber, string departure, string arrival)
        {
            try
            {
                var flight = new Flight
                {
                    FlightNumber = flightNumber,
                    Status = FlightStatus.Registering,
                    DepartureLocation = departure,
                    ArrivalLocation = arrival
                };
                _flightRepo.Add(flight);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error creating flight: {ex.Message}");
                throw;
            }
        }

        public Flight? GetFlight(int id)
        {
            try
            {
                return _flightRepo.GetById(id);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error getting flight with ID {id}: {ex.Message}");
                throw;
            }
        }

        public IEnumerable<Flight> GetAllFlights()
        {
            try
            {
                return _flightRepo.GetAll();
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error getting all flights: {ex.Message}");
                throw;
            }
        }

        public void UpdateFlight(Flight flight)
        {
            try
            {
                _flightRepo.Update(flight);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error updating flight with ID {flight.FlightId}: {ex.Message}");
                throw;
            }
        }

        public void DeleteFlight(int id)
        {
            try
            {
                _flightRepo.Delete(id);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error deleting flight with ID {id}: {ex.Message}");
                throw;
            }
        }

        public void ChangeStatus(int flightId, FlightStatus status)
        {
            try
            {
                var flight = _flightRepo.GetById(flightId);
                if (flight != null)
                {
                    flight.Status = status;
                    _flightRepo.Update(flight);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error changing status for flight with ID {flightId}: {ex.Message}");
                throw;
            }
        }
    }
}