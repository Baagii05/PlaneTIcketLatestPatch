using AirplaneFormApplication.apiClient;
using AirplaneFormApplication.Forms;
using AirplaneFormApplication.Services;
using AirplaneFormApplication.WebSocket;
using ModelAndDto.Dtos;
using ModelAndDto.Models;
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
        private WebSocketClient? webSocketClient;
        private Dictionary<int, Panel> passengerPanels = new();
        private Dictionary<int, string> _passengerStates = new();
        private bool _isAssigningPassenger = false;

        public CheckInMenu(WebSocketClient? webSocketClient = null)
        {
            InitializeComponent();
            SearchTxtBox.TextChanged += SearchTxtBox_TextChanged;
            this.Load += CheckInMenu_Load;
            ConfirmBtn.Click += ConfirmBtn_Click;

            this.webSocketClient = webSocketClient;

            InitializeWebSocket();
        }

        private async void InitializeWebSocket()
        {
            if (webSocketClient?.IsConnected == true)
            {
                webSocketClient.PassengerListRefreshRequested += OnPassengerListRefreshRequested;
                webSocketClient.PassengerSelectionChanged += OnPassengerSelectionChanged;
                webSocketClient.SeatStatesRequested += OnStateRequested;
            }
        }

        private void OnStateRequested(int requestedFlightId)
        {
            if (selectedPassenger != null && webSocketClient?.IsConnected == true)
            {
                _ = webSocketClient.SendPassengerSelectionNotification(selectedPassenger.Id, "selected");
            }
        }

        private void OnPassengerSelectionChanged(int passengerId, string status)
        {
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
            _passengerStates[passengerId] = status;

            if (passengerPanels.TryGetValue(passengerId, out var panel))
            {
                ApplyPassengerPanelState(panel, status);
            }
        }

        private void ApplyPassengerPanelState(Panel panel, string status)
        {
            switch (status)
            {
                case "selected":
                    panel.BackColor = Color.Yellow;
                    panel.Enabled = false;
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

        private async void OnPassengerListRefreshRequested(int flightId)
        {
            
            if (_isAssigningPassenger) return;

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

            _ = RequestCurrentPassengerStates();
        }

        private async Task RequestCurrentPassengerStates()
        {
            try
            {
                if (webSocketClient?.IsConnected == true)
                {
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

            
            var currentPassengerIds = passengers.Select(p => p.Id).ToHashSet();
            var panelsToRemove = passengerPanels.Keys.Where(id => !currentPassengerIds.Contains(id)).ToList();
            foreach (var id in panelsToRemove)
            {
                passengerPanels.Remove(id);
                _passengerStates.Remove(id);
            }

            foreach (var passenger in passengers)
            {
                var container = new Panel
                {
                    Size = new Size(427, 53),
                    BackColor = SystemColors.ActiveCaption,
                    Margin = new Padding(3),
                    Tag = passenger
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

                container.Click += async (s, e) => await SelectPassenger(passenger, container);
                nameLabel.Click += async (s, e) => await SelectPassenger(passenger, container);
                passportLabel.Click += async (s, e) => await SelectPassenger(passenger, container);

                passengerPanels[passenger.Id] = container;

                
                if (_passengerStates.TryGetValue(passenger.Id, out var savedState))
                {
                    ApplyPassengerPanelState(container, savedState);
                }
                else if (selectedPassenger != null && selectedPassenger.Id == passenger.Id)
                {
                    
                    ApplyPassengerPanelState(container, "selected");
                    _passengerStates[passenger.Id] = "selected";
                }
                else
                {
                    
                    ApplyPassengerPanelState(container, "available");
                }

                PassengersContainerLayout.Controls.Add(container);
            }
        }

        private async Task SelectPassenger(Passenger passenger, Panel panel)
        {
            if (!panel.Enabled) return;

            if (selectedPassengerPanel != null && selectedPassenger != null)
            {
                selectedPassengerPanel.BackColor = SystemColors.ActiveCaption;
                if (webSocketClient?.IsConnected == true)
                {
                    await webSocketClient.SendPassengerSelectionNotification(selectedPassenger.Id, "available");
                }
            }

            selectedPassenger = passenger;
            selectedPassengerPanel = panel;

            panel.BackColor = Color.LightGreen;

            NameLbl.Text = passenger.Name;
            PasswordNumLbl.Text = passenger.PassportNumber;

            if (webSocketClient?.IsConnected == true)
            {
                await webSocketClient.SendPassengerSelectionNotification(passenger.Id, "selected");
            }
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
                ConfirmBtn.Enabled = false;
                _isAssigningPassenger = true;

                var client = new ApiClient();
                var flightSeats = await client.GetSeatsByFlightIdAsync(selectedPassenger.FlightId);

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
                _isAssigningPassenger = false;
                ConfirmBtn.Enabled = true;
            }
        }

        private async Task AssignSeatToPassenger(Seat selectedSeat)
        {
            var passengerToAssign = selectedPassenger;

            if (passengerToAssign == null)
            {
                MessageBox.Show("No passenger selected for seat assignment.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var client = new ApiClient();
            var assignRequest = new SeatAssignmentRequest
            {
                PassengerId = passengerToAssign.Id,
                FlightId = passengerToAssign.FlightId,
                SeatNumber = selectedSeat.SeatNumber
            };

            var (success, message) = await client.AssignSeatAsync(assignRequest);

            if (success)
            {
                MessageBox.Show($"Seat {selectedSeat.SeatNumber} successfully assigned to {passengerToAssign.Name}!",
                               "Seat Assignment Successful",
                               MessageBoxButtons.OK,
                               MessageBoxIcon.Information);

                var printService = new PassengerTicketPrintService();
                printService.PrintPassengerTicket(passengerToAssign);

                if (webSocketClient?.IsConnected == true)
                {
                    await webSocketClient.SendPassengerListRefreshNotification(passengerToAssign.FlightId);
                }

                await RefreshPassengerList();

                if (webSocketClient?.IsConnected == true)
                {
                    await Task.Delay(200);
                    await RequestCurrentPassengerStates();
                }
            }
            else
            {
                MessageBox.Show($"Failed to assign seat: {message}",
                               "Seat Assignment Failed",
                               MessageBoxButtons.OK,
                               MessageBoxIcon.Error);

                if (webSocketClient?.IsConnected == true)
                {
                    await webSocketClient.SendPassengerSelectionNotification(passengerToAssign.Id, "available");
                }
            }
        }

        private async Task RefreshPassengerList()
        {
            
            var previousSelectedPassenger = selectedPassenger;
            var previousSelectedPanel = selectedPassengerPanel;

            var allPassengersFromApi = await GetPassengersAsync();
            allPassengers = allPassengersFromApi.Where(p => p.SeatId == null && p.SeatNumber == null).ToList();
            filteredPassengers = allPassengers;

            
            PopulatePassengers(filteredPassengers);

            
            if (previousSelectedPassenger != null &&
                allPassengers.Any(p => p.Id == previousSelectedPassenger.Id))
            {
                selectedPassenger = previousSelectedPassenger;

                
                if (passengerPanels.TryGetValue(previousSelectedPassenger.Id, out var newPanel))
                {
                    selectedPassengerPanel = newPanel;
                    newPanel.BackColor = Color.LightGreen;

                    
                    NameLbl.Text = previousSelectedPassenger.Name;
                    PasswordNumLbl.Text = previousSelectedPassenger.PassportNumber;

                    
                    _passengerStates[previousSelectedPassenger.Id] = "selected";
                }
            }
            else
            {
                
                selectedPassenger = null;
                selectedPassengerPanel = null;
                NameLbl.Text = "";
                PasswordNumLbl.Text = "";
            }

            
            if (webSocketClient?.IsConnected == true)
            {
                await Task.Delay(100); 
                await RequestCurrentPassengerStates();
            }
        }

        public void ReleaseSelectedPassenger()
        {
            try
            {
                if (webSocketClient?.IsConnected == true && selectedPassenger != null)
                {
                    webSocketClient.SendPassengerSelectionNotification(selectedPassenger.Id, "available").Wait(1000);

                    _passengerStates.Remove(selectedPassenger.Id);

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
                        Application.Exit();
                        break;
                    case DialogResult.No:
                        e.Cancel = true;
                        var mainMenu = Application.OpenForms.OfType<MainMenu>().FirstOrDefault();
                        if (mainMenu != null)
                        {
                            mainMenu.Show();
                            mainMenu.BringToFront();
                        }
                        this.Hide();
                        break;
                    case DialogResult.Cancel:
                        e.Cancel = true;
                        break;
                }
            }

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
                if (webSocketClient?.IsConnected == true && selectedPassenger != null)
                {
                    webSocketClient.SendPassengerSelectionNotification(selectedPassenger.Id, "available").Wait(1000);
                }

                if (webSocketClient != null)
                {
                    webSocketClient.PassengerListRefreshRequested -= OnPassengerListRefreshRequested;
                    webSocketClient.PassengerSelectionChanged -= OnPassengerSelectionChanged;
                    webSocketClient.SeatStatesRequested -= OnStateRequested;
                }
            }
            catch
            {

            }
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            CleanupWebSocket();
            base.OnFormClosed(e);
        }
    }
}