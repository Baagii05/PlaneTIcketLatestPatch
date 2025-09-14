using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using ModelAndDto.Models;
using ModelAndDto.Dtos;
using AirplaneFormApplication.apiClient;

namespace AirplaneFormApplication
{
    public partial class PassengerMenu : Form
    {
        private readonly ApiClient _api = new ApiClient();

        public PassengerMenu()
        {
            InitializeComponent();
            AddBtn.Click += AddBtn_Click;
            LoadAllPassengers();
        }

        private async void LoadAllPassengers()
        {
            var passengers = await _api.GetAllPassengersAsync();
            DrawPassengers(passengers);
        }

        private void DrawPassengers(List<Passenger> passengers)
        {
            PassengersContainerLayout.Controls.Clear();

            foreach (var passenger in passengers)
            {
                var panel = new Panel
                {
                    Width = 592,
                    Height = 69,
                    BackColor = SystemColors.ActiveCaption,
                    Margin = new Padding(5),
                    Tag = passenger
                };

                var nameLabel = new Label
                {
                    Text = passenger.Name,
                    Location = new Point(44, 25),
                    AutoSize = true
                };

                var passportLabel = new Label
                {
                    Text = passenger.PassportNumber,
                    Location = new Point(235, 26),
                    AutoSize = true
                };

                var deleteButton = new CustomControls.RoundedButton
                {
                    Text = "🗑️",
                    BackgroundColor = Color.Transparent,
                    ForeColor = Color.Red,
                    Location = new Point(533, 16),
                    Size = new Size(56, 50),
                    TabIndex = 0
                };

                deleteButton.Click += async (s, e) =>
                {
                    var selected = (Passenger)panel.Tag;
                    await DeletePassengerAsync(selected.Id);
                };

                panel.Controls.Add(nameLabel);
                panel.Controls.Add(passportLabel);
                panel.Controls.Add(deleteButton);

                PassengersContainerLayout.Controls.Add(panel);
            }
        }

        private async void AddBtn_Click(object sender, EventArgs e)
        {
            string name = NameTxtBox.Text.Trim();
            string passport = PasswordTxtBox.Text.Trim();
            string flightIdText = FlightIdTxtBox.Text.Trim();

            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(passport) || string.IsNullOrWhiteSpace(flightIdText))
            {
                MessageBox.Show("Please fill all fields.");
                return;
            }

            if (!int.TryParse(flightIdText, out int flightId))
            {
                MessageBox.Show("Flight Id must be a number.");
                return;
            }

            var request = new RegisterPassengerRequest
            {
                Name = name,
                PassportNumber = passport,
                FlightId = flightId
            };

            try
            {
                await _api.AddPassengerAsync(request);
                LoadAllPassengers();
                NameTxtBox.Text = "";
                PasswordTxtBox.Text = "";
                FlightIdTxtBox.Text = "";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding passenger: {ex.Message}");
            }
        }

        private async Task DeletePassengerAsync(int id)
        {
            var confirm = MessageBox.Show("Delete this passenger?", "Confirm", MessageBoxButtons.YesNo);
            if (confirm == DialogResult.Yes)
            {
                try
                {
                    await _api.DeletePassengerAsync(id);
                    LoadAllPassengers();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error deleting passenger: {ex.Message}");
                }
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
                    "Cancel = Stay in Passenger Menu",
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
                base.OnFormClosing(e);
            }
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            
            base.OnFormClosed(e);
        }
    }
}
