using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Stratego.Core;

namespace Stratego.AI
{
    [TestClass]
    public class ComputerPlayerTests
    {
        [TestMethod]
        public void ShouldSuggestMove()
        {
            var game = GameFactory.CreateWithDefaultSetup();
            var player = new ComputerPlayer();
            var move = player.SuggestMove(game, game.Players[0]);
            game.Board[move.From].Piece.Owner.Should().Be(game.Players[0]);
            game.GetPossibleMoves(move.From).Should().Contain(move.To);
        }
    }
}
