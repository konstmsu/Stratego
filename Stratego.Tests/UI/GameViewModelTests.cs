using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            var game = new Game();
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
            filter => game.Board.Cells.Where(filter).Select(c => c.Position);
        
        [TestMethod]
        public void ShouldHighlightAttackMoves()
        {
            var game = new Game();
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
    }
}