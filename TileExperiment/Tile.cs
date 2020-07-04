using System.Runtime;
using System.Text;

namespace TileExperiment
{

    public class Tile
    {
        private static int _nextId = 0;

        public int Id { get; }

        public char Character { get; }

        // Weight of raw tile is "how likely is this to be picked when resolving a random tile
        // from the entropy winners
        public int Weight { get; set; } = 1;

        public Tile(char character)
        {
            Id = _nextId;
            _nextId += 1;

            Character = character;
        }
    }

}