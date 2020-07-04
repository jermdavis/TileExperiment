namespace TileExperiment
{
    
    public class SimpleExampleGenerator : ITilesetGenerator
    {
        public TileSet Generate(IDirectionHelper dh)
        {
            var ts = new TileSet(dh);

            var one = ts.NewTile('█',1);
            var two = ts.NewTile('╬',1);
            var three = ts.NewTile('┼',1);

            ts.AddBidirectionalTileRules(one, one, 1, Directions.North, Directions.South, Directions.East, Directions.West);
            ts.AddBidirectionalTileRules(one, two, 1, Directions.North, Directions.South, Directions.East, Directions.West);

            ts.AddBidirectionalTileRules(two, two, 1, Directions.North, Directions.South, Directions.East, Directions.West);
            ts.AddBidirectionalTileRules(two, three, 1, Directions.North, Directions.South, Directions.East, Directions.West);

            ts.AddBidirectionalTileRules(three, three, 1, Directions.North, Directions.South, Directions.East, Directions.West);

            return ts;
        }
    }

}