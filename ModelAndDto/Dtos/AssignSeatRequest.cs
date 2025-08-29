namespace ModelAndDto.Dtos
{
    public class AssignSeatRequest
    {
        public int PassengerId { get; set; }
        public int FlightId { get; set; }
        public int SeatNumber { get; set; }
    }
}