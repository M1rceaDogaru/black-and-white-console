namespace Push.Models
{
    public class Box : EntityBase
    {
        public Tile TileItsOn { get; set; }

        public BoxColor Color { get; set; }
    }

    public enum BoxColor
    {
        Red,
        Blue
    }
}
