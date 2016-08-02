using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Stratego.Core
{
    [TestClass]
    public class InitialSetupTests
    {
        [TestMethod]
        public void ShouldSetup()
        {
            var game = new Game(Board.CreateStandard());
            var setup = KnownSetups.VincentDeboer;

            for (var player = 0; player <= 1; player++)
            {
                InitialSetup.Setup(game, setup, player: player);
                Validation.ValidatePlayerSetup(game, setup, player: player);
            }
        }
    }
}