namespace Stratego.Core
{
    public static class GameFactory
    {
        public static Game CreateWithDefaultSetup()
        {
            var game = new Game();

            InitialSetup.Setup(game, KnownSetups.VincentDeboer, 0);
            InitialSetup.Setup(game, KnownSetups.VincentDeboer, 1);

            return game;
        }
    }
}