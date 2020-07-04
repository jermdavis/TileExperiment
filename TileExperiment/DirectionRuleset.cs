using System.Collections.Generic;

namespace TileExperiment
{
    public class DirectionRuleset
    {
        public int SourceTileId;
        public int Direction;
        public Dictionary<int, TileRule> Rules { get; } = new Dictionary<int, TileRule>();
    }

}