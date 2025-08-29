using Microsoft.AspNetCore.SignalR.Client;
using ModelAndDto.Models;

namespace FlightDashboardWeb.Services
{
    public class FlightSignalRService : IAsyncDisposable
    {
        private HubConnection? _hubConnection;
        private readonly string _hubUrl = "http://localhost:8080/flightHub";

        // Events for components to subscribe to
        public event Action<int, FlightStatus>? FlightStatusChanged;
        public event Action<string>? ConnectionStatusChanged;

        public bool IsConnected => _hubConnection?.State == HubConnectionState.Connected;
        public string ConnectionStatus { get; private set; } = "Disconnected";

        /// <summary>
        /// Start SignalR connection
        /// </summary>
        public async Task StartAsync()
        {
            if (_hubConnection != null)
            {
                return; // Already started
            }

            _hubConnection = new HubConnectionBuilder()
                .WithUrl(_hubUrl)
                .WithAutomaticReconnect()
                .Build();

            // Listen for flight status changes
            _hubConnection.On<int, int>("FlightStatusChanged", (flightId, statusInt) =>
            {
                var newStatus = (FlightStatus)statusInt;
                Console.WriteLine($"🔔 SignalR: Flight {flightId} → {newStatus}");
                FlightStatusChanged?.Invoke(flightId, newStatus);
            });

            // Handle connection events
            _hubConnection.On<string>("Connected", (message) =>
            {
                UpdateConnectionStatus("Connected");
                Console.WriteLine($"✅ SignalR: {message}");
            });

            _hubConnection.Reconnecting += (exception) =>
            {
                UpdateConnectionStatus("Reconnecting...");
                Console.WriteLine("🔄 SignalR reconnecting...");
                return Task.CompletedTask;
            };

            _hubConnection.Reconnected += (connectionId) =>
            {
                UpdateConnectionStatus("Reconnected");
                Console.WriteLine("✅ SignalR reconnected");
                return Task.CompletedTask;
            };

            _hubConnection.Closed += (exception) =>
            {
                UpdateConnectionStatus("Disconnected");
                Console.WriteLine("❌ SignalR connection closed");
                return Task.CompletedTask;
            };

            try
            {
                await _hubConnection.StartAsync();
                UpdateConnectionStatus("Connected");
                Console.WriteLine("✅ SignalR connection established");
            }
            catch (Exception ex)
            {
                UpdateConnectionStatus("Failed to connect");
                Console.WriteLine($"❌ SignalR connection failed: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Join a specific flight group for targeted updates
        /// </summary>
        public async Task JoinFlightGroupAsync(int flightId)
        {
            if (_hubConnection is not null && IsConnected)
            {
                await _hubConnection.SendAsync("JoinFlightGroup", flightId);
                Console.WriteLine($"📡 Joined flight group: {flightId}");
            }
        }

        /// <summary>
        /// Stop SignalR connection
        /// </summary>
        public async Task StopAsync()
        {
            if (_hubConnection is not null)
            {
                await _hubConnection.StopAsync();
                UpdateConnectionStatus("Disconnected");
            }
        }

        private void UpdateConnectionStatus(string status)
        {
            ConnectionStatus = status;
            ConnectionStatusChanged?.Invoke(status);
        }

        public async ValueTask DisposeAsync()
        {
            if (_hubConnection is not null)
            {
                await _hubConnection.DisposeAsync();
            }
        }
    }
}