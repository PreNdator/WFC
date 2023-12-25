namespace WFC
{
    partial class Form1
    {

        private System.ComponentModel.IContainer components = null;

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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            pictureBox1 = new PictureBox();
            StartScript = new Button();
            OneStep = new Button();
            ChangeFolder = new Button();
            ResetButton = new Button();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // pictureBox1
            // 
            pictureBox1.Location = new Point(12, 41);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(776, 426);
            pictureBox1.SizeMode = PictureBoxSizeMode.AutoSize;
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            // 
            // StartScript
            // 
            StartScript.Location = new Point(12, 12);
            StartScript.Name = "StartScript";
            StartScript.Size = new Size(75, 23);
            StartScript.TabIndex = 1;
            StartScript.Text = "Start";
            StartScript.UseVisualStyleBackColor = true;
            StartScript.Click += StartScript_Click;
            // 
            // OneStep
            // 
            OneStep.Location = new Point(93, 12);
            OneStep.Name = "OneStep";
            OneStep.Size = new Size(75, 23);
            OneStep.TabIndex = 2;
            OneStep.Text = "Step";
            OneStep.UseVisualStyleBackColor = true;
            OneStep.Click += OneStep_Click;
            // 
            // ChangeFolder
            // 
            ChangeFolder.Location = new Point(693, 12);
            ChangeFolder.Name = "ChangeFolder";
            ChangeFolder.Size = new Size(95, 23);
            ChangeFolder.TabIndex = 3;
            ChangeFolder.Text = "ChangeFolder";
            ChangeFolder.UseVisualStyleBackColor = true;
            ChangeFolder.Click += ChangeFolder_Click_1;
            // 
            // ResetButton
            // 
            ResetButton.Location = new Point(612, 12);
            ResetButton.Name = "ResetButton";
            ResetButton.Size = new Size(75, 23);
            ResetButton.TabIndex = 4;
            ResetButton.Text = "Reset";
            ResetButton.UseVisualStyleBackColor = true;
            ResetButton.Click += ResetButton_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(831, 488);
            Controls.Add(ResetButton);
            Controls.Add(ChangeFolder);
            Controls.Add(OneStep);
            Controls.Add(StartScript);
            Controls.Add(pictureBox1);
            Name = "Form1";
            Text = "Form1";
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox pictureBox1;
        private Button StartScript;
        private Button OneStep;
        private Button ChangeFolder;
        private Button ResetButton;
    }
}
