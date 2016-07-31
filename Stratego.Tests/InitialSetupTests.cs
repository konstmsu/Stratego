using Microsoft.VisualStudio.TestTools.UnitTesting;
using Stratego.Core;

namespace Stratego
{
    [TestClass]
    public class InitialSetupTests
    {
        [TestMethod]
        public void ShouldSetup()
        {
            var game = new Game();
            var setup = KnownSetups.VincentDeboer;

            for (var player = 0; player <= 1; player++)
            {
                InitialSetup.Setup(game, setup, player: player);
                Validation.ValidatePlayerSetup(game, setup, player: player);
            }
        }
    }
}