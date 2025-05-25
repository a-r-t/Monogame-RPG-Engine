namespace MapEditor.src.TilesetsManager
{
    partial class CreateOrEditTilesetForm
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
            label1 = new Label();
            nameTextBox = new TextBox();
            label2 = new Label();
            widthTextBox = new NumericUpDown();
            label3 = new Label();
            heightTextBox = new NumericUpDown();
            label4 = new Label();
            scaleTextBox = new NumericUpDown();
            label5 = new Label();
            imageTextBox = new TextBox();
            submitButton = new Button();
            cancelButton = new Button();
            errorMessage = new Label();
            ((System.ComponentModel.ISupportInitialize)widthTextBox).BeginInit();
            ((System.ComponentModel.ISupportInitialize)heightTextBox).BeginInit();
            ((System.ComponentModel.ISupportInitialize)scaleTextBox).BeginInit();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 24);
            label1.Name = "label1";
            label1.Size = new Size(42, 15);
            label1.TabIndex = 0;
            label1.Text = "Name:";
            // 
            // nameTextBox
            // 
            nameTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            nameTextBox.Location = new Point(60, 21);
            nameTextBox.Name = "nameTextBox";
            nameTextBox.Size = new Size(199, 23);
            nameTextBox.TabIndex = 1;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(12, 62);
            label2.Name = "label2";
            label2.Size = new Size(42, 15);
            label2.TabIndex = 2;
            label2.Text = "Width:";
            // 
            // widthTextBox
            // 
            widthTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            widthTextBox.Location = new Point(60, 60);
            widthTextBox.Name = "widthTextBox";
            widthTextBox.Size = new Size(199, 23);
            widthTextBox.TabIndex = 4;
            widthTextBox.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(12, 101);
            label3.Name = "label3";
            label3.Size = new Size(46, 15);
            label3.TabIndex = 5;
            label3.Text = "Height:";
            // 
            // heightTextBox
            // 
            heightTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            heightTextBox.Location = new Point(60, 99);
            heightTextBox.Name = "heightTextBox";
            heightTextBox.Size = new Size(199, 23);
            heightTextBox.TabIndex = 6;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(12, 179);
            label4.Name = "label4";
            label4.Size = new Size(40, 15);
            label4.TabIndex = 7;
            label4.Text = "Image";
            // 
            // scaleTextBox
            // 
            scaleTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            scaleTextBox.Location = new Point(60, 140);
            scaleTextBox.Name = "scaleTextBox";
            scaleTextBox.Size = new Size(199, 23);
            scaleTextBox.TabIndex = 9;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(12, 142);
            label5.Name = "label5";
            label5.Size = new Size(37, 15);
            label5.TabIndex = 8;
            label5.Text = "Scale:";
            // 
            // imageTextBox
            // 
            imageTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            imageTextBox.Location = new Point(60, 176);
            imageTextBox.Name = "imageTextBox";
            imageTextBox.Size = new Size(199, 23);
            imageTextBox.TabIndex = 10;
            // 
            // submitButton
            // 
            submitButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            submitButton.Location = new Point(138, 215);
            submitButton.Name = "submitButton";
            submitButton.Size = new Size(120, 25);
            submitButton.TabIndex = 12;
            submitButton.Text = "Submit";
            submitButton.UseVisualStyleBackColor = true;
            submitButton.Click += submitButton_Click;
            // 
            // cancelButton
            // 
            cancelButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            cancelButton.Location = new Point(12, 215);
            cancelButton.Name = "cancelButton";
            cancelButton.Size = new Size(120, 25);
            cancelButton.TabIndex = 13;
            cancelButton.Text = "Cancel";
            cancelButton.UseVisualStyleBackColor = true;
            cancelButton.Click += cancelButton_Click;
            // 
            // errorMessage
            // 
            errorMessage.AutoSize = true;
            errorMessage.ForeColor = Color.Red;
            errorMessage.Location = new Point(16, 249);
            errorMessage.Name = "errorMessage";
            errorMessage.Size = new Size(81, 15);
            errorMessage.TabIndex = 14;
            errorMessage.Text = "Error Message";
            errorMessage.Visible = false;
            // 
            // CreateTilesetForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(271, 273);
            Controls.Add(errorMessage);
            Controls.Add(cancelButton);
            Controls.Add(submitButton);
            Controls.Add(imageTextBox);
            Controls.Add(scaleTextBox);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(heightTextBox);
            Controls.Add(label3);
            Controls.Add(widthTextBox);
            Controls.Add(label2);
            Controls.Add(nameTextBox);
            Controls.Add(label1);
            Name = "CreateTilesetForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Create Tileset";
            ((System.ComponentModel.ISupportInitialize)widthTextBox).EndInit();
            ((System.ComponentModel.ISupportInitialize)heightTextBox).EndInit();
            ((System.ComponentModel.ISupportInitialize)scaleTextBox).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private TextBox nameTextBox;
        private Label label2;
        private NumericUpDown widthTextBox;
        private Label label3;
        private NumericUpDown heightTextBox;
        private Label label4;
        private NumericUpDown scaleTextBox;
        private Label label5;
        private TextBox imageTextBox;
        private Button submitButton;
        private Button cancelButton;
        private Label errorMessage;
    }
}