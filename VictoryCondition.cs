using Push.Models;

namespace Push
{
    public class EndTileVictoryCondition
    {
        private readonly CharacterController _controller;

        public EndTileVictoryCondition(CharacterController controller)
        {
            _controller = controller;
        }

        public bool IsMet()
        {
            var character = _controller.GetCurrentCharacter();
            return character.TileItsOn is End;
        }
    }
}
