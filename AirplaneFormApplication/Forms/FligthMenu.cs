using AirplaneFormApplication.apiClient;
using AirplaneFormApplication.Forms;
using AirplaneFormApplication.StateMachine;
using ModelAndDto.Models;

namespace AirplaneFormApplication
{
    public partial class FligthMenu : Form
    {
        private readonly ApiClient _api = new ApiClient();
        private Flight _selectedFlight;

        public FligthMenu()
        {
            InitializeComponent();
            AddFlightBtn.Click += AddFlightBtn_Click;
            UpdateBtn.Click += UpdateBtn_Click;

            
            MainMenu.GlobalFlightStatusChanged += OnGlobalFlightStatusChanged;
            MainMenu.GlobalConnectionStatusChanged += OnGlobalConnectionStatusChanged;

            LoadAllFlights();
            UpdateConnectionStatusDisplay();
        }

        /// <summary>
        /// Handle global flight status changes from shared WebSocket
        /// </summary>
        private void OnGlobalFlightStatusChanged(int flightId, FlightStatus newStatus)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => OnGlobalFlightStatusChanged(flightId, newStatus)));
                return;
            }

            
            LoadAllFlights();

           
            MessageBox.Show($"🔄 Real-time update: Flight {flightId} status changed to {newStatus}!",
                "Real-time Update", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// Handle global connection status changes
        /// </summary>
        private void OnGlobalConnectionStatusChanged(string status)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => OnGlobalConnectionStatusChanged(status)));
                return;
            }

            UpdateConnectionStatusDisplay();
        }

        /// <summary>
        /// Update form title with connection status
        /// </summary>
        private void UpdateConnectionStatusDisplay()
        {
            string status = MainMenu.IsWebSocketConnected ? "Connected" : "Disconnected";
            string statusIcon = MainMenu.IsWebSocketConnected ? "🟢" : "🔴";
            this.Text = $"Flight Menu - WebSocket {statusIcon} {status}";
        }

        private async void LoadAllFlights()
        {
            var flights = await _api.GetAllFlightsAsync();
            DrawFlights(flights);
        }

        private void DrawFlights(List<Flight> flights)
        {
            FlightsContainerLayout.Controls.Clear();

            foreach (var flight in flights)
            {
                var panel = new Panel
                {
                    Width = 723,
                    Height = 47,
                    BackColor = SystemColors.ButtonHighlight,
                    Margin = new Padding(5),
                    Tag = flight
                };

                var flightNumberLbl = new Label
                {
                    Text = flight.FlightNumber,
                    Location = new Point(22, 13),
                    AutoSize = true
                };

                var statusLbl = new Label
                {
                    Text = flight.Status.ToString(),
                    Location = new Point(186, 13),
                    AutoSize = true
                };

                var departureLbl = new Label
                {
                    Text = flight.DepartureLocation,
                    Location = new Point(314, 13),
                    AutoSize = true
                };

                var arrivalLbl = new Label
                {
                    Text = flight.ArrivalLocation,
                    Location = new Point(494, 13),
                    AutoSize = true
                };

                var deleteBtn = new CustomControls.RoundedButton
                {
                    Text = "🗑️",
                    ForeColor = Color.Red,
                    Location = new Point(603, 7),
                    Size = new Size(39, 32),
                    TabIndex = 5
                };

                deleteBtn.Click += async (s, e) =>
                {
                    await _api.DeleteFlightAsync(flight.FlightId);
                    LoadAllFlights();
                };

                panel.Click += (s, e) =>
                {
                    var selectedFlight = (Flight)panel.Tag;
                    ShowFlightForUpdate(selectedFlight);
                };

                panel.Controls.Add(flightNumberLbl);
                panel.Controls.Add(statusLbl);
                panel.Controls.Add(departureLbl);
                panel.Controls.Add(arrivalLbl);
                panel.Controls.Add(deleteBtn);

                FlightsContainerLayout.Controls.Add(panel);
            }
        }

        private void ShowFlightForUpdate(Flight flight)
        {
            _selectedFlight = flight;
            FlightNumberPanel.Text = flight.FlightNumber;


            var validStatuses = FlightStatusStateMachine.GetTransitions(flight.Status)
                .Prepend(flight.Status)
                .Distinct()
                .ToArray();

            FligthStatusCmbBx.DataSource = validStatuses;
            FligthStatusCmbBx.SelectedItem = flight.Status;
        }

        /// <summary>
        /// Updated UpdateBtn_Click to use shared WebSocket connection
        /// </summary>
        private async void UpdateBtn_Click(object sender, EventArgs e)
        {
            if (_selectedFlight == null)
            {
                MessageBox.Show("Select a flight to update.");
                return;
            }

            var newStatus = (FlightStatus)FligthStatusCmbBx.SelectedItem;
            var currentStatus = _selectedFlight.Status;

            if (!FlightStatusStateMachine.CanTransition(currentStatus, newStatus))
            {
                MessageBox.Show($"Cannot change status from {currentStatus} to {newStatus}.");
                return;
            }

            try
            {
                
                _selectedFlight.Status = newStatus;
                await _api.UpdateFlightAsync(_selectedFlight);

                
                await MainMenu.SendFlightUpdateNotificationAsync(_selectedFlight.FlightId, (int)newStatus);

                
                LoadAllFlights();

                MessageBox.Show($"✅ Flight {_selectedFlight.FlightNumber} updated to {newStatus}!",
                    "Update Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ Error updating flight: {ex.Message}",
                    "Update Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void AddFlightBtn_Click(object sender, EventArgs e)
        {
            using var dialog = new AddFligthDialog();
            if (dialog.ShowDialog() == DialogResult.OK && dialog.NewFlight != null)
            {
                await _api.AddFlightAsync(dialog.NewFlight);
                LoadAllFlights();
            }
        }

        /// <summary>
        /// Unsubscribe from global events when form closes
        /// </summary>

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                var result = MessageBox.Show(
                    "Do you want to close the entire application?\n\n" +
                    "Yes = Close all forms and exit\n" +
                    "No = Return to Main Menu\n" +
                    "Cancel = Stay in Flight Menu",
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

                MainMenu.GlobalFlightStatusChanged -= OnGlobalFlightStatusChanged;
                MainMenu.GlobalConnectionStatusChanged -= OnGlobalConnectionStatusChanged;
                base.OnFormClosing(e);
            }
        }
    }
}