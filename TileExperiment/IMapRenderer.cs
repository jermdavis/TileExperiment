using System.Drawing;

namespace TileExperiment
{
    public interface IMapRenderer
    {
        void Render(Map map);
        void Render(Map map, Point p);
    }

}