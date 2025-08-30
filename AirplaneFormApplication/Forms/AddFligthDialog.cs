using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ModelAndDto.Models;

namespace AirplaneFormApplication.Forms
{
    public partial class AddFligthDialog : Form
    {
        public Flight NewFlight { get; private set; }

        public AddFligthDialog()
        {
            InitializeComponent();
            okButton.Click += okButton_Click;
            cancelButton.Click += cancelButton_Click;
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(flightNumberTextBox.Text) ||
                string.IsNullOrWhiteSpace(departureTextBox.Text) ||
                string.IsNullOrWhiteSpace(arrivalTextBox.Text))
            {
                MessageBox.Show("Please fill all fields.");
                return;
            }

            NewFlight = new Flight
            {
                FlightNumber = flightNumberTextBox.Text.Trim(),
                DepartureLocation = departureTextBox.Text.Trim(),
                ArrivalLocation = arrivalTextBox.Text.Trim(),
                Status = FlightStatus.Registering 
            };
            DialogResult = DialogResult.OK;
            Close();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
