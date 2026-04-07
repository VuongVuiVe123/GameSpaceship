using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace SpaceShip
{
    public partial class Defeat : Form
    {
        public Defeat()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;

            // Lop nen mo
            using (var bg = new SolidBrush(Color.FromArgb(190, 0, 0, 0)))
                g.FillRectangle(bg, ClientRectangle);

            int boxW = 600, boxH = 400;
            int boxX = (ClientSize.Width - boxW) / 2;
            int boxY = (ClientSize.Height - boxH) / 2;
            Rectangle box = new Rectangle(boxX, boxY, boxW, boxH);

            using (var boxBg = new SolidBrush(Color.FromArgb(220, 30, 5, 5)))
                g.FillRectangle(boxBg, box);

            using (var glow1 = new Pen(Color.FromArgb(80, 220, 30, 30), 8))
                g.DrawRectangle(glow1, box);
            using (var glow2 = new Pen(Color.FromArgb(200, 200, 40, 40), 2))
                g.DrawRectangle(glow2, box);

            string title = "DEFEAT";
            using (var font = new Font("Courier New", 52f, FontStyle.Bold, GraphicsUnit.Point))
            {
                SizeF sz = g.MeasureString(title, font);
                float tx = boxX + (boxW - sz.Width) / 2f;
                float ty = boxY + 32f;

                using (var glowBrush = new SolidBrush(Color.FromArgb(90, 220, 30, 30)))
                using (var glowFont = new Font("Courier New", 54f, FontStyle.Bold, GraphicsUnit.Point))
                {
                    SizeF gs = g.MeasureString(title, glowFont);
                    g.DrawString(title, glowFont, glowBrush,
                        boxX + (boxW - gs.Width) / 2f - 2, ty - 2);
                }

                using (var brush = new SolidBrush(Color.OrangeRed))
                    g.DrawString(title, font, brush, tx, ty);
            }

            string sub = "Your ship has been destroyed!";
            using (var font = new Font("Courier New", 13f, FontStyle.Regular, GraphicsUnit.Point))
            {
                SizeF sz = g.MeasureString(sub, font);
                using (var brush = new SolidBrush(Color.FromArgb(200, 200, 180, 180)))
                    g.DrawString(sub, font, brush,
                        boxX + (boxW - sz.Width) / 2f,
                        boxY + 130f);
            }

            string skull = "---  X  X  X  ---";
            using (var font = new Font("Courier New", 20f, FontStyle.Bold, GraphicsUnit.Point))
            using (var brush = new SolidBrush(Color.OrangeRed))
            {
                SizeF sz = g.MeasureString(skull, font);
                g.DrawString(skull, font, brush,
                    boxX + (boxW - sz.Width) / 2f,
                    boxY + 195f);
            }
        }

        private void btnReplay_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Retry;
            this.Close();
        }

        private void btnMenu_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}