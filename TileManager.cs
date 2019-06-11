using Push.Models;
using System.Collections.Generic;
using System.Linq;

namespace Push
{
    public class TileManager
    {
        private readonly List<Tile> _tiles;

        public TileManager()
        {
            _tiles = new List<Tile>();
        }

        public void AddTile(Tile tile)
        {
            _tiles.Add(tile);
        }

        public List<Tile> GetTiles()
        {
            return _tiles.ToList();
        }

        public Tile GetTileAt(int x, int y)
        {
            return _tiles.FirstOrDefault(t => t.PositionX == x && t.PositionY == y);
        }

        public void Clear()
        {
            _tiles.Clear();
        }
    }
}
