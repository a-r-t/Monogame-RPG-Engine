
namespace MapEditor.src.MapTilesetEditor
{
    partial class TilesetEditorHandler
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
            this.tilesetEditorPanel = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // tilesetEditorPanel
            // 
            this.tilesetEditorPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tilesetEditorPanel.Location = new System.Drawing.Point(0, 0);
            this.tilesetEditorPanel.Name = "tilesetEditorPanel";
            this.tilesetEditorPanel.Size = new System.Drawing.Size(150, 150);
            this.tilesetEditorPanel.TabIndex = 0;
            // 
            // TilesetEditorHandler
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tilesetEditorPanel);
            this.Name = "TilesetEditorHandler";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel tilesetEditorPanel;
    }
}
