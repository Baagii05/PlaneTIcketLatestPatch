using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BussinessLogic.Interfaces;
using ModelAndDto.Models;
using ModelAndDto.Dtos;

namespace BussinessLogic.Interfaces
{
    public interface IPassengerService
    {
        Passenger? GetByPassportNumber(string passportNumber);
        PassengerFlightInfo? GetPassengerWithFlight(string passportNumber, IFlightService flightService);
        void RegisterPassenger(string passport, string name, int flightId);
        Passenger? GetPassenger(int id);
        IEnumerable<Passenger> GetAllPassengers();
        void UpdatePassenger(Passenger passenger);
        void DeletePassenger(int id);
    }
}