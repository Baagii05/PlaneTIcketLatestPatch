using AirplaneFormApplication.apiClient;
using AirplaneFormApplication.Forms;
using AirplaneFormApplication.WebSocket;
using ModelAndDto.Models;
using ModelAndDto.Dtos;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AirplaneFormApplication
{
    public partial class CheckInMenu : Form
    {
        private List<Passenger> allPassengers = new();
        private List<Passenger> filteredPassengers = new();
        private Passenger? selectedPassenger = null;
        private Panel? selectedPassengerPanel = null;
        private WebSocketClient webSocketClient;
        private Dictionary<int, Panel> passengerPanels = new();

        public CheckInMenu()
        {
            InitializeComponent();
            SearchTxtBox.TextChanged += SearchTxtBox_TextChanged;
            this.Load += CheckInMenu_Load;
            ConfirmBtn.Click += ConfirmBtn_Click;
            
            // Initialize WebSocket
            InitializeWebSocket();
        }

        private async void InitializeWebSocket()
        {
            webSocketClient = new WebSocketClient();
            webSocketClient.PassengerListRefreshRequested += OnPassengerListRefreshRequested;
            
            // Subscribe to passenger selection events from other clients
            webSocketClient.PassengerSelectionChanged += OnPassengerSelectionChanged;
            
            // ✅ Subscribe to state requests
            webSocketClient.SeatStatesRequested += OnStateRequested;
            
            await webSocketClient.ConnectAsync();
        }

        // ✅ Handle when someone requests current states - send our passenger selection if we have one
        private void OnStateRequested(int requestedFlightId)
        {
            // If we have a selected passenger, re-broadcast it
            if (selectedPassenger != null)
            {
                _ = webSocketClient.SendPassengerSelectionNotification(selectedPassenger.Id, "selected");
            }
        }

        private void OnPassengerSelectionChanged(int passengerId, string status)
        {
            // Update UI on main thread
            if (InvokeRequired)
            {
                Invoke(new Action(() => UpdatePassengerPanelStatus(passengerId, status)));
            }
            else
            {
                UpdatePassengerPanelStatus(passengerId, status);
            }
        }

        private void UpdatePassengerPanelStatus(int passengerId, string status)
        {
            if (passengerPanels.TryGetValue(passengerId, out var panel))
            {
                switch (status)
                {
                    case "selected":
                        panel.BackColor = Color.Yellow;
                        panel.Enabled = false; // Disable for other users
                        break;
                    case "available":
                        panel.BackColor = SystemColors.ActiveCaption;
                        panel.Enabled = true;
                        break;
                    case "processing":
                        panel.BackColor = Color.Orange;
                        panel.Enabled = false;
                        break;
                }
            }
        }

        private async void OnPassengerListRefreshRequested(int flightId)
        {
            // Refresh passenger list on UI thread
            if (InvokeRequired)
            {
                Invoke(new Action(async () => await RefreshPassengerList()));
            }
            else
            {
                await RefreshPassengerList();
            }
        }

        private async void CheckInMenu_Load(object sender, EventArgs e)
        {
            var allPassengersFromApi = await GetPassengersAsync();
            allPassengers = allPassengersFromApi.Where(p => p.SeatId == null && p.SeatNumber == null).ToList();
            filteredPassengers = allPassengers;
            PopulatePassengers(filteredPassengers);

            // ✅ Request current passenger states when form loads
            _ = RequestCurrentPassengerStates();
        }

        // ✅ Add method to request current passenger states (simple version)
        private async Task RequestCurrentPassengerStates()
        {
            try
            {
                if (webSocketClient?.IsConnected == true)
                {
                    // Simple request - use dummy flight ID, let all clients respond
                    await webSocketClient.RequestCurrentStates(0);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to request current passenger states: {ex.Message}");
            }
        }

        private async Task<List<Passenger>> GetPassengersAsync()
        {
            var client = new ApiClient();
            var passengers = await client.GetAllPassengersAsync();
            return passengers ?? new List<Passenger>();
        }

        private void PopulatePassengers(List<Passenger> passengers)
        {
            PassengersContainerLayout.Controls.Clear();
            passengerPanels.Clear();
            
            foreach (var passenger in passengers)
            {
                var container = new Panel
                {
                    Size = new Size(427, 53),
                    BackColor = SystemColors.ActiveCaption,
                    Margin = new Padding(3),
                    Tag = passenger // Store passenger reference
                };

                var nameLabel = new Label
                {
                    Text = passenger.Name,
                    Location = new Point(16, 16),
                    AutoSize = true
                };

                var passportLabel = new Label
                {
                    Text = passenger.PassportNumber,
                    Location = new Point(200, 16),
                    AutoSize = true
                };

                container.Controls.Add(nameLabel);
                container.Controls.Add(passportLabel);

                // Click event for selecting passenger
                container.Click += async (s, e) => await SelectPassenger(passenger, container);
                nameLabel.Click += async (s, e) => await SelectPassenger(passenger, container);
                passportLabel.Click += async (s, e) => await SelectPassenger(passenger, container);

                // Store panel reference for later updates
                passengerPanels[passenger.Id] = container;
                
                PassengersContainerLayout.Controls.Add(container);
            }
        }

        private async Task SelectPassenger(Passenger passenger, Panel panel)
        {
            // Don't allow selection if panel is disabled (selected by another user)
            if (!panel.Enabled) return;

            // Reset previous selection
            if (selectedPassengerPanel != null && selectedPassenger != null)
            {
                selectedPassengerPanel.BackColor = SystemColors.ActiveCaption;
                // Notify other clients that previous passenger is available
                await webSocketClient.SendPassengerSelectionNotification(selectedPassenger.Id, "available");
            }

            // Set new selection
            selectedPassenger = passenger;
            selectedPassengerPanel = panel;
            
            // Highlight selected panel locally
            panel.BackColor = Color.LightGreen;
            
            // Show passenger info
            NameLbl.Text = passenger.Name;
            PasswordNumLbl.Text = passenger.PassportNumber;

            // Notify other clients about selection (they'll see it as yellow and disabled)
            await webSocketClient.SendPassengerSelectionNotification(passenger.Id, "selected");
        }

        private void ShowPassengerInfo(Passenger passenger)
        {
            selectedPassenger = passenger; 
            NameLbl.Text = passenger.Name;
            PasswordNumLbl.Text = passenger.PassportNumber;
        }

        private void SearchTxtBox_TextChanged(object sender, EventArgs e)
        {
            var search = SearchTxtBox.Text.ToLower();
            filteredPassengers = allPassengers
                .Where(p => (p.Name?.ToLower().Contains(search) ?? false) ||
                            (p.PassportNumber?.ToLower().Contains(search) ?? false))
                .ToList();
            PopulatePassengers(filteredPassengers);
        }

        private async void ConfirmBtn_Click(object sender, EventArgs e)
        {
            if (selectedPassenger == null)
            {
                MessageBox.Show("Please select a passenger first.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                // Disable the button to prevent double-clicks
                ConfirmBtn.Enabled = false;

                var client = new ApiClient();
                var flightSeats = await client.GetSeatsByFlightIdAsync(selectedPassenger.FlightId);

                // ✅ Pass WebSocket client safely, even if it's null
                var seatDialog = new SeatSelectionDialog(flightSeats, webSocketClient, selectedPassenger.FlightId);

                if (seatDialog.ShowDialog() == DialogResult.OK && seatDialog.SelectedSeat != null)
                {
                    await AssignSeatToPassenger(seatDialog.SelectedSeat);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error opening seat selection: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                ConfirmBtn.Enabled = true; 
            }
        }

        private async Task AssignSeatToPassenger(Seat selectedSeat)
        {
            var client = new ApiClient();
            var assignRequest = new SeatAssignmentRequest 
            {
                PassengerId = selectedPassenger.Id,
                FlightId = selectedPassenger.FlightId,
                SeatNumber = selectedSeat.SeatNumber
            };

            var (success, message) = await client.AssignSeatAsync(assignRequest);

            if (success)
            {
                MessageBox.Show($"Seat {selectedSeat.SeatNumber} successfully assigned to {selectedPassenger.Name}!", 
                               "Seat Assignment Successful", 
                               MessageBoxButtons.OK, 
                               MessageBoxIcon.Information);
                
                // Notify other clients about passenger list refresh
                await webSocketClient.SendPassengerListRefreshNotification(selectedPassenger.FlightId);
                
                await RefreshPassengerList();
            }
            else
            {
                MessageBox.Show($"Failed to assign seat: {message}", 
                               "Seat Assignment Failed", 
                               MessageBoxButtons.OK, 
                               MessageBoxIcon.Error);
                
                // Make passenger available again on failure
                await webSocketClient.SendPassengerSelectionNotification(selectedPassenger.Id, "available");
            }
        }

        private async Task RefreshPassengerList()
        {
            var allPassengersFromApi = await GetPassengersAsync();
            allPassengers = allPassengersFromApi.Where(p => p.SeatId == null && p.SeatNumber == null).ToList();
            filteredPassengers = allPassengers;
            PopulatePassengers(filteredPassengers);
            
            // Clear selection
            selectedPassenger = null;
            selectedPassengerPanel = null;
            NameLbl.Text = "";
            PasswordNumLbl.Text = "";
        }

        // ✅ Make this method public so HeaderMenu can call it
        public void ReleaseSelectedPassenger()
        {
            try
            {
                if (webSocketClient?.IsConnected == true && selectedPassenger != null)
                {
                    webSocketClient.SendPassengerSelectionNotification(selectedPassenger.Id, "available").Wait(1000);

                    // Clear local selection
                    if (selectedPassengerPanel != null)
                    {
                        selectedPassengerPanel.BackColor = SystemColors.ActiveCaption;
                    }
                    selectedPassenger = null;
                    selectedPassengerPanel = null;
                    NameLbl.Text = "";
                    PasswordNumLbl.Text = "";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error releasing selected passenger: {ex.Message}");
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            // Only show dialog if user is closing manually (not programmatically)
            if (e.CloseReason == CloseReason.UserClosing)
            {
                var result = MessageBox.Show(
                    "Do you want to close the entire application?\n\n" +
                    "Yes = Close all forms and exit\n" +
                    "No = Return to Main Menu\n" +
                    "Cancel = Stay in Check-In Menu",
                    "Close Application?",
                    MessageBoxButtons.YesNoCancel,
                    MessageBoxIcon.Question);

                switch (result)
                {
                    case DialogResult.Yes:
                        // Close entire application
                        Application.Exit();
                        break;
                    case DialogResult.No:
                        // Return to main menu
                        e.Cancel = true; // Cancel the close
                        var mainMenu = Application.OpenForms.OfType<MainMenu>().FirstOrDefault();
                        if (mainMenu != null)
                        {
                            mainMenu.Show();
                            mainMenu.BringToFront();
                        }
                        this.Hide(); // Hide current form instead of closing
                        break;
                    case DialogResult.Cancel:
                        // Stay in current form
                        e.Cancel = true;
                        break;
                }
            }

            // ✅ If form is actually closing, clean up properly
            if (!e.Cancel)
            {
                CleanupWebSocket();
                base.OnFormClosing(e);
            }
        }

        private void CleanupWebSocket()
        {
            try
            {
                // Release any selected passenger when form closes
                if (webSocketClient?.IsConnected == true && selectedPassenger != null)
                {
                    webSocketClient.SendPassengerSelectionNotification(selectedPassenger.Id, "available").Wait(1000);
                }

                // ✅ Explicitly unsubscribe from events
                if (webSocketClient != null)
                {
                    webSocketClient.PassengerListRefreshRequested -= OnPassengerListRefreshRequested;
                    webSocketClient.PassengerSelectionChanged -= OnPassengerSelectionChanged;
                    webSocketClient.SeatStatesRequested -= OnStateRequested; // ✅ Unsubscribe

                    webSocketClient.DisconnectAsync().Wait(1000);
                    webSocketClient.Dispose();
                }
            }
            catch
            {
                // Ignore errors during cleanup
            }
        }

        // Keep the existing OnFormClosed for safety
        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            CleanupWebSocket(); // Double cleanup safety
            base.OnFormClosed(e);
        }
    }
}