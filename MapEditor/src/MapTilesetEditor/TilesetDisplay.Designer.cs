namespace MapEditor.src.MapTilesetEditor
{
    partial class TilesetDisplay
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
            this.scaleLabel = new System.Windows.Forms.Label();
            this.tilesetLabel = new System.Windows.Forms.Label();
            this.changeTilesetInfoButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // scaleLabel
            // 
            this.scaleLabel.AutoSize = true;
            this.scaleLabel.Location = new System.Drawing.Point(9, 34);
            this.scaleLabel.Name = "scaleLabel";
            this.scaleLabel.Size = new System.Drawing.Size(37, 13);
            this.scaleLabel.TabIndex = 3;
            this.scaleLabel.Text = "Scale:";
            // 
            // tilesetLabel
            // 
            this.tilesetLabel.AutoSize = true;
            this.tilesetLabel.Location = new System.Drawing.Point(5, 9);
            this.tilesetLabel.Name = "tilesetLabel";
            this.tilesetLabel.Size = new System.Drawing.Size(41, 13);
            this.tilesetLabel.TabIndex = 2;
            this.tilesetLabel.Text = "Tileset:";
            // 
            // changeTilesetInfoButton
            // 
            this.changeTilesetInfoButton.Location = new System.Drawing.Point(2, 60);
            this.changeTilesetInfoButton.Name = "changeTilesetInfoButton";
            this.changeTilesetInfoButton.Size = new System.Drawing.Size(145, 35);
            this.changeTilesetInfoButton.TabIndex = 11;
            this.changeTilesetInfoButton.Text = "Change Tileset Info";
            this.changeTilesetInfoButton.UseVisualStyleBackColor = true;
            this.changeTilesetInfoButton.Click += new System.EventHandler(this.changeTilesetInfoButton_Click);
            // 
            // TilesetDisplay
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.changeTilesetInfoButton);
            this.Controls.Add(this.scaleLabel);
            this.Controls.Add(this.tilesetLabel);
            this.Name = "TilesetDisplay";
            this.Size = new System.Drawing.Size(150, 100);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label scaleLabel;
        private System.Windows.Forms.Label tilesetLabel;
        private System.Windows.Forms.Button changeTilesetInfoButton;
    }
}
