using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Push.Models
{
    public class Character
    {
        public Tile TileItsOn { get; set; }

        public CharacterColor Color { get; set; }
    }

    public enum CharacterColor
    {
        Black,
        White
    }
}
