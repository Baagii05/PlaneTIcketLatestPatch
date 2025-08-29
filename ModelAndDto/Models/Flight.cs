using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ModelAndDto.Models
{
    public class Flight
    {
        public int FlightId { get; set; }
        public string FlightNumber { get; set; } = string.Empty;
        public FlightStatus Status { get; set; }
        [JsonPropertyName("departure")]
        public string DepartureLocation { get; set; } = string.Empty;
        [JsonPropertyName("arrival")]
        public string ArrivalLocation { get; set; } = string.Empty;
    }


    public enum FlightStatus
    {
        Registering,    // Бүртгэж байна
        Boarding,       // Онгоцонд сууж байна
        Departed,       // Ниссэн
        Delayed,        // Хойшилсон
        Cancelled       // Цуцалсан
    }


}
