namespace ModelAndDto.Dtos
{
    public class CreateFlightRequest
    {
        public string FlightNumber { get; set; } = string.Empty;
        public string Departure { get; set; } = string.Empty;
        public string Arrival { get; set; } = string.Empty;
    }
}