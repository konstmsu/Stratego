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
            var game = new Game();
            var spy = new Spy(game.Players[0]);
            game.Board[new Position(0, 0)].Piece = spy;

            game.Move(new Position(0, 0), new Position(1, 0));

            game.Board[new Position(0, 0)].Piece.Should().BeNull();
            game.Board[new Position(1, 0)].Piece.Should().Be(spy);
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

            public MoveTestContext(Game game, OtherPiece attacker, OtherPiece defender)
            {
                Attacker = attacker;
                Defender = defender;
                Game = game;

                game.Board[P1].Piece = attacker;
                game.Board[P2].Piece = defender;
            }

            public static MoveTestContext RankBased(int attackerRank, int defenderRank)
            {
                var game = new Game();
                return new MoveTestContext(game, new OtherPiece(attackerRank, game.Players[0]), new OtherPiece(defenderRank, game.Players[1]));
            }
        }
    }
}