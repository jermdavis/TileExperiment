using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;

namespace TileExperiment
{
    public class MapResolver
    {
        private Random _rnd;
        private Map _map;
        private IDirectionHelper _helper;
        private List<TileOption> currentTiles = new List<TileOption>();
        private List<int> options = new List<int>();
        private Stack<Point> stack = new Stack<Point>();

        public MapResolver(Random rnd, Map map, IDirectionHelper helper)
        {
            _rnd = rnd;
            _map = map;
            _helper = helper;
        }

        public Point FindLowEntropyWaveform()
        {
            currentTiles.Clear();
            for (int x = 0; x < _map.Width; x++)
            {
                for (int y = 0; y < _map.Height; y++)
                {
                    var cell = _map.Cells[x, y];
                    if (!cell.IsResolved)
                    {
                        currentTiles.Add(new TileOption() { Location = new Point(x, y), Tile = cell });
                    }
                }
            }

            // order tiles by Entropy
            var options = currentTiles
                .GroupBy(t => t.Tile.Entropy)
                .OrderBy(g => g.Key)
                .First();

            TileOption choice;

            // If more than one lowest entropy, pick randomly from them
            if (options.Count() > 1)
            {
                var pick = _rnd.Next(options.Count());
                choice = options.ElementAt(pick);
            }
            else
            {
                choice = options.First();
            }

            return choice.Location;
        }

        public void Resolve(Point tile)
        {
            var cell = _map.Cells[tile.X, tile.Y];

            // find indexes of non-zero items
            options.Clear();
            for (int i = 0; i < cell.Choices.Length; i++)
            {
                if (cell.Choices[i] != null)
                {
                    for (int c = 0; c < cell.Choices[i].Weight; c++)
                    {
                        options.Add(i);
                    }
                }
            }

            // pick one of the remaining states to use
            // This needs weight if the individual tile choices can have a weight?
            var pick = _rnd.Next(options.Count());
            var index = options.ElementAt(pick);

            // null out the ones not picked
            for (int i = 0; i < cell.Choices.Length; i++)
            {
                if (i != index)
                {
                    cell.Choices[i] = null;
                }
            }
        }

        private bool TilePossible(Waveform source, Tile targetPossibility, Point direction)
        {
            foreach (var sourceTile in source.Choices)
            {
                if (sourceTile != null)
                {
                    var str = _map.Tileset.TileRulesets[sourceTile.Id];
                    var sourceRules = str.FetchDirectionRuleset(_helper.DirectionIndex(direction));

                    if (sourceRules != null)
                    {
                        foreach (var r in sourceRules.Rules.Values)
                        {
                            if (r.SourceTileId == sourceTile.Id && r.PossibleTileId == targetPossibility.Id)
                            {
                                return true;
                            }
                        }
                    }
                }
            }

            return false;
        }

        public void Propagate(Point tile)
        {
            stack.Push(tile);

            while (stack.Count > 0)
            {
                var point = stack.Pop();
                var cellWaveform = _map.Cells[point.X, point.Y];

                var d = Point.Empty;
                foreach (Point adjPos in _helper.Deltas)
                {
                    d = point
                        .Sum(adjPos);
                    //.Wrap(_map);

                    if (d.X < 0) continue;
                    if (d.X >= _map.Width) continue;
                    if (d.Y < 0) continue;
                    if (d.Y >= _map.Height) continue;

                    var adjWaveform = _map.Cells[d.X, d.Y];

                    adjWaveform.Considered = true;

                    //Debug.Write($"Cell {point}='{cellWaveform.Values}' {_helper.Names[_helper.DirectionIndex(adjPos)]} : Adj {d}='{adjWaveform.Values}'");

                    bool changed = false;
                    for (int i = 0; i < adjWaveform.Choices.Length; i++)
                    {
                        if (adjWaveform.Choices[i] != null)
                        {
                            if (!TilePossible(cellWaveform, adjWaveform.Choices[i], adjPos))
                            {
                                adjWaveform.Choices[i] = null;
                                changed = true;
                            }
                        }
                    }

                    if (changed)
                    {
                        stack.Push(d);
                        adjWaveform.PropagatedTo = true;
                    }

                    //Debug.WriteLine($" => {d}='{adjWaveform.Values}'");

                    if (adjWaveform.Choices.Where(t => t != null).Count() == 0)
                    {
                        throw new Exception("invalid state?");
                    }
                }
            }
        }
    }

}