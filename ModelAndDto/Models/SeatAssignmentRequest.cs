using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelAndDto.Models
{
    public class SeatAssignmentRequest
    {
        public int PassengerId { get; set; }
        public int FlightId { get; set; }
        public int SeatNumber { get; set; }
        public Action<bool, string>? Callback { get; set; }
    }
}