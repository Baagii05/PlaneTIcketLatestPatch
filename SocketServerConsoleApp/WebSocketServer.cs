using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Net.Http;

namespace SocketServerConsoleApp
{
    /// <summary>
    /// WebSocket Server - WinForms клиентүүдэд зориулсан + SignalR-рүү мэдэгдэл илгээх
    /// </summary>
    public class WebSocketServer
    {
        // Singleton pattern
        private static WebSocketServer _instance;
        private static readonly object _lock = new object();

        public static WebSocketServer Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        if (_instance == null)
                        {
                            _instance = new WebSocketServer(9009);
                        }
                    }
                }
                return _instance;
            }
        }

        private ConcurrentDictionary<int, Socket> _connectedSockets;
        private CancellationTokenSource _cancellationTokenSource;
        private List<Thread> _threads;
        private int _port = 9009;
        private Socket _serverSocket;
        private readonly HttpClient _httpClient;
        public bool HasStarted { get; private set; }
        private int _clientIdCounter = 0;

        // SignalR server URL
        private const string SignalRServerUrl = "http://localhost:8080";

        public WebSocketServer(int port = 9009)
        {
            _port = port;
            _connectedSockets = new ConcurrentDictionary<int, Socket>();
            _cancellationTokenSource = new CancellationTokenSource();
            _threads = new List<Thread>();
            _serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _httpClient = new HttpClient();
            HasStarted = false;
        }

        /// <summary>
        /// Start WebSocket server
        /// </summary>
        public void Start()
        {
            if (HasStarted)
            {
                Console.WriteLine($"WebSocket сервер аль хэдийн {_port} порт дээр ажиллаж байна");
                return;
            }

            try
            {
                _serverSocket.Bind(new IPEndPoint(IPAddress.Any, _port));
                _serverSocket.Listen(100);

                HasStarted = true;
                Console.WriteLine($"🚀 WebSocket сервер {_port} порт дээр амжилттай эхэллээ");

                Thread acceptThread = new Thread(AcceptConnections);
                acceptThread.IsBackground = true;
                acceptThread.Start();
                _threads.Add(acceptThread);
            }
            catch (SocketException ex) when (ex.ErrorCode == 10048)
            {
                Console.WriteLine($"❌ Порт {_port} аль хэдийн ашиглагдаж байна.");
                HasStarted = false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Сервер эхлүүлэхэд алдаа гарлаа: {ex.Message}");
                HasStarted = false;
            }
        }

        private void AcceptConnections()
        {
            try
            {
                while (!_cancellationTokenSource.Token.IsCancellationRequested)
                {
                    Socket clientSocket = _serverSocket.Accept();
                    int clientId = Interlocked.Increment(ref _clientIdCounter);

                    _connectedSockets.TryAdd(clientId, clientSocket);
                    Console.WriteLine($"🔗 Шинэ WinForms клиент холбогдлоо: {clientId} (Нийт: {_connectedSockets.Count})");

                    Thread receiveThread = new Thread(() => ReceiveMessages(clientId, clientSocket));
                    receiveThread.IsBackground = true;
                    receiveThread.Start();
                    _threads.Add(receiveThread);
                }
            }
            catch (Exception ex)
            {
                if (!_cancellationTokenSource.Token.IsCancellationRequested)
                {
                    Console.WriteLine($"❌ Холболт хүлээн авахад алдаа гарлаа: {ex.Message}");
                }
            }
        }

        private async void ReceiveMessages(int clientId, Socket clientSocket)
        {
            byte[] buffer = new byte[4096];

            try
            {
                while (!_cancellationTokenSource.Token.IsCancellationRequested && clientSocket.Connected)
                {
                    int bytesRead = clientSocket.Receive(buffer);

                    if (bytesRead > 0)
                    {
                        string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                        Console.WriteLine($"📨 Мессеж хүлээн авлаа (клиент {clientId}): {message}");

                        // Process the message (e.g., flight status update notification)
                        await ProcessClientMessage(clientId, message);
                    }
                    else
                    {
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Клиент {clientId} холболт салгагдлаа: {ex.Message}");
            }
            finally
            {
                CleanupClient(clientId, clientSocket);
            }
        }

        /// <summary>
        /// Process message from WinForms client
        /// </summary>
        private async Task ProcessClientMessage(int senderId, string message)
        {
            try
            {
                var jsonDoc = JsonDocument.Parse(message);
                var root = jsonDoc.RootElement;

                if (root.TryGetProperty("Event", out var eventElement))
                {
                    string eventName = eventElement.GetString();
                    var dataElement = root.GetProperty("Data");

                    switch (eventName)
                    {
                        case "FlightStatusUpdate":
                            int flightId = dataElement.GetProperty("FlightId").GetInt32();
                            int status = dataElement.GetProperty("Status").GetInt32();
                            await BroadcastToOtherClients(senderId, "FlightStatusChanged", new
                            {
                                FlightId = flightId,
                                Status = status,
                                Timestamp = DateTime.UtcNow
                            });
                            await NotifySignalRServer(flightId, status);
                            break;

                        case "SeatSelectionUpdate":
                            int seatFlightId = dataElement.GetProperty("FlightId").GetInt32();
                            int seatNumber = dataElement.GetProperty("SeatNumber").GetInt32();
                            string seatStatus = dataElement.GetProperty("Status").GetString();
                            
                            Console.WriteLine($"🪑 Seat update from client {senderId}: Flight {seatFlightId}, Seat {seatNumber} → {seatStatus}");
                            
                            await BroadcastToOtherClients(senderId, "SeatSelectionChanged", new
                            {
                                FlightId = seatFlightId,
                                SeatNumber = seatNumber,
                                Status = seatStatus,
                                Timestamp = DateTime.UtcNow
                            });
                            break;

                        case "RequestCurrentStates":
                            int requestFlightId = dataElement.GetProperty("FlightId").GetInt32();
                            
                            Console.WriteLine($"❓ State request from client {senderId}: Flight {requestFlightId}");
                            

                            // Forward the request to all other clients so they can respond with their current states
                            await BroadcastToOtherClients(senderId, "RequestCurrentStates", new
                            {
                                FlightId = requestFlightId,
                                Timestamp = DateTime.UtcNow
                            });
                            break;

                        case "PassengerListRefreshUpdate":
                            int refreshFlightId = dataElement.GetProperty("FlightId").GetInt32();
                            
                            Console.WriteLine($"🔄 Passenger list refresh from client {senderId}: Flight {refreshFlightId}");
                            
                            await BroadcastToOtherClients(senderId, "PassengerListRefresh", new
                            {
                                FlightId = refreshFlightId,
                                Timestamp = DateTime.UtcNow
                            });
                            break;

                        case "PassengerSelectionUpdate":
                            int passengerId = dataElement.GetProperty("PassengerId").GetInt32();
                            string passengerStatus = dataElement.GetProperty("Status").GetString();

                            Console.WriteLine($"👤 Passenger selection update from client {senderId}: Passenger {passengerId} → {passengerStatus}");

                            await BroadcastToOtherClients(senderId, "PassengerSelectionChanged", new
                            {
                                PassengerId = passengerId,
                                Status = passengerStatus,
                                Timestamp = DateTime.UtcNow
                            });
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Message processing error: {ex.Message}");
            }
        }

        /// <summary>
        /// Broadcast to all clients except the sender
        /// </summary>
        public async Task BroadcastToOtherClients(int senderClientId, string eventName, object data)
        {
            byte[] messageBytes = CreateMessageBytes(eventName, data);
            List<int> disconnectedClients = new List<int>();
            int sentCount = 0;

            foreach (var client in _connectedSockets)
            {
                // Skip the sender
                if (client.Key == senderClientId) continue;

                try
                {
                    if (client.Value.Connected)
                    {
                        client.Value.Send(messageBytes);
                        sentCount++;
                    }
                    else
                    {
                        disconnectedClients.Add(client.Key);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"❌ Мессеж илгээхэд алдаа гарлаа (клиент {client.Key}): {ex.Message}");
                    disconnectedClients.Add(client.Key);
                }
            }

            Console.WriteLine($"📡 {sentCount} WinForms клиентэд мэдэгдэл илгээгдлээ (илгээгч: {senderClientId})");

            // Cleanup disconnected clients
            foreach (int clientId in disconnectedClients)
            {
                if (_connectedSockets.TryRemove(clientId, out Socket socket))
                {
                    CloseSocketSafely(socket);
                }
            }
        }

        /// <summary>
        /// Send notification to SignalR server for Blazor clients
        /// </summary>
        private async Task NotifySignalRServer(int flightId, int status)
        {
            try
            {
                var notificationData = new
                {
                    FlightId = flightId,
                    Status = status,
                    Timestamp = DateTime.UtcNow,
                    Source = "WebSocketServer"
                };

                var json = JsonSerializer.Serialize(notificationData);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync($"{SignalRServerUrl}/notify-flight-change", content);

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"✅ SignalR серверт мэдэгдэл илгээгдлээ: Flight {flightId}");
                }
                else
                {
                    Console.WriteLine($"⚠️ SignalR мэдэгдэл илгээх алдаа: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ SignalR мэдэгдэл илгээх алдаа: {ex.Message}");
            }
        }

        /// <summary>
        /// Send message to all connected clients
        /// </summary>
        public void SendMessageToAll(string eventName, object data)
        {
            byte[] messageBytes = CreateMessageBytes(eventName, data);
            List<int> disconnectedClients = new List<int>();

            foreach (var client in _connectedSockets)
            {
                try
                {
                    if (client.Value.Connected)
                    {
                        client.Value.Send(messageBytes);
                    }
                    else
                    {
                        disconnectedClients.Add(client.Key);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"❌ Мессеж илгээхэд алдаа гарлаа (клиент {client.Key}): {ex.Message}");
                    disconnectedClients.Add(client.Key);
                }
            }

            // Cleanup disconnected clients
            foreach (int clientId in disconnectedClients)
            {
                if (_connectedSockets.TryRemove(clientId, out Socket socket))
                {
                    CloseSocketSafely(socket);
                }
            }
        }

        public void Stop()
        {
            if (!HasStarted) return;

            try
            {
                _cancellationTokenSource.Cancel();

                foreach (var client in _connectedSockets)
                {
                    CloseSocketSafely(client.Value);
                }

                _connectedSockets.Clear();
                CloseSocketSafely(_serverSocket);
                _httpClient?.Dispose();

                HasStarted = false;
                Console.WriteLine("🛑 WebSocket сервер зогслоо");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Сервер зогсооход алдаа гарлаа: {ex.Message}");
            }
        }

        #region Helper Methods

        private byte[] CreateMessageBytes(string eventName, object data)
        {
            var messageObj = new { Event = eventName, Data = data, Timestamp = DateTime.UtcNow };
            string jsonMessage = JsonSerializer.Serialize(messageObj);
            return Encoding.UTF8.GetBytes(jsonMessage);
        }

        private void CleanupClient(int clientId, Socket clientSocket)
        {
            _connectedSockets.TryRemove(clientId, out _);
            CloseSocketSafely(clientSocket);
            Console.WriteLine($"🧹 Клиент {clientId} холболт цэвэрлэгдлээ (Нийт: {_connectedSockets.Count})");
        }

        private void CloseSocketSafely(Socket socket)
        {
            try { socket?.Close(); } catch { }
        }

        #endregion
    }
}