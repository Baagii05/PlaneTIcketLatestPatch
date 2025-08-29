using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using ModelAndDto.Models;

namespace DataAccess.Repositories
{
    public class PassengerRepository
    {
        private readonly string _connectionString = "Data Source=flights.db";

        public IEnumerable<Passenger> GetAll()
        {
            var passengers = new List<Passenger>();
            try
            {
                using var connection = new SqliteConnection(_connectionString);
                connection.Open();
                var cmd = connection.CreateCommand();
                cmd.CommandText = "SELECT ID, FLIGHT_ID, NAME, SEAT_ID, SEAT_NUMBER, PASSPORT_NUMBER FROM Passenger;";
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    passengers.Add(new Passenger
                    {
                        Id = reader.GetInt32(0),
                        FlightId = reader.GetInt32(1),
                        Name = reader.GetString(2),
                        SeatId = reader.IsDBNull(3) ? null : reader.GetInt32(3),
                        SeatNumber = reader.IsDBNull(4) ? null : reader.GetInt32(4),
                        PassportNumber = reader.GetString(5)
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
            return passengers;
        }

        public Passenger? GetById(int id)
        {
            try
            {
                using var connection = new SqliteConnection(_connectionString);
                connection.Open();
                var cmd = connection.CreateCommand();
                cmd.CommandText = "SELECT ID, FLIGHT_ID, NAME, SEAT_ID, SEAT_NUMBER, PASSPORT_NUMBER FROM Passenger WHERE ID = $id;";
                cmd.Parameters.AddWithValue("$id", id);
                using var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    return new Passenger
                    {
                        Id = reader.GetInt32(0),
                        FlightId = reader.GetInt32(1),
                        Name = reader.GetString(2),
                        SeatId = reader.IsDBNull(3) ? null : reader.GetInt32(3),
                        SeatNumber = reader.IsDBNull(4) ? null : reader.GetInt32(4),
                        PassportNumber = reader.GetString(5)
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

        public void Add(Passenger passenger)
        {
            try
            {
                using var connection = new SqliteConnection(_connectionString);
                connection.Open();
                var cmd = connection.CreateCommand();
                cmd.CommandText = @"INSERT INTO Passenger (FLIGHT_ID, NAME, SEAT_ID, SEAT_NUMBER, PASSPORT_NUMBER)
                                    VALUES ($flightId, $name, $seatId, $seatNumber, $passportNumber);";
                cmd.Parameters.AddWithValue("$flightId", passenger.FlightId);
                cmd.Parameters.AddWithValue("$name", passenger.Name);
                cmd.Parameters.AddWithValue("$seatId", (object?)passenger.SeatId ?? DBNull.Value);
                cmd.Parameters.AddWithValue("$seatNumber", (object?)passenger.SeatNumber ?? DBNull.Value);
                cmd.Parameters.AddWithValue("$passportNumber", passenger.PassportNumber);
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

        public void Update(Passenger passenger)
        {
            try
            {
                using var connection = new SqliteConnection(_connectionString);
                connection.Open();
                var cmd = connection.CreateCommand();
                cmd.CommandText = @"UPDATE Passenger SET FLIGHT_ID = $flightId, NAME = $name, SEAT_ID = $seatId, SEAT_NUMBER = $seatNumber, PASSPORT_NUMBER = $passportNumber
                                    WHERE ID = $id;";
                cmd.Parameters.AddWithValue("$id", passenger.Id);
                cmd.Parameters.AddWithValue("$flightId", passenger.FlightId);
                cmd.Parameters.AddWithValue("$name", passenger.Name);
                cmd.Parameters.AddWithValue("$seatId", (object?)passenger.SeatId ?? DBNull.Value);
                cmd.Parameters.AddWithValue("$seatNumber", (object?)passenger.SeatNumber ?? DBNull.Value);
                cmd.Parameters.AddWithValue("$passportNumber", passenger.PassportNumber);
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
                cmd.CommandText = "DELETE FROM Passenger WHERE ID = $id;";
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

        public Passenger? GetByPassportNumber(string passportNumber)
        {
            try
            {
                using var connection = new SqliteConnection(_connectionString);
                connection.Open();
                var cmd = connection.CreateCommand();
                cmd.CommandText = "SELECT ID, FLIGHT_ID, NAME, SEAT_ID, SEAT_NUMBER, PASSPORT_NUMBER FROM Passenger WHERE PASSPORT_NUMBER = $passportNumber;";
                cmd.Parameters.AddWithValue("$passportNumber", passportNumber);
                using var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    return new Passenger
                    {
                        Id = reader.GetInt32(0),
                        FlightId = reader.GetInt32(1),
                        Name = reader.GetString(2),
                        SeatId = reader.IsDBNull(3) ? null : reader.GetInt32(3),
                        SeatNumber = reader.IsDBNull(4) ? null : reader.GetInt32(4),
                        PassportNumber = reader.GetString(5)
                    };
                }
                return null;
            }
            catch (SqliteException ex)
            {
                Console.Error.WriteLine($"Database error in GetByPassportNumber: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unexpected error in GetByPassportNumber: {ex.Message}");
                throw;
            }
        }
    }
}