using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AirplaneFormApplication.WebSocket;
using ModelAndDto.Models;

namespace AirplaneFormApplication
{
    public partial class MainMenu : Form
    {
        // Shared WebSocket client for the entire application
        public static WebSocketClient SharedWebSocketClient { get; private set; }

        // Events that other forms can subscribe to
        public static event Action<int, FlightStatus> GlobalFlightStatusChanged;
        public static event Action<string> GlobalConnectionStatusChanged;

        public MainMenu()
        {
            InitializeComponent();
            InitializeSharedWebSocketClient();
            UpdateConnectionStatusDisplay();
        }

        /// <summary>
        /// Initialize the shared WebSocket client for the entire application
        /// </summary>
        private async void InitializeSharedWebSocketClient()
        {
            try
            {
                if (SharedWebSocketClient == null)
                {
                    SharedWebSocketClient = new WebSocketClient();

                    // Subscribe to WebSocket events and relay them globally
                    SharedWebSocketClient.FlightStatusChanged += OnFlightStatusChanged;
                    SharedWebSocketClient.ConnectionStatusChanged += OnConnectionStatusChanged;

                    // Connect to WebSocket server
                    await SharedWebSocketClient.ConnectAsync();

                    Console.WriteLine("✅ Shared WebSocket client initialized in MainMenu");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to initialize WebSocket connection: {ex.Message}",
                    "Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// Handle flight status changes and broadcast to all forms
        /// </summary>
        private void OnFlightStatusChanged(int flightId, FlightStatus newStatus)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => OnFlightStatusChanged(flightId, newStatus)));
                return;
            }

            // Broadcast to all forms that are listening
            GlobalFlightStatusChanged?.Invoke(flightId, newStatus);

            Console.WriteLine($"🔄 Global flight update broadcast: Flight {flightId} → {newStatus}");
        }

        /// <summary>
        /// Handle connection status changes and broadcast to all forms
        /// </summary>
        private void OnConnectionStatusChanged(string status)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => OnConnectionStatusChanged(status)));
                return;
            }

            // Broadcast to all forms that are listening
            GlobalConnectionStatusChanged?.Invoke(status);

            // Update main menu display
            UpdateConnectionStatusDisplay();

            Console.WriteLine($"🔗 Global connection status: {status}");
        }

        /// <summary>
        /// Update the main menu to show connection status
        /// </summary>
        private void UpdateConnectionStatusDisplay()
        {
            string status = SharedWebSocketClient?.IsConnected == true ? "Connected" : "Disconnected";
            string statusColor = SharedWebSocketClient?.IsConnected == true ? "🟢" : "🔴";

            this.Text = $"Flight Management System - WebSocket {statusColor} {status}";
        }

        /// <summary>
        /// Send flight update notification through shared client
        /// </summary>
        public static async Task SendFlightUpdateNotificationAsync(int flightId, int status)
        {
            if (SharedWebSocketClient?.IsConnected == true)
            {
                await SharedWebSocketClient.SendFlightUpdateNotification(flightId, status);
            }
            else
            {
                Console.WriteLine("⚠️ Cannot send notification - WebSocket not connected");
            }
        }

        /// <summary>
        /// Get the connection status
        /// </summary>
        public static bool IsWebSocketConnected => SharedWebSocketClient?.IsConnected ?? false;

        /// <summary>
        /// Manually reconnect if needed
        /// </summary>
        public static async Task ReconnectWebSocketAsync()
        {
            if (SharedWebSocketClient != null)
            {
                try
                {
                    await SharedWebSocketClient.DisconnectAsync();
                    await SharedWebSocketClient.ConnectAsync();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"❌ Reconnection failed: {ex.Message}");
                }
            }
        }

        private void roundedButton1_Click(object sender, EventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
        }

        /// <summary>
        /// Clean up WebSocket connection when main menu closes
        /// </summary>
        

        /// <summary>
        /// Handle application exit
        /// </summary>
        /// <summary>
        /// Handle form closing - DIFFERENT from other forms since this is the main form
        /// </summary>
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            
            if (e.CloseReason == CloseReason.UserClosing)
            {
                var result = MessageBox.Show(
                    "Are you sure you want to exit the Flight Management System?\n\n" +
                    "This will close all open windows and disconnect from the server.",
                    "Exit Application?",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.No)
                {
                    e.Cancel = true;
                    return;
                }

                
                CloseAllSubForms();
            }

            // Clean up shared WebSocket client
            if (!e.Cancel)
            {
                CleanupSharedWebSocket();
                base.OnFormClosing(e);
            }
        }

        /// <summary>
        /// Close all sub-forms when main menu is closing
        /// </summary>
        private void CloseAllSubForms()
        {
            try
            {
                // Get all open forms except this one
                var formsToClose = Application.OpenForms.Cast<Form>()
                    .Where(f => f != this && f.GetType() != typeof(MainMenu))
                    .ToList();

                // Close all sub-forms programmatically 
                foreach (var form in formsToClose)
                {
                    try
                    {
                        form.Close();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error closing form {form.Name}: {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error closing sub-forms: {ex.Message}");
            }
        }

        /// <summary>
        /// Clean up the shared WebSocket client
        /// </summary>
        private void CleanupSharedWebSocket()
        {
            try
            {
                if (SharedWebSocketClient != null)
                {
                    // Unsubscribe from events
                    SharedWebSocketClient.FlightStatusChanged -= OnFlightStatusChanged;
                    SharedWebSocketClient.ConnectionStatusChanged -= OnConnectionStatusChanged;

                    // Disconnect and dispose
                    SharedWebSocketClient.DisconnectAsync().Wait(2000);
                    SharedWebSocketClient.Dispose();
                    SharedWebSocketClient = null;

                    Console.WriteLine("✅ Shared WebSocket client cleaned up");
                }

                // Clear global event handlers
                GlobalFlightStatusChanged = null;
                GlobalConnectionStatusChanged = null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error during WebSocket cleanup: {ex.Message}");
            }
        }

        /// <summary>
        /// ✅ Final cleanup when form is closed
        /// </summary>
        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            CleanupSharedWebSocket(); 
            base.OnFormClosed(e);
        }
    }
}