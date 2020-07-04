using System;
using System.Drawing;

namespace TileExperiment
{

    public class MapRenderer : IMapRenderer
    {
        public void Render(Map map)
        {
            Console.Clear();
            for (int y = 0; y < map.Height; y++)
            {
                for (int x = 0; x < map.Width; x++)
                {
                    var cell = map.Cells[x, y];
                    Console.Write(cell.Value);
                }
                Console.WriteLine();
            }
        }

        public void Render(Map map, Point p)
        {
            Console.Clear();
            for(int y=0; y<map.Height; y++)
            { 
                for(int x=0; x<map.Width; x++)
                {
                    var cell = map.Cells[x, y];
                    
                    if(p.X==x && p.Y==y)
                    {
                        Console.BackgroundColor = ConsoleColor.Blue;
                    }
                    else if(cell.PropagatedTo)
                    {
                        Console.BackgroundColor = ConsoleColor.Green;
                    }
                    else if(cell.Considered)
                    {
                        Console.BackgroundColor = ConsoleColor.Yellow;
                    }
                    else
                    {
                        Console.BackgroundColor = ConsoleColor.Black;
                    }
                    
                    Console.Write(cell.Value);

                    cell.PropagatedTo = false;
                    cell.Considered = false;
                }
                
                Console.BackgroundColor = ConsoleColor.Black;
                Console.WriteLine();
            }
        }
    }

}