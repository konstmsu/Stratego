using System;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Stratego.Core
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

        static void ForAll(Action<Piece> test)
        {
            for (var i = 1; i <= 10; i++)
                if (i != Scout.Rank)
                    test(InitialSetup.CreatePiece(i, null));
        }

        [TestMethod]
        public void ShouldMoveAround()
        {
            var board = new Board(5, 6, _ => false);

            ForAll(p => p.GetPossibleMoves(board, new Position(2, 3)).Should().BeEquivalentTo(new[]
            {
                new Position(2, 4),
                new Position(2, 2),
                new Position(1, 3),
                new Position(3, 3)
            }));
        }

        [TestMethod]
        public void ShouldStopAtLake()
        {
            var board = new Board(5, 6, p => p.Column == 4);

            ForAll(p => p.GetPossibleMoves(board, new Position(2, 3)).Should().BeEquivalentTo(new[]
            {
                new Position(2, 2),
                new Position(1, 3),
                new Position(3, 3)
            }));
        }

        [TestMethod]
        public void ShouldStopAtOtherPieces()
        {
            var board = new Board(5, 6, p => false);
            board[new Position(3, 3)].Piece = new Flag(null);

            ForAll(p => p.GetPossibleMoves(board, new Position(2, 3)).Should().BeEquivalentTo(new[]
            {
                new Position(2, 2),
                new Position(1, 3),
                new Position(2, 4)
            }));
        }
    }
}