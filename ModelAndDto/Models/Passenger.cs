using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelAndDto.Models
{
    public class Passenger
    {
        public int Id { get; set; }
        public int FlightId { get; set; }
        public string Name { get; set; } = string.Empty;
        public int? SeatId { get; set; }
        public int? SeatNumber { get; set; }
        public string PassportNumber { get; set; } = string.Empty;
    }
}
