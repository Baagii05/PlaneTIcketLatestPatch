using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelAndDto.Models;

namespace BussinessLogic.Interfaces
{
    public interface ISeatAssignmentManager : IDisposable
    {
        void EnqueueRequest(SeatAssignmentRequest request);
        Task<(bool Success, string Message)> AssignSeatAsync(int passengerId, int flightId, int seatNumber);
        Task<bool> IsSeatAvailableAsync(int flightId, int seatNumber);
    }
}