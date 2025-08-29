namespace AirplaneFormApplication.Forms
{
    partial class SeatSelectionDialog
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
            SeatsContainerLayout = new FlowLayoutPanel();
            ConfirmBtn = new AirplaneFormApplication.CustomControls.RoundedButton();
            SeatBtn = new AirplaneFormApplication.CustomControls.RoundedButton();
            SeatsContainerLayout.SuspendLayout();
            SuspendLayout();
            // 
            // SeatsContainerLayout
            // 
            SeatsContainerLayout.Controls.Add(SeatBtn);
            SeatsContainerLayout.Location = new Point(2, 1);
            SeatsContainerLayout.Name = "SeatsContainerLayout";
            SeatsContainerLayout.Size = new Size(280, 260);
            SeatsContainerLayout.TabIndex = 0;
            // 
            // ConfirmBtn
            // 
            ConfirmBtn.BackColor = Color.MediumSlateBlue;
            ConfirmBtn.BackgroundColor = Color.MediumSlateBlue;
            ConfirmBtn.BorderColor = Color.PaleVioletRed;
            ConfirmBtn.BorderRadius = 10;
            ConfirmBtn.BorderSize = 0;
            ConfirmBtn.ClickColor = Color.SlateBlue;
            ConfirmBtn.FlatAppearance.BorderSize = 0;
            ConfirmBtn.FlatStyle = FlatStyle.Flat;
            ConfirmBtn.ForeColor = Color.White;
            ConfirmBtn.HoverColor = Color.DarkSlateBlue;
            ConfirmBtn.Location = new Point(61, 272);
            ConfirmBtn.Name = "ConfirmBtn";
            ConfirmBtn.Size = new Size(162, 50);
            ConfirmBtn.TabIndex = 1;
            ConfirmBtn.Text = "Confirm";
            ConfirmBtn.TextColor = Color.White;
            ConfirmBtn.UseVisualStyleBackColor = false;
            // 
            // SeatBtn
            // 
            SeatBtn.BackColor = Color.MediumSlateBlue;
            SeatBtn.BackgroundColor = Color.MediumSlateBlue;
            SeatBtn.BorderColor = Color.PaleVioletRed;
            SeatBtn.BorderRadius = 0;
            SeatBtn.BorderSize = 0;
            SeatBtn.ClickColor = Color.SlateBlue;
            SeatBtn.FlatAppearance.BorderSize = 0;
            SeatBtn.FlatStyle = FlatStyle.Flat;
            SeatBtn.ForeColor = Color.White;
            SeatBtn.HoverColor = Color.DarkSlateBlue;
            SeatBtn.Location = new Point(3, 3);
            SeatBtn.Name = "SeatBtn";
            SeatBtn.Size = new Size(50, 50);
            SeatBtn.TabIndex = 0;
            SeatBtn.Text = "roundedButton2";
            SeatBtn.TextColor = Color.White;
            SeatBtn.UseVisualStyleBackColor = false;
            // 
            // SeatSelectionDialog
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(284, 334);
            Controls.Add(ConfirmBtn);
            Controls.Add(SeatsContainerLayout);
            Name = "SeatSelectionDialog";
            Text = "SeatSelectionDialog";
            SeatsContainerLayout.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private FlowLayoutPanel SeatsContainerLayout;
        private CustomControls.RoundedButton ConfirmBtn;
        private CustomControls.RoundedButton SeatBtn;
    }
}