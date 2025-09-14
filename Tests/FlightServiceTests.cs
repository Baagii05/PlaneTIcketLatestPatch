using BussinessLogic.Interfaces;
using BussinessLogic.Services;
using DataAccess.Repositories;
using Microsoft.Data.Sqlite;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ModelAndDto.Models;
using System.Collections.Generic;
using System.Linq;

namespace Tests
{
    [TestClass]
    public class FlightServiceTests
    {
        private SqliteConnection connection;
        private IFlightService flightService;

        [TestInitialize]
        public void Setup()
        {
            
            connection = new SqliteConnection("Data Source=:memory:");
            connection.Open();

            
            using (var cmd = connection.CreateCommand())
            {
                cmd.CommandText = @"
                    CREATE TABLE IF NOT EXISTS Flight (
                        ID INTEGER PRIMARY KEY AUTOINCREMENT,
                        FLIGHT_NUMBER TEXT NOT NULL,
                        STATUS TEXT NOT NULL,
                        DEPARTURE_AIRPORT TEXT NOT NULL,
                        ARRIVAL_AIRPORT TEXT NOT NULL
                    );
                ";
                cmd.ExecuteNonQuery();
            }

            
            var flightRepo = new FlightRepository(connection);
            flightService = new FlightService(flightRepo);
        }

        [TestCleanup]
        public void Cleanup()
        {
            connection?.Close();
            connection?.Dispose();
        }

        [TestMethod]
        public void CreateFlight_ShouldAddFlight()
        {
            flightService.CreateFlight("AB123", "Ulaanbaatar", "Tokyo");
            var flights = flightService.GetAllFlights();

            Assert.IsTrue(flights.Any(f => f.FlightNumber == "AB123" && f.DepartureLocation == "Ulaanbaatar" && f.ArrivalLocation == "Tokyo"));
        }

        [TestMethod]
        public void GetFlight_ShouldReturnCorrectFlight()
        {
            flightService.CreateFlight("CD456", "Beijing", "Seoul");
            var flight = flightService.GetAllFlights().FirstOrDefault();

            var result = flightService.GetFlight(flight.FlightId);

            Assert.IsNotNull(result);
            Assert.AreEqual("CD456", result.FlightNumber);
        }

        [TestMethod]
        public void ChangeStatus_ShouldUpdateFlightStatus()
        {
            flightService.CreateFlight("EF789", "London", "Paris");
            var flight = flightService.GetAllFlights().FirstOrDefault();

            flightService.ChangeStatus(flight.FlightId, FlightStatus.Boarding);
            var updatedFlight = flightService.GetFlight(flight.FlightId);

            Assert.AreEqual(FlightStatus.Boarding, updatedFlight.Status);
        }

        [TestMethod]
        public void DeleteFlight_ShouldRemoveFlight()
        {
            flightService.CreateFlight("GH012", "Berlin", "Rome");
            var flight = flightService.GetAllFlights().FirstOrDefault();

            flightService.DeleteFlight(flight.FlightId);
            var deletedFlight = flightService.GetFlight(flight.FlightId);

            Assert.IsNull(deletedFlight);
        }
    }
}