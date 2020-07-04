namespace TileExperiment.SimpleTiles
{

    public class SimpleTile
    {
        public char Character { get; set; }
        public bool North { get; set; }
        public bool South { get; set; }
        public bool East { get; set; }
        public bool West { get; set; }

        public int Weight { get; set; }

        public SimpleTile(char character, bool n = false, bool s = false, bool e = false, bool w = false, int weight = 1)
        {
            Character = character;
            North = n;
            South = s;
            East = e;
            West = w;
            Weight = weight;
        }
    }

}