using System.Runtime;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Stratego.Core;

namespace Stratego
{
    [TestClass]
    public class ScoutTests
    {
        [TestMethod]
        public void ShouldMoveFar()
        {
            var board = new Board(4, 5, _ => false);

            new Scout(null).GetPossibleMoves(board, new Position(1, 2)).Should().BeEquivalentTo(new[]
            {
                new Position(0, 2),
                new Position(2, 2),
                new Position(3, 2),

                new Position(1, 0),
                new Position(1, 1),
                new Position(1, 3),
                new Position(1, 4)
            });
        }

        [TestMethod]
        public void ShouldStopAtOtherPieces()
        {
            var board = new Board(1, 10, _ => false);
            board[new Position(0, 3)].Piece = new Flag(null);

            new Scout(null).GetPossibleMoves(board, new Position(0, 0)).Should().BeEquivalentTo(new[]
            {
                new Position(0, 1),
                new Position(0, 2)
            });
        }

        [TestMethod]
        public void ShouldStopAtLake()
        {
            var board = new Board(1, 10, p => p.Column == 2);

            new Scout(null).GetPossibleMoves(board, new Position(0, 0)).Should().BeEquivalentTo(new[]
            {
                new Position(0, 1)
            });
        }
    }
}
