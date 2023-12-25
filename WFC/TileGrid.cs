using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WFC
{
    internal class TileGrid
    {
        private record class TilesWithCoord
        {
            public required LinkedList<Tile> Tiles { get; init; }
            public required int X {get; init; }
            public required int Y { get; init; }
        }
        private int _findTry = 100;
        private LinkedList<Tile>[,] _tiles;
        private bool[,] _addedToPossible; 
        public Tile[,] ResultTiles { get; private set; }
        private LinkedList<TilesWithCoord> _possibleNextTiles = new LinkedList<TilesWithCoord>();
        private int _tileSize;
        private readonly int _sizeX, _sizeY;
        public TileGrid(TilesHandler tilesHandler, int sizeX, int sizeY) {
            _sizeX = sizeX;
            _sizeY = sizeY;
            _tiles = new LinkedList<Tile>[sizeX, sizeY];
            _addedToPossible = new bool[sizeX, sizeY];
            ResultTiles = new Tile[sizeX, sizeY];
            Clear(tilesHandler);
            _tileSize = tilesHandler.Tiles.First().Size;
        }

        public void AddRandom()
        {
            Random random = new Random();
            int randomIndexX = random.Next(_sizeX);
            int randomIndexY = random.Next(_sizeY);

            Add(_tiles[randomIndexX, randomIndexY].GetRandomElement(), randomIndexX, randomIndexY);
        }

        public SixLabors.ImageSharp.Image<Rgba32> GetImage()
        {
            SixLabors.ImageSharp.Image<Rgba32> result 
                = new SixLabors.ImageSharp.Image<Rgba32>(_sizeX*_tileSize, _sizeY*_tileSize);
            for(int i=0; i<_sizeX; ++i)
            {
                for (int j = 0; j < _sizeY; ++j)
                {
                    if (ResultTiles[i, j] != null)
                    {
                        SixLabors.ImageSharp.Point pos = new SixLabors.ImageSharp.Point(i*_tileSize, j*_tileSize);
                        result.Mutate(
                            ctx => ctx.DrawImage(ResultTiles[i, j].Img, pos, 1.0f));
                    }
                }
            }
            
            return result;
        }

        private void Clear(TilesHandler tileHndl)
        {
            for(int i=0; i<_sizeX; i++)
            {
                for (int j = 0; j < _sizeY; j++)
                {
                    _tiles[i, j] = new LinkedList<Tile>();
                    _addedToPossible[i, j] = false;
                    foreach (var tile in tileHndl.Tiles)
                    {
                        _tiles[i, j].AddLast(tile);
                    }
                }
            }
        }

        public void Add(Tile tile, int x, int y)
        {
            if (ResultTiles[x,y]==null)
            {
                ResultTiles[x, y] = tile;
            }
            RemoveNotAllowedNeighbours(tile, x, y);
            AddPossibleTiles(x, y);
        }

        public void GenNextStep()
        {
            if (_possibleNextTiles.Count > 0)
            {
                int min = _possibleNextTiles.First().Tiles.Count;
                TilesWithCoord nextList = _possibleNextTiles.First();
                foreach (TilesWithCoord tileList in _possibleNextTiles)
                {
                    if (min > tileList.Tiles.Count)
                    {
                        min = tileList.Tiles.Count;
                        nextList = tileList;
                    }
                }
                _possibleNextTiles.Remove(nextList);

                bool isFind = false;
                int findTry = 0;
                Tile tile = nextList.Tiles.GetRandomElement(); ;
                while (!isFind && findTry < _findTry)
                {
                    if (CheckNeighbours(tile, nextList.X, nextList.Y))
                    {
                        isFind = true;
                    }
                    else
                    {
                        tile = nextList.Tiles.GetRandomElement();
                        findTry++;
                    }
                }
                if (!isFind)
                {
                    //throw new Exception("Suitable tile not found.");
                }
                else
                {
                    Add(tile, nextList.X, nextList.Y);
                }

            }
        }

        private bool CheckNeighbours(Tile tile, int x, int y)
        {
            
            
            
            
            if (CanAddToNextTiles(x - 1, y) && ResultTiles[x - 1, y] == null)
            {
                LinkedList<Tile> tilesLeft = _tiles[x - 1, y].Clone();
                tilesLeft.RemoveElementsByCondition(x => !tile.AllowedTilesLeft.Contains(x));
                if (tilesLeft.Count == 0)
                {
                    return false;
                }
            }
            if (CanAddToNextTiles(x + 1, y) && ResultTiles[x + 1, y] == null)
            {
                LinkedList<Tile> tilesRight = _tiles[x + 1, y].Clone();
                tilesRight.RemoveElementsByCondition(x => !tile.AllowedTilesRight.Contains(x));
                if(tilesRight.Count == 0)
                {
                    return false;
                }
            }
            if (CanAddToNextTiles(x, y - 1) && ResultTiles[x, y - 1] == null)
            {
                LinkedList<Tile> tilesUp = _tiles[x, y - 1].Clone();
                tilesUp.RemoveElementsByCondition(x => !tile.AllowedTilesUp.Contains(x));
                if (tilesUp.Count == 0)
                {
                    return false;
                }
            }
            if (CanAddToNextTiles(x, y + 1) && ResultTiles[x, y + 1] == null)
            {
                LinkedList<Tile> tilesDown = _tiles[x, y + 1].Clone();
                tilesDown.RemoveElementsByCondition(x => !tile.AllowedTilesDown.Contains(x));
                if (tilesDown.Count == 0)
                {
                    return false;
                }
            }

            return true;
        }

        private void AddPossibleTiles(int x, int y)
        {
            if (CanAddToNextTiles(x - 1, y) && ResultTiles[x - 1, y] == null && !_addedToPossible[x-1, y])
            {
                _addedToPossible[x-1, y] = true;
                _possibleNextTiles.AddLast(
                    new TilesWithCoord { 
                        Tiles = _tiles[x - 1, y], 
                        X = x-1, 
                        Y= y 
                    });
            }
            if (CanAddToNextTiles(x + 1, y) && ResultTiles[x + 1, y] == null && !_addedToPossible[x+1, y])
            {
                _addedToPossible[x+1, y] = true;
                _possibleNextTiles.AddLast(
                    new TilesWithCoord { 
                        Tiles = _tiles[x + 1, y], 
                        X = x + 1, 
                        Y = y 
                    });
            }
            if (CanAddToNextTiles(x, y + 1) && ResultTiles[x, y + 1] == null && !_addedToPossible[x, y+1])
            {
                _addedToPossible[x, y+1] = true;
                _possibleNextTiles.AddLast(
                    new TilesWithCoord { 
                        Tiles = _tiles[x, y+1], 
                        X = x, 
                        Y = y+1
                    });
            }
            if (CanAddToNextTiles(x, y - 1) && ResultTiles[x, y - 1] == null && !_addedToPossible[x, y - 1])
            {
                _addedToPossible[x, y - 1] = true;
                _possibleNextTiles.AddLast(
                    new TilesWithCoord { 
                        Tiles = _tiles[x, y-1], 
                        X = x, 
                        Y = y-1 
                    });
            }
        }

        private void RemoveNotAllowedNeighbours(Tile tile, int x, int y)
        {
            if(tile != null)
            {
                if (CanAddToNextTiles(x-1, y) && ResultTiles[x - 1,y] == null)
                {
                    _tiles[x-1, y].RemoveElementsByCondition(x => !tile.AllowedTilesLeft.Contains(x));   
                }
                if (CanAddToNextTiles(x+1, y) && ResultTiles[x+1, y] == null)
                {
                    _tiles[x+1, y].RemoveElementsByCondition(x => !tile.AllowedTilesRight.Contains(x));
                }
                if (CanAddToNextTiles(x, y-1) && ResultTiles[x, y-1] == null)
                {
                    _tiles[x, y - 1].RemoveElementsByCondition(x => !tile.AllowedTilesUp.Contains(x));
                }
                if (CanAddToNextTiles(x, y+1) && ResultTiles[x, y+1] == null)
                {
                    _tiles[x, y+1].RemoveElementsByCondition(x => !tile.AllowedTilesDown.Contains(x));
                }
            }
        }

        private bool CanAddToNextTiles(int x, int y)
        {
            bool result = false;
            if(x >=0 && y >= 0 && x<_sizeX && y < _sizeY && _tiles[x,y].Count>0)
            {
                result = true;
            }
            return result;
        }

        
    }
}
