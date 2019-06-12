using System;
using System.Diagnostics;
using System.IO;

namespace Push
{
    class Program
    {
        private static TileManager _tileManager;
        private static BoxManager _boxManager;

        private static MapLoader _mapLoader;
        private static CharacterController _characterController;
        private static Renderer _renderer;
        private static EndTileVictoryCondition _victoryCondition;

        private static Stopwatch _stopWatch;

        static void Main(string[] args)
        {
            _tileManager = new TileManager();
            _boxManager = new BoxManager();
            _mapLoader = new MapLoader(_tileManager, _boxManager);
            _characterController = new CharacterController(_tileManager, _boxManager);
            _victoryCondition = new EndTileVictoryCondition(_characterController);
            _renderer = new Renderer(_tileManager, _characterController, _boxManager);

            Splash();

            var currentLevel = 1;
            LoadLevel(currentLevel);
            _stopWatch = Stopwatch.StartNew();

            while(true)
            {
                if (_victoryCondition.IsMet())
                {
                    currentLevel += 1;
                    var levelLoaded = LoadLevel(currentLevel);
                    if (!levelLoaded)
                    {
                        EndGame();
                        return;
                    }
                }

                _renderer.Update(currentLevel, _stopWatch.Elapsed.TotalSeconds);

                var key = Console.ReadKey();
                var keyChar = char.ToLower(key.KeyChar);
                if (keyChar == 'r')
                {
                    LoadLevel(currentLevel);
                }
                else
                {
                    _characterController.Update(keyChar);
                }
            }
        }

        private static void EndGame()
        {
            _stopWatch.Stop();
            Console.Clear();
            Console.WriteLine("CONGRATULATIONS! YOU FINISHED THE PUZZLE!");
            Console.WriteLine($"YOUR TIME : {_stopWatch.Elapsed.TotalSeconds:0.00} seconds");
            Console.ReadKey();
        }

        private static bool LoadLevel(int levelIndex)
        {
            var isLevelLoaded = _mapLoader.LoadMapFrom($"levels/l{levelIndex}.txt");
            if (isLevelLoaded)
            {
                _characterController.Initialize();
            }

            return isLevelLoaded;
        }

        private static void Splash()
        {
            var splashText = File.ReadAllText("splash.txt");

            Console.Clear();
            Console.WriteLine(splashText);
            Console.ReadKey();
        }
    }
}
