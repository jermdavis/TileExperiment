using System;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Xml.Schema;
using TileExperiment.FastConsole;

namespace TileExperiment
{

    class Program
    {
        static void Main()
        {
            Console.WriteLine("Choose source of rules:");
            Console.WriteLine("1) DescriptiveTilesetGenerator (Turn simple rules into a rule set)");
            Console.WriteLine("2) SimpleExampleGenerator (Hand-coded rules)");
            Console.WriteLine("3) InferredRuleGenerator (Rules infered from a text file)");

            int choice = 0;
            do
            {
                Console.Write(">> ");
                var input = Console.ReadLine();
                int.TryParse(input, out choice);

                if(choice < 1 || choice > 3)
                {
                    choice = 0;
                }
            }
            while (choice == 0);

            ITilesetGenerator generator = null;
            switch (choice)
            {
                case 1: 
                    generator = new SimpleTiles.DescriptiveTilesetGenerator();
                    break;
                case 2: 
                    generator = new SimpleExampleGenerator();
                    break;
                case 3: 
                    generator = new InferredRuleGenerator("Example.txt");
                    break;
            }

            Run(generator);
        }

        private static void Run(ITilesetGenerator generator, bool pause = true)
        {
            var helper = new DirectionHelper();
            var tileset = generator.Generate(helper);

            Random rnd = new Random(10);

            var w = Console.WindowWidth;
            var h = Console.WindowHeight - 1;

            using (var fastConsole = new ConsoleWrapper((short)w, (short)h))
            {
                var renderer = new FastConsoleMapRenderer(fastConsole);

                do
                {
                    var map = new Map(w, h, tileset);
                    var resolver = new MapResolver(rnd, map, helper);

                    var sw = Stopwatch.StartNew();

                    // while choices remain
                    while (!map.IsResolved)
                    {
                        // pick a (random) tile with lowest entropy
                        var tile = resolver.FindLowEntropyWaveFunction();

                        // give it a value, by weights
                        resolver.Resolve(tile);

                        try
                        {
                            // reduce the choices for surrounding tiles, propagting out
                            resolver.Propagate(tile);
                        }
                        catch
                        {
                            pause = true;
                            break;
                        }

                        renderer.Render(map, tile);
                        fastConsole.RenderBuffer();
                    }

                    sw.Stop();

                    renderer.Render(map);
                    fastConsole.RenderBuffer();

                    Console.SetCursorPosition(0, Console.WindowHeight - 1);
                    Console.Write($"Time: {sw.ElapsedMilliseconds}");

                    if (pause)
                    {
                        Console.ReadKey();
                        pause = false;
                    }
                } while (true);

            }
        }
    }

}