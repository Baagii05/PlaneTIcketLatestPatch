using ModelAndDto.Models;
using ModelAndDto.Dtos;
using System.Net.Http.Json;

namespace AirplaneFormApplication.apiClient
{
    public class ApiClient
    {
        private readonly HttpClient _http = new();
        private const string BaseUrl = "https://localhost:7156/api";

        // Passenger APIs
        public async Task<List<Passenger>> GetAllPassengersAsync()
        {
            return await _http.GetFromJsonAsync<List<Passenger>>($"{BaseUrl}/Passenger");
        }

        public async Task AddPassengerAsync(RegisterPassengerRequest request)
        {
            var response = await _http.PostAsJsonAsync($"{BaseUrl}/Passenger", request);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeletePassengerAsync(int id)
        {
            var response = await _http.DeleteAsync($"{BaseUrl}/Passenger/{id}");
            response.EnsureSuccessStatusCode();
        }

        public async Task UpdatePassengerAsync(Passenger passenger)
        {
            var response = await _http.PutAsJsonAsync($"{BaseUrl}/Passenger/{passenger.Id}", passenger);
            response.EnsureSuccessStatusCode();
        }

        // Flight APIs
        public async Task<List<Flight>> GetAllFlightsAsync()
        {
            return await _http.GetFromJsonAsync<List<Flight>>($"{BaseUrl}/Flight");
        }

        public async Task AddFlightAsync(Flight flight)
        {
            var response = await _http.PostAsJsonAsync($"{BaseUrl}/Flight", flight);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteFlightAsync(int id)
        {
            var response = await _http.DeleteAsync($"{BaseUrl}/Flight/{id}");
            response.EnsureSuccessStatusCode();
        }

        public async Task UpdateFlightAsync(Flight flight)
        {
            var response = await _http.PutAsJsonAsync($"{BaseUrl}/Flight/{flight.FlightId}", flight);
            response.EnsureSuccessStatusCode();
        }


        //seat api
        public async Task<List<Seat>> GetAllSeatsAsync()
        {
            return await _http.GetFromJsonAsync<List<Seat>>($"{BaseUrl}/Seat");
        }

        public async Task<Seat?> GetSeatAsync(int id)
        {
            return await _http.GetFromJsonAsync<Seat>($"{BaseUrl}/Seat/{id}");
        }

        public async Task<List<Seat>> GetSeatsByFlightIdAsync(int flightId)
        {
            var allSeats = await GetAllSeatsAsync();
            return allSeats.Where(s => s.FlightId == flightId).ToList();
        }

        public async Task<(bool Success, string Message)> AssignSeatAsync(SeatAssignmentRequest request)
        {
            try
            {
                var response = await _http.PostAsJsonAsync($"{BaseUrl}/SeatAssignment/assign", request);

                if (response.IsSuccessStatusCode)
                {
                    var message = await response.Content.ReadAsStringAsync();
                    return (true, message);
                }
                else
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    return (false, errorMessage);
                }
            }
            catch (Exception ex)
            {
                return (false, $"Error: {ex.Message}");
            }
        }

        public async Task<bool> IsSeatAvailableAsync(int flightId, int seatNumber)
        {
            try
            {
                return await _http.GetFromJsonAsync<bool>($"{BaseUrl}/SeatAssignment/available?flightId={flightId}&seatNumber={seatNumber}");
            }
            catch
            {
                return false;
            }
        }
    }
}
