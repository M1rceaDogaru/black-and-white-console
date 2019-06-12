using Push.Models;
using System;
using System.IO;

namespace Push
{
    public class MapLoader
    {
        private readonly TileManager _tileManager;
        private readonly BoxManager _boxManager;

        public MapLoader(TileManager tileManager, BoxManager boxManager)
        {
            _tileManager = tileManager;
            _boxManager = boxManager;
        }

        public bool LoadMapFrom(string filePath)
        {
            if (!File.Exists(filePath))
            {
                return false;
            }

            var fileContent = File.ReadAllText(filePath);
            var fileLines = fileContent.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

            _tileManager.Clear();
            _boxManager.Clear();
            var positionY = 0;

            foreach (var line in fileLines)
            {
                if (line.StartsWith("-"))
                {
                    _tileManager.AddLevelMessage(line);
                    continue;
                }

                var positionX = 0;
                foreach (var character in line)
                {
                    AddTile(character, positionX, positionY);
                    positionX++;
                }

                positionY++;
            }

            return true;
        }

        private void AddTile(char character, int positionX, int positionY)
        {
            switch (character)
            {
                case 'B':
                    AddTile(positionX, positionY, TileType.Black);
                    break;
                case 'W':
                    AddTile(positionX, positionY, TileType.White);
                    break;
                case 'S':
                    AddSwitch(positionX, positionY, TileType.White);
                    break;
                case 'A':
                    AddStart(positionX, positionY);
                    break;
                case 'Y':
                    AddEnd(positionX, positionY, TileType.White);
                    break;
                case 'Z':
                    AddEnd(positionX, positionY, TileType.Black);
                    break;
                case '#':
                    AddTileWithBox(positionX, positionY, BoxColor.Red, TileType.White);
                    break;
                case '%':
                    AddTileWithBox(positionX, positionY, BoxColor.Blue, TileType.Black);
                    break;
                default:
                    AddTile(positionX, positionY, TileType.Null);
                    break;
            }
        }

        private void AddTileWithBox(int positionX, int positionY, BoxColor boxColor, TileType tileType)
        {
            var tile = new Tile
            {
                Id = Guid.NewGuid(),
                TileType = tileType,
                PositionX = positionX,
                PositionY = positionY
            };
            _tileManager.AddTile(tile);

            _boxManager.AddBox(new Box
            {
                Id = Guid.NewGuid(),
                Color = boxColor,
                TileItsOn = tile
            });
        }

        private void AddTile(int positionX, int positionY, TileType type)
        {
            _tileManager.AddTile(new Tile
            {
                Id = Guid.NewGuid(),
                TileType = type,
                PositionX = positionX,
                PositionY = positionY
            });
        }

        private void AddSwitch(int positionX, int positionY, TileType type)
        {
            _tileManager.AddTile(new Switch
            {
                Id = Guid.NewGuid(),
                TileType = type,
                PositionX = positionX,
                PositionY = positionY
            });
        }

        private void AddStart(int positionX, int positionY)
        {
            _tileManager.AddTile(new Start
            {
                Id = Guid.NewGuid(),
                PositionX = positionX,
                PositionY = positionY
            });
        }

        private void AddEnd(int positionX, int positionY, TileType tileType)
        {
            _tileManager.AddTile(new End
            {
                Id = Guid.NewGuid(),
                TileType = tileType,
                PositionX = positionX,
                PositionY = positionY
            });
        }
    }
}
