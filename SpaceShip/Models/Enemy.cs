using System.Drawing;
using System.Drawing.Drawing2D;

namespace SpaceShip
{
    public class Enemy : GameObject
    {
        public int Type { get; set; }
        public int Health { get; set; }
        public int MaxHealth { get; set; }

        private Image sprite;
        private int srcW, srcH;

        private static System.Random rnd = new System.Random();

        public Enemy(float x, float y, int type)
        {
            Type = type; X = x; Y = y;

            switch (type)
            {
                case 0:
                    Width = 36; Height = 36;
                    Health = MaxHealth = 1;
                    sprite = Properties.Resources.Enimi; srcW = 6; srcH = 6;
                    break;
                case 1:
                    Width = 48; Height = 36;
                    Health = MaxHealth = 3;
                    sprite = Properties.Resources.Enimi2; srcW = 8; srcH = 6;
                    break;
                case 2: // BOSS
                    Width = 140;
                    Height = 160;
                    Health = MaxHealth = 50;
                    sprite = Properties.Resources.Enimi3;
                    srcW = 14; srcH = 16;
                    break;
            }
        }

        public bool ShouldDropPowerUp() => rnd.NextDouble() < 0.10;
        public bool ShouldDropHeart() => rnd.NextDouble() < 0.05;

        public bool TakeDamage()
        {
            Health--;
            if (Health <= 0) { IsAlive = false; return true; }
            return false;
        }

        public override void Update() { }

        public override void Draw(Graphics g)
        {
            if (!IsAlive) return;

            var oldInterp = g.InterpolationMode;
            var oldPixel = g.PixelOffsetMode;
            g.InterpolationMode = InterpolationMode.NearestNeighbor;
            g.PixelOffsetMode = PixelOffsetMode.Half;

            Rectangle srcRect = new Rectangle(0, 0, srcW, srcH);
            Rectangle dstRect = new Rectangle((int)X, (int)Y, Width, Height);
            g.DrawImage(sprite, dstRect, srcRect, GraphicsUnit.Pixel);

            g.InterpolationMode = oldInterp;
            g.PixelOffsetMode = oldPixel;
        }
    }
}