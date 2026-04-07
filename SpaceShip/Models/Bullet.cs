using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace SpaceShip.Models
{
    //Class Bullet
    public class Bullet : GameObject
    {
        public float SpeedY { get; set; }
        public bool IsPlayerBullet { get; set; }

        private static Image spritePlayer = Properties.Resources.bulletplayer;
        private static Image spriteEnemy = Properties.Resources.bulletEnemy;

        public Bullet(float x, float y, float speedY, bool isPlayerBullet)
        {
            IsPlayerBullet = isPlayerBullet;
            SpeedY = speedY;

            if (isPlayerBullet) { Width = 3; Height = 6; }
            else { Width = 6; Height = 6; }

            X = x - Width / 2f;
            Y = y;
        }

        public override void Update()
        {
            Y += SpeedY;
            if (Y < -20 || Y > 720) IsAlive = false;
        }

        public override void Draw(Graphics g)
        {
            if (!IsAlive) return;

            Image sprite = IsPlayerBullet ? spritePlayer : spriteEnemy;

            Rectangle src, dst;
            if (IsPlayerBullet)
            {
                src = new Rectangle(0, 0, Width, Height);
                dst = new Rectangle((int)X, (int)Y, Width * 6, Height * 6);
            }
            else
            {
                src = new Rectangle(0, 0, Width, Height);
                dst = new Rectangle((int)X, (int)Y, Width * 4, Height * 4);
            }

            var oldInterp = g.InterpolationMode;
            var oldPixel = g.PixelOffsetMode;
            g.InterpolationMode = InterpolationMode.NearestNeighbor;
            g.PixelOffsetMode = PixelOffsetMode.Half;
            g.DrawImage(sprite, dst, src, GraphicsUnit.Pixel);
            g.InterpolationMode = oldInterp;
            g.PixelOffsetMode = oldPixel;
        }
    }

    //Dan 3 tia
    public class AngledBullet : Bullet
    {
        public float SpeedX { get; set; }

        public AngledBullet(float x, float y, float speedY, float speedX, bool isPlayerBullet)
            : base(x, y, speedY, isPlayerBullet)
        {
            SpeedX = speedX;
        }

        public override void Update()
        {
            X += SpeedX;
            Y += SpeedY;
            if (Y < -20 || Y > 720 || X < -20 || X > 1300) IsAlive = false;
        }
    }

    //Dan Duoi
    public class HomingBullet : GameObject
    {
        private static Image sprite = Properties.Resources.bulletEnemy2;

        private float speedX;
        private float speedY;
        private float speed = 4f;
        private float angle;

        private const int DRAW_SIZE = 40;
        private const int HIT_SIZE = 28;

        public HomingBullet(float x, float y)
        {
            Width = HIT_SIZE;
            Height = HIT_SIZE;
            X = x - Width / 2f;
            Y = y - Height / 2f;
            speedX = 0;
            speedY = speed;
        }

        public void UpdateHoming(float playerCenterX, float playerCenterY)
        {
            float cx = X + Width / 2f;
            float cy = Y + Height / 2f;

            float dx = playerCenterX - cx;
            float dy = playerCenterY - cy;
            float dist = (float)Math.Sqrt(dx * dx + dy * dy);

            if (dist > 0)
            {
                speedX = (dx / dist) * speed;
                speedY = (dy / dist) * speed;
                angle = (float)Math.Atan2(dy, dx)
                       - (float)(Math.PI / 2)
                       + (float)(135 * Math.PI / 180);
            }

            X += speedX;
            Y += speedY;

            if (Y > 740 || Y < -20 || X < -20 || X > 1300) IsAlive = false;
        }

        public override void Update() {}

        public override void Draw(Graphics g)
        {
            if (!IsAlive) return;

            float cx = X + Width / 2f;
            float cy = Y + Height / 2f;

            var oldInterp = g.InterpolationMode;
            var oldPixel = g.PixelOffsetMode;
            var oldTransform = g.Transform;

            g.InterpolationMode = InterpolationMode.NearestNeighbor;
            g.PixelOffsetMode = PixelOffsetMode.Half;

            g.TranslateTransform(cx, cy);
            g.RotateTransform(angle * 180f / (float)Math.PI);
            g.TranslateTransform(-DRAW_SIZE / 2f, -DRAW_SIZE / 2f);

            Rectangle src = new Rectangle(0, 0, sprite.Width, sprite.Height);
            Rectangle dst = new Rectangle(0, 0, DRAW_SIZE, DRAW_SIZE);
            g.DrawImage(sprite, dst, src, GraphicsUnit.Pixel);

            g.Transform = oldTransform;
            g.InterpolationMode = oldInterp;
            g.PixelOffsetMode = oldPixel;
        }
    }
 
    //  BossBullet 
    public class BossBullet : GameObject
    {
        private static Image sprite = Properties.Resources.BossBullet;

        private float speedX;
        private float speedY;
        private float angle;

        private const int DRAW_SIZE = 24;
        private const int HIT_SIZE = 16;

        public BossBullet(float x, float y, float dirX, float dirY, float speed = 6f)
        {
            Width = HIT_SIZE;
            Height = HIT_SIZE;
            X = x - Width / 2f;
            Y = y - Height / 2f;

            float len = (float)Math.Sqrt(dirX * dirX + dirY * dirY);
            if (len > 0) { dirX /= len; dirY /= len; }

            speedX = dirX * speed;
            speedY = dirY * speed;
            angle = (float)Math.Atan2(dirY, dirX) + (float)(Math.PI / 2);
        }

        public override void Update()
        {
            X += speedX;
            Y += speedY;
            if (Y > 760 || Y < -40 || X < -40 || X > 1320) IsAlive = false;
        }

        public override void Draw(Graphics g)
        {
            if (!IsAlive) return;

            float cx = X + Width / 2f;
            float cy = Y + Height / 2f;

            var oldInterp = g.InterpolationMode;
            var oldPixel = g.PixelOffsetMode;
            var oldTransform = g.Transform;

            g.InterpolationMode = InterpolationMode.NearestNeighbor;
            g.PixelOffsetMode = PixelOffsetMode.Half;

            g.TranslateTransform(cx, cy);
            g.RotateTransform(angle * 180f / (float)Math.PI);
            g.TranslateTransform(-DRAW_SIZE / 2f, -DRAW_SIZE / 2f);

            Rectangle src = new Rectangle(0, 0, sprite.Width, sprite.Height);
            Rectangle dst = new Rectangle(0, 0, DRAW_SIZE, DRAW_SIZE);
            g.DrawImage(sprite, dst, src, GraphicsUnit.Pixel);

            g.Transform = oldTransform;
            g.InterpolationMode = oldInterp;
            g.PixelOffsetMode = oldPixel;
        }
    }
}