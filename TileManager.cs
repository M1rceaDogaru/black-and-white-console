using Push.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Push
{
    public class TileManager
    {
        private readonly List<Tile> _tiles;
        private StringBuilder _levelMessage;

        public TileManager()
        {
            _tiles = new List<Tile>();
            _levelMessage = new StringBuilder();
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
            _levelMessage.Clear();
        }

        public void AddLevelMessage(string message)
        {
            _levelMessage.AppendLine(message);
        }

        public string GetLevelMessage()
        {
            return _levelMessage.ToString();
        }
    }
}
