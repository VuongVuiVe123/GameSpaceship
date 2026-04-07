namespace SpaceShip
{
    partial class Defeat
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
            this.btnReplay = new System.Windows.Forms.Button();
            this.btnMenu = new System.Windows.Forms.Button();

            this.SuspendLayout();

            // ── btnReplay ──────────────────────────────
            this.btnReplay.BackColor = System.Drawing.Color.Transparent;
            this.btnReplay.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnReplay.FlatAppearance.BorderColor = System.Drawing.Color.Cyan;
            this.btnReplay.FlatAppearance.BorderSize = 2;
            this.btnReplay.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(60, 0, 255, 255);
            this.btnReplay.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(120, 0, 255, 255);
            this.btnReplay.Font = new System.Drawing.Font("Courier New", 14F, System.Drawing.FontStyle.Bold);
            this.btnReplay.ForeColor = System.Drawing.Color.Cyan;
            this.btnReplay.Location = new System.Drawing.Point(390, 450);
            this.btnReplay.Name = "btnReplay";
            this.btnReplay.Size = new System.Drawing.Size(220, 55);
            this.btnReplay.TabIndex = 0;
            this.btnReplay.Text = "▶  PLAY AGAIN";
            this.btnReplay.UseVisualStyleBackColor = false;
            this.btnReplay.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnReplay.Click += new System.EventHandler(this.btnReplay_Click);

            // ── btnMenu ────────────────────────────────
            this.btnMenu.BackColor = System.Drawing.Color.Transparent;
            this.btnMenu.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMenu.FlatAppearance.BorderColor = System.Drawing.Color.OrangeRed;
            this.btnMenu.FlatAppearance.BorderSize = 2;
            this.btnMenu.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(60, 255, 69, 0);
            this.btnMenu.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(120, 255, 69, 0);
            this.btnMenu.Font = new System.Drawing.Font("Courier New", 14F, System.Drawing.FontStyle.Bold);
            this.btnMenu.ForeColor = System.Drawing.Color.OrangeRed;
            this.btnMenu.Location = new System.Drawing.Point(670, 450);
            this.btnMenu.Name = "btnMenu";
            this.btnMenu.Size = new System.Drawing.Size(220, 55);
            this.btnMenu.TabIndex = 1;
            this.btnMenu.Text = "⌂  MENU";
            this.btnMenu.UseVisualStyleBackColor = false;
            this.btnMenu.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnMenu.Click += new System.EventHandler(this.btnMenu_Click);

            // ── Defeat Form ────────────────────────────
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(1280, 720);
            this.Controls.Add(this.btnReplay);
            this.Controls.Add(this.btnMenu);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Defeat";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.TransparencyKey = System.Drawing.Color.Black;
            this.Opacity = 0.92;

            this.ResumeLayout(false);
        }

        private System.Windows.Forms.Button btnReplay;
        private System.Windows.Forms.Button btnMenu;
    }
}