using Microsoft.AspNetCore.SignalR;

namespace SignalRServerWebApp.Hubs
{
    public class FlightHub : Hub
    {
        /// <summary>
        /// Notify all clients about flight status change
        /// </summary>
        public async Task NotifyFlightStatusChanged(int flightId, int status)
        {
            Console.WriteLine($"========== FLIGHT STATUS CHANGED ===========");
            Console.WriteLine($"FlightHub: Flight ID: {flightId}, new status: {status}");

            try
            {
                await Clients.All.SendAsync("FlightStatusChanged", flightId, status);
                Console.WriteLine($"SignalR notification sent successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending SignalR notification: {ex.Message}");
            }
        }

        /// <summary>
        /// Join a specific flight group for targeted updates
        /// </summary>
        public async Task JoinFlightGroup(int flightId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"flight_{flightId}");
        }

        /// <summary>
        /// Called when client connects
        /// </summary>
        public override async Task OnConnectedAsync()
        {
            await Clients.Caller.SendAsync("Connected", "Successfully connected to flight updates");
            Console.WriteLine($"🔗 Blazor client connected: {Context.ConnectionId}");
            await base.OnConnectedAsync();
        }

        /// <summary>
        /// Called when client disconnects
        /// </summary>
        public override async Task OnDisconnectedAsync(Exception exception)
        {
            Console.WriteLine($"❌ Blazor client disconnected: {Context.ConnectionId}");
            await base.OnDisconnectedAsync(exception);
        }
    }
}