using System;
using System.Linq;

namespace TileExperiment
{

    public class WaveFunction
    {
        public bool Considered { get; set; }
        public bool PropagatedTo { get; set; }

        public Tile[] Choices { get; set; }

        public int Entropy => Choices.Where(t => t != null).Count();
        public bool IsResolved => Entropy == 1;

        public string Values
        {
            get
            {
                return new string(Choices.Where(t => t != null).Select(t => t.Character).ToArray());
            }
        }

        public char Value
        {
            get
            {
                var choices = Choices.Where(t => t != null);

                if (choices.Count() == 1)
                {
                    var choice = choices.First();
                    return choice.Character;
                }
                else
                {
                    // make it hex
                    return (choices.Count()).ToString("x")[0];
                }
            }
        }
    }

}