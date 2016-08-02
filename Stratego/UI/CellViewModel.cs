using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Media;
using Stratego.Core;
using Stratego.Properties;

namespace Stratego.UI
{
    public sealed class CellViewModel : INotifyPropertyChanged
    {
        public readonly Cell Cell;
        readonly GameViewModel _game;

        SolidColorBrush _color;
        string _content;
        bool _isPossibleMove;
        bool _isLake;
        bool _isPlannedMoveStart;
        bool _isPossibleAttack;
        bool _isMouseOver;
        SolidColorBrush _background;

        public CellViewModel(GameViewModel game, Cell cell)
        {
            _game = game;
            Cell = cell;
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
        void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void OnMouseOver()
        {
            HighlightCell();

            if (_game.Board.PlannedMoveStart == null)
                HighlightPossibleMoves();

            _game.UpdateContents();
        }

        void HighlightCell()
        {
            foreach (var c in _game.Board.Cells)
                c.IsMouseOver = c == this;
        }

        public void HighlightPossibleMoves()
        {
            var moves = _game.Game.GetPossibleMoves(Cell.Position);

            foreach (var c in _game.Board.Cells)
            {
                c.IsPossibleMove = moves.Contains(c.Cell.Position);
                c.IsPossibleAttack = c.IsPossibleMove && Piece != null && c.Piece != null && c.Piece.Owner != Piece.Owner;
            }
        }

        Piece Piece => _game.Game.Board[Cell.Position].Piece;

        public bool IsPlannedMoveStart
        {
            get { return _isPlannedMoveStart; }
            set
            {
                if (value == _isPlannedMoveStart) return;
                _isPlannedMoveStart = value;
                OnPropertyChanged();
            }
        }

        public bool IsMouseOver
        {
            get { return _isMouseOver; }
            set
            {
                if (_isMouseOver == value) return;
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

        public bool IsPossibleAttack
        {
            get { return _isPossibleAttack; }
            set
            {
                if (value == _isPossibleAttack) return;
                _isPossibleAttack = value;
                OnPropertyChanged();
            }
        }

        public SolidColorBrush Background
        {
            get { return _background; }
            set
            {
                if (value.Equals(_background)) return;
                _background = value;
                OnPropertyChanged();
            }
        }

        public void OnClick()
        {
            ToggleAsPlannedMoveStart();
        }

        public void ToggleAsPlannedMoveStart()
        {
            foreach (var c in _game.Board.Cells)
                c.IsPlannedMoveStart = c == this && c.Piece != null && !c.IsPlannedMoveStart;

            HighlightPossibleMoves();

            _game.UpdateContents();
        }

        public SolidColorBrush GetHighlighting()
        {
            if (IsMouseOver)
            {
                if (IsLake)
                    return KnownColors.LakeMouseOver;

                if (IsPlannedMoveStart)
                    return KnownColors.PlannedMoveStartMouseOver;

                if (IsPossibleAttack)
                    return KnownColors.PossibleAttackMouseOver;

                if (IsPossibleMove)
                    return KnownColors.PossibleMoveMouseOver;

                return KnownColors.EmptyMouseOver;
            }

            if (IsLake)
                return KnownColors.Lake;

            if (IsPlannedMoveStart)
                return KnownColors.PlannedMoveStart;

            if (IsPossibleAttack)
                return KnownColors.PossibleAttack;

            if (IsPossibleMove)
                return KnownColors.PossibleMove;

            return Brushes.Transparent;
        }
    }
}