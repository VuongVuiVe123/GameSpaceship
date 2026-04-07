using SpaceShip.Models;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace SpaceShip
{
    public class EnemyWave
    {
        public List<Enemy> Enemies { get; private set; } = new List<Enemy>();
        public List<Bullet> EnemyBullets { get; private set; } = new List<Bullet>();
        public List<HomingBullet> HomingBullets { get; private set; } = new List<HomingBullet>();
        public List<PowerUp> PowerUps { get; private set; } = new List<PowerUp>();
        public List<BossBullet> BossBullets { get; private set; } = new List<BossBullet>();

        public bool IsBossWave { get; private set; }

        private float moveDir = 1f;
        private float moveSpeed = 1.2f;
        private float dropAmount = 20f;

        private int shootTimer = 0;
        private int homingTimer = 0;

        // Boss timers
        private int bossShootTimer = 0;
        private int minionTimer = 0;
        private const int BOSS_SHOOT_INTERVAL = 90; 
        private const int MINION_INTERVAL = 240;

        private Random rnd = new Random();
        private int waveLevel;

        public bool IsCleared => Enemies.TrueForAll(e => !e.IsAlive);

        public EnemyWave(int waveNumber)
        {
            IsBossWave = (waveNumber % 2 == 0);
            waveLevel = (waveNumber + 1) / 2;
            moveSpeed = 1.2f + (waveLevel - 1) * 0.3f;
            SpawnWave();
        }

        private void SpawnWave()
        {
            Enemies.Clear();
            if (IsBossWave) SpawnBoss();
            else SpawnNormalWave();
        }

        private void SpawnNormalWave()
        {
            int rows = Math.Min(3 + (waveLevel - 1) / 2, 5);
            int cols = Math.Min(8 + waveLevel, 12);
            for (int row = 0; row < rows; row++)
                for (int col = 0; col < cols; col++)
                {
                    int type = row == 0 ? 1 : 0;
                    int totalWidth = cols * 65;
                    int startX = (1280 - totalWidth) / 2;
                    Enemies.Add(new Enemy(startX + col * 65, 60 + row * 55, type));
                }
        }

        private void SpawnBoss()
        {
            int bossX = (1280 - 140) / 2;
            Enemies.Add(new Enemy(bossX, 60, 2));
        }

        private void SpawnMinions()
        {
            var boss = Enemies.Find(e => e.IsAlive && e.Type == 2);
            if (boss == null) return;

            float leftX = boss.X - 60;
            float rightX = boss.X + boss.Width + 20;
            float spawnY = boss.Y + boss.Height / 2f - 18;

            if (leftX > 0) Enemies.Add(new Enemy(leftX, spawnY, 0));
            if (rightX < 1240) Enemies.Add(new Enemy(rightX, spawnY, 1));
        }

        private void BossFireSpread(Enemy boss, float playerCenterX, float playerCenterY)
        {
            float bx = boss.X + boss.Width / 2f;
            float by = boss.Y + boss.Height;

            int numRays = 5 + (waveLevel - 1) * 2;
            numRays = Math.Min(numRays, 12);

            float dx = playerCenterX - bx;
            float dy = playerCenterY - by;
            float len = (float)Math.Sqrt(dx * dx + dy * dy);
            float baseAngle = (len > 0)
                ? (float)Math.Atan2(dy, dx)
                : (float)(Math.PI / 2);

            float spread = (float)(Math.PI * 0.8);
            float step = spread / (numRays - 1);
            float startAngle = baseAngle - spread / 2f;

            float speed = 4f + (waveLevel - 1) * 0.5f;

            for (int i = 0; i < numRays; i++)
            {
                float a = startAngle + step * i;
                float dirX = (float)Math.Cos(a);
                float dirY = (float)Math.Sin(a);
                BossBullets.Add(new BossBullet(bx, by, dirX, dirY, speed));
            }
        }

        public void TrySpawnPowerUp(Enemy deadEnemy)
        {
            if (deadEnemy.ShouldDropPowerUp())
                PowerUps.Add(new PowerUp(
                    deadEnemy.X + deadEnemy.Width / 2f,
                    deadEnemy.Y + deadEnemy.Height / 2f,
                    PowerUp.PowerUpType.TripleShot));
        }

        public void TrySpawnHeart(Enemy deadEnemy)
        {
            if (deadEnemy.ShouldDropHeart())
                PowerUps.Add(new PowerUp(
                    deadEnemy.X + deadEnemy.Width / 2f,
                    deadEnemy.Y + deadEnemy.Height / 2f,
                    PowerUp.PowerUpType.HeartDrop));
        }

        public void Update(int screenWidth, float playerCenterX, float playerCenterY)
        {
            if (!IsBossWave)
            {
                bool hitEdge = false;
                foreach (var e in Enemies)
                {
                    if (!e.IsAlive) continue;
                    if ((e.X + e.Width >= screenWidth - 10 && moveDir > 0) ||
                        (e.X <= 10 && moveDir < 0))
                    { hitEdge = true; break; }
                }
                if (hitEdge)
                {
                    moveDir *= -1;
                    foreach (var e in Enemies) e.Y += dropAmount;
                }
                else
                {
                    foreach (var e in Enemies) { e.X += moveDir * moveSpeed; e.Update(); }
                }

                shootTimer++;
                int shootInterval = Math.Max(30, 80 - (waveLevel - 1) * 8);
                if (shootTimer >= shootInterval)
                {
                    shootTimer = 0;
                    var shooters = Enemies.FindAll(e => e.IsAlive && e.Type == 0);
                    if (shooters.Count > 0)
                    {
                        var shooter = shooters[rnd.Next(shooters.Count)];
                        EnemyBullets.Add(new Bullet(
                            shooter.X + shooter.Width / 2f,
                            shooter.Y + shooter.Height,
                            8f, false));
                    }
                }

                homingTimer++;
                int homingInterval = Math.Max(50, 120 - (waveLevel - 1) * 10);
                if (homingTimer >= homingInterval)
                {
                    homingTimer = 0;
                    var aliveType1 = Enemies.FindAll(e => e.IsAlive && e.Type == 1);
                    if (aliveType1.Count > 0)
                    {
                        var shooter = aliveType1[rnd.Next(aliveType1.Count)];
                        HomingBullets.Add(new HomingBullet(
                            shooter.X + shooter.Width / 2f,
                            shooter.Y + shooter.Height));
                    }
                }
            }
            else
            {
                //BOSS WAVE
                var boss = Enemies.Find(e => e.IsAlive && e.Type == 2);

                // tau con di chuyen
                bool hitEdge = false;
                foreach (var e in Enemies)
                {
                    if (!e.IsAlive || e.Type == 2) continue;
                    if ((e.X + e.Width >= screenWidth - 10 && moveDir > 0) ||
                        (e.X <= 10 && moveDir < 0))
                    { hitEdge = true; break; }
                }
                if (hitEdge) moveDir *= -1;
                foreach (var e in Enemies)
                {
                    if (!e.IsAlive || e.Type == 2) continue;
                    e.X += moveDir * moveSpeed;
                    e.Update();
                }

                // tau con thu nhat
                shootTimer++;
                if (shootTimer >= 60)
                {
                    shootTimer = 0;
                    var minions = Enemies.FindAll(e => e.IsAlive && e.Type == 0);
                    if (minions.Count > 0)
                    {
                        var shooter = minions[rnd.Next(minions.Count)];
                        EnemyBullets.Add(new Bullet(
                            shooter.X + shooter.Width / 2f,
                            shooter.Y + shooter.Height,
                            8f, false));
                    }
                }

                // tau con thu 2
                homingTimer++;
                if (homingTimer >= 90)
                {
                    homingTimer = 0;
                    var minions1 = Enemies.FindAll(e => e.IsAlive && e.Type == 1);
                    if (minions1.Count > 0)
                    {
                        var shooter = minions1[rnd.Next(minions1.Count)];
                        HomingBullets.Add(new HomingBullet(
                            shooter.X + shooter.Width / 2f,
                            shooter.Y + shooter.Height));
                    }
                }

                // Boss ban den 4 tia
                if (boss != null)
                {
                    bossShootTimer++;
                    if (bossShootTimer >= BOSS_SHOOT_INTERVAL)
                    {
                        bossShootTimer = 0;
                        BossFireSpread(boss, playerCenterX, playerCenterY);
                    }

                    minionTimer++;
                    int liveMinions = Enemies.FindAll(e => e.IsAlive && e.Type != 2).Count;
                    if (minionTimer >= MINION_INTERVAL && liveMinions < 4)
                    {
                        minionTimer = 0;
                        SpawnMinions();
                    }

                    boss.Update();
                }
            }

            foreach (var b in EnemyBullets) b.Update();
            EnemyBullets.RemoveAll(b => !b.IsAlive);

            foreach (var h in HomingBullets) h.UpdateHoming(playerCenterX, playerCenterY);
            HomingBullets.RemoveAll(h => !h.IsAlive);

            foreach (var bb in BossBullets) bb.Update();
            BossBullets.RemoveAll(bb => !bb.IsAlive);

            foreach (var p in PowerUps) p.Update();
            PowerUps.RemoveAll(p => !p.IsAlive);
        }

        public void Draw(Graphics g)
        {
            foreach (var e in Enemies) e.Draw(g);
            foreach (var b in EnemyBullets) b.Draw(g);
            foreach (var h in HomingBullets) h.Draw(g);
            foreach (var bb in BossBullets) bb.Draw(g);
            foreach (var p in PowerUps) p.Draw(g);
        }
    }
}