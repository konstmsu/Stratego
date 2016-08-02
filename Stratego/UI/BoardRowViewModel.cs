using System.Collections.ObjectModel;
using System.Linq;
using Stratego.Core;

namespace Stratego.UI
{
    public class BoardRowViewModel
    {
        public BoardRowViewModel(GameViewModel game, int row)
        {
            foreach (var column in Enumerable.Range(0, game.Game.Board.ColumnCount))
                Cells.Add(new CellViewModel(game, game.Game.Board[new Position(row, column)]));
        }

        public ObservableCollection<CellViewModel> Cells { get; } = new ObservableCollection<CellViewModel>();
    }
}