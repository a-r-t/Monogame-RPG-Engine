namespace MapEditor.src.TilesetEditor
{
    partial class TileGraphicChooserForm
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
            panel1 = new Panel();
            cancelButton = new Button();
            okButton = new Button();
            tileGraphicPanel = new Panel();
            tileGraphicPictureBox = new PictureBox();
            panel1.SuspendLayout();
            tileGraphicPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)tileGraphicPictureBox).BeginInit();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.Controls.Add(cancelButton);
            panel1.Controls.Add(okButton);
            panel1.Dock = DockStyle.Bottom;
            panel1.Location = new Point(0, 350);
            panel1.Name = "panel1";
            panel1.Size = new Size(800, 100);
            panel1.TabIndex = 0;
            // 
            // cancelButton
            // 
            cancelButton.Anchor = AnchorStyles.Top | AnchorStyles.Bottom;
            cancelButton.Location = new Point(409, 29);
            cancelButton.Name = "cancelButton";
            cancelButton.Size = new Size(110, 43);
            cancelButton.TabIndex = 1;
            cancelButton.Text = "Cancel";
            cancelButton.UseVisualStyleBackColor = true;
            // 
            // okButton
            // 
            okButton.Anchor = AnchorStyles.Top | AnchorStyles.Bottom;
            okButton.Location = new Point(281, 29);
            okButton.Name = "okButton";
            okButton.Size = new Size(110, 43);
            okButton.TabIndex = 0;
            okButton.Text = "OK";
            okButton.UseVisualStyleBackColor = true;
            // 
            // tileGraphicPanel
            // 
            tileGraphicPanel.AutoScroll = true;
            tileGraphicPanel.Controls.Add(tileGraphicPictureBox);
            tileGraphicPanel.Dock = DockStyle.Fill;
            tileGraphicPanel.Location = new Point(0, 0);
            tileGraphicPanel.Name = "tileGraphicPanel";
            tileGraphicPanel.Size = new Size(800, 350);
            tileGraphicPanel.TabIndex = 1;
            // 
            // tileGraphicPictureBox
            // 
            tileGraphicPictureBox.Location = new Point(3, 3);
            tileGraphicPictureBox.Name = "tileGraphicPictureBox";
            tileGraphicPictureBox.Size = new Size(100, 50);
            tileGraphicPictureBox.TabIndex = 0;
            tileGraphicPictureBox.TabStop = false;
            // 
            // TileGraphicChooserForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(tileGraphicPanel);
            Controls.Add(panel1);
            Name = "TileGraphicChooserForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "TileGraphicChooser";
            panel1.ResumeLayout(false);
            tileGraphicPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)tileGraphicPictureBox).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private Panel tileGraphicPanel;
        private Button okButton;
        private Button cancelButton;
        private PictureBox tileGraphicPictureBox;
    }
}