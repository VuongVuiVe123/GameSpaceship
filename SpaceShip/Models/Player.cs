using SpaceShip.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace SpaceShip
{
    public class Player : GameObject
    {
        public int Lives { get; set; } = 3;
        public float Speed { get; set; } = 6f;
        public bool MoveLeft { get; set; }
        public bool MoveRight { get; set; }
        public bool MoveUp { get; set; }
        public bool MoveDown { get; set; }
        public int ShootCooldown { get; private set; } = 0;
        public List<Bullet> Bullets { get; private set; } = new List<Bullet>();

        private int tripleShotTimer = 0;
        private const int TRIPLE_SHOT_DURATION = 600;
        public bool HasTripleShot => tripleShotTimer > 0;

        private int invincibleTimer = 0;

        private Image spriteSheet;
        private const int FRAME_W = 8;
        private const int FRAME_H = 8;
        private const int FRAME_LEFT = 0;
        private const int FRAME_STRAIGHT = 1;
        private const int FRAME_RIGHT = 2;
        private int currentFrame = FRAME_STRAIGHT;

        private Image fireSheet;
        private const int FIRE_TOTAL_FRAMES = 4;
        private int fireFrame = 0;
        private int fireTimer = 0;

        public Player(int screenWidth, int screenHeight)
        {
            Width = 48; Height = 48;
            X = screenWidth / 2 - Width / 2;
            Y = screenHeight - Height - 20;
            spriteSheet = Properties.Resources.ShipAnimation;
            fireSheet = Properties.Resources.Fire;
        }

        public void ActivateTripleShot()
        {
            tripleShotTimer = TRIPLE_SHOT_DURATION;
        }

        public void Shoot()
        {
            if (ShootCooldown <= 0)
            {
                float centerX = X + Width / 2f;
                float topY = Y - 10;

                if (HasTripleShot)
                {
                    Bullets.Add(new Bullet(centerX - 9, topY, -12f, true));

                    Bullets.Add(new AngledBullet(centerX - 9 - 10, topY, -12f, -2.5f, true));

                    Bullets.Add(new AngledBullet(centerX - 9 + 10, topY, -12f, 2.5f, true));
                }
                else
                {
                    Bullets.Add(new Bullet(centerX - 9, topY, -12f, true));
                }

                ShootCooldown = 15;
            }
        }

        public void TakeDamage()
        {
            if (invincibleTimer > 0) return;
            Lives--;
            invincibleTimer = 60;
            if (Lives <= 0) IsAlive = false;
        }

        public bool IsInvincible => invincibleTimer > 0;

        public float TripleShotSecondsLeft => tripleShotTimer / 60f;

        public override void Update()
        {
            if (MoveLeft && X > 0) X -= Speed;
            if (MoveRight && X < 1280 - Width) X += Speed;
            if (MoveUp && Y > 0) Y -= Speed;
            if (MoveDown && Y < 720 - Height) Y += Speed;

            if (MoveLeft && !MoveRight) currentFrame = FRAME_LEFT;
            else if (MoveRight && !MoveLeft) currentFrame = FRAME_RIGHT;
            else currentFrame = FRAME_STRAIGHT;

            if (ShootCooldown > 0) ShootCooldown--;
            if (invincibleTimer > 0) invincibleTimer--;
            if (tripleShotTimer > 0) tripleShotTimer--;

            fireTimer++;
            if (fireTimer >= 4) { fireFrame = (fireFrame + 1) % FIRE_TOTAL_FRAMES; fireTimer = 0; }

            foreach (var b in Bullets) b.Update();
            Bullets.RemoveAll(b => !b.IsAlive);
        }

        public override void Draw(Graphics g)
        {
            if (!IsAlive) return;
            if (IsInvincible && (invincibleTimer % 6 < 3)) goto drawBullets;

            if (spriteSheet != null)
            {
                Rectangle srcRect = new Rectangle(currentFrame * FRAME_W, 0, FRAME_W, FRAME_H);
                Rectangle dstRect = new Rectangle((int)X, (int)Y, Width, Height);

                var oldInterp = g.InterpolationMode;
                var oldPixel = g.PixelOffsetMode;
                g.InterpolationMode = InterpolationMode.NearestNeighbor;
                g.PixelOffsetMode = PixelOffsetMode.Half;
                g.DrawImage(spriteSheet, dstRect, srcRect, GraphicsUnit.Pixel);
                g.InterpolationMode = oldInterp;
                g.PixelOffsetMode = oldPixel;
            }

            if (fireSheet != null)
            {
                Rectangle fireSrc = new Rectangle(fireFrame * 8, 0, 2, 4);
                int fireW = Width / 3;
                int fireH = 32;
                int drawX = (int)Math.Round(X) + (Width - fireW) / 2 + 2;
                int drawY = (int)Math.Round(Y) + Height;
                Rectangle fireDst = new Rectangle(drawX, drawY, fireW, fireH);

                var oldInterp = g.InterpolationMode;
                var oldPixel = g.PixelOffsetMode;
                g.InterpolationMode = InterpolationMode.NearestNeighbor;
                g.PixelOffsetMode = PixelOffsetMode.Half;
                g.DrawImage(fireSheet, fireDst, fireSrc, GraphicsUnit.Pixel);
                g.InterpolationMode = oldInterp;
                g.PixelOffsetMode = oldPixel;
            }

        drawBullets:
            foreach (var b in Bullets) b.Draw(g);
        }
    }
}