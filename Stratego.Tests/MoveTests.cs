using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Stratego.Core;

namespace Stratego
{
    [TestClass]
    public class MoveTests
    {
        [TestMethod]
        public void FlagAndBombShouldNotMove()
        {
            var board = new Board(3, 3, _ => false);
            new Flag(null).GetPossibleMoves(board, new Position(1, 1)).Should().BeEmpty();
            new Bomb(null).GetPossibleMoves(board, new Position(1, 1)).Should().BeEmpty();
        }
    }
}