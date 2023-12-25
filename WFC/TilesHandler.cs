using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WFC
{
    internal class TilesHandler
    {
        public HashSet<Tile> Tiles { get; private set; } = new HashSet<Tile>();

        public TilesHandler(string path) {
            FillFromPath(path, true);
            foreach (var tile in Tiles)
            {
                tile.CalcAllowedTiles(Tiles);
            }
        }

        private void FillFromPath(string path, bool isRotate)
        {
            var imageFiles = Directory.GetFiles(path, "*.*")
                                  .Where(file => file.ToLower().EndsWith(".png") || file.ToLower().EndsWith(".jpg") || file.ToLower().EndsWith(".jpeg"))
                                  .ToList();

            foreach (string imagePath in imageFiles)
            {
                SixLabors.ImageSharp.Image<Rgba32> image = SixLabors.ImageSharp.Image.Load<Rgba32>(imagePath);
                Tiles.Add(new Tile(image));
            }

            if (isRotate) {
                AddRotatedTiles();
            }
        }

        
        
        private void AddRotatedTiles()
        {
            HashSet<Tile> newTiles = new HashSet<Tile>();
            foreach (Tile tile in Tiles)
            {
                for (int i = 1; i < 4; ++i)
                {
                    var image = tile.Img.Clone();
                    image.Mutate(ctx => ctx.Rotate(90*i));
                    newTiles.Add(new Tile(image));
                }
            }
            foreach (Tile tile in newTiles)
            {
                Tiles.Add(tile);
            }
            
        }
        

    }
}
