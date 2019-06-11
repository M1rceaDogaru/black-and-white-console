using System;
using System.Linq;
using System.Text;
using Push.Models;

namespace Push
{
    public class Renderer
    {
        private readonly TileManager _tileManager;
        private readonly CharacterController _characterController;
        private readonly BoxManager _boxManager;

        public Renderer(TileManager manager, CharacterController characterController, BoxManager boxManager)
        {
            _tileManager = manager;
            _characterController = characterController;
            _boxManager = boxManager;
        }

        public void Update()
        {
            var canvas = GetCanvas();

            PlotBoxes(canvas);
            PlotCharacter(canvas);            
            Render(canvas);
        }

        private void PlotBoxes(char[,] canvas)
        {
            foreach (var box in _boxManager.GetBoxes())
            {
                canvas[box.TileItsOn.PositionX, box.TileItsOn.PositionY] = box.Color == BoxColor.Red ? 'O' : 'U';
            }
        }

        private void PlotCharacter(char[,] canvas)
        {
            var character = _characterController.GetCurrentCharacter();
            canvas[character.TileItsOn.PositionX, character.TileItsOn.PositionY] = 'X';
        }

        private void Render(char[,] canvas)
        {
            Console.Clear();
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
            Console.WriteLine($"Your character is {character.Color.ToString().ToLower()}.");
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

        private char GetCharFromTile(Tile tile)
        {
            if (tile is Switch) return 'S';
            if (tile is Start) return 'A';
            if (tile is End) return 'Z';
            if (tile.TileType == TileType.Black) return 'B';
            if (tile.TileType == TileType.White) return 'W';
            return ' ';
        }
    }
}
