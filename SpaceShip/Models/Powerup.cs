using System.Drawing;
using System.Drawing.Drawing2D;

namespace SpaceShip
{
    public class PowerUp : GameObject
    {
        public enum PowerUpType { TripleShot, HeartDrop }
        public PowerUpType Type { get; set; }

        private static Image spriteTriple = Properties.Resources.updrage;
        private static Image spriteHeart = Properties.Resources.heart;

        private float speedY = 2.5f;
        private float bobOffset = 0f;
        private int bobTimer = 0;

        public PowerUp(float x, float y)
        {
            Type = PowerUpType.TripleShot;
            Width = 24; Height = 24;
            X = x - Width / 2f;
            Y = y;
        }

        public PowerUp(float x, float y, PowerUpType type)
        {
            Type = type;
            Width = 24; Height = 24;
            X = x - Width / 2f;
            Y = y;
        }

        public override void Update()
        {
            Y += speedY;
            bobTimer++;
            bobOffset = (float)System.Math.Sin(bobTimer * 0.1f) * 3f;
            if (Y > 740) IsAlive = false;
        }

        public override void Draw(Graphics g)
        {
            if (!IsAlive) return;

            Color glowColor = (Type == PowerUpType.HeartDrop)
                ? Color.FromArgb(70, 255, 60, 60)
                : Color.FromArgb(60, 0, 255, 120);

            using (var glow = new SolidBrush(glowColor))
                g.FillEllipse(glow, X - 6, Y + bobOffset - 6, Width + 12, Height + 12);

            Image sprite = (Type == PowerUpType.HeartDrop) ? spriteHeart : spriteTriple;
            Rectangle src = new Rectangle(0, 0, sprite.Width, sprite.Height);
            Rectangle dst = new Rectangle((int)X + 1, (int)(Y + bobOffset), Width, Height);

            var oldInterp = g.InterpolationMode;
            var oldPixel = g.PixelOffsetMode;
            g.InterpolationMode = InterpolationMode.NearestNeighbor;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
            g.DrawImage(sprite, dst, src, GraphicsUnit.Pixel);
            g.InterpolationMode = oldInterp;
            g.PixelOffsetMode = oldPixel;
        }
    }
}