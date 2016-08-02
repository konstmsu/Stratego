using System.Diagnostics;
using System.Windows.Input;
using Stratego.Core;
using Stratego.Core.Utility;
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

        public void UpdateContents()
        {
            Debug.Assert(Game.Board.RowCount == Board.Rows.Count);
            foreach (var r in Board.Rows)
                Debug.Assert(Game.Board.ColumnCount == r.Cells.Count);

            foreach (var cellViewModel in Board.Cells)
            {
                var cell = Game.Board[cellViewModel.Cell.Position];
                var piece = cell.Piece;

                cellViewModel.IsLake = cell.IsLake;

                if (piece == null)
                    cellViewModel.Content = null;
                else
                {
                    cellViewModel.Content = piece.ShortDisplayName;
                    cellViewModel.Color = KnownColors.Players[Game.Players.IndexOf(piece.Owner)];
                }
            }
        }

        public ICommand CancelPlannedMoveStart { get; }
    }
}