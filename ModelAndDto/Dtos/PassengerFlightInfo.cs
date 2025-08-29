using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelAndDto.Dtos
{
    public class PassengerFlightInfo
    {
        public int PassengerId { get; set; }

        public string PassengerName { get; set; }
        public string PassportNumber { get; set; }

        public string FlightNumber { get; set; }
        public string DepartingLocation { get; set; }
        public string ArrivalLocation { get; set; }

    }
}
