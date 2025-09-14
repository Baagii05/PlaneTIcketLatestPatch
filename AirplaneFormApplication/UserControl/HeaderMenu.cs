using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AirplaneFormApplication
{
    public partial class HeaderMenu : UserControl
    {
        public HeaderMenu()
        {
            InitializeComponent();
        }

        private void OpenOrActivateForm<T>() where T : Form, new()
        {
            var openForm = Application.OpenForms.OfType<T>().FirstOrDefault();
            var currentForm = this.FindForm();

            if (openForm != null)
            {
                if (!openForm.Visible)
                    openForm.Show();
                openForm.WindowState = FormWindowState.Normal;
                openForm.BringToFront();
                openForm.Activate();
            }
            else
            {
                var form = new T();
                form.Show();
            }

            if (currentForm != null && currentForm.GetType() != typeof(T))
            {
                if (currentForm is CheckInMenu checkInForm)
                {
                    checkInForm.ReleaseSelectedPassenger();
                }
                currentForm.Hide();
            }
        }

        private void OpenOrActivateCheckInMenu()
        {
            var openForm = Application.OpenForms.OfType<CheckInMenu>().FirstOrDefault();
            var currentForm = this.FindForm();

            if (openForm != null)
            {
                if (!openForm.Visible)
                    openForm.Show();
                openForm.WindowState = FormWindowState.Normal;
                openForm.BringToFront();
                openForm.Activate();
            }
            else
            {
                var checkInForm = new CheckInMenu(MainMenu.SharedWebSocketClient);
                checkInForm.Show();
            }

            if (currentForm != null && currentForm.GetType() != typeof(CheckInMenu))
            {
                if (currentForm is CheckInMenu checkInForm)
                {
                    checkInForm.ReleaseSelectedPassenger();
                }
                currentForm.Hide();
            }
        }

        private void BtnMainMenu_Click(object sender, EventArgs e)
        {
            OpenOrActivateForm<MainMenu>();
        }

        private void BtnCheckInMenu_Click(object sender, EventArgs e)
        {
            OpenOrActivateCheckInMenu();
        }

        private void BtnFlightMenu_Click(object sender, EventArgs e)
        {
            OpenOrActivateForm<FligthMenu>();
        }

        private void BtnTicketMenu_Click(object sender, EventArgs e)
        {
            OpenOrActivateForm<PassengerMenu>();
        }
    }
}