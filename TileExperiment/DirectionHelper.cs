using System.Drawing;
using System.Linq;

namespace TileExperiment
{
    public class DirectionHelper : IDirectionHelper
    {
        public int Count => 4;

        public string[] Names => new string[] { "North", "South", "East", "West" };
        public Point[] Deltas => new Point[] { new Point(0, -1), new Point(0, +1), new Point(+1, 0), new Point(-1, 0) };
        public int[] Opposites => new int[] { 1, 0, 3, 2 };

        public int DirectionIndex(Point p)
        {
            for(int i=0; i<Deltas.Length; i++)
            {
                if(Deltas[i] == p)
                {
                    return i;
                }
            }

            return -1;
        }
    }

}