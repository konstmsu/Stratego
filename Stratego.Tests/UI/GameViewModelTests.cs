using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Stratego.Core;

namespace Stratego.UI
{
    [TestClass]
    public class GameViewModelTests
    {
        [TestMethod]
        public void ShouldHighlightPossibleMoves()
        {
            var game = new Game(Board.CreateStandard());
            var p = new Position(1, 2);
            game.Board[p].Piece = new Spy(game.Players[0]);

            var viewModel = new GameViewModel(game);

            viewModel.Board[p].OnClick();

            var cellPositions = GetCellPositions(viewModel);

            cellPositions(c => c.IsPlannedMoveStart).Should().BeEquivalentTo(new[]
            {
                p
            });

            cellPositions(c => c.IsPossibleMove).Should().BeEquivalentTo(new[]
            {
                p + new Position(1, 0),
                p + new Position(-1, 0),
                p + new Position(0, 1),
                p + new Position(0, -1)
            });
        }

        static Func<Func<CellViewModel, bool>, IEnumerable<Position>> GetCellPositions(GameViewModel game) =>
            filter => game.Board.Cells.Where(filter).Select(c => c.Cell.Position);

        [TestMethod]
        public void ShouldHighlightAttackMoves()
        {
            var game = new Game(Board.CreateStandard());
            var p = new Position(1, 2);

            game.Board[p].Piece = new Spy(game.Players[0]);

            game.Board[p + new Position(-1, 0)].Piece = new Spy(game.Players[0]);

            game.Board[p + new Position(1, 0)].Piece = new Spy(game.Players[1]);
            game.Board[p + new Position(0, 1)].Piece = new Flag(game.Players[1]);
            game.Board[p + new Position(0, 2)].Piece = new Flag(game.Players[1]);

            var viewModel = new GameViewModel(game);

            viewModel.Board[p].OnClick();

            var cellPositions = GetCellPositions(viewModel);

            cellPositions(c => c.IsPossibleMove).Should().BeEquivalentTo(new[]
            {
                p + new Position(1, 0),
                p + new Position(0, 1),
                p + new Position(0, -1)
            });

            cellPositions(c => c.IsPossibleAttack).Should().BeEquivalentTo(new[]
            {
                p + new Position(1, 0),
                p + new Position(0, 1),
            });
        }

        [TestMethod]
        public void ShouldAssignDifferentColorsToDifferentPlayers()
        {
            var game = new Game(new Board(2, 4, p => p.Row == 1 && p.Column == 1));
            var viewModel = new GameViewModel(game);

            var p0 = new Position(0, 1);
            var p1 = new Position(0, 2);

            game.Board[p0].Piece = new Spy(game.Players[0]);
            game.Board[p1].Piece = new Spy(game.Players[1]);

            viewModel.UpdateContents();

            viewModel.Board[p0].Color.Should().Be(KnownColors.Players[0]);
            viewModel.Board[p1].Color.Should().Be(KnownColors.Players[1]);
        }

        [TestMethod]
        public void ShouldHighlightMouseOverCell()
        {
            var game = new Game(new Board(2, 4, _ => false));
            var viewModel = new GameViewModel(game);

            var p0 = new Position(0, 1);
            viewModel.Board[p0].OnMouseOver();
            GetCellPositions(viewModel)(c => c.IsMouseOver).Should().Equal(new[] { p0 });

            var p1 = new Position(1, 2);
            viewModel.Board[p1].OnMouseOver();
            GetCellPositions(viewModel)(c => c.IsMouseOver).Should().Equal(new[] { p1 });
        }

        [TestMethod]
        public void ShouldHighlightPossibleMovesOnMouseOver()
        {
            var game = new Game(new Board(2, 4, _ => false));
            var p1 = new Position(0, 0);
            game.Board[p1].Piece = new Spy(game.Players[0]);
            var viewModel = new GameViewModel(game);
            var cellPositions = GetCellPositions(viewModel);

            viewModel.Board[p1].OnMouseOver();
            cellPositions(c => c.IsMouseOver).Should().BeEquivalentTo(new[] { p1 });
            cellPositions(c => c.IsPlannedMoveStart).Should().BeEmpty();
            cellPositions(c => c.IsPossibleMove).Should().BeEquivalentTo(new[] { p1 + new Position(1, 0), p1 + new Position(0, 1) });

            viewModel.Board[p1].OnClick();
            //GetCellPositions(viewModel)(c => c.IsMouseOver).Should().;
            cellPositions(c => c.IsPlannedMoveStart).Should().BeEquivalentTo(new[] { p1 });
            //GetCellPositions(viewModel)(c => c.IsPossibleMoveStart).Should().;
            cellPositions(c => c.IsPossibleMove).Should().BeEquivalentTo(new[] { p1 + new Position(1, 0), p1 + new Position(0, 1) });

            var p2 = new Position(0, 0);
            viewModel.Board[p2].OnMouseOver();
            cellPositions(c => c.IsMouseOver).Should().BeEquivalentTo(new[] { p2 });
            cellPositions(c => c.IsPlannedMoveStart).Should().BeEquivalentTo(new[] { p1 });
            //GetCellPositions(viewModel)(c => c.IsPossibleMoveStart).Should().;
            cellPositions(c => c.IsPossibleMove).Should().BeEquivalentTo(new[] { p1 + new Position(1, 0), p1 + new Position(0, 1) });
        }

        [TestMethod]
        public void ShouldHighlightLake()
        {
            var game = new Game(new Board(1, 2, _ => true));
            var viewModel = new GameViewModel(game);

            var p1 = new Position(0, 0);
            var p2 = new Position(0, 1);
            viewModel.Board[p1].Background.Should().Be(KnownColors.Lake);
            viewModel.Board[p2].Background.Should().Be(KnownColors.Lake);

            viewModel.Board[p1].OnMouseOver();
            viewModel.Board[p1].Background.Should().Be(KnownColors.LakeMouseOver);
            viewModel.Board[p2].Background.Should().Be(KnownColors.Lake);

            viewModel.Board[p1].OnClick();
            viewModel.Board[p1].Background.Should().Be(KnownColors.LakeMouseOver);
            viewModel.Board[p2].Background.Should().Be(KnownColors.Lake);

            viewModel.Board[p2].OnMouseOver();
            viewModel.Board[p1].Background.Should().Be(KnownColors.Lake);
            viewModel.Board[p2].Background.Should().Be(KnownColors.LakeMouseOver);
        }

        [TestMethod]
        public void ShouldSetCorrectBackground()
        {
            var game = new Game(new Board(2, 2, _ => false));
            game.Board[new Position(0, 0)].Piece = new Spy(game.Players[0]);
            game.Board[new Position(1, 0)].Piece = new Spy(game.Players[1]);
            var viewModel = new GameViewModel(game);

            Action<int, int, SolidColorBrush> assert = (row, column, expectedColor) => viewModel.Board[new Position(row, column)].Background.Should().Be(expectedColor);

            viewModel.Board[new Position(1, 1)].OnMouseOver();
            assert(1, 1, KnownColors.EmptyMouseOver);

            viewModel.Board[new Position(1, 1)].OnClick();
            assert(1, 1, KnownColors.EmptyMouseOver);

            viewModel.Board[new Position(0, 0)].OnMouseOver();
            assert(1, 1, Brushes.Transparent);
            assert(0, 0, KnownColors.EmptyMouseOver);

            viewModel.Board[new Position(0, 0)].OnClick();
            assert(0, 0, KnownColors.PlannedMoveStartMouseOver);
            assert(1, 0, KnownColors.PossibleAttack);
            assert(0, 1, KnownColors.PossibleMove);

            viewModel.Board[new Position(1, 0)].OnMouseOver();
            assert(0, 0, KnownColors.PlannedMoveStart);
            assert(1, 0, KnownColors.PossibleAttackMouseOver);
            assert(0, 1, KnownColors.PossibleMove);

            viewModel.Board[new Position(0, 1)].OnMouseOver();
            assert(0, 0, KnownColors.PlannedMoveStart);
            assert(1, 0, KnownColors.PossibleAttack);
            assert(0, 1, KnownColors.PossibleMoveMouseOver);

            viewModel.Board[new Position(1, 1)].OnMouseOver();
            assert(0, 0, KnownColors.PlannedMoveStart);
            assert(1, 0, KnownColors.PossibleAttack);
            assert(0, 1, KnownColors.PossibleMove);
            assert(1, 1, KnownColors.EmptyMouseOver);

            viewModel.CancelPlannedMoveStart.Execute(null);
            assert(0, 0, Brushes.Transparent);
            assert(1, 0, Brushes.Transparent);
            assert(0, 1, Brushes.Transparent);
            assert(1, 1, KnownColors.EmptyMouseOver);
        }

        [TestMethod]
        public void ShouldCancelPlannedMove()
        {
            var game = new Game(new Board(2, 2, _ => false));
            game.Board[new Position(0, 0)].Piece = new Spy(game.Players[0]);
            game.Board[new Position(1, 0)].Piece = new Spy(game.Players[1]);
            var viewModel = new GameViewModel(game);

            var cellPositions = GetCellPositions(viewModel);

            viewModel.Board[new Position(0, 0)].OnMouseOver();
            viewModel.Board[new Position(0, 0)].OnClick();
            cellPositions(c => c.IsPlannedMoveStart).Should().Equal(new Position(0, 0));
            cellPositions(c => c.IsPossibleMove).Should().Equal(new Position(0, 1), new Position(1, 0));
            cellPositions(c => c.IsPossibleAttack).Should().Equal(new Position(1, 0));
            cellPositions(c => c.IsMouseOver).Should().Equal(new Position(0, 0));

            viewModel.Board[new Position(1, 1)].OnMouseOver();
            viewModel.Board[new Position(1, 1)].OnClick();
            cellPositions(c => c.IsPlannedMoveStart).Should().BeEmpty();
            cellPositions(c => c.IsPossibleMove).Should().BeEmpty();
            cellPositions(c => c.IsPossibleAttack).Should().BeEmpty();
            cellPositions(c => c.IsMouseOver).Should().Equal(new Position(1, 1));
        }

        class AttackTestContext
        {
            public readonly Game Game;
            public readonly Piece Attacker;
            public readonly Piece Defender;
            public readonly Position Position1 = new Position(0, 0);
            public readonly Position Position2 = new Position(1, 0);
            public readonly GameViewModel GameViewModel;

            public Piece Piece1 => GameViewModel.Board[Position1].Cell.Piece;
            public Piece Piece2 => GameViewModel.Board[Position2].Cell.Piece;

            AttackTestContext(int attackerRank, int defenderRank)
            {
                Game = new Game(new Board(2, 1, _ => false));

                Attacker = Game.Board[Position1].Piece = InitialSetup.CreatePiece(attackerRank, Game.Players[0]);
                Defender = Game.Board[Position2].Piece = InitialSetup.CreatePiece(defenderRank, Game.Players[1]);

                GameViewModel = new GameViewModel(Game);
            }

            public static AttackTestContext ModelAttack(int attackerRank, int defenderRank)
            {
                var context = new AttackTestContext(attackerRank, defenderRank);

                context.GameViewModel.Board[context.Position1].OnClick();
                context.GameViewModel.Board[context.Position2].OnClick();

                var cellPositions = GetCellPositions(context.GameViewModel);

                cellPositions(c => c.IsPlannedMoveStart).Should().BeEmpty();
                cellPositions(c => c.IsPossibleMove).Should().BeEmpty();
                cellPositions(c => c.IsPossibleAttack).Should().BeEmpty();

                return context;
            }

            public void AssertAttackerWon()
            {
                Piece1.Should().BeNull();
                Piece2.Should().Be(Attacker);
            }

            public void AssertAttackerLost()
            {
                Piece1.Should().BeNull();
                Piece2.Should().Be(Defender);
            }

            public void AssertBothDied()
            {
                Piece1.Should().BeNull();
                Piece2.Should().BeNull();
            }
        }

        [TestMethod]
        public void ShouldAttackAndKill()
        {
            AttackTestContext.ModelAttack(attackerRank: 5, defenderRank: 4).AssertAttackerWon();
        }

        [TestMethod]
        public void ShouldAttackAndDie()
        {
            AttackTestContext.ModelAttack(attackerRank: 6, defenderRank: 7).AssertAttackerLost();
        }

        [TestMethod]
        public void ShouldAttackAndBothDie()
        {
            AttackTestContext.ModelAttack(attackerRank: 6, defenderRank: 6).AssertBothDied();
        }

        [TestMethod]
        public void MinerShouldAttackMineAndWin()
        {
            AttackTestContext.ModelAttack(attackerRank: Miner.Rank, defenderRank: Bomb.Rank).AssertAttackerWon();
        }

        [TestMethod]
        public void SpyShouldAttackMarshalAndWin()
        {
            AttackTestContext.ModelAttack(attackerRank: Spy.Rank, defenderRank: OtherPiece.MarshalRank).AssertAttackerWon();
        }

        [TestMethod]
        public void MarshalShouldAttackSpyAndWin()
        {
            AttackTestContext.ModelAttack(attackerRank: OtherPiece.MarshalRank, defenderRank: Spy.Rank).AssertAttackerWon();
        }

        [TestMethod]
        public void ShouldShowPieceNames()
        {
            var min = Flag.Rank;
            var max = Bomb.Rank;

            var game = new Game(new Board(1, max - min + 1, _ => false));

            for (var rank = min; rank <= max; rank++)
                game.Board[new Position(0, rank - min)].Piece = InitialSetup.CreatePiece(rank, game.Players[1]);

            var viewModel = new GameViewModel(game);
            viewModel.Board.Cells.Select(c => c.PieceShortName).Should().Equal(new[] { "F", "S", "2", "3", "4", "5", "6", "7", "8", "9", "10", "B" });
            viewModel.Board.Cells.Select(c => c.PieceLongName).Should().Equal(new[] { "Flag", "Spy", "Scout", "Miner", "Sergeant", "Lieutenant", "Captain", "Major", "Colonel", "General", "Marshal", "Bomb" });
        }
    }
}