namespace AirplaneFormApplication
{
    partial class CheckInMenu
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
            PassengerContainer = new Panel();
            PssngrPssNmLbl = new Label();
            SearchContainerPnl = new Panel();
            SearchTxtBox = new TextBox();
            PassengerInfoPanel = new Panel();
            PasswordNumLbl = new Label();
            NameLbl = new Label();
            label2 = new Label();
            label1 = new Label();
            ConfirmBtn = new AirplaneFormApplication.CustomControls.RoundedButton();
            PassengersContainerLayout.SuspendLayout();
            PassengerContainer.SuspendLayout();
            SearchContainerPnl.SuspendLayout();
            PassengerInfoPanel.SuspendLayout();
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
            PassengersContainerLayout.Controls.Add(PassengerContainer);
            PassengersContainerLayout.Location = new Point(90, 185);
            PassengersContainerLayout.Name = "PassengersContainerLayout";
            PassengersContainerLayout.Size = new Size(464, 363);
            PassengersContainerLayout.TabIndex = 2;
            // 
            // PassengerContainer
            // 
            PassengerContainer.BackColor = SystemColors.ActiveCaption;
            PassengerContainer.Controls.Add(PssngrPssNmLbl);
            PassengerContainer.Location = new Point(3, 3);
            PassengerContainer.Name = "PassengerContainer";
            PassengerContainer.Size = new Size(427, 53);
            PassengerContainer.TabIndex = 0;
            // 
            // PssngrPssNmLbl
            // 
            PssngrPssNmLbl.AutoSize = true;
            PssngrPssNmLbl.Location = new Point(16, 16);
            PssngrPssNmLbl.Name = "PssngrPssNmLbl";
            PssngrPssNmLbl.Size = new Size(50, 20);
            PssngrPssNmLbl.TabIndex = 0;
            PssngrPssNmLbl.Text = "label3";
            // 
            // SearchContainerPnl
            // 
            SearchContainerPnl.BackColor = SystemColors.ButtonHighlight;
            SearchContainerPnl.Controls.Add(SearchTxtBox);
            SearchContainerPnl.Location = new Point(90, 87);
            SearchContainerPnl.Name = "SearchContainerPnl";
            SearchContainerPnl.Size = new Size(464, 83);
            SearchContainerPnl.TabIndex = 4;
            // 
            // SearchTxtBox
            // 
            SearchTxtBox.Location = new Point(26, 20);
            SearchTxtBox.Name = "SearchTxtBox";
            SearchTxtBox.Size = new Size(422, 27);
            SearchTxtBox.TabIndex = 0;
            // 
            // PassengerInfoPanel
            // 
            PassengerInfoPanel.BackColor = SystemColors.ButtonHighlight;
            PassengerInfoPanel.Controls.Add(PasswordNumLbl);
            PassengerInfoPanel.Controls.Add(NameLbl);
            PassengerInfoPanel.Controls.Add(label2);
            PassengerInfoPanel.Controls.Add(label1);
            PassengerInfoPanel.Controls.Add(ConfirmBtn);
            PassengerInfoPanel.Location = new Point(712, 134);
            PassengerInfoPanel.Name = "PassengerInfoPanel";
            PassengerInfoPanel.Size = new Size(387, 362);
            PassengerInfoPanel.TabIndex = 5;
            // 
            // PasswordNumLbl
            // 
            PasswordNumLbl.AutoSize = true;
            PasswordNumLbl.Location = new Point(106, 168);
            PasswordNumLbl.Name = "PasswordNumLbl";
            PasswordNumLbl.Size = new Size(50, 20);
            PasswordNumLbl.TabIndex = 4;
            PasswordNumLbl.Text = "label4";
            // 
            // NameLbl
            // 
            NameLbl.AutoSize = true;
            NameLbl.Location = new Point(106, 107);
            NameLbl.Name = "NameLbl";
            NameLbl.Size = new Size(50, 20);
            NameLbl.TabIndex = 3;
            NameLbl.Text = "label3";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(106, 148);
            label2.Name = "label2";
            label2.Size = new Size(125, 20);
            label2.TabIndex = 2;
            label2.Text = "Password number";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(106, 87);
            label1.Name = "label1";
            label1.Size = new Size(49, 20);
            label1.TabIndex = 1;
            label1.Text = "Name";
            // 
            // ConfirmBtn
            // 
            ConfirmBtn.BackColor = Color.Lime;
            ConfirmBtn.BackgroundColor = Color.Lime;
            ConfirmBtn.BorderColor = Color.PaleVioletRed;
            ConfirmBtn.BorderRadius = 10;
            ConfirmBtn.BorderSize = 0;
            ConfirmBtn.ClickColor = Color.SlateBlue;
            ConfirmBtn.FlatAppearance.BorderSize = 0;
            ConfirmBtn.FlatStyle = FlatStyle.Flat;
            ConfirmBtn.ForeColor = Color.White;
            ConfirmBtn.HoverColor = Color.DarkSlateBlue;
            ConfirmBtn.Location = new Point(106, 251);
            ConfirmBtn.Name = "ConfirmBtn";
            ConfirmBtn.Size = new Size(188, 50);
            ConfirmBtn.TabIndex = 0;
            ConfirmBtn.Text = "Confirm";
            ConfirmBtn.TextColor = Color.White;
            ConfirmBtn.UseVisualStyleBackColor = false;
            // 
            // CheckInMenu
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1182, 553);
            Controls.Add(PassengerInfoPanel);
            Controls.Add(SearchContainerPnl);
            Controls.Add(PassengersContainerLayout);
            Controls.Add(headerMenu1);
            Name = "CheckInMenu";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "CheckInMenu";
            PassengersContainerLayout.ResumeLayout(false);
            PassengerContainer.ResumeLayout(false);
            PassengerContainer.PerformLayout();
            SearchContainerPnl.ResumeLayout(false);
            SearchContainerPnl.PerformLayout();
            PassengerInfoPanel.ResumeLayout(false);
            PassengerInfoPanel.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private HeaderMenu headerMenu1;
        private FlowLayoutPanel PassengersContainerLayout;
        private Panel SearchContainerPnl;
        private TextBox SearchTxtBox;
        private Panel PassengerInfoPanel;
        private Panel PassengerContainer;
        private Label PssngrPssNmLbl;
        private Label PasswordNumLbl;
        private Label NameLbl;
        private Label label2;
        private Label label1;
        private CustomControls.RoundedButton ConfirmBtn;
    }
}