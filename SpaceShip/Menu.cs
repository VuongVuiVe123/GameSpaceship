using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace SpaceShip
{
    public partial class Menu : Form
    {
        // Thêm ảnh background vào Resources tên "menuBG"
        private Image background = Properties.Resources.menuBG;

        public Menu()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
        }

        // ── Vẽ background + logo ───────────────────────
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;

            if (background != null)
                g.DrawImage(background, 0, 0, ClientSize.Width, ClientSize.Height);

            using (var overlay = new SolidBrush(Color.FromArgb(140, 0, 0, 0)))
                g.FillRectangle(overlay, ClientRectangle);
        }

        

        // ── Hiệu ứng hover: phóng to nhẹ ─────────────
        private void PictureBox_MouseEnter(object sender, EventArgs e)
        {
            var pb = (PictureBox)sender;
            pb.Location = new Point(pb.Location.X - 6, pb.Location.Y - 3);
            pb.Size = new Size(312, 66);
        }

        private void PictureBox_MouseLeave(object sender, EventArgs e)
        {
            var pb = (PictureBox)sender;
            pb.Size = new Size(300, 60);
            // Trả về vị trí gốc theo tên
            switch (pb.Name)
            {
                case "pbStart": pb.Location = new Point(490, 410); break;
                case "pbTutorial": pb.Location = new Point(490, 500); break;
                case "pbExit": pb.Location = new Point(490, 590); break;
            }
        }

        // ── Sự kiện click ─────────────────────────────
        private void pbStart_Click(object sender, EventArgs e)
        {
            var game = new Form1();
            game.FormClosed += (s, args) => this.Show();
            this.Hide();
            game.Show();
        }

        private void pbTutorial_Click(object sender, EventArgs e)
        {
            MessageBox.Show(
                "🎮  HƯỚNG DẪN CHƠI\n\n" +
                "← → ↑ ↓     Di chuyển tàu\n" +
                "⬆  PowerUp  →  Triple Shot (10 giây)\n" +
                "❤️  PowerUp   →  Hồi 1 mạng\n\n" +
                "Tiêu diệt tất cả kẻ thù để qua màn!\n" +
                "chúc may mắn!",
                "Tutorial",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        private void pbExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}