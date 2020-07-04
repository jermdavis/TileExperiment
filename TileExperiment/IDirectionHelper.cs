using System.Drawing;

namespace TileExperiment
{
    public interface IDirectionHelper
    {
        int Count { get; }
        string[] Names { get; }
        Point[] Deltas { get; }
        int[] Opposites { get; }
        int DirectionIndex(Point p);
    }

}