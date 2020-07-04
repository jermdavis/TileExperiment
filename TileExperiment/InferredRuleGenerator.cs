using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;

namespace TileExperiment
{
 
    public class InferredRuleGenerator : ITilesetGenerator
    {
        private IDirectionHelper _dh;
        private int width;
        private int height;
        private char[,] exampleMap;
        Dictionary<char, Tile> seen = new Dictionary<char, Tile>();

        public InferredRuleGenerator(string file)
        {
            var text = System.IO.File.ReadAllLines(file);

            width = text.Select(l => l.Length).OrderByDescending(l => l).First();
            height = text.Length;
            exampleMap = new char[width, height];
            
            for(int y=0; y<text.Length; y++)
            {
                for(int x=0; x<text[y].Length; x++)
                {
                    exampleMap[x, y] = text[y][x];
                }
            }
        }

        private void mapTiles(TileSet ts)
        {
            for(int y=0; y<height; y++)
            {
                for(int x=0; x<width; x++)
                {
                    char ch = exampleMap[x, y];
                    if (ch != 0)
                    {
                        if (!seen.ContainsKey(ch))
                        {
                            var tile = ts.NewTile(ch, 1);
                            seen.Add(ch, tile);
                        }
                        else
                        {
                            seen[ch].Weight += 1;
                        }
                    }
                }
            }
        }

        private void mapRules(TileSet ts, bool wrap = false)
        {
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    char ch = exampleMap[x, y];
                    if (ch != 0)
                    {
                        Tile currentTile = seen[ch];

                        foreach (var delta in _dh.Deltas)
                        {
                            var xx = x + delta.X;
                            var yy = y + delta.Y;

                            if (wrap) // do we allow rules to wrap around the edges of the source data?
                            {
                                if (xx < 0) xx += width;
                                if (xx >= width) xx -= width;
                                if (yy < 0) yy += height;
                                if (yy >= height) yy -= height;
                            }
                            else
                            {
                                if (xx < 0) continue;
                                if (xx >= width) continue;
                                if (yy < 0) continue;
                                if (yy >= height) continue;
                            }

                            char dch = exampleMap[xx, yy];
                            if (dch != 0)
                            {
                                Tile deltaTile = seen[dch];

                                ts.AddTileRule(currentTile, _dh.DirectionIndex(delta), deltaTile);
                            }
                        }
                    }
                }
            }
        }

        public void debugRules(TileSet ts)
        {
            foreach(var charRule in ts.TileRulesets.Values)
            {
                Tile t = ts.Tiles.Where(i => i.Id == charRule.SourceTileId).First();

                foreach(var directionRule in charRule.DirectionRulesets.Values)
                {
                    var name = _dh.Names[directionRule.Direction];

                    foreach(var rule in directionRule.Rules.Values)
                    {
                        var p = ts.Tiles.Where(i => i.Id == rule.PossibleTileId).First();

                        Debug.WriteLine($"'{t.Character}' {name} => '{p.Character}' w:{rule.Weight}");
                    }
                }
            }
        }

        public TileSet Generate(IDirectionHelper dh)
        {
            _dh = dh;
            var ts = new TileSet(dh);

            mapTiles(ts);
            mapRules(ts);

            debugRules(ts);

            return ts;
        }
    }

}