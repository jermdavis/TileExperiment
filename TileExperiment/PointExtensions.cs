using System.Drawing;

namespace TileExperiment
{
    public static class PointExtensions
    {
        public static Point Wrap(this Point d, Map map)
        {
            if (d.X >= map.Width) d.X -= map.Width;
            if (d.X < 0) d.X += map.Width;
            if (d.Y >= map.Height) d.Y -= map.Height;
            if (d.Y < 0) d.Y += map.Height;

            return d;
        }

        public static Point Sum(this Point a, Point b)
        {
            return new Point(a.X + b.X, a.Y + b.Y);
        }
    }

}