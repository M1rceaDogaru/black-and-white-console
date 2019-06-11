namespace Push.Models
{
    public class Tile : EntityBase
    {
        public int PositionX { get; set; }

        public int PositionY { get; set; }

        public TileType TileType { get; set; }
    }

    public enum TileType
    {
        Null,
        Black,
        White        
    }
}
