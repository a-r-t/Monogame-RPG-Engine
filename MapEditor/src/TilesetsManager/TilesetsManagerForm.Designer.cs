namespace MapEditor.src.TilesetEditor
{
    partial class TilesetsManagerForm
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
            tilesetsListBox = new ListBox();
            tilesetImagePreviewPanel = new Panel();
            tilesetPreviewPictureBox = new PictureBox();
            deleteTilesetButton = new Button();
            createTilesetButton = new Button();
            editTilesetNameButton = new Button();
            tilesetImagePreviewPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)tilesetPreviewPictureBox).BeginInit();
            SuspendLayout();
            // 
            // tilesetsListBox
            // 
            tilesetsListBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            tilesetsListBox.FormattingEnabled = true;
            tilesetsListBox.ItemHeight = 15;
            tilesetsListBox.Location = new Point(8, 12);
            tilesetsListBox.Name = "tilesetsListBox";
            tilesetsListBox.Size = new Size(173, 334);
            tilesetsListBox.TabIndex = 0;
            // 
            // tilesetImagePreviewPanel
            // 
            tilesetImagePreviewPanel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            tilesetImagePreviewPanel.Controls.Add(tilesetPreviewPictureBox);
            tilesetImagePreviewPanel.Location = new Point(187, 12);
            tilesetImagePreviewPanel.Name = "tilesetImagePreviewPanel";
            tilesetImagePreviewPanel.Size = new Size(698, 466);
            tilesetImagePreviewPanel.TabIndex = 1;
            // 
            // tilesetPreviewPictureBox
            // 
            tilesetPreviewPictureBox.Location = new Point(3, 3);
            tilesetPreviewPictureBox.Name = "tilesetPreviewPictureBox";
            tilesetPreviewPictureBox.Size = new Size(100, 50);
            tilesetPreviewPictureBox.TabIndex = 0;
            tilesetPreviewPictureBox.TabStop = false;
            // 
            // deleteTilesetButton
            // 
            deleteTilesetButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            deleteTilesetButton.Location = new Point(8, 442);
            deleteTilesetButton.Name = "deleteTilesetButton";
            deleteTilesetButton.Size = new Size(173, 36);
            deleteTilesetButton.TabIndex = 2;
            deleteTilesetButton.Text = "Delete Tileset";
            deleteTilesetButton.UseVisualStyleBackColor = true;
            deleteTilesetButton.Click += deleteTilesetButton_Click;
            // 
            // createTilesetButton
            // 
            createTilesetButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            createTilesetButton.Location = new Point(8, 358);
            createTilesetButton.Name = "createTilesetButton";
            createTilesetButton.Size = new Size(173, 36);
            createTilesetButton.TabIndex = 3;
            createTilesetButton.Text = "Create New Tileset";
            createTilesetButton.UseVisualStyleBackColor = true;
            createTilesetButton.Click += createTilesetButton_Click;
            // 
            // editTilesetNameButton
            // 
            editTilesetNameButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            editTilesetNameButton.Location = new Point(8, 400);
            editTilesetNameButton.Name = "editTilesetNameButton";
            editTilesetNameButton.Size = new Size(173, 36);
            editTilesetNameButton.TabIndex = 4;
            editTilesetNameButton.Text = "Edit Tileset Name";
            editTilesetNameButton.UseVisualStyleBackColor = true;
            editTilesetNameButton.Click += editTilesetNameButton_Click;
            // 
            // TilesetsManagerForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(897, 490);
            Controls.Add(editTilesetNameButton);
            Controls.Add(createTilesetButton);
            Controls.Add(deleteTilesetButton);
            Controls.Add(tilesetImagePreviewPanel);
            Controls.Add(tilesetsListBox);
            Name = "TilesetsManagerForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Tilesets Manager";
            tilesetImagePreviewPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)tilesetPreviewPictureBox).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private ListBox tilesetsListBox;
        private Panel tilesetImagePreviewPanel;
        private Button deleteTilesetButton;
        private Button createTilesetButton;
        private PictureBox tilesetPreviewPictureBox;
        private Button editTilesetNameButton;
    }
}