using ModelAndDto.Models;

namespace FlightDashboardWeb.Models
{
    public class FlightUpdateInfo
    {
        public int FlightId { get; set; }
        public FlightStatus NewStatus { get; set; }
        public DateTime Timestamp { get; set; }
    }
}