using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ModelAndDto.Models;
using AirplaneFormApplication.CustomControls;
using AirplaneFormApplication.WebSocket;

namespace AirplaneFormApplication.Forms
{
    public partial class SeatSelectionDialog : Form
    {
        public Seat? SelectedSeat { get; private set; }
        private RoundedButton? selectedButton = null;
        private WebSocketClient? webSocketClient;
        private int flightId;
        private Dictionary<int, RoundedButton> seatButtons = new();

        public SeatSelectionDialog(List<Seat> seats, WebSocketClient? webSocketClient = null, int flightId = 0)
        {
            InitializeComponent();
            this.webSocketClient = webSocketClient;
            this.flightId = flightId;

            
            if (webSocketClient?.IsConnected == true)
            {
                webSocketClient.SeatSelectionChanged += OnSeatSelectionChanged;
                webSocketClient.SeatStatesRequested += OnSeatStatesRequested; 
            }

            PopulateSeats(seats);
            ConfirmBtn.Click += ConfirmBtn_Click;

            
            _ = RequestCurrentStatesFromOtherClients();
        }

        
        private async Task RequestCurrentStatesFromOtherClients()
        {
            try
            {
                if (webSocketClient?.IsConnected == true)
                {
                    await webSocketClient.RequestCurrentStates(flightId);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to request current states: {ex.Message}");
            }
        }

        
        private void OnSeatStatesRequested(int requestedFlightId)
        {
            if (requestedFlightId != flightId) return;

            
            if (SelectedSeat != null)
            {
                _ = SendSeatNotificationSafe(SelectedSeat.SeatNumber, "selected");
            }
        }

        private void OnSeatSelectionChanged(int receivedFlightId, int seatNumber, string status)
        {
            if (receivedFlightId != flightId) return;

            
            if (InvokeRequired)
            {
                Invoke(new Action(() => UpdateSeatColor(seatNumber, status)));
            }
            else
            {
                UpdateSeatColor(seatNumber, status);
            }
        }

        private void UpdateSeatColor(int seatNumber, string status)
        {
            if (seatButtons.TryGetValue(seatNumber, out var button))
            {
                switch (status)
                {
                    case "selected":
                        button.BackColor = Color.Yellow;
                        button.BackgroundColor = Color.Yellow;
                        button.Enabled = false;
                        break;
                    case "confirmed":
                        button.BackColor = Color.Red;
                        button.BackgroundColor = Color.Red;
                        button.Enabled = false;
                        break;
                    case "available":
                        
                        var seat = (Seat)button.Tag;
                        if (seat.IsAvailable)
                        {
                            button.BackColor = Color.MediumSlateBlue;
                            button.BackgroundColor = Color.MediumSlateBlue;
                            button.Enabled = true;
                        }
                        break;
                }
            }
        }

        private void PopulateSeats(List<Seat> seats)
        {
            SeatsContainerLayout.Controls.Clear();
            seatButtons.Clear();

            foreach (var seat in seats)
            {
                var seatButton = new RoundedButton
                {
                    Text = seat.SeatNumber.ToString(),
                    Size = new Size(50, 50),
                    Margin = new Padding(3),
                    Tag = seat,
                    BorderRadius = 0,
                    BorderSize = 0,
                    FlatStyle = FlatStyle.Flat,
                    ForeColor = Color.White,
                    TextColor = Color.White,
                    UseVisualStyleBackColor = false
                };

                
                if (seat.IsAvailable)
                {
                    seatButton.BackColor = Color.MediumSlateBlue;
                    seatButton.BackgroundColor = Color.MediumSlateBlue;
                    seatButton.HoverColor = Color.DarkSlateBlue;
                    seatButton.ClickColor = Color.SlateBlue;
                }
                else
                {
                    seatButton.BackColor = Color.Red;
                    seatButton.BackgroundColor = Color.Red;
                    seatButton.HoverColor = Color.DarkRed;
                    seatButton.ClickColor = Color.DarkRed;
                    seatButton.Enabled = false;
                }

                
                if (seat.IsAvailable)
                {
                    seatButton.Click += (s, e) => SelectSeat(seatButton, seat); 
                }

                seatButtons[seat.SeatNumber] = seatButton;
                SeatsContainerLayout.Controls.Add(seatButton);
            }
        }

        private void SelectSeat(RoundedButton button, Seat seat)
        {
            
            if (selectedButton != null)
            {
                selectedButton.BackColor = Color.MediumSlateBlue;
                selectedButton.BackgroundColor = Color.MediumSlateBlue;

                
                var prevSeat = (Seat)selectedButton.Tag;
                _ = SendSeatNotificationSafe(prevSeat.SeatNumber, "available");
            }

            
            button.BackColor = Color.Green;
            button.BackgroundColor = Color.Green;

            selectedButton = button;
            SelectedSeat = seat;

            
            _ = SendSeatNotificationSafe(seat.SeatNumber, "selected");
        }

        
        private async Task SendSeatNotificationSafe(int seatNumber, string status)
        {
            try
            {
                if (webSocketClient?.IsConnected == true)
                {
                    await webSocketClient.SendSeatSelectionNotification(flightId, seatNumber, status);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"WebSocket notification failed: {ex.Message}");
                
            }
        }

        private async void ConfirmBtn_Click(object sender, EventArgs e)
        {
            if (SelectedSeat == null)
            {
                MessageBox.Show("Please select a seat first.", "No Seat Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            
            ConfirmBtn.Enabled = false;

            try
            {
                
                await SendSeatNotificationSafe(SelectedSeat.SeatNumber, "confirmed");

                DialogResult = DialogResult.OK;


                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error confirming seat: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ConfirmBtn.Enabled = true; 
            }
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            try
            {
                
                if (webSocketClient != null)
                {
                    webSocketClient.SeatSelectionChanged -= OnSeatSelectionChanged;
                    webSocketClient.SeatStatesRequested -= OnSeatStatesRequested; 

                    
                    if (SelectedSeat != null && DialogResult != DialogResult.OK)
                    {
                        _ = SendSeatNotificationSafe(SelectedSeat.SeatNumber, "available");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during form cleanup: {ex.Message}");
            }

            base.OnFormClosed(e);
        }
    }
}