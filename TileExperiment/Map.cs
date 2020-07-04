namespace TileExperiment
{
    public class Map
    {
        public int Width { get; set; }
        public int Height { get; set; }

        public bool IsResolved
        {
            get
            {
                for (int x = 0; x < Width; x++)
                {
                    for (int y = 0; y < Height; y++)
                    {
                        var cell = Cells[x, y];
                        if (!cell.IsResolved)
                        {
                            return false;
                        }
                    }
                }

                return true;
            }
        }

        public Waveform[,] Cells { get; set; }

        public TileSet Tileset { get; }

        public Map(int w, int h, TileSet tileset)
        {
            Width = w;
            Height = h;

            Cells = new Waveform[Width, Height];
            Tileset = tileset;

            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    var waveform = new Waveform();
                    waveform.Choices = tileset.Fetch();
                    Cells[x, y] = waveform;
                }
            }
        }
    }

}