namespace MapEditor
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            splitContainer1 = new SplitContainer();
            mapListPanel = new Panel();
            mapBuilderPanel = new Panel();
            menuStrip1 = new MenuStrip();
            fileToolStripMenuItem = new ToolStripMenuItem();
            saveToolStripMenuItem = new ToolStripMenuItem();
            toolsToolStripMenuItem = new ToolStripMenuItem();
            tilesetEditorToolStripMenuItem = new ToolStripMenuItem();
            toolStrip1 = new ToolStrip();
            saveButton = new ToolStripButton();
            tilesetsManagerToolStripMenuItem = new ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            menuStrip1.SuspendLayout();
            toolStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = DockStyle.Fill;
            splitContainer1.FixedPanel = FixedPanel.Panel1;
            splitContainer1.Location = new Point(0, 49);
            splitContainer1.Margin = new Padding(4, 3, 4, 3);
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(mapListPanel);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(mapBuilderPanel);
            splitContainer1.Size = new Size(1148, 598);
            splitContainer1.SplitterDistance = 146;
            splitContainer1.SplitterWidth = 5;
            splitContainer1.TabIndex = 0;
            // 
            // mapListPanel
            // 
            mapListPanel.Dock = DockStyle.Fill;
            mapListPanel.Location = new Point(0, 0);
            mapListPanel.Margin = new Padding(4, 3, 4, 3);
            mapListPanel.Name = "mapListPanel";
            mapListPanel.Size = new Size(146, 598);
            mapListPanel.TabIndex = 0;
            // 
            // mapBuilderPanel
            // 
            mapBuilderPanel.Dock = DockStyle.Fill;
            mapBuilderPanel.Location = new Point(0, 0);
            mapBuilderPanel.Margin = new Padding(4, 3, 4, 3);
            mapBuilderPanel.Name = "mapBuilderPanel";
            mapBuilderPanel.Size = new Size(997, 598);
            mapBuilderPanel.TabIndex = 0;
            // 
            // menuStrip1
            // 
            menuStrip1.Items.AddRange(new ToolStripItem[] { fileToolStripMenuItem, toolsToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Padding = new Padding(7, 2, 0, 2);
            menuStrip1.Size = new Size(1148, 24);
            menuStrip1.TabIndex = 1;
            menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { saveToolStripMenuItem });
            fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            fileToolStripMenuItem.Size = new Size(37, 20);
            fileToolStripMenuItem.Text = "File";
            // 
            // saveToolStripMenuItem
            // 
            saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            saveToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.S;
            saveToolStripMenuItem.Size = new Size(138, 22);
            saveToolStripMenuItem.Text = "Save";
            saveToolStripMenuItem.Click += saveToolStripMenuItem_Click;
            // 
            // toolsToolStripMenuItem
            // 
            toolsToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { tilesetsManagerToolStripMenuItem, tilesetEditorToolStripMenuItem });
            toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            toolsToolStripMenuItem.Size = new Size(46, 20);
            toolsToolStripMenuItem.Text = "Tools";
            // 
            // tilesetEditorToolStripMenuItem
            // 
            tilesetEditorToolStripMenuItem.Name = "tilesetEditorToolStripMenuItem";
            tilesetEditorToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.T;
            tilesetEditorToolStripMenuItem.Size = new Size(181, 22);
            tilesetEditorToolStripMenuItem.Text = "Tileset Editor";
            tilesetEditorToolStripMenuItem.Click += tilesetEditorToolStripMenuItem_Click;
            // 
            // toolStrip1
            // 
            toolStrip1.BackColor = Color.Gainsboro;
            toolStrip1.Items.AddRange(new ToolStripItem[] { saveButton });
            toolStrip1.Location = new Point(0, 24);
            toolStrip1.Name = "toolStrip1";
            toolStrip1.Size = new Size(1148, 25);
            toolStrip1.TabIndex = 2;
            toolStrip1.Text = "toolStrip1";
            // 
            // saveButton
            // 
            saveButton.DisplayStyle = ToolStripItemDisplayStyle.Image;
            saveButton.Image = (Image)resources.GetObject("saveButton.Image");
            saveButton.ImageTransparentColor = Color.Magenta;
            saveButton.Name = "saveButton";
            saveButton.Size = new Size(23, 22);
            saveButton.Text = "toolStripButton1";
            saveButton.Click += saveButton_Click;
            // 
            // tilesetsManagerToolStripMenuItem
            // 
            tilesetsManagerToolStripMenuItem.Name = "tilesetsManagerToolStripMenuItem";
            tilesetsManagerToolStripMenuItem.Size = new Size(181, 22);
            tilesetsManagerToolStripMenuItem.Text = "Tilesets Manager";
            tilesetsManagerToolStripMenuItem.Click += tilesetsManagerToolStripMenuItem_Click;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1148, 647);
            Controls.Add(splitContainer1);
            Controls.Add(toolStrip1);
            Controls.Add(menuStrip1);
            MainMenuStrip = menuStrip1;
            Margin = new Padding(4, 3, 4, 3);
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Map Editor";
            FormClosing += MainForm_FormClosing;
            Load += MainForm_Load;
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            toolStrip1.ResumeLayout(false);
            toolStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private SplitContainer splitContainer1;
        private Panel mapListPanel;
        private Panel mapBuilderPanel;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem saveToolStripMenuItem;
        private ToolStrip toolStrip1;
        private ToolStripButton saveButton;
        private ToolStripMenuItem toolsToolStripMenuItem;
        private ToolStripMenuItem tilesetEditorToolStripMenuItem;
        private ToolStripMenuItem tilesetsManagerToolStripMenuItem;
    }
}

