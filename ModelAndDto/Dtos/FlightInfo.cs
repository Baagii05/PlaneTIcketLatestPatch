
using ModelAndDto.Models;

namespace ModelAndDto.Dtos
{
    public class FlightInfo
    {
        public int FlightId { get; set; }
        public string FlightNumber { get; set; } = "";
        public FlightStatus Status { get; set; } 
        public string DepartureLocation { get; set; } = "";
        public string ArrivalLocation { get; set; } = "";
    }
}
