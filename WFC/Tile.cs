using SixLabors.ImageSharp.PixelFormats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WFC
{
    enum Direction{
        Left,
        Right,
        Up,
        Down
    }

    class Tile
    {
        static int id = 0;
        int m_id;
        public HashSet<Tile> AllowedTilesLeft { get; private set; } = new HashSet<Tile>();
        public HashSet<Tile> AllowedTilesRight { get; private set; } = new HashSet<Tile>();
        public HashSet<Tile> AllowedTilesUp { get; private set; } = new HashSet<Tile>();
        public HashSet<Tile> AllowedTilesDown { get; private set; } = new HashSet<Tile>();

        public SixLabors.ImageSharp.Image<Rgba32> Img {  get; private set; }
        public int Size { get; init; }

        public Tile(SixLabors.ImageSharp.Image<Rgba32> image)
        {
            m_id = id;
            id++;
            if(image.Width != image.Height)
            {
                throw new ArgumentException("The image must be of size NxN.");
            }
            Size = image.Width;
            Img = image;
        }

        /// <summary>
        /// Calculate allowed tiles for all directions
        /// </summary>
        /// <param name="tiles">List of all tiles in set</param>
        /// <exception cref="ArgumentException">The size of all tiles must be the same.</exception>
        public void CalcAllowedTiles(IEnumerable<Tile> tiles)
        {
            foreach(Tile tile in tiles)
            {
                if (tile.Size != Size)
                {
                    throw new ArgumentException("The size of all tiles must be the same.");
                }
                
                if(CheckAllowed(tile, Direction.Left))
                {
                    AllowedTilesLeft.Add(tile);
                }
                if (CheckAllowed(tile, Direction.Right))
                {
                    AllowedTilesRight.Add(tile);
                }
                if (CheckAllowed(tile, Direction.Up))
                {
                    AllowedTilesUp.Add(tile);
                }
                if (CheckAllowed(tile, Direction.Down))
                {
                    AllowedTilesDown.Add(tile);
                }
            }

        }

        public bool IsAllowed(Tile tile, Direction dir)
        {
            bool allowed = false;
            switch (dir)
            {
                case Direction.Left:
                    if (AllowedTilesLeft.Contains(tile)) allowed = true;
                    break;
                case Direction.Right:
                    if (AllowedTilesRight.Contains(tile)) allowed = true;
                    break;
                case Direction.Up:
                    if (AllowedTilesUp.Contains(tile)) allowed = true;
                    break;
                case Direction.Down:
                    if (AllowedTilesDown.Contains(tile)) allowed = true;
                    break;
            }

            return allowed;
        }

        private bool CheckAllowed(Tile tile, Direction dir)
        {
            int size = tile.Size;
            bool isSame = true;
            switch (dir)
            {
                case Direction.Left:
                    for (int i = 0; i < size; ++i)
                    {
                        if (Img[0, i] != tile.Img[size - 1, i]) isSame = false;
                    }
                    break;
                case Direction.Right:
                    for (int i = 0; i < size; ++i)
                    {
                        if (Img[size - 1, i] != tile.Img[0, i]) isSame = false;
                    }
                    break;
                case Direction.Up:
                    for (int i = 0; i < size; ++i)
                    {
                        if (Img[i, 0] != tile.Img[i, size - 1]) isSame = false;
                    }
                    break;
                case Direction.Down:
                    for (int i = 0; i < size; ++i)
                    {
                        if (Img[i, size - 1] != tile.Img[i, 0]) isSame = false;
                    }
                    break;
            }

            return isSame;

        }

        


    }
}
