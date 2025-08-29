using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using ModelAndDto.Models;

namespace DataAccess.Repositories
{
    public class SeatRepository
    {
        private readonly string _connectionString = "Data Source=flights.db";

        public IEnumerable<Seat> GetAll()
        {
            var seats = new List<Seat>();
            try
            {
                using var connection = new SqliteConnection(_connectionString);
                connection.Open();
                var cmd = connection.CreateCommand();
                cmd.CommandText = "SELECT ID, FLIGHT_ID, SEAT_NUMBER, IS_AVAILABLE FROM Seat;";
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    seats.Add(new Seat
                    {
                        Id = reader.GetInt32(0),
                        FlightId = reader.GetInt32(1),
                        SeatNumber = reader.GetInt32(2),
                        IsAvailable = reader.GetInt32(3) == 1
                    });
                }
            }
            catch (SqliteException ex)
            {
                Console.Error.WriteLine($"Database error in GetAll: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unexpected error in GetAll: {ex.Message}");
                throw;
            }
            return seats;
        }

        public Seat? GetById(int id)
        {
            try
            {
                using var connection = new SqliteConnection(_connectionString);
                connection.Open();
                var cmd = connection.CreateCommand();
                cmd.CommandText = "SELECT ID, FLIGHT_ID, SEAT_NUMBER, IS_AVAILABLE FROM Seat WHERE ID = $id;";
                cmd.Parameters.AddWithValue("$id", id);
                using var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    return new Seat
                    {
                        Id = reader.GetInt32(0),
                        FlightId = reader.GetInt32(1),
                        SeatNumber = reader.GetInt32(2),
                        IsAvailable = reader.GetInt32(3) == 1
                    };
                }
                return null;
            }
            catch (SqliteException ex)
            {
                Console.Error.WriteLine($"Database error in GetById: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unexpected error in GetById: {ex.Message}");
                throw;
            }
        }

        public void Add(Seat seat)
        {
            try
            {
                using var connection = new SqliteConnection(_connectionString);
                connection.Open();
                var cmd = connection.CreateCommand();
                cmd.CommandText = @"INSERT INTO Seat (FLIGHT_ID, SEAT_NUMBER, IS_AVAILABLE)
                                    VALUES ($flightId, $seatNumber, $isAvailable);";
                cmd.Parameters.AddWithValue("$flightId", seat.FlightId);
                cmd.Parameters.AddWithValue("$seatNumber", seat.SeatNumber);
                cmd.Parameters.AddWithValue("$isAvailable", seat.IsAvailable ? 1 : 0);
                cmd.ExecuteNonQuery();
            }
            catch (SqliteException ex)
            {
                Console.Error.WriteLine($"Database error in Add: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unexpected error in Add: {ex.Message}");
                throw;
            }
        }

        public void Update(Seat seat)
        {
            try
            {
                using var connection = new SqliteConnection(_connectionString);
                connection.Open();
                var cmd = connection.CreateCommand();
                cmd.CommandText = @"UPDATE Seat SET FLIGHT_ID = $flightId, SEAT_NUMBER = $seatNumber, IS_AVAILABLE = $isAvailable
                                    WHERE ID = $id;";
                cmd.Parameters.AddWithValue("$id", seat.Id);
                cmd.Parameters.AddWithValue("$flightId", seat.FlightId);
                cmd.Parameters.AddWithValue("$seatNumber", seat.SeatNumber);
                cmd.Parameters.AddWithValue("$isAvailable", seat.IsAvailable ? 1 : 0);
                cmd.ExecuteNonQuery();
            }
            catch (SqliteException ex)
            {
                Console.Error.WriteLine($"Database error in Update: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unexpected error in Update: {ex.Message}");
                throw;
            }
        }

        public void Delete(int id)
        {
            try
            {
                using var connection = new SqliteConnection(_connectionString);
                connection.Open();
                var cmd = connection.CreateCommand();
                cmd.CommandText = "DELETE FROM Seat WHERE ID = $id;";
                cmd.Parameters.AddWithValue("$id", id);
                cmd.ExecuteNonQuery();
            }
            catch (SqliteException ex)
            {
                Console.Error.WriteLine($"Database error in Delete: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unexpected error in Delete: {ex.Message}");
                throw;
            }
        }
    }
}