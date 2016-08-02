using System;
using System.Collections.Generic;
using System.Linq;
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

            viewModel.Board[p].ToggleAsPlannedMoveStart();

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

            viewModel.Board[p].HighlightPossibleMoves();

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
    }
}