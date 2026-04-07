namespace SpaceShip
{
    partial class Menu
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            pbStart = new PictureBox();
            pbTutorial = new PictureBox();
            pbExit = new PictureBox();
            pictureBox1 = new PictureBox();
            ((System.ComponentModel.ISupportInitialize)pbStart).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pbTutorial).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pbExit).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // pbStart
            // 
            pbStart.BackColor = Color.Transparent;
            pbStart.Cursor = Cursors.Hand;
            pbStart.Image = Properties.Resources.StartButton;
            pbStart.Location = new Point(490, 410);
            pbStart.Name = "pbStart";
            pbStart.Size = new Size(300, 60);
            pbStart.SizeMode = PictureBoxSizeMode.Zoom;
            pbStart.TabIndex = 0;
            pbStart.TabStop = false;
            pbStart.Click += pbStart_Click;
            pbStart.MouseEnter += PictureBox_MouseEnter;
            pbStart.MouseLeave += PictureBox_MouseLeave;
            // 
            // pbTutorial
            // 
            pbTutorial.BackColor = Color.Transparent;
            pbTutorial.Cursor = Cursors.Hand;
            pbTutorial.Image = Properties.Resources.TutorialButton;
            pbTutorial.Location = new Point(490, 500);
            pbTutorial.Name = "pbTutorial";
            pbTutorial.Size = new Size(300, 60);
            pbTutorial.SizeMode = PictureBoxSizeMode.Zoom;
            pbTutorial.TabIndex = 1;
            pbTutorial.TabStop = false;
            pbTutorial.Click += pbTutorial_Click;
            pbTutorial.MouseEnter += PictureBox_MouseEnter;
            pbTutorial.MouseLeave += PictureBox_MouseLeave;
            // 
            // pbExit
            // 
            pbExit.BackColor = Color.Transparent;
            pbExit.Cursor = Cursors.Hand;
            pbExit.Image = Properties.Resources.ExitButton;
            pbExit.Location = new Point(490, 590);
            pbExit.Name = "pbExit";
            pbExit.Size = new Size(300, 60);
            pbExit.SizeMode = PictureBoxSizeMode.Zoom;
            pbExit.TabIndex = 2;
            pbExit.TabStop = false;
            pbExit.Click += pbExit_Click;
            pbExit.MouseEnter += PictureBox_MouseEnter;
            pbExit.MouseLeave += PictureBox_MouseLeave;
            // 
            // pictureBox1
            // 
            pictureBox1.BackColor = Color.Transparent;
            pictureBox1.Image = Properties.Resources.Logo;
            pictureBox1.Location = new Point(310, 50);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(687, 326);
            pictureBox1.TabIndex = 3;
            pictureBox1.TabStop = false;
            // 
            // Menu
            // 
            AutoScaleMode = AutoScaleMode.None;
            BackColor = Color.Black;
            ClientSize = new Size(1280, 720);
            Controls.Add(pictureBox1);
            Controls.Add(pbStart);
            Controls.Add(pbTutorial);
            Controls.Add(pbExit);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "Menu";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Space Battleship";
            ((System.ComponentModel.ISupportInitialize)pbStart).EndInit();
            ((System.ComponentModel.ISupportInitialize)pbTutorial).EndInit();
            ((System.ComponentModel.ISupportInitialize)pbExit).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
        }

        private System.Windows.Forms.PictureBox pbStart;
        private System.Windows.Forms.PictureBox pbTutorial;
        private System.Windows.Forms.PictureBox pbExit;
        private PictureBox pictureBox1;
    }
}