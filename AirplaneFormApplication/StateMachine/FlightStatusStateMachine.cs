using ModelAndDto.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirplaneFormApplication.StateMachine
{
    public static class FlightStatusStateMachine
    {
        private static readonly Dictionary<FlightStatus, FlightStatus[]> transitions = new()
    {
        { FlightStatus.Registering, new[] { FlightStatus.Boarding, FlightStatus.Delayed, FlightStatus.Cancelled } },
        { FlightStatus.Boarding,    new[] { FlightStatus.Departed, FlightStatus.Delayed, FlightStatus.Cancelled } },
        { FlightStatus.Departed,    Array.Empty<FlightStatus>() },
        { FlightStatus.Delayed,     new[] { FlightStatus.Boarding, FlightStatus.Cancelled } },
        { FlightStatus.Cancelled,   Array.Empty<FlightStatus>() }
    };

        public static bool CanTransition(FlightStatus from, FlightStatus to)
        {
            return transitions.TryGetValue(from, out var allowed) && allowed.Contains(to);
        }

        public static FlightStatus[] GetTransitions(FlightStatus from)
        {
            return transitions.TryGetValue(from, out var allowed) ? allowed : Array.Empty<FlightStatus>();
        }
    }
}
