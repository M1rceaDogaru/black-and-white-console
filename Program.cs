using System;

namespace Push
{
    class Program
    {
        static void Main(string[] args)
        {
            var tileManager = new TileManager();
            var boxManager = new BoxManager();

            var tileLoader = new MapLoader(tileManager, boxManager);
            tileLoader.LoadMapFrom("l1.txt");

            var characterController = new CharacterController(tileManager, boxManager);
            characterController.Initialize();

            var endTileVictoryCondition = new EndTileVictoryCondition(characterController);
            var renderer = new Renderer(tileManager, characterController, boxManager);
            
            while(true)
            {
                renderer.Update();
                var key = Console.ReadKey();
                characterController.Update(key.KeyChar);

                if (endTileVictoryCondition.IsMet())
                {
                    Console.WriteLine("CONGRATS! YOU FINISHED THE LEVEL! Press any key to exit.");
                    Console.ReadKey();
                    return;
                }
            }
        }
    }
}
