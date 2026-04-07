using SpaceShip.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace SpaceShip
{
    public partial class Form1 : Form
    {
        public Player player;
        public EnemyWave currentWave;
        public List<Explosion> explosions = new List<Explosion>();
        public List<Star> stars = new List<Star>();

        public enum GameState { Playing, Paused, GameOver, Win }
        public GameState state = GameState.Playing;
        public int waveNumber = 1;

        private bool keyLeft, keyRight, keyUp, keyDown;

        public int frameCount = 0;
        private Bitmap backBuffer;
        private Graphics backGraphics;

        public const int SCREEN_W = 1280;
        public const int SCREEN_H = 720;
        private const int MAX_WAVES = 2;

        public Form1()
        {
            InitializeComponent();

            backBuffer = new Bitmap(SCREEN_W, SCREEN_H);
            backGraphics = Graphics.FromImage(backBuffer);
            backGraphics.SmoothingMode = SmoothingMode.AntiAlias;

            for (int i = 0; i < 120; i++)
                stars.Add(new Star(SCREEN_W, SCREEN_H));

            gameTimer.Start();

            StartGame();
        }

        public void StartGame()
        {
            player = new Player(SCREEN_W, SCREEN_H);
            currentWave = new EnemyWave(waveNumber);
            explosions.Clear();
            state = GameState.Playing;
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left) keyLeft = true;
            if (e.KeyCode == Keys.Right) keyRight = true;
            if (e.KeyCode == Keys.Up) keyUp = true;
            if (e.KeyCode == Keys.Down) keyDown = true;

            if (state == GameState.GameOver && e.KeyCode == Keys.Enter)
            { waveNumber = 1; StartGame(); }
            if (state == GameState.Win && e.KeyCode == Keys.Enter)
            { waveNumber = 1; StartGame(); }
            if (state == GameState.Playing && e.KeyCode == Keys.Escape)
                state = GameState.Paused;
            else if (state == GameState.Paused && e.KeyCode == Keys.Escape)
                state = GameState.Playing;
        }

        private void OnKeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left) keyLeft = false;
            if (e.KeyCode == Keys.Right) keyRight = false;
            if (e.KeyCode == Keys.Up) keyUp = false;
            if (e.KeyCode == Keys.Down) keyDown = false;
        }

        private void GameLoop(object sender, EventArgs e)
        {
            if (state == GameState.Playing) Update();
            this.Invalidate();
        }

        private void Update()
        {
            frameCount++;

            foreach (var s in stars) s.Update(SCREEN_H);

            player.MoveLeft = keyLeft;
            player.MoveRight = keyRight;
            player.MoveUp = keyUp;
            player.MoveDown = keyDown;
            player.Shoot();
            player.Update();

            currentWave.Update(SCREEN_W, player.X + player.Width / 2f, player.Y + player.Height / 2f);

            foreach (var ex in explosions) ex.Update();
            explosions.RemoveAll(ex => !ex.IsAlive);

            // Đạn player vs kẻ thù
            foreach (var bullet in player.Bullets)
            {
                if (!bullet.IsAlive) continue;

                foreach (var homing in currentWave.HomingBullets)
                {
                    if (!homing.IsAlive) continue;
                    if (bullet.CollidesWith(homing))
                    {
                        bullet.IsAlive = false;
                        homing.IsAlive = false;
                        explosions.Add(new Explosion(homing.X, homing.Y, 12, Color.Magenta));
                    }
                }

                foreach (var enemy in currentWave.Enemies)
                {
                    if (!enemy.IsAlive) continue;
                    if (bullet.CollidesWith(enemy))
                    {
                        bullet.IsAlive = false;
                        bool killed = enemy.TakeDamage();
                        if (killed)
                        {
                            Color exColor = enemy.Type == 2 ? Color.Gold : Color.OrangeRed;
                            int exSize = enemy.Type == 2 ? 80 : 40;
                            explosions.Add(new Explosion(
                                enemy.X + enemy.Width / 2, enemy.Y + enemy.Height / 2,
                                exSize, exColor));
                            currentWave.TrySpawnPowerUp(enemy);
                            currentWave.TrySpawnHeart(enemy);
                        }
                        else
                        {
                            explosions.Add(new Explosion(bullet.X, bullet.Y, 15, Color.White));
                        }
                    }
                }
            }

            // Player nhặt PowerUp
            foreach (var powerUp in currentWave.PowerUps)
            {
                if (!powerUp.IsAlive) continue;
                if (powerUp.CollidesWith(player))
                {
                    powerUp.IsAlive = false;
                    if (powerUp.Type == PowerUp.PowerUpType.TripleShot)
                    {
                        player.ActivateTripleShot();
                        explosions.Add(new Explosion(player.X + player.Width / 2, player.Y, 30, Color.LimeGreen));
                    }
                    else if (powerUp.Type == PowerUp.PowerUpType.HeartDrop)
                    {
                        if (player.Lives < 5) player.Lives++;
                        explosions.Add(new Explosion(player.X + player.Width / 2, player.Y, 30, Color.Red));
                    }
                }
            }

            // Đạn thường kẻ thù vs player
            foreach (var bullet in currentWave.EnemyBullets)
            {
                if (!bullet.IsAlive) continue;
                if (bullet.CollidesWith(player))
                {
                    bullet.IsAlive = false;
                    player.TakeDamage();
                    explosions.Add(new Explosion(player.X + player.Width / 2, player.Y + player.Height / 2, 30, Color.Cyan));
                }
            }

            // Homing vs player
            foreach (var homing in currentWave.HomingBullets)
            {
                if (!homing.IsAlive) continue;
                if (homing.CollidesWith(player))
                {
                    homing.IsAlive = false;
                    player.TakeDamage();
                    explosions.Add(new Explosion(player.X + player.Width / 2, player.Y + player.Height / 2, 30, Color.Magenta));
                }
            }

            // Boss spread bullets vs player
            foreach (var bb in currentWave.BossBullets)
            {
                if (!bb.IsAlive) continue;
                if (bb.CollidesWith(player))
                {
                    bb.IsAlive = false;
                    player.TakeDamage();
                    explosions.Add(new Explosion(player.X + player.Width / 2, player.Y + player.Height / 2, 30, Color.Gold));
                }
            }

            foreach (var enemy in currentWave.Enemies)
            {
                if (!enemy.IsAlive) continue;
                if (enemy.CollidesWith(player))
                    player.TakeDamage();
                if (enemy.Y + enemy.Height > SCREEN_H)
                {
                    player.Lives = 0;
                    player.IsAlive = false;
                }
            }

            if (!player.IsAlive || player.Lives <= 0)
            {
                explosions.Add(new Explosion(player.X + player.Width / 2, player.Y, 80, Color.Cyan));
                state = GameState.GameOver;
                gameTimer.Stop();
                ShowDefeat();
                return;
            }

            if (currentWave.IsCleared)
            {
                waveNumber++;
                if (waveNumber > MAX_WAVES)
                {
                    state = GameState.Win;
                    gameTimer.Stop();
                    ShowVictory();
                    return;
                }
                else
                {
                    currentWave = new EnemyWave(waveNumber);
                    bool prevWasBoss = ((waveNumber - 1) % 2 == 0);
                    player.Lives = Math.Min(player.Lives + (prevWasBoss ? 2 : 1), 5);
                }
            }
        }

        private void ShowVictory()
        {
            var victory = new Victory();
            // Đặt form Victory đúng vị trí đè lên Form1
            victory.Location = this.Location;
            victory.StartPosition = FormStartPosition.Manual;

            var result = victory.ShowDialog(this);

            if (result == DialogResult.Retry)
            {
                // Chơi lại từ đầu
                waveNumber = 1;
                StartGame();
                gameTimer.Start();
            }
            else
            {
                // Về menu
                var menu = new Menu();
                menu.Show();
                this.Close();
            }
        }

        private void ShowDefeat()
        {
            var defeat = new Defeat();
            defeat.Location = this.Location;
            defeat.StartPosition = FormStartPosition.Manual;

            var result = defeat.ShowDialog(this);

            if (result == DialogResult.Retry)
            {
                waveNumber = 1;
                StartGame();
                gameTimer.Start();
            }
            else
            {
                var menu = new Menu();
                menu.Show();
                this.Close();
            }
        }

        private void OnPaint(object sender, PaintEventArgs e)
        {
            Render(backGraphics);
            e.Graphics.DrawImage(backBuffer, 0, 0);
        }

        private void Render(Graphics g)
        {
            g.Clear(Color.Black);
            foreach (var s in stars) s.Draw(g);

            if (state == GameState.Playing || state == GameState.Paused)
            {
                currentWave.Draw(g);
                player.Draw(g);
                foreach (var ex in explosions) ex.Draw(g);
            }
            if (state == GameState.GameOver)
                foreach (var ex in explosions) ex.Draw(g);

            DrawHUD(g);
        }

        private void DrawHUD(Graphics g)
        {
            Image avatarSprite = Properties.Resources.avatarplayer;
            int avatarSize = 40;
            int avatarFrame = (frameCount / 8) % 5;
            Rectangle srcAvatar = new Rectangle(avatarFrame * 8, 0, 8, 8);
            Rectangle dstAvatar = new Rectangle(10, 8, avatarSize, avatarSize);
            var oldInterp = g.InterpolationMode;
            var oldPixel = g.PixelOffsetMode;
            g.InterpolationMode = InterpolationMode.NearestNeighbor;
            g.PixelOffsetMode = PixelOffsetMode.Half;
            g.DrawImage(avatarSprite, dstAvatar, srcAvatar, GraphicsUnit.Pixel);
            g.InterpolationMode = oldInterp;
            g.PixelOffsetMode = oldPixel;

            Image heartSprite = Properties.Resources.heart;
            int heartSize = 32;
            int heartSpacing = 6;
            int heartStartX = 10 + avatarSize + 8;
            int heartStartY = 16;
            for (int i = 0; i < player.Lives; i++)
            {
                Rectangle srcHeart = new Rectangle(0, 0, 8, 8);
                Rectangle dstHeart = new Rectangle(heartStartX + i * (heartSize + heartSpacing), heartStartY, heartSize, heartSize);
                oldInterp = g.InterpolationMode;
                oldPixel = g.PixelOffsetMode;
                g.InterpolationMode = InterpolationMode.NearestNeighbor;
                g.PixelOffsetMode = PixelOffsetMode.Half;
                g.DrawImage(heartSprite, dstHeart, srcHeart, GraphicsUnit.Pixel);
                g.InterpolationMode = oldInterp;
                g.PixelOffsetMode = oldPixel;
            }

            if (player.HasTripleShot)
            {
                Image upgradeSprite = Properties.Resources.updrage;
                Rectangle srcUpg = new Rectangle(0, 0, upgradeSprite.Width, upgradeSprite.Height);
                Rectangle dstUpg = new Rectangle(10, 58, 24, 24);
                oldInterp = g.InterpolationMode;
                oldPixel = g.PixelOffsetMode;
                g.InterpolationMode = InterpolationMode.NearestNeighbor;
                g.PixelOffsetMode = PixelOffsetMode.Half;
                g.DrawImage(upgradeSprite, dstUpg, srcUpg, GraphicsUnit.Pixel);
                g.InterpolationMode = oldInterp;
                g.PixelOffsetMode = oldPixel;
            }
        }
    }
}