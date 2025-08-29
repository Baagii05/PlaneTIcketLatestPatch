using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelAndDto.Models;


namespace BussinessLogic.Interfaces
{
    public interface IFlightService
    {
        void CreateFlight(string flightNumber, string departure, string arrival);
        Flight? GetFlight(int id);
        IEnumerable<Flight> GetAllFlights();
        void UpdateFlight(Flight flight);
        void DeleteFlight(int id);
        void ChangeStatus(int flightId, FlightStatus status);
    }
}