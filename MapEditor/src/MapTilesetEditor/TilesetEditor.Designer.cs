
namespace MapEditor.src.MapTilesetEditor
{
    partial class TilesetEditor
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
            this.tilesetLabel = new System.Windows.Forms.Label();
            this.scaleLabel = new System.Windows.Forms.Label();
            this.tilesetCombobox = new System.Windows.Forms.ComboBox();
            this.scaleTextbox = new System.Windows.Forms.TextBox();
            this.cancelButton = new System.Windows.Forms.Button();
            this.okButton = new System.Windows.Forms.Button();
            this.errorMessageLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // tilesetLabel
            // 
            this.tilesetLabel.AutoSize = true;
            this.tilesetLabel.Location = new System.Drawing.Point(3, 10);
            this.tilesetLabel.Name = "tilesetLabel";
            this.tilesetLabel.Size = new System.Drawing.Size(41, 13);
            this.tilesetLabel.TabIndex = 0;
            this.tilesetLabel.Text = "Tileset:";
            // 
            // scaleLabel
            // 
            this.scaleLabel.AutoSize = true;
            this.scaleLabel.Location = new System.Drawing.Point(3, 43);
            this.scaleLabel.Name = "scaleLabel";
            this.scaleLabel.Size = new System.Drawing.Size(37, 13);
            this.scaleLabel.TabIndex = 1;
            this.scaleLabel.Text = "Scale:";
            // 
            // tilesetCombobox
            // 
            this.tilesetCombobox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.tilesetCombobox.FormattingEnabled = true;
            this.tilesetCombobox.Location = new System.Drawing.Point(50, 7);
            this.tilesetCombobox.Name = "tilesetCombobox";
            this.tilesetCombobox.Size = new System.Drawing.Size(165, 21);
            this.tilesetCombobox.TabIndex = 2;
            // 
            // scaleTextbox
            // 
            this.scaleTextbox.Location = new System.Drawing.Point(50, 40);
            this.scaleTextbox.Name = "scaleTextbox";
            this.scaleTextbox.Size = new System.Drawing.Size(67, 20);
            this.scaleTextbox.TabIndex = 3;
            // 
            // cancelButton
            // 
            this.cancelButton.Location = new System.Drawing.Point(119, 93);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(107, 37);
            this.cancelButton.TabIndex = 15;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // okButton
            // 
            this.okButton.Location = new System.Drawing.Point(6, 93);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(107, 37);
            this.okButton.TabIndex = 14;
            this.okButton.Text = "OK";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // errorMessageLabel
            // 
            this.errorMessageLabel.AutoSize = true;
            this.errorMessageLabel.ForeColor = System.Drawing.Color.DarkRed;
            this.errorMessageLabel.Location = new System.Drawing.Point(6, 139);
            this.errorMessageLabel.Name = "errorMessageLabel";
            this.errorMessageLabel.Size = new System.Drawing.Size(32, 13);
            this.errorMessageLabel.TabIndex = 16;
            this.errorMessageLabel.Text = "Error:";
            // 
            // TilesetEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.errorMessageLabel);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.scaleTextbox);
            this.Controls.Add(this.tilesetCombobox);
            this.Controls.Add(this.scaleLabel);
            this.Controls.Add(this.tilesetLabel);
            this.Name = "TilesetEditor";
            this.Size = new System.Drawing.Size(247, 159);
            this.Load += new System.EventHandler(this.TilesetEditor_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label tilesetLabel;
        private System.Windows.Forms.Label scaleLabel;
        private System.Windows.Forms.ComboBox tilesetCombobox;
        private System.Windows.Forms.TextBox scaleTextbox;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Label errorMessageLabel;
    }
}
