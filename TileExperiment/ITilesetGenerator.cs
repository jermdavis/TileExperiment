namespace TileExperiment
{
    public interface ITilesetGenerator
    {
        TileSet Generate(IDirectionHelper dh);
    }

}