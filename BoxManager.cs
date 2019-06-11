using Push.Models;
using System.Collections.Generic;
using System.Linq;

namespace Push
{
    public class BoxManager
    {
        private readonly List<Box> _boxes;

        public BoxManager()
        {
            _boxes = new List<Box>();
        }

        public void AddBox(Box box)
        {
            _boxes.Add(box);
        }

        public void Clear()
        {
            _boxes.Clear();
        }

        public List<Box> GetBoxes()
        {
            return _boxes.ToList();
        }

        public Box GetBoxForTile(Tile tile)
        {
            return _boxes.FirstOrDefault(b => b.TileItsOn == tile);
        }
    }
}
