using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Stratego.Core
{
    [TestClass]
    public class GameTests
    {
        [TestMethod]
        public void ShouldMove()
        {
            for (var rank = Spy.Rank; rank <= OtherPiece.MarshalRank; rank++)
            {
                var game = new Game(Board.CreateStandard());

                var piece = InitialSetup.CreatePiece(rank, game.Players[0]);

                var p1 = new Position(0, 0);
                var p2 = new Position(1, 0);

                game.Board[p1].Piece = piece;

                game.Move(p1, p2);

                game.Board[p1].Piece.Should().BeNull();
                game.Board[p2].Piece.Should().Be(piece);
            }
        }

        [TestMethod]
        public void ShouldAttackAndWin()
        {
            var c = MoveTestContext.RankBased(5, 4);

            c.Game.Move(c.P1, c.P2);

            c.Game.Board[c.P1].Piece.Should().BeNull();
            c.Game.Board[c.P2].Piece.Should().Be(c.Attacker);
        }

        [TestMethod]
        public void ShouldAttackAndLoose()
        {
            var c = MoveTestContext.RankBased(5, 8);

            c.Game.Move(c.P1, c.P2);

            c.Game.Board[c.P1].Piece.Should().BeNull();
            c.Game.Board[c.P2].Piece.Should().Be(c.Defender);
        }

        [TestMethod]
        public void ShouldAttackAndTie()
        {
            var c = MoveTestContext.RankBased(7, 7);

            c.Game.Move(c.P1, c.P2);

            c.Game.Board[c.P1].Piece.Should().BeNull();
            c.Game.Board[c.P2].Piece.Should().BeNull();
        }

        class MoveTestContext
        {
            public readonly OtherPiece Attacker;
            public readonly OtherPiece Defender;
            public readonly Game Game;
            public readonly Position P1 = new Position(1, 3);
            public readonly Position P2 = new Position(2, 3);

            MoveTestContext(Game game, OtherPiece attacker, OtherPiece defender)
            {
                Attacker = attacker;
                Defender = defender;
                Game = game;

                game.Board[P1].Piece = attacker;
                game.Board[P2].Piece = defender;
            }

            public static MoveTestContext RankBased(int attackerRank, int defenderRank)
            {
                var game = new Game(Board.CreateStandard());
                return new MoveTestContext(game, new OtherPiece(attackerRank, game.Players[0]), new OtherPiece(defenderRank, game.Players[1]));
            }
        }

        [TestMethod]
        public void BombAndFlagCantMove()
        {
            Action<Func<Game, Piece>> test = createPiece =>
            {
                var game = new Game(Board.CreateStandard());

                var p1 = new Position(1, 1);
                game.Board[p1].Piece = createPiece(game);

                new Position(0, 1).Invoking(delta => game.Move(p1, p1 + delta)).ShouldThrow<InvalidOperationException>();
            };

            test(game => new Bomb(game.Players[0]));
            test(game => new Flag(game.Players[0]));
        }

        [TestMethod]
        public void ShouldSuggestMovablePositionsBetweenTurns()
        {
            var game = new Game(new Board(3, 3, _ => false));

            game.Board[new Position(0, 0)].Piece = InitialSetup.CreatePiece(1, game.Players[0]);
            game.Board[new Position(0, 1)].Piece = InitialSetup.CreatePiece(1, game.Players[0]);
            game.Board[new Position(1, 0)].Piece = InitialSetup.CreatePiece(1, game.Players[0]);

            game.Board[new Position(2, 2)].Piece = InitialSetup.CreatePiece(1, game.Players[1]);
            game.Board[new Position(2, 1)].Piece = InitialSetup.CreatePiece(1, game.Players[1]);
            game.Board[new Position(1, 2)].Piece = InitialSetup.CreatePiece(1, game.Players[1]);

            Func<IEnumerable<Position>> getMovablePositions = () => game.Board.Cells.Where(c => game.IsMovable(c.Position)).Select(c => c.Position);

            getMovablePositions().Should().BeEquivalentTo(new[] { new Position(0, 1), new Position(1, 0) });
            game.Move(new Position(0, 1), new Position(1,1));

            getMovablePositions().Should().BeEquivalentTo(new[] { new Position(2, 1), new Position(1, 2) });
            game.Move(new Position(2, 1), new Position(2, 0));

            getMovablePositions().Should().BeEquivalentTo(new[] { new Position(0, 0), new Position(1, 1), new Position(1, 0) });
            game.Move(new Position(1, 0), new Position(2, 0));

            getMovablePositions().Should().BeEquivalentTo(new[] { new Position(2, 2), new Position(1, 2) });
        }
    }
}