using System.Collections.Generic;

namespace TileExperiment
{
    public class TileRuleset
    {
        public int SourceTileId;
        public Dictionary<int, DirectionRuleset> DirectionRulesets = new Dictionary<int, DirectionRuleset>();

        public DirectionRuleset FetchDirectionRuleset(int direction)
        {
            if(DirectionRulesets.ContainsKey(direction))
            {
                return DirectionRulesets[direction];
            }
            else
            {
                return null;
            }
        }
    }

}