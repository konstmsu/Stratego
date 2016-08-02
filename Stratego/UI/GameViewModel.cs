using System.Diagnostics;
using System.Windows.Input;
using System.Windows.Media;
using Stratego.Core;
using Stratego.UI.Utility;

namespace Stratego.UI
{
    public class GameViewModel
    {
        public readonly Game Game;

        public GameViewModel(Game game)
        {
            Game = game;
            CancelPlannedMoveStart = new DelegateCommand(() => Board.PlannedMoveStart?.ToggleAsPlannedMoveStart());
            Board = new BoardViewModel(this);
            UpdateContents();
        }

        public BoardViewModel Board { get; }

        void UpdateContents()
        {
            Debug.Assert(Game.Board.RowCount == Board.Rows.Count);
            foreach (var r in Board.Rows)
                Debug.Assert(Game.Board.ColumnCount == r.Cells.Count);

            foreach (var cellViewModel in Board.Cells)
            {
                var cell = Game.Board[cellViewModel.Position];
                var piece = cell.Piece;

                cellViewModel.IsLake = cell.IsLake;

                if (piece == null)
                    cellViewModel.Content = null;
                else
                {
                    cellViewModel.Content = piece.ShortDisplayName;
                    cellViewModel.Color = piece.Owner == Game.Players[0] ? Brushes.Red : Brushes.Blue;
                }
            }
        }

        public ICommand CancelPlannedMoveStart { get; }
    }
}