namespace AirplaneFormApplication.Forms
{
    partial class AddFligthDialog
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
            okButton = new AirplaneFormApplication.CustomControls.RoundedButton();
            label1 = new Label();
            flightNumberTextBox = new TextBox();
            label2 = new Label();
            label3 = new Label();
            arrivalTextBox = new TextBox();
            departureTextBox = new TextBox();
            cancelButton = new AirplaneFormApplication.CustomControls.RoundedButton();
            SuspendLayout();
            // 
            // okButton
            // 
            okButton.BackColor = Color.MediumSlateBlue;
            okButton.BackgroundColor = Color.MediumSlateBlue;
            okButton.BorderColor = Color.PaleVioletRed;
            okButton.BorderRadius = 10;
            okButton.BorderSize = 0;
            okButton.ClickColor = Color.SlateBlue;
            okButton.FlatAppearance.BorderSize = 0;
            okButton.FlatStyle = FlatStyle.Flat;
            okButton.ForeColor = Color.White;
            okButton.HoverColor = Color.DarkSlateBlue;
            okButton.Location = new Point(12, 232);
            okButton.Name = "okButton";
            okButton.Size = new Size(125, 50);
            okButton.TabIndex = 0;
            okButton.Text = "Add";
            okButton.TextColor = Color.White;
            okButton.UseVisualStyleBackColor = false;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 9);
            label1.Name = "label1";
            label1.Size = new Size(104, 20);
            label1.TabIndex = 1;
            label1.Text = "Flight Number";
            // 
            // flightNumberTextBox
            // 
            flightNumberTextBox.Location = new Point(12, 32);
            flightNumberTextBox.Name = "flightNumberTextBox";
            flightNumberTextBox.Size = new Size(125, 27);
            flightNumberTextBox.TabIndex = 2;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(12, 62);
            label2.Name = "label2";
            label2.Size = new Size(137, 20);
            label2.TabIndex = 3;
            label2.Text = "Departure Location";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(12, 115);
            label3.Name = "label3";
            label3.Size = new Size(109, 20);
            label3.TabIndex = 4;
            label3.Text = "ArrivalLocation";
            // 
            // arrivalTextBox
            // 
            arrivalTextBox.Location = new Point(12, 138);
            arrivalTextBox.Name = "arrivalTextBox";
            arrivalTextBox.Size = new Size(125, 27);
            arrivalTextBox.TabIndex = 5;
            // 
            // departureTextBox
            // 
            departureTextBox.Location = new Point(12, 85);
            departureTextBox.Name = "departureTextBox";
            departureTextBox.Size = new Size(125, 27);
            departureTextBox.TabIndex = 6;
            // 
            // cancelButton
            // 
            cancelButton.BackColor = Color.Crimson;
            cancelButton.BackgroundColor = Color.Crimson;
            cancelButton.BorderColor = Color.PaleVioletRed;
            cancelButton.BorderRadius = 0;
            cancelButton.BorderSize = 0;
            cancelButton.ClickColor = Color.SlateBlue;
            cancelButton.FlatAppearance.BorderSize = 0;
            cancelButton.FlatStyle = FlatStyle.Flat;
            cancelButton.ForeColor = Color.White;
            cancelButton.HoverColor = Color.DarkSlateBlue;
            cancelButton.Location = new Point(167, 26);
            cancelButton.Name = "cancelButton";
            cancelButton.Size = new Size(35, 33);
            cancelButton.TabIndex = 7;
            cancelButton.Text = "x";
            cancelButton.TextColor = Color.White;
            cancelButton.UseVisualStyleBackColor = false;
            // 
            // AddFligthDialog
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(214, 316);
            Controls.Add(cancelButton);
            Controls.Add(departureTextBox);
            Controls.Add(arrivalTextBox);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(flightNumberTextBox);
            Controls.Add(label1);
            Controls.Add(okButton);
            Name = "AddFligthDialog";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "AddFligthDialog";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private CustomControls.RoundedButton okButton;
        private Label label1;
        private TextBox flightNumberTextBox;
        private Label label2;
        private Label label3;
        private TextBox arrivalTextBox;
        private TextBox departureTextBox;
        private CustomControls.RoundedButton cancelButton;
    }
}