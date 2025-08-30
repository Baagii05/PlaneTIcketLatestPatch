using System;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using ModelAndDto.Models;

namespace AirplaneFormApplication.WebSocket
{
    public class WebSocketClient : IDisposable
    {
        private Socket _clientSocket;
        private bool _isConnected = false;
        private CancellationTokenSource _cancellationTokenSource;
        private Task _receiveTask;
        private readonly string _serverAddress;
        private readonly int _serverPort;
        private bool _disposed = false;

        public event Action<int, FlightStatus> FlightStatusChanged;
        public event Action<string> ConnectionStatusChanged;
        public event Action<int, string> PassengerSelectionChanged;
        public event Action<int, int, string> SeatSelectionChanged;
        public event Action<int> PassengerListRefreshRequested;
        public event Action<int> SeatStatesRequested; // ✅ New event for state requestsa

        public bool IsConnected => _isConnected && _clientSocket != null && _clientSocket.Connected && !_disposed;

        public WebSocketClient(string serverAddress = "127.0.0.1", int serverPort = 9009)
        {
            _serverAddress = serverAddress;
            _serverPort = serverPort;
            _cancellationTokenSource = new CancellationTokenSource();
        }

        public async Task ConnectAsync()
        {
            if (_disposed) return;

            try
            {
                _clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                
                // Set socket options for better cleanup
                _clientSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
                _clientSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Linger, new LingerOption(true, 0));
                
                await Task.Run(() => _clientSocket.Connect(_serverAddress, _serverPort));

                _isConnected = true;
                ConnectionStatusChanged?.Invoke("Connected");

                _receiveTask = Task.Run(ReceiveMessages);

                Console.WriteLine("✅ WebSocket клиент холбогдлоо");
            }
            catch (Exception ex)
            {
                _isConnected = false;
                ConnectionStatusChanged?.Invoke($"Connection failed: {ex.Message}");
                Console.WriteLine($"❌ Холболтод алдаа гарлаа: {ex.Message}");
            }
        }

        private async Task ReceiveMessages()
        {
            byte[] buffer = new byte[4096];

            try
            {
                while (!_cancellationTokenSource.Token.IsCancellationRequested && IsConnected)
                {
                    try
                    {
                        // Check if socket is still valid before receiving
                        if (_clientSocket == null || !_clientSocket.Connected || _disposed)
                        {
                            Console.WriteLine("🔌 Socket disconnected or disposed, exiting receive loop");
                            break;
                        }

                        int bytesRead = _clientSocket.Receive(buffer, SocketFlags.None);

                        if (bytesRead > 0)
                        {
                            string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                            ProcessMessage(message);
                        }
                        else
                        {
                            Console.WriteLine("🔌 Server closed connection gracefully");
                            break;
                        }
                    }
                    catch (SocketException socketEx) when (socketEx.SocketErrorCode == SocketError.ConnectionAborted)
                    {
                        // Expected when disposing - don't log as error
                        Console.WriteLine("🔌 Connection aborted during shutdown");
                        break;
                    }
                    catch (SocketException socketEx)
                    {
                        HandleSocketException(socketEx);
                        break;
                    }
                    catch (ObjectDisposedException)
                    {
                        Console.WriteLine("🔌 Socket disposed");
                        break;
                    }
                    catch (OperationCanceledException)
                    {
                        Console.WriteLine("🔌 Receive operation cancelled");
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                if (!_cancellationTokenSource.Token.IsCancellationRequested && !_disposed)
                {
                    Console.WriteLine($"❌ Receive messages error: {ex.Message}");
                }
            }
            finally
            {
                _isConnected = false;
                if (!_disposed)
                {
                    ConnectionStatusChanged?.Invoke("Disconnected");
                }
            }
        }

        private void HandleSocketException(SocketException socketEx)
        {
            if (_disposed) return;

            switch (socketEx.SocketErrorCode)
            {
                case SocketError.ConnectionAborted:
                    Console.WriteLine("🔌 Connection was aborted");
                    break;
                case SocketError.ConnectionReset:
                    Console.WriteLine("🔌 Connection was reset by remote host");
                    break;
                case SocketError.TimedOut:
                    Console.WriteLine("🔌 Connection timed out");
                    break;
                default:
                    Console.WriteLine($"🔌 Socket error: {socketEx.SocketErrorCode} - {socketEx.Message}");
                    break;
            }
            
            _isConnected = false;
        }

        // Add this method to send a request for current states
        public async Task RequestCurrentStates(int flightId)
        {
            if (!IsConnected) return;

            try
            {
                var request = new
                {
                    Event = "RequestCurrentStates",
                    Data = new
                    {
                        FlightId = flightId,
                        Timestamp = DateTime.UtcNow
                    }
                };

                var json = JsonSerializer.Serialize(request);
                var messageBytes = Encoding.UTF8.GetBytes(json);

                _clientSocket.Send(messageBytes);
                Console.WriteLine($"📤 Current states request sent for flight: {flightId}");
            }
            catch (Exception ex)
            {
                if (!_disposed)
                    Console.WriteLine($"❌ Current states request error: {ex.Message}");
            }
        }

        private void ProcessMessage(string message)
        {
            if (_disposed) return;

            try
            {
                var jsonDoc = JsonDocument.Parse(message);
                var root = jsonDoc.RootElement;

                if (root.TryGetProperty("Event", out var eventElement) &&
                    root.TryGetProperty("Data", out var dataElement))
                {
                    string eventName = eventElement.GetString();

                    switch (eventName)
                    {
                        case "FlightStatusChanged":
                            if (dataElement.TryGetProperty("FlightId", out var flightIdElement) &&
                                dataElement.TryGetProperty("Status", out var statusElement))
                            {
                                int flightId = flightIdElement.GetInt32();
                                int status = statusElement.GetInt32();
                                FlightStatusChanged?.Invoke(flightId, (FlightStatus)status);
                            }
                            break;

                        case "SeatSelectionChanged":
                            if (dataElement.TryGetProperty("FlightId", out var seatFlightIdElement) &&
                                dataElement.TryGetProperty("SeatNumber", out var seatNumberElement) &&
                                dataElement.TryGetProperty("Status", out var seatStatusElement))
                            {
                                int flightId = seatFlightIdElement.GetInt32();
                                int seatNumber = seatNumberElement.GetInt32();
                                string seatStatus = seatStatusElement.GetString();
                                SeatSelectionChanged?.Invoke(flightId, seatNumber, seatStatus);
                            }
                            break;

                        // ✅ Add this new case
                        case "RequestCurrentStates":
                            if (dataElement.TryGetProperty("FlightId", out var requestFlightIdElement))
                            {
                                int flightId = requestFlightIdElement.GetInt32();
                                SeatStatesRequested?.Invoke(flightId);
                            }
                            break;

                        case "PassengerListRefresh":
                            if (dataElement.TryGetProperty("FlightId", out var refreshFlightIdElement))
                            {
                                int flightId = refreshFlightIdElement.GetInt32();
                                PassengerListRefreshRequested?.Invoke(flightId);
                            }
                            break;

                        case "PassengerSelectionChanged":
                            if (dataElement.TryGetProperty("PassengerId", out var passengerIdElement) &&
                                dataElement.TryGetProperty("Status", out var passengerStatusElement))
                            {
                                int passengerId = passengerIdElement.GetInt32();
                                string passengerStatus = passengerStatusElement.GetString();
                                PassengerSelectionChanged?.Invoke(passengerId, passengerStatus);
                            }
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                if (!_disposed)
                    Console.WriteLine($"❌ Message processing error: {ex.Message}");
            }
        }

        // Send methods with better error handling...
        public async Task SendSeatSelectionNotification(int flightId, int seatNumber, string status)
        {
            if (!IsConnected) return;

            try
            {
                var notification = new
                {
                    Event = "SeatSelectionUpdate",
                    Data = new
                    {
                        FlightId = flightId,
                        SeatNumber = seatNumber,
                        Status = status,
                        Timestamp = DateTime.UtcNow
                    }
                };

                var json = JsonSerializer.Serialize(notification);
                var messageBytes = Encoding.UTF8.GetBytes(json);

                _clientSocket.Send(messageBytes);
                Console.WriteLine($"📤 Seat selection notification sent: Flight {flightId}, Seat {seatNumber} → {status}");
            }
            catch (Exception ex)
            {
                if (!_disposed)
                    Console.WriteLine($"❌ Seat notification error: {ex.Message}");
            }
        }

        public async Task SendPassengerSelectionNotification(int passengerId, string status)
        {
            if (!IsConnected) return;

            try
            {
                var notification = new
                {
                    Event = "PassengerSelectionUpdate",
                    Data = new
                    {
                        PassengerId = passengerId,
                        Status = status,
                        Timestamp = DateTime.UtcNow
                    }
                };

                var json = JsonSerializer.Serialize(notification);
                var messageBytes = Encoding.UTF8.GetBytes(json);

                _clientSocket.Send(messageBytes);
                Console.WriteLine($"📤 Passenger selection notification sent: Passenger {passengerId} → {status}");
            }
            catch (Exception ex)
            {
                if (!_disposed)
                    Console.WriteLine($"❌ Passenger selection notification error: {ex.Message}");
            }
        }

        public async Task SendPassengerListRefreshNotification(int flightId)
        {
            if (!IsConnected) return;

            try
            {
                var notification = new
                {
                    Event = "PassengerListRefreshUpdate",
                    Data = new
                    {
                        FlightId = flightId,
                        Timestamp = DateTime.UtcNow
                    }
                };

                var json = JsonSerializer.Serialize(notification);
                var messageBytes = Encoding.UTF8.GetBytes(json);

                _clientSocket.Send(messageBytes);
                Console.WriteLine($"📤 Passenger refresh notification sent: Flight {flightId}");
            }
            catch (Exception ex)
            {
                if (!_disposed)
                    Console.WriteLine($"❌ Passenger refresh notification error: {ex.Message}");
            }
        }

        public async Task SendFlightUpdateNotification(int flightId, int status)
        {
            if (!IsConnected) return;

            try
            {
                var notification = new
                {
                    Event = "FlightStatusUpdate",
                    Data = new
                    {
                        FlightId = flightId,
                        Status = status,
                        Timestamp = DateTime.UtcNow
                    }
                };

                var json = JsonSerializer.Serialize(notification);
                var messageBytes = Encoding.UTF8.GetBytes(json);

                _clientSocket.Send(messageBytes);
                Console.WriteLine($"📤 Flight update notification sent: {flightId} → {status}");
            }
            catch (Exception ex)
            {
                if (!_disposed)
                    Console.WriteLine($"❌ Flight update notification error: {ex.Message}");
            }
        }

        public async Task DisconnectAsync()
        {
            if (_disposed) return;

            try
            {
                _isConnected = false;
                _cancellationTokenSource?.Cancel();

                // Wait for receive task to complete with timeout
                if (_receiveTask != null && !_receiveTask.IsCompleted)
                {
                    try
                    {
                        await Task.WhenAny(_receiveTask, Task.Delay(1000));
                    }
                    catch
                    {
                        // Ignore exceptions during shutdown
                    }
                }

                CloseSocketSafely();
                ConnectionStatusChanged?.Invoke("Disconnected");
                Console.WriteLine("✅ WebSocket client disconnected");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Disconnect error: {ex.Message}");
            }
        }

        private void CloseSocketSafely()
        {
            try
            {
                _clientSocket?.Shutdown(SocketShutdown.Both);
            }
            catch { /* Ignore shutdown errors */ }

            try
            {
                _clientSocket?.Close();
            }
            catch { /* Ignore close errors */ }
        }

        public void Dispose()
        {
            if (_disposed) return;
            _disposed = true;

            try
            {
                _isConnected = false;
                _cancellationTokenSource?.Cancel();

                CloseSocketSafely();

                _clientSocket?.Dispose();
                _cancellationTokenSource?.Dispose();

                // Clear event handlers
                FlightStatusChanged = null;
                ConnectionStatusChanged = null;
                PassengerSelectionChanged = null;
                SeatSelectionChanged = null;
                PassengerListRefreshRequested = null;

                Console.WriteLine("✅ WebSocket client disposed");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Dispose error: {ex.Message}");
            }
        }
    }
}