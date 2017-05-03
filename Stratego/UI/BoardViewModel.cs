using System.Collections.ObjectModel;
using System.Linq;
using Stratego.Core;

namespace Stratego.UI
{
    public class BoardViewModel
    {
        public GameViewModel Game { get; }

        public BoardViewModel(GameViewModel game)
        {
            Game = game;

            for (var row = 0; row < game.Game.Board.RowCount; row++)
            for (var column = 0; column < game.Game.Board.ColumnCount; column++)
                Cells.Add(new CellViewModel(game, game.Game.Board[new Position(row, column)]));
        }

        public ObservableCollection<CellViewModel> Cells { get; } = new ObservableCollection<CellViewModel>();

        public CellViewModel PlannedMoveStart => Cells.SingleOrDefault(c => c.IsPlannedMoveStart);

        public CellViewModel this[Position position] => Cells.Single(c => c.Cell.Position == position);
    }
}