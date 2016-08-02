using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Stratego.Core;

namespace Stratego.UI
{
    public class BoardViewModel
    {
        public BoardViewModel(GameViewModel game)
        {
            foreach (var row in Enumerable.Range(0, game.Game.Board.RowCount))
                Rows.Add(new BoardRowViewModel(game, row));
        }

        public ObservableCollection<BoardRowViewModel> Rows { get; } = new ObservableCollection<BoardRowViewModel>();
        public IEnumerable<CellViewModel> Cells => Rows.SelectMany(r => r.Cells);
        public CellViewModel PlannedMoveStart => Cells.SingleOrDefault(c => c.IsPlannedMoveStart);

        public CellViewModel this[Position position] => Rows[position.Row].Cells[position.Column];
    }
}