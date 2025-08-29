namespace AirplaneFormApplication
{
    partial class PassengerMenu
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
            PassengersContainerLayout = new FlowLayoutPanel();
            PassengerContainerPanel = new Panel();
            PasswordLabel = new Label();
            NameLabel = new Label();
            roundedButton1 = new AirplaneFormApplication.CustomControls.RoundedButton();
            AddContainerPanel = new Panel();
            AddBtn = new AirplaneFormApplication.CustomControls.RoundedButton();
            label4 = new Label();
            label3 = new Label();
            label2 = new Label();
            label1 = new Label();
            FlightIdTxtBox = new TextBox();
            PasswordTxtBox = new TextBox();
            NameTxtBox = new TextBox();
            PassengersContainerLayout.SuspendLayout();
            PassengerContainerPanel.SuspendLayout();
            AddContainerPanel.SuspendLayout();
            SuspendLayout();
            // 
            // headerMenu1
            // 
            headerMenu1.Location = new Point(0, 0);
            headerMenu1.Name = "headerMenu1";
            headerMenu1.Size = new Size(1500, 81);
            headerMenu1.TabIndex = 1;
            // 
            // PassengersContainerLayout
            // 
            PassengersContainerLayout.AutoScroll = true;
            PassengersContainerLayout.Controls.Add(PassengerContainerPanel);
            PassengersContainerLayout.Location = new Point(40, 104);
            PassengersContainerLayout.Name = "PassengersContainerLayout";
            PassengersContainerLayout.Size = new Size(629, 423);
            PassengersContainerLayout.TabIndex = 2;
            // 
            // PassengerContainerPanel
            // 
            PassengerContainerPanel.BackColor = SystemColors.ActiveCaption;
            PassengerContainerPanel.Controls.Add(PasswordLabel);
            PassengerContainerPanel.Controls.Add(NameLabel);
            PassengerContainerPanel.Controls.Add(roundedButton1);
            PassengerContainerPanel.Location = new Point(3, 3);
            PassengerContainerPanel.Name = "PassengerContainerPanel";
            PassengerContainerPanel.Size = new Size(592, 69);
            PassengerContainerPanel.TabIndex = 0;
            // 
            // PasswordLabel
            // 
            PasswordLabel.AutoSize = true;
            PasswordLabel.Location = new Point(235, 26);
            PasswordLabel.Name = "PasswordLabel";
            PasswordLabel.Size = new Size(106, 20);
            PasswordLabel.TabIndex = 2;
            PasswordLabel.Text = "PasswordLabel";
            // 
            // NameLabel
            // 
            NameLabel.AutoSize = true;
            NameLabel.Location = new Point(44, 25);
            NameLabel.Name = "NameLabel";
            NameLabel.Size = new Size(86, 20);
            NameLabel.TabIndex = 1;
            NameLabel.Text = "Name label";
            // 
            // roundedButton1
            // 
            roundedButton1.BackColor = Color.Transparent;
            roundedButton1.BackgroundColor = Color.Transparent;
            roundedButton1.BorderColor = Color.Transparent;
            roundedButton1.BorderRadius = 0;
            roundedButton1.BorderSize = 0;
            roundedButton1.ClickColor = Color.Transparent;
            roundedButton1.FlatAppearance.BorderSize = 0;
            roundedButton1.FlatStyle = FlatStyle.Flat;
            roundedButton1.ForeColor = Color.Red;
            roundedButton1.HoverColor = Color.Transparent;
            roundedButton1.Location = new Point(533, 16);
            roundedButton1.Name = "roundedButton1";
            roundedButton1.Size = new Size(56, 50);
            roundedButton1.TabIndex = 0;
            roundedButton1.Text = "🗑️";
            roundedButton1.TextColor = Color.Red;
            roundedButton1.UseVisualStyleBackColor = false;
            // 
            // AddContainerPanel
            // 
            AddContainerPanel.BackColor = SystemColors.ButtonHighlight;
            AddContainerPanel.Controls.Add(AddBtn);
            AddContainerPanel.Controls.Add(label4);
            AddContainerPanel.Controls.Add(label3);
            AddContainerPanel.Controls.Add(label2);
            AddContainerPanel.Controls.Add(label1);
            AddContainerPanel.Controls.Add(FlightIdTxtBox);
            AddContainerPanel.Controls.Add(PasswordTxtBox);
            AddContainerPanel.Controls.Add(NameTxtBox);
            AddContainerPanel.Location = new Point(750, 145);
            AddContainerPanel.Name = "AddContainerPanel";
            AddContainerPanel.Size = new Size(347, 335);
            AddContainerPanel.TabIndex = 3;
            // 
            // AddBtn
            // 
            AddBtn.BackColor = Color.MediumSlateBlue;
            AddBtn.BackgroundColor = Color.MediumSlateBlue;
            AddBtn.BorderColor = Color.PaleVioletRed;
            AddBtn.BorderRadius = 0;
            AddBtn.BorderSize = 0;
            AddBtn.ClickColor = Color.SlateBlue;
            AddBtn.FlatAppearance.BorderSize = 0;
            AddBtn.FlatStyle = FlatStyle.Flat;
            AddBtn.ForeColor = Color.White;
            AddBtn.HoverColor = Color.DarkSlateBlue;
            AddBtn.Location = new Point(127, 260);
            AddBtn.Name = "AddBtn";
            AddBtn.Size = new Size(125, 41);
            AddBtn.TabIndex = 7;
            AddBtn.Text = "Add";
            AddBtn.TextColor = Color.White;
            AddBtn.UseVisualStyleBackColor = false;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Verdana", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label4.Location = new Point(127, 145);
            label4.Name = "label4";
            label4.Size = new Size(142, 18);
            label4.TabIndex = 6;
            label4.Text = "Password number";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Verdana", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label3.Location = new Point(127, 196);
            label3.Name = "label3";
            label3.Size = new Size(66, 18);
            label3.TabIndex = 5;
            label3.Text = "Flight Id";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Verdana", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label2.Location = new Point(127, 94);
            label2.Name = "label2";
            label2.Size = new Size(52, 18);
            label2.TabIndex = 4;
            label2.Text = "Name";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Verdana", 18F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label1.Location = new Point(110, 24);
            label1.Name = "label1";
            label1.Size = new Size(154, 36);
            label1.TabIndex = 3;
            label1.Text = "Add User";
            // 
            // FlightIdTxtBox
            // 
            FlightIdTxtBox.Location = new Point(127, 217);
            FlightIdTxtBox.Name = "FlightIdTxtBox";
            FlightIdTxtBox.Size = new Size(125, 27);
            FlightIdTxtBox.TabIndex = 2;
            // 
            // PasswordTxtBox
            // 
            PasswordTxtBox.Location = new Point(127, 166);
            PasswordTxtBox.Name = "PasswordTxtBox";
            PasswordTxtBox.Size = new Size(125, 27);
            PasswordTxtBox.TabIndex = 1;
            // 
            // NameTxtBox
            // 
            NameTxtBox.Location = new Point(127, 115);
            NameTxtBox.Name = "NameTxtBox";
            NameTxtBox.Size = new Size(125, 27);
            NameTxtBox.TabIndex = 0;
            // 
            // PassengerMenu
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1182, 560);
            Controls.Add(AddContainerPanel);
            Controls.Add(PassengersContainerLayout);
            Controls.Add(headerMenu1);
            Name = "PassengerMenu";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "PassengerMenu";
            PassengersContainerLayout.ResumeLayout(false);
            PassengerContainerPanel.ResumeLayout(false);
            PassengerContainerPanel.PerformLayout();
            AddContainerPanel.ResumeLayout(false);
            AddContainerPanel.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private HeaderMenu headerMenu1;
        private FlowLayoutPanel PassengersContainerLayout;
        private Panel AddContainerPanel;
        private Label label4;
        private Label label3;
        private Label label2;
        private Label label1;
        private TextBox FlightIdTxtBox;
        private TextBox PasswordTxtBox;
        private TextBox NameTxtBox;
        private Panel PassengerContainerPanel;
        private CustomControls.RoundedButton roundedButton1;
        private Label NameLabel;
        private Label PasswordLabel;
        private CustomControls.RoundedButton AddBtn;
    }
}