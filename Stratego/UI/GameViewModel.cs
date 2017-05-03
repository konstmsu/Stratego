using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using System.Windows.Media;
using Stratego.Core;
using Stratego.Core.Utility;
using Stratego.UI.Utility;
using Stratego.AI;

namespace Stratego.UI
{
    public class GameViewModel
    {
        public readonly Game Game;
        public List<IPlayer> Players { get; set; }

        public GameViewModel(Game game)
        {
            Game = game;
            CancelPlannedMoveStart = new DelegateCommand(() =>
            {
                Board.PlannedMoveStart?.ToggleAsPlannedMoveStart();
                Board.Cells.SingleOrDefault(c => c.IsMouseOver)?.HighlightPossibleMoves();
                UpdateContents();
            });
            Board = new BoardViewModel(this);
            UpdateContents();

            Players = new IPlayer[]
            {
                new HumanPlayer(),
                new HumanPlayer(), 
            }.ToList();
        }

        public BoardViewModel Board { get; }

        public void UpdateContents()
        {
            foreach (var cellViewModel in Board.Cells)
            {
                var cell = Game.Board[cellViewModel.Cell.Position];
                var piece = cell.Piece;

                cellViewModel.IsMovable = Game.IsMovable(cell.Position);

                cellViewModel.IsLake = cell.IsLake;

                if (piece == null)
                {
                    cellViewModel.PieceShortName = null;
                    cellViewModel.PieceLongName = null;
                }
                else
                {
                    cellViewModel.PieceShortName = piece.ShortDisplayName;
                    cellViewModel.PieceLongName = piece.Name;
                    cellViewModel.Color = GetColor(piece.Owner);
                }

                cellViewModel.Background = cellViewModel.GetHighlighting();
            }
        }

        public SolidColorBrush GetColor(Player player)
        {
            return KnownColors.Players[Game.Players.IndexOf(player)];
        }

        public ICommand CancelPlannedMoveStart { get; }
        public event Action<MoveResult> Animate;

        public void OnMoveComplete(MoveResult result)
        {
            UpdateContents();

            Animate?.Invoke(result);

            var move = Players[Game.CurrentPlayerIndex].SuggestMove(Game, Game.CurrentPlayer);

            if (move != null)
                Board[move.From].MoveTo(move.To);
        }
    }
}