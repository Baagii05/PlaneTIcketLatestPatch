using System;
using Microsoft.Data.Sqlite;

namespace DataAccess.SetUp
{
    public static class DatabaseInitializer
    {
        /// <summary>
        /// Ensures the database schema and seed data are created using the provided connection.
        /// </summary>
        public static void EnsureDatabaseCreated(SqliteConnection connection)
        {
            try
            {
                // Do NOT open/close the connection here; DI or caller manages it

                var createFlightsTableCmd = connection.CreateCommand();
                createFlightsTableCmd.CommandText =
                    @"CREATE TABLE IF NOT EXISTS Flight (
                        ID INTEGER PRIMARY KEY AUTOINCREMENT,
                        FLIGHT_NUMBER TEXT NOT NULL,
                        STATUS TEXT NOT NULL,
                        DEPARTURE_AIRPORT TEXT NOT NULL,
                        ARRIVAL_AIRPORT TEXT NOT NULL
                    );";
                createFlightsTableCmd.ExecuteNonQuery();

                var createSeatsTableCmd = connection.CreateCommand();
                createSeatsTableCmd.CommandText =
                    @"CREATE TABLE IF NOT EXISTS Seat (
                        ID INTEGER PRIMARY KEY AUTOINCREMENT,
                        FLIGHT_ID INTEGER NOT NULL,
                        SEAT_NUMBER INTEGER NOT NULL,
                        IS_AVAILABLE INTEGER NOT NULL,
                        FOREIGN KEY(FLIGHT_ID) REFERENCES Flight(ID)
                    );";
                createSeatsTableCmd.ExecuteNonQuery();

                var createPassengersTableCmd = connection.CreateCommand();
                createPassengersTableCmd.CommandText =
                    @"CREATE TABLE IF NOT EXISTS Passenger (
                        ID INTEGER PRIMARY KEY AUTOINCREMENT,
                        FLIGHT_ID INTEGER NOT NULL,
                        NAME TEXT NOT NULL,
                        SEAT_ID INTEGER,
                        SEAT_NUMBER INTEGER,
                        PASSPORT_NUMBER TEXT NOT NULL,
                        FOREIGN KEY(FLIGHT_ID) REFERENCES Flight(ID),
                        FOREIGN KEY(SEAT_ID) REFERENCES Seat(ID)
                    );";
                createPassengersTableCmd.ExecuteNonQuery();

                // Seed initial data if tables are empty
                var checkCmd = connection.CreateCommand();
                checkCmd.CommandText = "SELECT COUNT(*) FROM Flight;";
                var flightCount = (long)checkCmd.ExecuteScalar();

                if (flightCount == 0)
                {
                    // Seed flights
                    var seedFlightsCmd = connection.CreateCommand();
                    seedFlightsCmd.CommandText =
                        @"INSERT INTO Flight (FLIGHT_NUMBER, STATUS, DEPARTURE_AIRPORT, ARRIVAL_AIRPORT) VALUES
                        ('AA101', 'Registering', 'JFK', 'LAX'),
                        ('DL202', 'Delayed', 'ATL', 'ORD'),
                        ('UA303', 'Cancelled', 'SFO', 'SEA');";
                    seedFlightsCmd.ExecuteNonQuery();

                    // Seed seats
                    for (int flightId = 1; flightId <= 3; flightId++)
                    {
                        for (int seatNumber = 1; seatNumber <= 20; seatNumber++)
                        {
                            var seedSeatsCmd = connection.CreateCommand();
                            seedSeatsCmd.CommandText =
                                @"INSERT INTO Seat (FLIGHT_ID, SEAT_NUMBER, IS_AVAILABLE) VALUES
                                ($flightId, $seatNumber, 1);";
                            seedSeatsCmd.Parameters.AddWithValue("$flightId", flightId);
                            seedSeatsCmd.Parameters.AddWithValue("$seatNumber", seatNumber);
                            seedSeatsCmd.ExecuteNonQuery();
                        }
                    }

                    // Seed passengers
                    var seedPassengersCmd = connection.CreateCommand();
                    seedPassengersCmd.CommandText =
                        @"INSERT INTO Passenger (FLIGHT_ID, NAME, SEAT_ID, SEAT_NUMBER, PASSPORT_NUMBER) VALUES
                        (1, 'John', 1, 1, 'A12345678'),
                        (1, 'Jane', 2, 2, 'B87654321'),
                        (2, 'Alice', 21, 1, 'C11223344');";
                    seedPassengersCmd.ExecuteNonQuery();
                }
            }
            catch (SqliteException ex)
            {
                Console.Error.WriteLine($"Database error in EnsureDatabaseCreated: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unexpected error in EnsureDatabaseCreated: {ex.Message}");
                throw;
            }
        }
    }
}