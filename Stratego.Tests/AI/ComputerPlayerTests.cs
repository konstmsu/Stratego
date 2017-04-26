using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Stratego.Core;
using System.Linq;

namespace Stratego.AI
{
    [TestClass]
    public class ComputerPlayerTests
    {
        [TestMethod]
        public void ShouldSuggestMove()
        {
            var game = GameFactory.CreateWithDefaultSetup();
            var player = new ComputerPlayer(game.Players[0]);
            var move = player.SuggestMove(game);
            game.Board[move.From].Piece.Owner.Should().Be(player.Player);
            game.GetPossibleMoves(move.From).Should().Contain(move.To);
        }
    }
}
