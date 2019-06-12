using System;
using System.Linq;
using System.Text;
using Push.Models;

namespace Push
{
    public class Renderer
    {
        private const char PlayerChar = '@';
        private const char WhiteChar = '·';
        private const char BlackChar = '+';
        private const char RedBoxChar = '#';
        private const char BlueBoxChar = '%';
        private const char StartChar = '·';
        private const char EndChar = 'X';
        private const char SwitchChar = 'S';
        private const char EmptyChar = '0';

        private readonly TileManager _tileManager;
        private readonly CharacterController _characterController;
        private readonly BoxManager _boxManager;

        public Renderer(TileManager manager, CharacterController characterController, BoxManager boxManager)
        {
            _tileManager = manager;
            _characterController = characterController;
            _boxManager = boxManager;
        }

        public void Update(int level, double elapsedSeconds)
        {
            var canvas = GetCanvas();

            PlotBoxes(canvas);
            PlotCharacter(canvas);            
            Render(canvas, level, elapsedSeconds);            
        }

        private void Render(char[,] canvas, int level, double elapsedSeconds)
        {
            Console.Clear();
            Console.WriteLine($"========LEVEL {level}========");
            Console.WriteLine($"Elapsed time -> {elapsedSeconds:0.00} seconds{Environment.NewLine}");

            Console.WriteLine($"{_tileManager.GetLevelMessage()}{System.Environment.NewLine}");

            for (var y = 0; y <= canvas.GetUpperBound(1); y++)
            {
                var line = new StringBuilder();
                for (var x = 0; x <= canvas.GetUpperBound(0); x++)
                {
                    line.Append(canvas[x, y]);
                }
                Console.WriteLine(line.ToString());
            }

            var character = _characterController.GetCurrentCharacter();
            Console.WriteLine($"Your character is {character.Color.ToString().ToLower()} and can walk on {(character.Color == CharacterColor.Black ? WhiteChar : BlackChar)} tiles.");
            if (character.TileItsOn is Switch)
            {
                Console.WriteLine("Your character is on a switch. Press SPACE to change its color.");
            }
        }

        private char[,] GetCanvas()
        {
            var allTiles = _tileManager.GetTiles();
            var renderLines = allTiles.GroupBy(t => t.PositionY).OrderBy(g => g.Key);

            var canvas = new char[allTiles.Max(t => t.PositionX) + 1, renderLines.Count() + 1];

            foreach (var line in renderLines)
            {
                var columns = line.OrderBy(t => t.PositionX);
                foreach (var tile in columns)
                {
                    ComputeTile(canvas, tile);
                }
            }

            return canvas;
        }

        private void ComputeTile(char[,] canvas, Tile tile)
        {
            canvas[tile.PositionX, tile.PositionY] = GetCharFromTile(tile);
        }

        private void PlotBoxes(char[,] canvas)
        {
            foreach (var box in _boxManager.GetBoxes())
            {
                canvas[box.TileItsOn.PositionX, box.TileItsOn.PositionY] = box.Color == BoxColor.Red ? RedBoxChar : BlueBoxChar;
            }
        }

        private void PlotCharacter(char[,] canvas)
        {
            var character = _characterController.GetCurrentCharacter();
            canvas[character.TileItsOn.PositionX, character.TileItsOn.PositionY] = PlayerChar;
        }

        private char GetCharFromTile(Tile tile)
        {
            if (tile is Switch) return SwitchChar;
            if (tile is Start) return StartChar;
            if (tile is End) return EndChar;
            if (tile.TileType == TileType.Black) return BlackChar;
            if (tile.TileType == TileType.White) return WhiteChar;
            return EmptyChar;
        }
    }
}
