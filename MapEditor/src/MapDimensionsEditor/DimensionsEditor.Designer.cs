namespace MapEditor.src.MapDimensionsEditor
{
    partial class DimensionsEditor
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
            this.currentHeightLabel = new System.Windows.Forms.Label();
            this.editWidthTextbox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.editHeightTextbox = new System.Windows.Forms.TextBox();
            this.okButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.widthChangeSettingsPanel = new System.Windows.Forms.Panel();
            this.widthChangeRightRadioButton = new System.Windows.Forms.RadioButton();
            this.widthChangeLeftRadioButton = new System.Windows.Forms.RadioButton();
            this.cancelButton = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.heightChangeBottomRadioButton = new System.Windows.Forms.RadioButton();
            this.heightChangeTopRadioButton = new System.Windows.Forms.RadioButton();
            this.errorMessageLabel = new System.Windows.Forms.Label();
            this.widthChangeSettingsPanel.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // currentHeightLabel
            // 
            this.currentHeightLabel.AutoSize = true;
            this.currentHeightLabel.Location = new System.Drawing.Point(3, 121);
            this.currentHeightLabel.Name = "currentHeightLabel";
            this.currentHeightLabel.Size = new System.Drawing.Size(41, 13);
            this.currentHeightLabel.TabIndex = 10;
            this.currentHeightLabel.Text = "Height:";
            // 
            // editWidthTextbox
            // 
            this.editWidthTextbox.Location = new System.Drawing.Point(47, 7);
            this.editWidthTextbox.Name = "editWidthTextbox";
            this.editWidthTextbox.Size = new System.Drawing.Size(80, 20);
            this.editWidthTextbox.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 10);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(38, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Width:";
            // 
            // editHeightTextbox
            // 
            this.editHeightTextbox.Location = new System.Drawing.Point(47, 118);
            this.editHeightTextbox.Name = "editHeightTextbox";
            this.editHeightTextbox.Size = new System.Drawing.Size(80, 20);
            this.editHeightTextbox.TabIndex = 1;
            // 
            // okButton
            // 
            this.okButton.Location = new System.Drawing.Point(6, 218);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(107, 37);
            this.okButton.TabIndex = 3;
            this.okButton.Text = "OK";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 45);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(212, 13);
            this.label1.TabIndex = 11;
            this.label1.Text = "Width change from left or right side of map?";
            // 
            // widthChangeSettingsPanel
            // 
            this.widthChangeSettingsPanel.BackColor = System.Drawing.Color.Transparent;
            this.widthChangeSettingsPanel.Controls.Add(this.widthChangeRightRadioButton);
            this.widthChangeSettingsPanel.Controls.Add(this.widthChangeLeftRadioButton);
            this.widthChangeSettingsPanel.Location = new System.Drawing.Point(6, 61);
            this.widthChangeSettingsPanel.Name = "widthChangeSettingsPanel";
            this.widthChangeSettingsPanel.Size = new System.Drawing.Size(129, 27);
            this.widthChangeSettingsPanel.TabIndex = 12;
            // 
            // widthChangeRightRadioButton
            // 
            this.widthChangeRightRadioButton.AutoSize = true;
            this.widthChangeRightRadioButton.Location = new System.Drawing.Point(71, 7);
            this.widthChangeRightRadioButton.Name = "widthChangeRightRadioButton";
            this.widthChangeRightRadioButton.Size = new System.Drawing.Size(50, 17);
            this.widthChangeRightRadioButton.TabIndex = 1;
            this.widthChangeRightRadioButton.TabStop = true;
            this.widthChangeRightRadioButton.Text = "Right";
            this.widthChangeRightRadioButton.UseVisualStyleBackColor = true;
            // 
            // widthChangeLeftRadioButton
            // 
            this.widthChangeLeftRadioButton.AutoSize = true;
            this.widthChangeLeftRadioButton.Location = new System.Drawing.Point(7, 7);
            this.widthChangeLeftRadioButton.Name = "widthChangeLeftRadioButton";
            this.widthChangeLeftRadioButton.Size = new System.Drawing.Size(43, 17);
            this.widthChangeLeftRadioButton.TabIndex = 0;
            this.widthChangeLeftRadioButton.TabStop = true;
            this.widthChangeLeftRadioButton.Text = "Left";
            this.widthChangeLeftRadioButton.UseVisualStyleBackColor = true;
            // 
            // cancelButton
            // 
            this.cancelButton.Location = new System.Drawing.Point(124, 218);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(107, 37);
            this.cancelButton.TabIndex = 13;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 156);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(228, 13);
            this.label3.TabIndex = 14;
            this.label3.Text = "Height change from top or bottom side of map?";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Transparent;
            this.panel1.Controls.Add(this.heightChangeBottomRadioButton);
            this.panel1.Controls.Add(this.heightChangeTopRadioButton);
            this.panel1.Location = new System.Drawing.Point(6, 172);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(130, 27);
            this.panel1.TabIndex = 13;
            // 
            // heightChangeBottomRadioButton
            // 
            this.heightChangeBottomRadioButton.AutoSize = true;
            this.heightChangeBottomRadioButton.Location = new System.Drawing.Point(71, 7);
            this.heightChangeBottomRadioButton.Name = "heightChangeBottomRadioButton";
            this.heightChangeBottomRadioButton.Size = new System.Drawing.Size(58, 17);
            this.heightChangeBottomRadioButton.TabIndex = 1;
            this.heightChangeBottomRadioButton.TabStop = true;
            this.heightChangeBottomRadioButton.Text = "Bottom";
            this.heightChangeBottomRadioButton.UseVisualStyleBackColor = true;
            // 
            // heightChangeTopRadioButton
            // 
            this.heightChangeTopRadioButton.AutoSize = true;
            this.heightChangeTopRadioButton.Location = new System.Drawing.Point(7, 7);
            this.heightChangeTopRadioButton.Name = "heightChangeTopRadioButton";
            this.heightChangeTopRadioButton.Size = new System.Drawing.Size(44, 17);
            this.heightChangeTopRadioButton.TabIndex = 0;
            this.heightChangeTopRadioButton.TabStop = true;
            this.heightChangeTopRadioButton.Text = "Top";
            this.heightChangeTopRadioButton.UseVisualStyleBackColor = true;
            // 
            // errorMessageLabel
            // 
            this.errorMessageLabel.AutoSize = true;
            this.errorMessageLabel.ForeColor = System.Drawing.Color.DarkRed;
            this.errorMessageLabel.Location = new System.Drawing.Point(6, 262);
            this.errorMessageLabel.Name = "errorMessageLabel";
            this.errorMessageLabel.Size = new System.Drawing.Size(32, 13);
            this.errorMessageLabel.TabIndex = 15;
            this.errorMessageLabel.Text = "Error:";
            // 
            // DimensionsEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.errorMessageLabel);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.widthChangeSettingsPanel);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.editHeightTextbox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.editWidthTextbox);
            this.Controls.Add(this.currentHeightLabel);
            this.Controls.Add(this.label1);
            this.Name = "DimensionsEditor";
            this.Size = new System.Drawing.Size(256, 281);
            this.widthChangeSettingsPanel.ResumeLayout(false);
            this.widthChangeSettingsPanel.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label currentHeightLabel;
        private System.Windows.Forms.TextBox editWidthTextbox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox editHeightTextbox;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel widthChangeSettingsPanel;
        private System.Windows.Forms.RadioButton widthChangeRightRadioButton;
        private System.Windows.Forms.RadioButton widthChangeLeftRadioButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.RadioButton heightChangeBottomRadioButton;
        private System.Windows.Forms.RadioButton heightChangeTopRadioButton;
        private System.Windows.Forms.Label errorMessageLabel;
    }
}
