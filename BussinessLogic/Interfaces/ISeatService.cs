using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelAndDto.Models;

namespace BussinessLogic.Interfaces
{
    public interface ISeatService
    {
        void AddSeat(int flightId, int seatNumber);
        Seat? GetSeat(int id);
        IEnumerable<Seat> GetAllSeats();
        void UpdateSeat(Seat seat);
        void DeleteSeat(int id);
    }
}