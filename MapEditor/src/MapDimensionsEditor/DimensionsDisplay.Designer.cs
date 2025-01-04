namespace MapEditor.src.MapDimensionsEditor
{
    partial class DimensionsDisplay
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
            this.changeMapDimensionsButton = new System.Windows.Forms.Button();
            this.heightLabel = new System.Windows.Forms.Label();
            this.widthLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // changeMapDimensionsButton
            // 
            this.changeMapDimensionsButton.Location = new System.Drawing.Point(2, 60);
            this.changeMapDimensionsButton.Name = "changeMapDimensionsButton";
            this.changeMapDimensionsButton.Size = new System.Drawing.Size(145, 35);
            this.changeMapDimensionsButton.TabIndex = 10;
            this.changeMapDimensionsButton.Text = "Change Map Dimensions";
            this.changeMapDimensionsButton.UseVisualStyleBackColor = true;
            this.changeMapDimensionsButton.Click += new System.EventHandler(this.changeMapDimensionsButton_Click);
            // 
            // heightLabel
            // 
            this.heightLabel.AutoSize = true;
            this.heightLabel.Location = new System.Drawing.Point(3, 34);
            this.heightLabel.Name = "heightLabel";
            this.heightLabel.Size = new System.Drawing.Size(41, 13);
            this.heightLabel.TabIndex = 9;
            this.heightLabel.Text = "Height:";
            // 
            // widthLabel
            // 
            this.widthLabel.AutoSize = true;
            this.widthLabel.Location = new System.Drawing.Point(5, 9);
            this.widthLabel.Name = "widthLabel";
            this.widthLabel.Size = new System.Drawing.Size(38, 13);
            this.widthLabel.TabIndex = 8;
            this.widthLabel.Text = "Width:";
            // 
            // DimensionsDisplay
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.changeMapDimensionsButton);
            this.Controls.Add(this.heightLabel);
            this.Controls.Add(this.widthLabel);
            this.Name = "DimensionsDisplay";
            this.Size = new System.Drawing.Size(150, 100);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button changeMapDimensionsButton;
        private System.Windows.Forms.Label heightLabel;
        private System.Windows.Forms.Label widthLabel;
    }
}
