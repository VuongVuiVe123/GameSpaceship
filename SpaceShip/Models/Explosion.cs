using System;
using System.Collections.Generic;
using System.Drawing;

namespace SpaceShip.Models
{
    public class Explosion : GameObject
    {
        private int frame = 0;
        private int maxFrames;
        private Color color;
        private List<PointF> particles = new List<PointF>();
        private List<PointF> velocities = new List<PointF>();
        private Random rnd = new Random();

        public Explosion(float x, float y, int size, Color c)
        {
            X = x; Y = y; Width = size; Height = size;
            maxFrames = 20; color = c;
            for (int i = 0; i < 12; i++)
            {
                particles.Add(new PointF(x, y));
                float angle = (float)(rnd.NextDouble() * Math.PI * 2);
                float speed = (float)(rnd.NextDouble() * 4 + 1);
                velocities.Add(new PointF((float)Math.Cos(angle) * speed, (float)Math.Sin(angle) * speed));
            }
        }

        public override void Update()
        {
            frame++;
            if (frame >= maxFrames) { IsAlive = false; return; }
            for (int i = 0; i < particles.Count; i++)
                particles[i] = new PointF(
                    particles[i].X + velocities[i].X,
                    particles[i].Y + velocities[i].Y);
        }

        public override void Draw(Graphics g)
        {
            if (!IsAlive) return;
            int alpha = (int)(255 * (1f - (float)frame / maxFrames));
            float ratio = (float)frame / maxFrames;
            float radius = Width * ratio;

            using (var glowPen = new Pen(Color.FromArgb(alpha / 2, Color.White), 3))
                g.DrawEllipse(glowPen, X - radius, Y - radius, radius * 2, radius * 2);
            using (var firePen = new Pen(Color.FromArgb(alpha, color), 2))
                g.DrawEllipse(firePen, X - radius * 0.6f, Y - radius * 0.6f, radius * 1.2f, radius * 1.2f);

            int pSize = Math.Max(1, (int)(4 * (1 - ratio)));
            for (int i = 0; i < particles.Count; i++)
                using (var pb = new SolidBrush(Color.FromArgb(alpha, color)))
                    g.FillEllipse(pb, particles[i].X - pSize / 2, particles[i].Y - pSize / 2, pSize, pSize);
        }
    }
}