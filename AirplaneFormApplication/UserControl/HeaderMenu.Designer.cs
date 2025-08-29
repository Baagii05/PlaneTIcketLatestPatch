namespace AirplaneFormApplication
{
    partial class HeaderMenu
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            panel1 = new Panel();
            BtnFlightMenu = new AirplaneFormApplication.CustomControls.RoundedButton();
            BtnCheckInMenu = new AirplaneFormApplication.CustomControls.RoundedButton();
            BtnTicketMenu = new AirplaneFormApplication.CustomControls.RoundedButton();
            BtnMainMenu = new AirplaneFormApplication.CustomControls.RoundedButton();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.BackColor = SystemColors.MenuHighlight;
            panel1.Controls.Add(BtnFlightMenu);
            panel1.Controls.Add(BtnCheckInMenu);
            panel1.Controls.Add(BtnTicketMenu);
            panel1.Controls.Add(BtnMainMenu);
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(1200, 80);
            panel1.TabIndex = 0;
            // 
            // BtnFlightMenu
            // 
            BtnFlightMenu.BackColor = SystemColors.MenuHighlight;
            BtnFlightMenu.BackgroundColor = SystemColors.MenuHighlight;
            BtnFlightMenu.BorderColor = Color.White;
            BtnFlightMenu.BorderRadius = 10;
            BtnFlightMenu.BorderSize = 0;
            BtnFlightMenu.ClickColor = SystemColors.MenuHighlight;
            BtnFlightMenu.FlatAppearance.BorderSize = 0;
            BtnFlightMenu.FlatStyle = FlatStyle.Flat;
            BtnFlightMenu.Font = new Font("Verdana", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            BtnFlightMenu.ForeColor = Color.White;
            BtnFlightMenu.HoverColor = SystemColors.MenuHighlight;
            BtnFlightMenu.Location = new Point(768, 31);
            BtnFlightMenu.Name = "BtnFlightMenu";
            BtnFlightMenu.Size = new Size(150, 30);
            BtnFlightMenu.TabIndex = 10;
            BtnFlightMenu.Text = "Fligths ✈️";
            BtnFlightMenu.TextColor = Color.White;
            BtnFlightMenu.UseVisualStyleBackColor = false;
            BtnFlightMenu.Click += BtnFlightMenu_Click;
            // 
            // BtnCheckInMenu
            // 
            BtnCheckInMenu.BackColor = SystemColors.MenuHighlight;
            BtnCheckInMenu.BackgroundColor = SystemColors.MenuHighlight;
            BtnCheckInMenu.BorderColor = Color.White;
            BtnCheckInMenu.BorderRadius = 10;
            BtnCheckInMenu.BorderSize = 0;
            BtnCheckInMenu.ClickColor = SystemColors.MenuHighlight;
            BtnCheckInMenu.FlatAppearance.BorderSize = 0;
            BtnCheckInMenu.FlatStyle = FlatStyle.Flat;
            BtnCheckInMenu.Font = new Font("Verdana", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            BtnCheckInMenu.ForeColor = Color.White;
            BtnCheckInMenu.HoverColor = SystemColors.MenuHighlight;
            BtnCheckInMenu.Location = new Point(517, 31);
            BtnCheckInMenu.Name = "BtnCheckInMenu";
            BtnCheckInMenu.Size = new Size(150, 30);
            BtnCheckInMenu.TabIndex = 9;
            BtnCheckInMenu.Text = "Check In";
            BtnCheckInMenu.TextColor = Color.White;
            BtnCheckInMenu.UseVisualStyleBackColor = false;
            BtnCheckInMenu.Click += BtnCheckInMenu_Click;
            // 
            // BtnTicketMenu
            // 
            BtnTicketMenu.BackColor = SystemColors.MenuHighlight;
            BtnTicketMenu.BackgroundColor = SystemColors.MenuHighlight;
            BtnTicketMenu.BorderColor = Color.White;
            BtnTicketMenu.BorderRadius = 10;
            BtnTicketMenu.BorderSize = 0;
            BtnTicketMenu.ClickColor = SystemColors.MenuHighlight;
            BtnTicketMenu.FlatAppearance.BorderSize = 0;
            BtnTicketMenu.FlatStyle = FlatStyle.Flat;
            BtnTicketMenu.Font = new Font("Verdana", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            BtnTicketMenu.ForeColor = Color.White;
            BtnTicketMenu.HoverColor = SystemColors.MenuHighlight;
            BtnTicketMenu.Location = new Point(985, 31);
            BtnTicketMenu.Name = "BtnTicketMenu";
            BtnTicketMenu.Size = new Size(150, 30);
            BtnTicketMenu.TabIndex = 8;
            BtnTicketMenu.Text = "Tickets 🎫";
            BtnTicketMenu.TextColor = Color.White;
            BtnTicketMenu.UseVisualStyleBackColor = false;
            BtnTicketMenu.Click += BtnTicketMenu_Click;
            // 
            // BtnMainMenu
            // 
            BtnMainMenu.BackColor = SystemColors.MenuHighlight;
            BtnMainMenu.BackgroundColor = SystemColors.MenuHighlight;
            BtnMainMenu.BorderColor = SystemColors.MenuHighlight;
            BtnMainMenu.BorderRadius = 0;
            BtnMainMenu.BorderSize = 0;
            BtnMainMenu.ClickColor = SystemColors.MenuHighlight;
            BtnMainMenu.FlatAppearance.BorderSize = 0;
            BtnMainMenu.FlatStyle = FlatStyle.Flat;
            BtnMainMenu.Font = new Font("Verdana", 18F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 0);
            BtnMainMenu.ForeColor = Color.White;
            BtnMainMenu.HoverColor = SystemColors.MenuHighlight;
            BtnMainMenu.Location = new Point(21, 15);
            BtnMainMenu.Name = "BtnMainMenu";
            BtnMainMenu.Size = new Size(412, 50);
            BtnMainMenu.TabIndex = 2;
            BtnMainMenu.Text = "Airplane Ticket System";
            BtnMainMenu.TextColor = Color.White;
            BtnMainMenu.UseVisualStyleBackColor = false;
            BtnMainMenu.Click += BtnMainMenu_Click;
            // 
            // HeaderMenu
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(panel1);
            Name = "HeaderMenu";
            Size = new Size(1200, 80);
            panel1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private CustomControls.RoundedButton BtnMainMenu;
        private CustomControls.RoundedButton BtnFlightMenu;
        private CustomControls.RoundedButton BtnCheckInMenu;
        private CustomControls.RoundedButton BtnTicketMenu;
    }
}
