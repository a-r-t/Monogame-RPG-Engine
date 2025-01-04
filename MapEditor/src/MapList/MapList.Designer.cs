namespace MapEditor.src.MapList
{
    partial class MapList
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
            this.mapTreeView = new System.Windows.Forms.TreeView();
            this.SuspendLayout();
            // 
            // mapTreeView
            // 
            this.mapTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mapTreeView.Location = new System.Drawing.Point(0, 0);
            this.mapTreeView.Name = "mapTreeView";
            this.mapTreeView.Size = new System.Drawing.Size(150, 150);
            this.mapTreeView.TabIndex = 0;
            this.mapTreeView.AfterLabelEdit += new System.Windows.Forms.NodeLabelEditEventHandler(this.mapTreeView_AfterLabelEdit);
            this.mapTreeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.mapTreeView_AfterSelect);
            this.mapTreeView.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.mapTreeView_NodeMouseClick);
            this.mapTreeView.DoubleClick += new System.EventHandler(this.mapTreeView_DoubleClick);
            // 
            // MapList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this.mapTreeView);
            this.Name = "MapList";
            this.Load += new System.EventHandler(this.MapList_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView mapTreeView;
    }
}
