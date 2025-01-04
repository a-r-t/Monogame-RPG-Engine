namespace MapEditor.src.MapBuilder
{
    partial class MapBuilder
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
            this.mapBuilderTabControl = new System.Windows.Forms.TabControl();
            this.tileEditorTab = new System.Windows.Forms.TabPage();
            this.dimensionsTab = new System.Windows.Forms.TabPage();
            this.tilesetTab = new System.Windows.Forms.TabPage();
            this.mapBuilderTabControl.SuspendLayout();
            this.SuspendLayout();
            // 
            // mapBuilderTabControl
            // 
            this.mapBuilderTabControl.Controls.Add(this.tileEditorTab);
            this.mapBuilderTabControl.Controls.Add(this.dimensionsTab);
            this.mapBuilderTabControl.Controls.Add(this.tilesetTab);
            this.mapBuilderTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mapBuilderTabControl.Location = new System.Drawing.Point(0, 0);
            this.mapBuilderTabControl.Name = "mapBuilderTabControl";
            this.mapBuilderTabControl.SelectedIndex = 0;
            this.mapBuilderTabControl.Size = new System.Drawing.Size(513, 384);
            this.mapBuilderTabControl.TabIndex = 0;
            // 
            // tileEditorTab
            // 
            this.tileEditorTab.Location = new System.Drawing.Point(4, 22);
            this.tileEditorTab.Name = "tileEditorTab";
            this.tileEditorTab.Padding = new System.Windows.Forms.Padding(3);
            this.tileEditorTab.Size = new System.Drawing.Size(505, 358);
            this.tileEditorTab.TabIndex = 0;
            this.tileEditorTab.Text = "Map";
            this.tileEditorTab.UseVisualStyleBackColor = true;
            // 
            // dimensionsTab
            // 
            this.dimensionsTab.Location = new System.Drawing.Point(4, 22);
            this.dimensionsTab.Name = "dimensionsTab";
            this.dimensionsTab.Padding = new System.Windows.Forms.Padding(3);
            this.dimensionsTab.Size = new System.Drawing.Size(505, 358);
            this.dimensionsTab.TabIndex = 1;
            this.dimensionsTab.Text = "Dimensions";
            this.dimensionsTab.UseVisualStyleBackColor = true;
            // 
            // tilesetTab
            // 
            this.tilesetTab.Location = new System.Drawing.Point(4, 22);
            this.tilesetTab.Name = "tilesetTab";
            this.tilesetTab.Padding = new System.Windows.Forms.Padding(3);
            this.tilesetTab.Size = new System.Drawing.Size(505, 358);
            this.tilesetTab.TabIndex = 2;
            this.tilesetTab.Text = "Tileset";
            this.tilesetTab.UseVisualStyleBackColor = true;
            // 
            // MapBuilder
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.mapBuilderTabControl);
            this.Name = "MapBuilder";
            this.Size = new System.Drawing.Size(513, 384);
            this.mapBuilderTabControl.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl mapBuilderTabControl;
        private System.Windows.Forms.TabPage tileEditorTab;
        private System.Windows.Forms.TabPage dimensionsTab;
        private System.Windows.Forms.TabPage tilesetTab;
    }
}
