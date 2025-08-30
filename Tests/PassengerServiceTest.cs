using BussinessLogic.Interfaces;
using BussinessLogic.Services;
using DataAccess.Repositories;
using Microsoft.Data.Sqlite;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ModelAndDto.Models;
using System.Linq;

namespace Tests
{
    [TestClass]
    public class PassengerServiceTests
    {
        private SqliteConnection connection;
        private IPassengerService passengerService;

        [TestInitialize]
        public void Setup()
        {
            // Create and open the in-memory SQLite connection
            connection = new SqliteConnection("Data Source=:memory:");
            connection.Open();

            // Create schema for Passenger table
            using (var cmd = connection.CreateCommand())
            {
                cmd.CommandText = @"
                    CREATE TABLE IF NOT EXISTS Passenger (
                        ID INTEGER PRIMARY KEY AUTOINCREMENT,
                        FLIGHT_ID INTEGER NOT NULL,
                        NAME TEXT NOT NULL,
                        SEAT_ID INTEGER,
                        SEAT_NUMBER INTEGER,
                        PASSPORT_NUMBER TEXT NOT NULL
                    );
                ";
                cmd.ExecuteNonQuery();
            }

            // Use the repository and service with the same connection
            var passengerRepo = new PassengerRepository(connection);
            passengerService = new PassengerService(passengerRepo);
        }

        [TestCleanup]
        public void Cleanup()
        {
            connection?.Close();
            connection?.Dispose();
        }

        [TestMethod]
        public void RegisterPassenger_ShouldAddPassenger()
        {
            passengerService.RegisterPassenger("A12345678", "John Doe", 1);
            var passengers = passengerService.GetAllPassengers();

            Assert.IsTrue(passengers.Any(p => p.PassportNumber == "A12345678" && p.Name == "John Doe" && p.FlightId == 1));
        }

        [TestMethod]
        public void GetPassenger_ShouldReturnCorrectPassenger()
        {
            passengerService.RegisterPassenger("B87654321", "Jane Smith", 2);
            var passenger = passengerService.GetAllPassengers().FirstOrDefault();

            var result = passengerService.GetPassenger(passenger.Id);

            Assert.IsNotNull(result);
            Assert.AreEqual("Jane Smith", result.Name);
        }

        [TestMethod]
        public void GetByPassportNumber_ShouldReturnCorrectPassenger()
        {
            passengerService.RegisterPassenger("C11223344", "Alice", 3);
            var result = passengerService.GetByPassportNumber("C11223344");

            Assert.IsNotNull(result);
            Assert.AreEqual("Alice", result.Name);
        }

        [TestMethod]
        public void UpdatePassenger_ShouldModifyPassenger()
        {
            passengerService.RegisterPassenger("D99887766", "Bob", 4);
            var passenger = passengerService.GetAllPassengers().FirstOrDefault();
            passenger.Name = "Bob Updated";

            passengerService.UpdatePassenger(passenger);
            var updatedPassenger = passengerService.GetPassenger(passenger.Id);

            Assert.AreEqual("Bob Updated", updatedPassenger.Name);
        }

        [TestMethod]
        public void DeletePassenger_ShouldRemovePassenger()
        {
            passengerService.RegisterPassenger("E55443322", "Charlie", 5);
            var passenger = passengerService.GetAllPassengers().FirstOrDefault();

            passengerService.DeletePassenger(passenger.Id);
            var deletedPassenger = passengerService.GetPassenger(passenger.Id);

            Assert.IsNull(deletedPassenger);
        }
    }
}