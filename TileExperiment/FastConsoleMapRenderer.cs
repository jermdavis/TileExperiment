using System;
using System.Drawing;
using TileExperiment.FastConsole;

namespace TileExperiment
{
    public class FastConsoleMapRenderer : IMapRenderer
    {
        private short _width;
        private short _height;
        private CharInfo[] _buffer;

        public FastConsoleMapRenderer(ConsoleWrapper consoleWrapper)
        {
            _width = consoleWrapper.Width;
            _height = consoleWrapper.Height;
            _buffer = consoleWrapper.Buffer;
        }

        public void Render(Map map)
        {
            for (int y = 0; y < map.Height; y++)
            {
                if (y >= _height) break;

                for (int x = 0; x < map.Width; x++)
                {
                    if (x >= _width) continue;

                    var cell = map.Cells[x, y];
                    int pos = (y * _width) + x;

                    _buffer[pos].Char.UnicodeChar = cell.Value;

                    _buffer[pos].Attributes = (short)ConsoleColor.White;
                }
            }
        }

        public void Render(Map map, Point p)
        {
            for (int y = 0; y < map.Height; y++)
            {
                if (y >= _height) break;

                for (int x = 0; x < map.Width; x++)
                {
                    if (x >= _width) continue;

                    var cell = map.Cells[x, y];
                    int pos = (y * _width) + x;

                    _buffer[pos].Char.UnicodeChar = cell.Value;

                    var attr = (short)ConsoleColor.White;

                    if (p.X == x && p.Y == y)
                    {
                        short colour = (short)ConsoleColor.Blue;
                        short val = (short)(colour << 4);
                        attr += val;
                    }
                    else if (cell.PropagatedTo)
                    {
                        short colour = (short)ConsoleColor.Green;
                        attr += (short)(colour << 4);
                    }
                    else if (cell.Considered)
                    {
                        short colour = (short)ConsoleColor.Yellow;
                        attr += (short)(colour << 4);
                    }

                    _buffer[pos].Attributes = attr;

                    cell.PropagatedTo = false;
                    cell.Considered = false;
                }
            }
        }
    }

}