using BussinessLogic.Interfaces;
using BussinessLogic.Services;
using DataAccess.Repositories;
using Microsoft.Data.Sqlite;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ModelAndDto.Models;
using System.Collections.Concurrent;
using System.Text;
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

            connection = new SqliteConnection("Data Source=:memory:");
            connection.Open();


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


            passengerRepo = new PassengerRepository(connection);
            seatRepo = new SeatRepository(connection);
            var passengerService = new PassengerService(passengerRepo);
            var seatService = new SeatService(seatRepo);
            seatAssignmentManager = new SeatAssignmentManager(seatService, passengerService);
        }

        [TestCleanup]
        public void Cleanup()
        {
            seatAssignmentManager?.Dispose();
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


            var updatedSeat = seatRepo.GetById(seat.Id);
            Assert.IsFalse(updatedSeat.IsAvailable);


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

            var passenger = passengerRepo.GetAll().First();
            await seatAssignmentManager.AssignSeatAsync(passenger.Id, seat.FlightId, seat.SeatNumber);

            var isAvailable = await seatAssignmentManager.IsSeatAvailableAsync(seat.FlightId, seat.SeatNumber);

            Assert.IsFalse(isAvailable);
        }

        [TestMethod]
        public async Task ConcurrentSeatAssignment_ShouldOnlyAllowOneSeatAssignment()
        {
            
            var seatNumber = 1;
            var flightId = 1;

           
            using (var cmd = connection.CreateCommand())
            {
                cmd.CommandText = @"
                    INSERT INTO Passenger (FLIGHT_ID, NAME, SEAT_ID, SEAT_NUMBER, PASSPORT_NUMBER)
                    VALUES 
                        (1, 'Passenger 2', NULL, NULL, 'P123457'),
                        (1, 'Passenger 3', NULL, NULL, 'P123458'),
                        (1, 'Passenger 4', NULL, NULL, 'P123459'),
                        (1, 'Passenger 5', NULL, NULL, 'P123460');
                ";
                cmd.ExecuteNonQuery();
            }

            var passengers = passengerRepo.GetAll().ToList();
            var tasks = new List<Task<(bool Success, string Message, int PassengerId)>>();

        
            foreach (var passenger in passengers)
            {
                var task = Task.Run(async () =>
                {
                    var result = await seatAssignmentManager.AssignSeatAsync(passenger.Id, flightId, seatNumber);
                    return (result.Success, result.Message, passenger.Id);
                });
                tasks.Add(task);
            }

           
            var results = await Task.WhenAll(tasks);

     
            var successfulAssignments = results.Where(r => r.Success).ToList();
            var failedAssignments = results.Where(r => !r.Success).ToList();

            Assert.AreEqual(1, successfulAssignments.Count, "Only one seat assignment should succeed");
            Assert.AreEqual(passengers.Count - 1, failedAssignments.Count, "All other assignments should fail");

         
            var seatAfterAssignment = seatRepo.GetAll().First(s => s.SeatNumber == seatNumber);
            Assert.IsFalse(seatAfterAssignment.IsAvailable, "Seat should be marked as unavailable");

         
            var passengersWithSeat = passengerRepo.GetAll().Where(p => p.SeatNumber == seatNumber).ToList();
            Assert.AreEqual(1, passengersWithSeat.Count, "Only one passenger should have the seat assigned");

            
            var assignedPassenger = passengersWithSeat.First();
            var successfulResult = successfulAssignments.First();
            Assert.AreEqual(assignedPassenger.Id, successfulResult.PassengerId, "The successful assignment should match the assigned passenger");

        
            foreach (var failedResult in failedAssignments)
            {
                Assert.IsTrue(failedResult.Message.Contains("already reserved") ||
                             failedResult.Message.Contains("Seat already reserved"),
                             $"Failed assignment should indicate seat is already reserved. Actual message: {failedResult.Message}");
            }
        }

        [TestMethod]
        public async Task SequentialSeatAssignment_ShouldProcessInOrder()
        {
      
            using (var cmd = connection.CreateCommand())
            {
                cmd.CommandText = @"
                    INSERT INTO Passenger (FLIGHT_ID, NAME, SEAT_ID, SEAT_NUMBER, PASSPORT_NUMBER)
                    VALUES 
                        (1, 'Passenger 2', NULL, NULL, 'P123457'),
                        (1, 'Passenger 3', NULL, NULL, 'P123458');
                    
                    INSERT INTO Seat (FLIGHT_ID, SEAT_NUMBER, IS_AVAILABLE)
                    VALUES 
                        (1, 2, 1),
                        (1, 3, 1);
                ";
                cmd.ExecuteNonQuery();
            }

            var passengers = passengerRepo.GetAll().ToList();
            var seats = seatRepo.GetAll().OrderBy(s => s.SeatNumber).ToList();

          
            var results = new List<(bool Success, string Message, int PassengerId, int SeatNumber)>();

            for (int i = 0; i < passengers.Count && i < seats.Count; i++)
            {
                var result = await seatAssignmentManager.AssignSeatAsync(
                    passengers[i].Id,
                    seats[i].FlightId,
                    seats[i].SeatNumber);

                results.Add((result.Success, result.Message, passengers[i].Id, seats[i].SeatNumber));
            }

           
            Assert.IsTrue(results.All(r => r.Success), "All sequential assignments should succeed");

            
            foreach (var result in results)
            {
                var passenger = passengerRepo.GetById(result.PassengerId);
                Assert.AreEqual(result.SeatNumber, passenger.SeatNumber,
                    $"Passenger {result.PassengerId} should have seat {result.SeatNumber}");
            }
        }

        [TestMethod]
        public async Task EnqueueRequest_ShouldProcessRequestsInQueue()
        {
            
            var passengers = passengerRepo.GetAll().ToList();
            var seat = seatRepo.GetAll().First();

            using (var cmd = connection.CreateCommand())
            {
                cmd.CommandText = @"
                    INSERT INTO Passenger (FLIGHT_ID, NAME, SEAT_ID, SEAT_NUMBER, PASSPORT_NUMBER)
                    VALUES 
                        (1, 'Queue Passenger 1', NULL, NULL, 'Q123456'),
                        (1, 'Queue Passenger 2', NULL, NULL, 'Q123457');
                ";
                cmd.ExecuteNonQuery();
            }

            passengers = passengerRepo.GetAll().ToList();
            var results = new ConcurrentBag<(bool Success, string Message, int PassengerId)>();

            
            var taskCompletionSources = new List<TaskCompletionSource<(bool Success, string Message)>>();

            foreach (var passenger in passengers)
            {
                var tcs = new TaskCompletionSource<(bool Success, string Message)>();
                taskCompletionSources.Add(tcs);

                var request = new SeatAssignmentRequest
                {
                    PassengerId = passenger.Id,
                    FlightId = seat.FlightId,
                    SeatNumber = seat.SeatNumber,
                    Callback = (success, message) =>
                    {
                        results.Add((success, message, passenger.Id));
                        tcs.SetResult((success, message));
                    }
                };

                seatAssignmentManager.EnqueueRequest(request);
            }

           
            var allTasks = taskCompletionSources.Select(tcs => tcs.Task).ToArray();
            var timeoutTask = Task.Delay(TimeSpan.FromSeconds(10));

            var completedTask = await Task.WhenAny(Task.WhenAll(allTasks), timeoutTask);

            if (completedTask == timeoutTask)
            {
                Assert.Fail("Test timed out waiting for all requests to be processed");
            }

            
            var taskResults = await Task.WhenAll(allTasks);

            
            Assert.AreEqual(passengers.Count, results.Count,
                $"All requests should have results. Expected: {passengers.Count}, Actual: {results.Count}");

            Assert.AreEqual(passengers.Count, taskResults.Length,
                $"All tasks should complete. Expected: {passengers.Count}, Actual: {taskResults.Length}");

            var successfulResults = results.Where(r => r.Success).ToList();
            var failedResults = results.Where(r => !r.Success).ToList();

            Assert.AreEqual(1, successfulResults.Count, "Only one request should succeed");
            Assert.AreEqual(passengers.Count - 1, failedResults.Count, "All other requests should fail");

            
            var updatedSeat = seatRepo.GetById(seat.Id);
            Assert.IsFalse(updatedSeat.IsAvailable, "Seat should be marked as unavailable after processing");

           
            foreach (var failedResult in failedResults)
            {
                Assert.IsTrue(failedResult.Message.Contains("already reserved") ||
                             failedResult.Message.Contains("Seat already reserved"),
                             $"Failed assignment should indicate seat is already reserved. Actual message: {failedResult.Message}");
            }
        }

        [TestMethod]
        public async Task HighConcurrencySeatAssignment_ShouldMaintainDataIntegrity()
        {
           
            using (var cmd = connection.CreateCommand())
            {
               
                var insertPassengerQuery = new StringBuilder("INSERT INTO Passenger (FLIGHT_ID, NAME, SEAT_ID, SEAT_NUMBER, PASSPORT_NUMBER) VALUES ");
                var passengerValues = new List<string>();

                for (int i = 1; i <= 20; i++)
                {
                    passengerValues.Add($"(1, 'Stress Test Passenger {i}', NULL, NULL, 'ST{i:D6}')");
                }

                insertPassengerQuery.Append(string.Join(", ", passengerValues));
                cmd.CommandText = insertPassengerQuery.ToString();
                cmd.ExecuteNonQuery();

               
                cmd.CommandText = @"
                    INSERT INTO Seat (FLIGHT_ID, SEAT_NUMBER, IS_AVAILABLE)
                    VALUES 
                        (1, 2, 1),
                        (1, 3, 1),
                        (1, 4, 1);
                ";
                cmd.ExecuteNonQuery();
            }

            var allPassengers = passengerRepo.GetAll().ToList();
            var allSeats = seatRepo.GetAll().ToList();
            var totalSeats = allSeats.Count;

            
            var tasks = allPassengers.Select(passenger =>
            {
                
                var randomSeat = allSeats[passenger.Id % totalSeats];
                return Task.Run(async () =>
                {
                    var result = await seatAssignmentManager.AssignSeatAsync(
                        passenger.Id,
                        randomSeat.FlightId,
                        randomSeat.SeatNumber);
                    return (result.Success, result.Message, passenger.Id, randomSeat.SeatNumber);
                });
            }).ToList();

            var results = await Task.WhenAll(tasks);

            
            var successfulAssignments = results.Where(r => r.Success).ToList();
            var failedAssignments = results.Where(r => !r.Success).ToList();

            
            Assert.AreEqual(totalSeats, successfulAssignments.Count,
                $"Should have exactly {totalSeats} successful assignments");

            Assert.AreEqual(allPassengers.Count - totalSeats, failedAssignments.Count,
                "Remaining passengers should have failed assignments");

            
            var assignedPassengers = passengerRepo.GetAll().Where(p => p.SeatNumber.HasValue).ToList();
            Assert.AreEqual(totalSeats, assignedPassengers.Count,
                "Should have exactly as many passengers with seats as total seats");

            
            var unavailableSeats = seatRepo.GetAll().Where(s => !s.IsAvailable).ToList();
            Assert.AreEqual(totalSeats, unavailableSeats.Count,
                "All seats should be marked as unavailable");

            
            var seatAssignments = assignedPassengers.GroupBy(p => p.SeatNumber).ToList();
            foreach (var group in seatAssignments)
            {
                Assert.AreEqual(1, group.Count(),
                    $"Seat {group.Key} should only be assigned to one passenger");
            }
        }
    }
}