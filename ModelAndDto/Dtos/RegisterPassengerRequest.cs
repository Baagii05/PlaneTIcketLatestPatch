namespace ModelAndDto.Dtos
{
    public class RegisterPassengerRequest
    {
        public string PassportNumber { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public int FlightId { get; set; }
    }
}