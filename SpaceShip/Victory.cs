using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace SpaceShip
{
    public partial class Victory : Form
    {
        public Victory()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;

            using (var bg = new SolidBrush(Color.FromArgb(170, 0, 0, 0)))
                g.FillRectangle(bg, ClientRectangle);

            int boxW = 600, boxH = 380;
            int boxX = (ClientSize.Width - boxW) / 2;
            int boxY = (ClientSize.Height - boxH) / 2;
            Rectangle box = new Rectangle(boxX, boxY, boxW, boxH);

            using (var boxBg = new SolidBrush(Color.FromArgb(210, 10, 10, 30)))
                g.FillRectangle(boxBg, box);

            using (var glow1 = new Pen(Color.FromArgb(80, 255, 215, 0), 8))
                g.DrawRectangle(glow1, box);
            using (var glow2 = new Pen(Color.FromArgb(180, 255, 215, 0), 2))
                g.DrawRectangle(glow2, box);

            string title = "VICTORY!";
            using (var font = new Font("Courier New", 48f, FontStyle.Bold, GraphicsUnit.Point))
            {
                SizeF sz = g.MeasureString(title, font);
                float tx = boxX + (boxW - sz.Width) / 2f;
                float ty = boxY + 40f;

                using (var glowBrush = new SolidBrush(Color.FromArgb(80, 255, 215, 0)))
                using (var glowFont = new Font("Courier New", 50f, FontStyle.Bold, GraphicsUnit.Point))
                {
                    SizeF gs = g.MeasureString(title, glowFont);
                    g.DrawString(title, glowFont, glowBrush,
                        boxX + (boxW - gs.Width) / 2f - 2, ty - 2);
                }

                using (var brush = new SolidBrush(Color.Gold))
                    g.DrawString(title, font, brush, tx, ty);
            }

            string sub = "You defeated the Boss!";
            using (var font = new Font("Courier New", 14f, FontStyle.Regular, GraphicsUnit.Point))
            {
                SizeF sz = g.MeasureString(sub, font);
                using (var brush = new SolidBrush(Color.FromArgb(200, 200, 200, 200)))
                    g.DrawString(sub, font, brush,
                        boxX + (boxW - sz.Width) / 2f,
                        boxY + 130f);
            }

            DrawStars(g, boxX, boxY, boxW);
        }

        private void DrawStars(Graphics g, int boxX, int boxY, int boxW)
        {
            string stars = "★  ★  ★";
            using (var font = new Font("Segoe UI", 28f, FontStyle.Bold, GraphicsUnit.Point))
            using (var brush = new SolidBrush(Color.Gold))
            {
                SizeF sz = g.MeasureString(stars, font);
                g.DrawString(stars, font, brush,
                    boxX + (boxW - sz.Width) / 2f,
                    boxY + 165f);
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