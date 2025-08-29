using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using ModelAndDto.Models;

namespace DataAccess.Repositories
{
    public class FlightRepository
    {
        private readonly string _connectionString = "Data Source=flights.db";

        public IEnumerable<Flight> GetAll()
        {
            var flights = new List<Flight>();
            try
            {
                using var connection = new SqliteConnection(_connectionString);
                connection.Open();
                var cmd = connection.CreateCommand();
                cmd.CommandText = "SELECT ID, FLIGHT_NUMBER, STATUS, DEPARTURE_AIRPORT, ARRIVAL_AIRPORT FROM Flight;";
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    flights.Add(new Flight
                    {
                        FlightId = reader.GetInt32(0),
                        FlightNumber = reader.GetString(1),
                        Status = Enum.Parse<FlightStatus>(reader.GetString(2)),
                        DepartureLocation = reader.GetString(3),
                        ArrivalLocation = reader.GetString(4)
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
            return flights;
        }

        public Flight? GetById(int id)
        {
            try
            {
                using var connection = new SqliteConnection(_connectionString);
                connection.Open();
                var cmd = connection.CreateCommand();
                cmd.CommandText = "SELECT ID, FLIGHT_NUMBER, STATUS, DEPARTURE_AIRPORT, ARRIVAL_AIRPORT FROM Flight WHERE ID = $id;";
                cmd.Parameters.AddWithValue("$id", id);
                using var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    return new Flight
                    {
                        FlightId = reader.GetInt32(0),
                        FlightNumber = reader.GetString(1),
                        Status = Enum.Parse<FlightStatus>(reader.GetString(2)),
                        DepartureLocation = reader.GetString(3),
                        ArrivalLocation = reader.GetString(4)
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

        public void Add(Flight flight)
        {
            try
            {
                using var connection = new SqliteConnection(_connectionString);
                connection.Open();
                var cmd = connection.CreateCommand();
                cmd.CommandText = @"INSERT INTO Flight (FLIGHT_NUMBER, STATUS, DEPARTURE_AIRPORT, ARRIVAL_AIRPORT)
                                    VALUES ($number, $status, $departure, $arrival);";
                cmd.Parameters.AddWithValue("$number", flight.FlightNumber);
                cmd.Parameters.AddWithValue("$status", flight.Status.ToString());
                cmd.Parameters.AddWithValue("$departure", flight.DepartureLocation);
                cmd.Parameters.AddWithValue("$arrival", flight.ArrivalLocation);
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

        public void Update(Flight flight)
        {
            try
            {
                using var connection = new SqliteConnection(_connectionString);
                connection.Open();
                var cmd = connection.CreateCommand();
                cmd.CommandText = @"UPDATE Flight SET FLIGHT_NUMBER = $number, STATUS = $status, DEPARTURE_AIRPORT = $departure, ARRIVAL_AIRPORT = $arrival
                                    WHERE ID = $id;";
                cmd.Parameters.AddWithValue("$id", flight.FlightId);
                cmd.Parameters.AddWithValue("$number", flight.FlightNumber);
                cmd.Parameters.AddWithValue("$status", flight.Status.ToString());
                cmd.Parameters.AddWithValue("$departure", flight.DepartureLocation);
                cmd.Parameters.AddWithValue("$arrival", flight.ArrivalLocation);
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
                cmd.CommandText = "DELETE FROM Flight WHERE ID = $id;";
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