using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Media;
using Stratego.Annotations;
using Stratego.Core;

namespace Stratego
{
    public class GameViewModel
    {
        public readonly Game Game;

        public GameViewModel()
        {
            Game = new Game();
            Board = new BoardViewModel(this);

            InitialSetup.Setup(Game, KnownSetups.VincentDeboer, 0);
            InitialSetup.Setup(Game, KnownSetups.VincentDeboer, 1);

            UpdateContents();
        }

        public BoardViewModel Board { get; }

        void UpdateContents()
        {
            for (var row = 0; row < Game.Board.RowCount; row++)
                for (var column = 0; column < Game.Board.ColumnCount; column++)
                {
                    var cell = Game.Board[new Position(row, column)];
                    var piece = cell.Piece;
                    var cellViewModel = Board.Rows[row].Cells[column];

                    cellViewModel.IsLake = cell.IsLake;

                    if (piece != null)
                    {
                        cellViewModel.Content = piece.ShortDisplayName;
                        cellViewModel.Color = piece.Owner == Game.Players[0] ? Brushes.Red : Brushes.Blue;
                    }
                    else
                    {
                        cellViewModel.Content = null;
                    }
                }
        }
    }

    public class BoardViewModel
    {
        public BoardViewModel(GameViewModel game)
        {
            foreach (var row in Enumerable.Range(0, game.Game.Board.RowCount))
                Rows.Add(new BoardRowViewModel(game, row));
        }

        public ObservableCollection<BoardRowViewModel> Rows { get; } = new ObservableCollection<BoardRowViewModel>();
    }

    public class BoardRowViewModel
    {
        public BoardRowViewModel(GameViewModel game, int row)
        {
            foreach (var column in Enumerable.Range(0, game.Game.Board.ColumnCount))
                Cells.Add(new CellViewModel(game, new Position(row, column)));
        }

        public ObservableCollection<CellViewModel> Cells { get; } = new ObservableCollection<CellViewModel>();
    }

    public class CellViewModel : INotifyPropertyChanged
    {
        readonly GameViewModel _game;
        readonly Position _position;

        SolidColorBrush _color;
        string _content;
        bool _isPossibleMove;
        bool _isLake;
        bool _isMouseOver;

        public CellViewModel(GameViewModel game, Position position)
        {
            _game = game;
            _position = position;
        }

        public string Content
        {
            get { return _content; }
            set
            {
                if (value == _content) return;
                _content = value;
                OnPropertyChanged();
            }
        }

        public SolidColorBrush Color
        {
            get { return _color; }
            set
            {
                if (value.Equals(_color)) return;
                _color = value;
                OnPropertyChanged();
            }
        }

        public bool IsLake
        {
            get { return _isLake; }
            set
            {
                if (value == _isLake) return;
                _isLake = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void HighlightPossibleMoves()
        {
            var moves = _game.Game.GetPossibleMoves(_position);

            foreach (var r in _game.Board.Rows)
                foreach (var c in r.Cells)
                {
                    c.IsMouseOver = c == this;
                    c.IsPossibleMove = moves.Contains(c._position);
                }
        }

        public bool IsMouseOver
        {
            get { return _isMouseOver; }
            set
            {
                if (value == _isMouseOver) return;
                _isMouseOver = value;
                OnPropertyChanged();
            }
        }

        public bool IsPossibleMove
        {
            get { return _isPossibleMove; }
            set
            {
                if (value == _isPossibleMove) return;
                _isPossibleMove = value;
                OnPropertyChanged();
            }
        }
    }
}