using System;
using System.Drawing;

namespace SpaceShip.Models
{
    public class Star
    {
        public float X, Y, Speed, Brightness;
        public int Size;
        private static Random rnd = new Random();

        public Star(int screenWidth, int screenHeight)
        {
            X = (float)(rnd.NextDouble() * screenWidth);
            Y = (float)(rnd.NextDouble() * screenHeight);
            Speed = (float)(rnd.NextDouble() * 2 + 0.5f);
            Size = rnd.Next(1, 3);
            Brightness = (float)(rnd.NextDouble() * 155 + 100);
        }

        public void Update(int screenHeight)
        {
            Y += Speed;
            if (Y > screenHeight) { Y = 0; X = new Random().Next(0, 1280); }
        }

        public void Draw(Graphics g)
        {
            int b = (int)Brightness;
            using (var sb = new SolidBrush(Color.FromArgb(b, b, b)))
                g.FillEllipse(sb, X, Y, Size, Size);
        }
    }
}