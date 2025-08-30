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
    public class SeatServiceTests
    {
        private SqliteConnection connection;
        private ISeatService seatService;

        [TestInitialize]
        public void Setup()
        {
            // Create and open the in-memory SQLite connection
            connection = new SqliteConnection("Data Source=:memory:");
            connection.Open();

            // Create schema for Seat table
            using (var cmd = connection.CreateCommand())
            {
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

            // Use the repository and service with the same connection
            var seatRepo = new SeatRepository(connection);
            seatService = new SeatService(seatRepo);
        }

        [TestCleanup]
        public void Cleanup()
        {
            connection?.Close();
            connection?.Dispose();
        }

        [TestMethod]
        public void AddSeat_ShouldAddSeat()
        {
            seatService.AddSeat(1, 10);
            var seats = seatService.GetAllSeats();

            Assert.IsTrue(seats.Any(s => s.FlightId == 1 && s.SeatNumber == 10));
        }

        [TestMethod]
        public void GetSeat_ShouldReturnCorrectSeat()
        {
            seatService.AddSeat(2, 20);
            var seat = seatService.GetAllSeats().FirstOrDefault();

            var result = seatService.GetSeat(seat.Id);

            Assert.IsNotNull(result);
            Assert.AreEqual(20, result.SeatNumber);
        }

        [TestMethod]
        public void UpdateSeat_ShouldModifySeat()
        {
            seatService.AddSeat(3, 30);
            var seat = seatService.GetAllSeats().FirstOrDefault();
            seat.IsAvailable = false;

            seatService.UpdateSeat(seat);
            var updatedSeat = seatService.GetSeat(seat.Id);

            Assert.IsFalse(updatedSeat.IsAvailable);
        }

        [TestMethod]
        public void DeleteSeat_ShouldRemoveSeat()
        {
            seatService.AddSeat(4, 40);
            var seat = seatService.GetAllSeats().FirstOrDefault();

            seatService.DeleteSeat(seat.Id);
            var deletedSeat = seatService.GetSeat(seat.Id);

            Assert.IsNull(deletedSeat);
        }
    }
}