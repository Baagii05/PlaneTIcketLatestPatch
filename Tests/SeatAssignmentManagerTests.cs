using BussinessLogic.Interfaces;
using BussinessLogic.Services;
using DataAccess.Repositories;
using Microsoft.Data.Sqlite;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ModelAndDto.Models;
using System.Threading.Tasks;

namespace Tests
{
    [TestClass]
    public class SeatAssignmentManagerTests
    {
        private SqliteConnection connection;
        private ISeatAssignmentManager seatAssignmentManager;
        private PassengerRepository passengerRepo;
        private SeatRepository seatRepo;

        [TestInitialize]
        public void Setup()
        {
            // Create and open the in-memory SQLite connection
            connection = new SqliteConnection("Data Source=:memory:");
            connection.Open();

            // Create schema for Passenger and Seat tables
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

                cmd.CommandText = @"
                    CREATE TABLE IF NOT EXISTS Seat (
                        ID INTEGER PRIMARY KEY AUTOINCREMENT,
                        FLIGHT_ID INTEGER NOT NULL,
                        SEAT_NUMBER INTEGER NOT NULL,
                        IS_AVAILABLE INTEGER NOT NULL
                    );
                ";
                cmd.ExecuteNonQuery();
            }

            // Seed a passenger and a seat
            using (var cmd = connection.CreateCommand())
            {
                cmd.CommandText = @"
                    INSERT INTO Passenger (FLIGHT_ID, NAME, SEAT_ID, SEAT_NUMBER, PASSPORT_NUMBER)
                    VALUES (1, 'Test Passenger', NULL, NULL, 'P123456');
                ";
                cmd.ExecuteNonQuery();

                cmd.CommandText = @"
                    INSERT INTO Seat (FLIGHT_ID, SEAT_NUMBER, IS_AVAILABLE)
                    VALUES (1, 1, 1);
                ";
                cmd.ExecuteNonQuery();
            }

            // Use the repositories and manager with the same connection
            passengerRepo = new PassengerRepository(connection);
            seatRepo = new SeatRepository(connection);
            var passengerService = new PassengerService(passengerRepo);
            var seatService = new SeatService(seatRepo);
            seatAssignmentManager = new SeatAssignmentManager(seatService, passengerService);
        }

        [TestCleanup]
        public void Cleanup()
        {
            connection?.Close();
            connection?.Dispose();
        }

        [TestMethod]
        public async Task AssignSeatAsync_ShouldAssignSeatToPassenger()
        {
            var passenger = passengerRepo.GetAll().First();
            var seat = seatRepo.GetAll().First();

            var result = await seatAssignmentManager.AssignSeatAsync(passenger.Id, seat.FlightId, seat.SeatNumber);

            Assert.IsTrue(result.Success, result.Message);

            // Check that the seat is no longer available
            var updatedSeat = seatRepo.GetById(seat.Id);
            Assert.IsFalse(updatedSeat.IsAvailable);

            // Check that the passenger has the seat assigned
            var updatedPassenger = passengerRepo.GetById(passenger.Id);
            Assert.AreEqual(seat.SeatNumber, updatedPassenger.SeatNumber);
        }

        [TestMethod]
        public async Task IsSeatAvailableAsync_ShouldReturnTrueForAvailableSeat()
        {
            var seat = seatRepo.GetAll().First();
            var isAvailable = await seatAssignmentManager.IsSeatAvailableAsync(seat.FlightId, seat.SeatNumber);

            Assert.IsTrue(isAvailable);
        }

        [TestMethod]
        public async Task IsSeatAvailableAsync_ShouldReturnFalseForUnavailableSeat()
        {
            var seat = seatRepo.GetAll().First();
            // Assign the seat to make it unavailable
            var passenger = passengerRepo.GetAll().First();
            await seatAssignmentManager.AssignSeatAsync(passenger.Id, seat.FlightId, seat.SeatNumber);

            var isAvailable = await seatAssignmentManager.IsSeatAvailableAsync(seat.FlightId, seat.SeatNumber);

            Assert.IsFalse(isAvailable);
        }
    }
}