namespace AirplaneFormApplication
{
    partial class FligthMenu
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            headerMenu1 = new HeaderMenu();
            FlightsContainerPanel = new Panel();
            label4 = new Label();
            label3 = new Label();
            label2 = new Label();
            FligthNumberLabel = new Label();
            FlightsContainerLayout = new FlowLayoutPanel();
            FlightContainerPanel = new Panel();
            DeleteBtn = new AirplaneFormApplication.CustomControls.RoundedButton();
            ArrivalLbl = new Label();
            DepartureLbl = new Label();
            StatusLbl = new Label();
            FligthNumberLbl = new Label();
            FlightUpdatePnl = new Panel();
            AddFlightBtn = new AirplaneFormApplication.CustomControls.RoundedButton();
            label1 = new Label();
            label5 = new Label();
            label6 = new Label();
            label7 = new Label();
            FlightNumberPanel = new Label();
            FligthStatusCmbBx = new ComboBox();
            UpdateBtn = new AirplaneFormApplication.CustomControls.RoundedButton();
            FlightsContainerPanel.SuspendLayout();
            FlightsContainerLayout.SuspendLayout();
            FlightContainerPanel.SuspendLayout();
            FlightUpdatePnl.SuspendLayout();
            SuspendLayout();
            // 
            // headerMenu1
            // 
            headerMenu1.Location = new Point(0, 0);
            headerMenu1.Name = "headerMenu1";
            headerMenu1.Size = new Size(1500, 81);
            headerMenu1.TabIndex = 1;
            // 
            // FlightsContainerPanel
            // 
            FlightsContainerPanel.BackColor = SystemColors.ButtonHighlight;
            FlightsContainerPanel.Controls.Add(label4);
            FlightsContainerPanel.Controls.Add(label3);
            FlightsContainerPanel.Controls.Add(label2);
            FlightsContainerPanel.Controls.Add(FligthNumberLabel);
            FlightsContainerPanel.Controls.Add(FlightsContainerLayout);
            FlightsContainerPanel.Location = new Point(0, 163);
            FlightsContainerPanel.Name = "FlightsContainerPanel";
            FlightsContainerPanel.Size = new Size(793, 396);
            FlightsContainerPanel.TabIndex = 2;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(509, 22);
            label4.Name = "label4";
            label4.Size = new Size(110, 20);
            label4.TabIndex = 4;
            label4.Text = "Arrival location";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(329, 22);
            label3.Name = "label3";
            label3.Size = new Size(134, 20);
            label3.TabIndex = 3;
            label3.Text = "Departure location";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(201, 22);
            label2.Name = "label2";
            label2.Size = new Size(49, 20);
            label2.TabIndex = 2;
            label2.Text = "Status";
            // 
            // FligthNumberLabel
            // 
            FligthNumberLabel.AutoSize = true;
            FligthNumberLabel.Location = new Point(37, 22);
            FligthNumberLabel.Name = "FligthNumberLabel";
            FligthNumberLabel.Size = new Size(101, 20);
            FligthNumberLabel.TabIndex = 1;
            FligthNumberLabel.Text = "Flight number";
            // 
            // FlightsContainerLayout
            // 
            FlightsContainerLayout.AutoScroll = true;
            FlightsContainerLayout.Controls.Add(FlightContainerPanel);
            FlightsContainerLayout.Location = new Point(12, 67);
            FlightsContainerLayout.Name = "FlightsContainerLayout";
            FlightsContainerLayout.Size = new Size(765, 329);
            FlightsContainerLayout.TabIndex = 0;
            // 
            // FlightContainerPanel
            // 
            FlightContainerPanel.Controls.Add(DeleteBtn);
            FlightContainerPanel.Controls.Add(ArrivalLbl);
            FlightContainerPanel.Controls.Add(DepartureLbl);
            FlightContainerPanel.Controls.Add(StatusLbl);
            FlightContainerPanel.Controls.Add(FligthNumberLbl);
            FlightContainerPanel.Location = new Point(3, 3);
            FlightContainerPanel.Name = "FlightContainerPanel";
            FlightContainerPanel.Size = new Size(723, 47);
            FlightContainerPanel.TabIndex = 0;
            // 
            // DeleteBtn
            // 
            DeleteBtn.BackColor = Color.Transparent;
            DeleteBtn.BackgroundColor = Color.Transparent;
            DeleteBtn.BorderColor = Color.Transparent;
            DeleteBtn.BorderRadius = 0;
            DeleteBtn.BorderSize = 0;
            DeleteBtn.ClickColor = Color.Transparent;
            DeleteBtn.FlatAppearance.BorderSize = 0;
            DeleteBtn.FlatStyle = FlatStyle.Flat;
            DeleteBtn.ForeColor = Color.Red;
            DeleteBtn.HoverColor = Color.Transparent;
            DeleteBtn.Location = new Point(603, 7);
            DeleteBtn.Name = "DeleteBtn";
            DeleteBtn.Size = new Size(39, 32);
            DeleteBtn.TabIndex = 5;
            DeleteBtn.Text = "🗑️";
            DeleteBtn.TextColor = Color.Red;
            DeleteBtn.UseVisualStyleBackColor = false;
            // 
            // ArrivalLbl
            // 
            ArrivalLbl.AutoSize = true;
            ArrivalLbl.Location = new Point(494, 13);
            ArrivalLbl.Name = "ArrivalLbl";
            ArrivalLbl.Size = new Size(50, 20);
            ArrivalLbl.TabIndex = 3;
            ArrivalLbl.Text = "arrival";
            // 
            // DepartureLbl
            // 
            DepartureLbl.AutoSize = true;
            DepartureLbl.Location = new Point(314, 13);
            DepartureLbl.Name = "DepartureLbl";
            DepartureLbl.Size = new Size(74, 20);
            DepartureLbl.TabIndex = 2;
            DepartureLbl.Text = "departure";
            // 
            // StatusLbl
            // 
            StatusLbl.AutoSize = true;
            StatusLbl.Location = new Point(186, 13);
            StatusLbl.Name = "StatusLbl";
            StatusLbl.Size = new Size(49, 20);
            StatusLbl.TabIndex = 1;
            StatusLbl.Text = "Status";
            // 
            // FligthNumberLbl
            // 
            FligthNumberLbl.AutoSize = true;
            FligthNumberLbl.Location = new Point(22, 13);
            FligthNumberLbl.Name = "FligthNumberLbl";
            FligthNumberLbl.Size = new Size(100, 20);
            FligthNumberLbl.TabIndex = 0;
            FligthNumberLbl.Text = "FligthNumber";
            // 
            // FlightUpdatePnl
            // 
            FlightUpdatePnl.BackColor = SystemColors.ButtonHighlight;
            FlightUpdatePnl.Controls.Add(UpdateBtn);
            FlightUpdatePnl.Controls.Add(FligthStatusCmbBx);
            FlightUpdatePnl.Controls.Add(FlightNumberPanel);
            FlightUpdatePnl.Controls.Add(label7);
            FlightUpdatePnl.Controls.Add(label6);
            FlightUpdatePnl.Controls.Add(label5);
            FlightUpdatePnl.Controls.Add(label1);
            FlightUpdatePnl.Location = new Point(842, 78);
            FlightUpdatePnl.Name = "FlightUpdatePnl";
            FlightUpdatePnl.Size = new Size(341, 481);
            FlightUpdatePnl.TabIndex = 3;
            // 
            // AddFlightBtn
            // 
            AddFlightBtn.BackColor = SystemColors.MenuHighlight;
            AddFlightBtn.BackgroundColor = SystemColors.MenuHighlight;
            AddFlightBtn.BorderColor = SystemColors.MenuHighlight;
            AddFlightBtn.BorderRadius = 10;
            AddFlightBtn.BorderSize = 1;
            AddFlightBtn.ClickColor = SystemColors.MenuHighlight;
            AddFlightBtn.FlatAppearance.BorderSize = 0;
            AddFlightBtn.FlatStyle = FlatStyle.Flat;
            AddFlightBtn.ForeColor = Color.White;
            AddFlightBtn.HoverColor = SystemColors.MenuHighlight;
            AddFlightBtn.Location = new Point(24, 96);
            AddFlightBtn.Name = "AddFlightBtn";
            AddFlightBtn.Size = new Size(188, 50);
            AddFlightBtn.TabIndex = 4;
            AddFlightBtn.Text = "Add Flight";
            AddFlightBtn.TextColor = Color.White;
            AddFlightBtn.UseVisualStyleBackColor = false;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(78, 84);
            label1.Name = "label1";
            label1.Size = new Size(101, 20);
            label1.TabIndex = 0;
            label1.Text = "Fligth number";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(109, 33);
            label5.Name = "label5";
            label5.Size = new Size(100, 20);
            label5.TabIndex = 1;
            label5.Text = "Update status";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(128, 238);
            label6.Name = "label6";
            label6.Size = new Size(0, 20);
            label6.TabIndex = 2;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(78, 152);
            label7.Name = "label7";
            label7.Size = new Size(101, 20);
            label7.TabIndex = 3;
            label7.Text = "Fligth number";
            // 
            // FlightNumberPanel
            // 
            FlightNumberPanel.AutoSize = true;
            FlightNumberPanel.Location = new Point(78, 118);
            FlightNumberPanel.Name = "FlightNumberPanel";
            FlightNumberPanel.Size = new Size(100, 20);
            FlightNumberPanel.TabIndex = 4;
            FlightNumberPanel.Text = "FlightNumber";
            // 
            // FligthStatusCmbBx
            // 
            FligthStatusCmbBx.FormattingEnabled = true;
            FligthStatusCmbBx.Location = new Point(78, 189);
            FligthStatusCmbBx.Name = "FligthStatusCmbBx";
            FligthStatusCmbBx.Size = new Size(151, 28);
            FligthStatusCmbBx.TabIndex = 5;
            // 
            // UpdateBtn
            // 
            UpdateBtn.BackColor = Color.MidnightBlue;
            UpdateBtn.BackgroundColor = Color.MidnightBlue;
            UpdateBtn.BorderColor = Color.Transparent;
            UpdateBtn.BorderRadius = 10;
            UpdateBtn.BorderSize = 0;
            UpdateBtn.ClickColor = Color.Transparent;
            UpdateBtn.FlatAppearance.BorderSize = 0;
            UpdateBtn.FlatStyle = FlatStyle.Flat;
            UpdateBtn.ForeColor = Color.White;
            UpdateBtn.HoverColor = Color.Transparent;
            UpdateBtn.Location = new Point(78, 248);
            UpdateBtn.Name = "UpdateBtn";
            UpdateBtn.Size = new Size(151, 50);
            UpdateBtn.TabIndex = 6;
            UpdateBtn.Text = "Update";
            UpdateBtn.TextColor = Color.White;
            UpdateBtn.UseVisualStyleBackColor = false;
            // 
            // FligthMenu
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1182, 560);
            Controls.Add(AddFlightBtn);
            Controls.Add(FlightUpdatePnl);
            Controls.Add(FlightsContainerPanel);
            Controls.Add(headerMenu1);
            Name = "FligthMenu";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "FligthMenu";
            FlightsContainerPanel.ResumeLayout(false);
            FlightsContainerPanel.PerformLayout();
            FlightsContainerLayout.ResumeLayout(false);
            FlightContainerPanel.ResumeLayout(false);
            FlightContainerPanel.PerformLayout();
            FlightUpdatePnl.ResumeLayout(false);
            FlightUpdatePnl.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private HeaderMenu headerMenu1;
        private Panel FlightsContainerPanel;
        private FlowLayoutPanel FlightsContainerLayout;
        private Panel FlightUpdatePnl;
        private CustomControls.RoundedButton AddFlightBtn;
        private Panel FlightContainerPanel;
        private Label label4;
        private Label label3;
        private Label label2;
        private Label FligthNumberLabel;
        private Label FligthNumberLbl;
        private Label ArrivalLbl;
        private Label DepartureLbl;
        private Label StatusLbl;
        private CustomControls.RoundedButton DeleteBtn;
        private CustomControls.RoundedButton UpdateBtn;
        private ComboBox FligthStatusCmbBx;
        private Label FlightNumberPanel;
        private Label label7;
        private Label label6;
        private Label label5;
        private Label label1;
    }
}