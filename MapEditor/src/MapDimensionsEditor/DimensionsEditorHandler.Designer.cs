
namespace MapEditor.src.MapDimensionsEditor
{
    partial class DimensionsEditorHandler
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
            this.dimensionsEditorPanel = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // dimensionsEditorPanel
            // 
            this.dimensionsEditorPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dimensionsEditorPanel.Location = new System.Drawing.Point(0, 0);
            this.dimensionsEditorPanel.Name = "dimensionsEditorPanel";
            this.dimensionsEditorPanel.Size = new System.Drawing.Size(150, 150);
            this.dimensionsEditorPanel.TabIndex = 0;
            // 
            // DimensionsEditorHandler
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dimensionsEditorPanel);
            this.Name = "DimensionsEditorHandler";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel dimensionsEditorPanel;
    }
}
