using System.Collections.Generic;

namespace TileExperiment.SimpleTiles
{

    public class DescriptiveTilesetGenerator : ITilesetGenerator
    {
        public TileSet Generate(IDirectionHelper dh)
        {
            var ts = new TileSet(dh);

            var oldTiles = new SimpleTile[] {
                new SimpleTile('┼', n:true, s:true, e:true, w:true,1),
                new SimpleTile('│', n:true, s:true, weight:40),
                new SimpleTile('─', e:true, w:true, weight:40),
                new SimpleTile('┐', s:true, w:true, weight:10),
                new SimpleTile('└', n:true, e:true, weight:10),
                new SimpleTile('┘', n:true, w:true, weight:10),
                new SimpleTile('┌', s:true, e:true, weight:10),
                new SimpleTile(' ', weight:50),
                new SimpleTile('├', n:true, s:true, e:true, weight:5),
                new SimpleTile('┤', n:true, s:true, w:true, weight:5),
                new SimpleTile('┴', e:true, w:true, n:true, weight:5),
                new SimpleTile('┬', e:true, w:true, s:true, weight:5)
            };
            var th = new SimpleTileHelper();
            var newLookup = new Dictionary<char, Tile>();

            foreach(var oldTile in oldTiles)
            {
                var newTile = ts.NewTile(oldTile.Character, oldTile.Weight);
                newLookup.Add(oldTile.Character, newTile);
            }

            foreach(var startTile in oldTiles)
            {
                foreach(var direction in dh.Deltas)
                {
                    var startResolver = th.FindResolver(direction);
                    var startValue = startResolver.Invoke(startTile);
                    foreach(var targetTile in oldTiles)
                    {
                        var reverse = th.Reverse(direction);
                        var targetResolver = th.FindResolver(reverse);
                        var targetValue = targetResolver.Invoke(targetTile);

                        if(startValue == targetValue)
                        {
                            var s = newLookup[startTile.Character];
                            var t = newLookup[targetTile.Character];

                            ts.AddTileRule(s, dh.DirectionIndex(direction), t, 5);
                            ts.AddTileRule(t, dh.DirectionIndex(reverse), s, 1);
                        }
                    }
                }
            }

            return ts;
        }
    }

}