using System.Collections.Generic;
using System.Diagnostics;

namespace TileExperiment
{
    public class TileSet
    {
        private IDirectionHelper _dh;

        public List<Tile> Tiles { get; } = new List<Tile>();

        public Dictionary<int, TileRuleset> TileRulesets = new Dictionary<int, TileRuleset>();

        public TileSet(IDirectionHelper dh)
        {
            _dh = dh;
        }

        public Tile NewTile(char character, int weight = 1)
        {
            var t = new Tile(character) { Weight = weight };
            Tiles.Add(t);

            return t;
        }

        public void AddBidirectionalTileRules(Tile sourceTile, Tile possibleTile, int weight, params int[] directions)
        {
            foreach (int direction in directions)
            {
                AddBidirectionalTileRule(sourceTile, direction, possibleTile, weight);
            }
        }

        public void AddBidirectionalTileRule(Tile sourceTile, int direction, Tile possibleTile, int weight = 1)
        {
            AddTileRule(sourceTile, direction, possibleTile, weight);
            AddTileRule(possibleTile, _dh.Opposites[direction], sourceTile, weight);
        }

        public void AddTileRules(Tile sourceTile, Tile possibleTile, int weight, params int[] directions)
        {
            foreach(int direction in directions)
            {
                AddTileRule(sourceTile, direction, possibleTile, weight);
            }
        }

        public void AddTileRule(Tile sourceTile, int direction, Tile possibleTile, int weight=1)
        {
            //Debug.WriteLine($"'{sourceTile.Character}' {_dh.Names[direction]} '{possibleTile.Character}'");

            TileRuleset trs;
            if(!this.TileRulesets.ContainsKey(sourceTile.Id))
            {
                trs = new TileRuleset() { SourceTileId = sourceTile.Id };
                this.TileRulesets.Add(sourceTile.Id, trs);
            }
            else
            {
                trs = this.TileRulesets[sourceTile.Id];
            }

            DirectionRuleset dr;
            if (!trs.DirectionRulesets.ContainsKey(direction))
            {
                dr = new DirectionRuleset() { SourceTileId = sourceTile.Id, Direction = direction };
                trs.DirectionRulesets.Add(direction, dr);
            }
            else
            {
                dr = trs.DirectionRulesets[direction];
            }

            TileRule tr;
            if(!dr.Rules.ContainsKey(possibleTile.Id))
            {
                tr = new TileRule() { SourceTileId = sourceTile.Id, Direction = direction, PossibleTileId = possibleTile.Id, Weight = 1 };
                dr.Rules.Add(possibleTile.Id, tr);
            }
            else
            {
                dr.Rules[possibleTile.Id].Weight += 1;
            }
        }

        public Tile[] Fetch()
        {
            return Tiles.ToArray();
        }
    }

}