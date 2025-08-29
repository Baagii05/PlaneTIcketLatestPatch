using ModelAndDto.Dtos;
using System.Net.Http.Json;

namespace FlightDashboardWeb.Services
{
    public class FlightDataService
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "https://localhost:7156/api";

        public FlightDataService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        /// <summary>
        /// Load all flights from REST API
        /// </summary>
        public async Task<List<FlightInfo>> GetAllFlightsAsync()
        {
            try
            {
                Console.WriteLine("🔄 Loading flights from REST API...");
                var flights = await _httpClient.GetFromJsonAsync<List<FlightInfo>>($"{BaseUrl}/flight");
                Console.WriteLine($"✅ Loaded {flights?.Count ?? 0} flights");
                return flights ?? new List<FlightInfo>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error loading flights: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Get a specific flight by ID
        /// </summary>
        public async Task<FlightInfo?> GetFlightByIdAsync(int flightId)
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<FlightInfo>($"{BaseUrl}/flight/{flightId}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error loading flight {flightId}: {ex.Message}");
                return null;
            }
        }
    }
}