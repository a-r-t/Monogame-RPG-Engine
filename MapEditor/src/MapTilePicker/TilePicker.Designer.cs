namespace MapEditor.src.MapTilePicker
{
    partial class TilePicker
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
            this.tilePickerPanel = new System.Windows.Forms.Panel();
            this.tilePickerPictureBox = new System.Windows.Forms.PictureBox();
            this.tilePickerPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tilePickerPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // tilePickerPanel
            // 
            this.tilePickerPanel.AutoScroll = true;
            this.tilePickerPanel.Controls.Add(this.tilePickerPictureBox);
            this.tilePickerPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tilePickerPanel.Location = new System.Drawing.Point(0, 0);
            this.tilePickerPanel.Name = "tilePickerPanel";
            this.tilePickerPanel.Size = new System.Drawing.Size(150, 150);
            this.tilePickerPanel.TabIndex = 0;
            this.tilePickerPanel.Resize += new System.EventHandler(this.tilePickerPanel_Resize);
            // 
            // tilePickerPictureBox
            // 
            this.tilePickerPictureBox.Location = new System.Drawing.Point(3, 3);
            this.tilePickerPictureBox.Name = "tilePickerPictureBox";
            this.tilePickerPictureBox.Size = new System.Drawing.Size(144, 144);
            this.tilePickerPictureBox.TabIndex = 0;
            this.tilePickerPictureBox.TabStop = false;
            this.tilePickerPictureBox.Paint += new System.Windows.Forms.PaintEventHandler(this.tilePickerPictureBox_Paint);
            this.tilePickerPictureBox.MouseClick += new System.Windows.Forms.MouseEventHandler(this.tilePickerPictureBox_MouseClick);
            this.tilePickerPictureBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.tilePickerPictureBox_MouseDown);
            this.tilePickerPictureBox.MouseLeave += new System.EventHandler(this.tilePickerPictureBox_MouseLeave);
            this.tilePickerPictureBox.MouseMove += new System.Windows.Forms.MouseEventHandler(this.tilePickerPictureBox_MouseMove);
            // 
            // TilePicker
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.Controls.Add(this.tilePickerPanel);
            this.Name = "TilePicker";
            this.tilePickerPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tilePickerPictureBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel tilePickerPanel;
        private System.Windows.Forms.PictureBox tilePickerPictureBox;
    }
}
