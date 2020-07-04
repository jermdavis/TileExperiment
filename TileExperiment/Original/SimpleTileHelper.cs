using System;
using System.Drawing;

namespace TileExperiment.SimpleTiles
{
    public class SimpleTileHelper
    {
        public readonly Point[] AdjacentPoints = new Point[] { new Point(0, -1), new Point(-1, 0), new Point(0, 1), new Point(1, 0) };

        public Point Reverse(Point dir)
        {
            var rx = 0;
            if (dir.X == 1) rx = -1;
            if (dir.X == -1) rx = 1;

            var ry = 0;
            if (dir.Y == 1) ry = -1;
            if (dir.Y == -1) ry = 1;

            return new Point(rx, ry);
        }

        public Func<SimpleTile, bool> FindResolver(Point direction)
        {
            if (direction.X == 1 && direction.Y == 0)
            {
                return t => t.East;
            }
            else if (direction.X == -1 && direction.Y == 0)
            {
                return t => t.West;
            }
            else if (direction.X == 0 && direction.Y == 1)
            {
                return t => t.South;
            }
            else if (direction.X == 0 && direction.Y == -1)
            {
                return t => t.North;
            }
            else
            {
                throw new ArgumentOutOfRangeException();
            }
        }

        public string ResolverName(Point direction)
        {
            if (direction.X == 1 && direction.Y == 0)
            {
                return "→ East →";
            }
            else if (direction.X == -1 && direction.Y == 0)
            {
                return "← West ←";
            }
            else if (direction.X == 0 && direction.Y == 1)
            {
                return "↓ South ↓";
            }
            else if (direction.X == 0 && direction.Y == -1)
            {
                return "↑ North ↑";
            }
            else
            {
                throw new ArgumentOutOfRangeException();
            }
        }
    }

}