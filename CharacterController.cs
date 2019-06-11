using Push.Models;
using System;
using System.Linq;

namespace Push
{
    public class CharacterController
    {
        private readonly TileManager _manager;
        private readonly BoxManager _boxManager;
        private Character _player;

        public CharacterController(TileManager manager, BoxManager boxManager)
        {
            _manager = manager;
            _boxManager = boxManager;
        }

        public void Initialize()
        {
            _player = new Character();
            var startTile = _manager.GetTiles().FirstOrDefault(t => t is Start);
            _player.TileItsOn = startTile ?? throw new Exception("No start tile found");
            _player.Color = CharacterColor.Black;
        }

        public Character GetCurrentCharacter()
        {
            return _player;
        }

        public void Update(char input)
        {
            if (input == ' ')
            {
                TryChangeColor();
                return;
            }

            var direction = GetDirectionFromInput(input);
            if (direction == MoveDirection.Undefined) return;

            var destinationTile = GetDestinationTileFromDirection(_player.TileItsOn, direction);
            if (destinationTile == null) return;

            var boxOnDestinationTile = _boxManager.GetBoxForTile(destinationTile);

            if (IsValidDestination(direction, destinationTile, boxOnDestinationTile))
            {
                MoveTo(destinationTile);
            }
        }

        private void TryChangeColor()
        {
            if (_player.TileItsOn is Switch)
            {
                _player.Color = _player.Color == CharacterColor.Black ? CharacterColor.White : CharacterColor.Black;
            }
        }

        private void MoveTo(Tile destinationTile)
        {
            _player.TileItsOn = destinationTile;
        }

        private bool IsValidDestination(MoveDirection direction, Tile tile, Box boxOnDestinationTile)
        {            
            if (boxOnDestinationTile != null && IsPushableBox(boxOnDestinationTile))
            {
                var boxDestinationTile = GetDestinationTileFromDirection(boxOnDestinationTile.TileItsOn, direction);
                if (boxDestinationTile == null)
                {
                    return false;
                }

                if ((boxOnDestinationTile.Color == BoxColor.Red && boxDestinationTile.TileType == TileType.Black) ||
                    (boxOnDestinationTile.Color == BoxColor.Blue && boxDestinationTile.TileType == TileType.White))
                {
                    return false;
                }

                if (_boxManager.GetBoxForTile(boxDestinationTile) != null)
                {
                    return false;
                }

                MoveBox(boxOnDestinationTile, boxDestinationTile);
            }

            if (_player.Color == CharacterColor.Black)
            {
                return tile is Start || tile is Switch || tile.TileType == TileType.White || (boxOnDestinationTile != null && boxOnDestinationTile.Color == BoxColor.Blue);
            }

            if (_player.Color == CharacterColor.White)
            {
                return tile is End || tile is Switch || tile.TileType == TileType.Black || (boxOnDestinationTile != null && boxOnDestinationTile.Color == BoxColor.Red);
            }

            return false;
        }

        private void MoveBox(Box boxOnDestinationTile, Tile boxDestinationTile)
        {
            boxOnDestinationTile.TileItsOn = boxDestinationTile;
        }

        private bool IsPushableBox(Box boxOnDestinationTile)
        {
            return (_player.Color == CharacterColor.Black && boxOnDestinationTile.Color == BoxColor.Red) ||
                (_player.Color == CharacterColor.White && boxOnDestinationTile.Color == BoxColor.Blue);
        }

        private Tile GetDestinationTileFromDirection(Tile startTile, MoveDirection direction)
        {
            var destinationX = startTile.PositionX;
            var destinationY = startTile.PositionY;

            if (direction == MoveDirection.Up) destinationY -= 1;
            if (direction == MoveDirection.Down) destinationY += 1;
            if (direction == MoveDirection.Left) destinationX -= 1;
            if (direction == MoveDirection.Right) destinationX += 1;

            return _manager.GetTileAt(destinationX, destinationY);
        }

        private MoveDirection GetDirectionFromInput(char input)
        {
            if (input == 'w') return MoveDirection.Up;
            if (input == 's') return MoveDirection.Down;
            if (input == 'a') return MoveDirection.Left;
            if (input == 'd') return MoveDirection.Right;
            return MoveDirection.Undefined;
        }

        internal enum MoveDirection
        {
            Undefined,
            Up,
            Down,
            Left,
            Right
        }
    }
}
