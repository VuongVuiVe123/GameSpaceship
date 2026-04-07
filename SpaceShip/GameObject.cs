using System.Drawing;

namespace SpaceShip
{
    public abstract class GameObject
    {
        public float X { get; set; }
        public float Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public bool IsAlive { get; set; } = true;

        public abstract void Draw(Graphics g);
        public abstract void Update();

        public bool CollidesWith(GameObject other)
        {
            Rectangle r1 = new Rectangle((int)X, (int)Y, Width, Height);
            Rectangle r2 = new Rectangle((int)other.X, (int)other.Y, other.Width, other.Height);
            return r1.IntersectsWith(r2);
        }
    }
}