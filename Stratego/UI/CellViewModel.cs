using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using Stratego.Annotations;
using Stratego.Core;

namespace Stratego.UI
{
    public sealed class CellViewModel : INotifyPropertyChanged
    {
        public readonly Cell Cell;
        readonly GameViewModel _game;

        SolidColorBrush _color;
        string _pieceShortName;
        bool _isPossibleMove;
        bool _isLake;
        bool _isPlannedMoveStart;
        bool _isPossibleAttack;
        bool _isMouseOver;
        SolidColorBrush _background;
        string _pieceLongName;
        bool _isMovable;
        bool _isMoving;

        public CellViewModel(GameViewModel game, Cell cell)
        {
            _game = game;
            Cell = cell;
        }

        public string PieceShortName
        {
            get => _pieceShortName;
            set
            {
                if (value == _pieceShortName) return;
                _pieceShortName = value;
                OnPropertyChanged();
            }
        }

        public SolidColorBrush Color
        {
            get => _color;
            set
            {
                if (value.Equals(_color)) return;
                _color = value;
                OnPropertyChanged();
            }
        }

        public bool IsLake
        {
            get => _isLake;
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
            get => _isPlannedMoveStart;
            set
            {
                if (value == _isPlannedMoveStart) return;
                _isPlannedMoveStart = value;
                OnPropertyChanged();
            }
        }

        public bool IsMouseOver
        {
            get => _isMouseOver;
            set
            {
                if (_isMouseOver == value) return;
                _isMouseOver = value;
                OnPropertyChanged();
            }
        }

        public bool IsPossibleMove
        {
            get => _isPossibleMove;
            set
            {
                if (value == _isPossibleMove) return;
                _isPossibleMove = value;
                OnPropertyChanged();
            }
        }

        public bool IsPossibleAttack
        {
            get => _isPossibleAttack;
            set
            {
                if (value == _isPossibleAttack) return;
                _isPossibleAttack = value;
                OnPropertyChanged();
            }
        }

        public SolidColorBrush Background
        {
            get => _background;
            set
            {
                if (value.Equals(_background)) return;
                _background = value;
                OnPropertyChanged();
            }
        }

        public string PieceLongName
        {
            get => _pieceLongName;
            set
            {
                if (value == _pieceLongName) return;
                _pieceLongName = value;
                OnPropertyChanged();
            }
        }

        public bool IsMovable
        {
            get => _isMovable;
            set
            {
                if (value == _isMovable) return;
                _isMovable = value;
                OnPropertyChanged();
            }
        }

        public bool IsMoving
        {
            get => _isMoving;
            set
            {
                if (value == _isMoving) return;
                _isMoving = value;
                OnPropertyChanged();
            }
        }

        public void OnClick(FrameworkElement frameworkElement = null)
        {
            if (IsPossibleMove)
                MoveHere(frameworkElement);
            else
                ToggleAsPlannedMoveStart();
        }

        void MoveHere(FrameworkElement frameworkElement)
        {
            IsMoving = true;

            var originalPosition = _game.Board.PlannedMoveStart.Cell.Position;

            _game.Game.Move(originalPosition, Cell.Position);

            foreach (var c in _game.Board.Cells)
            {
                c.IsPlannedMoveStart = false;
                c.IsPossibleMove = false;
                c.IsPossibleAttack = false;
            }

            _game.UpdateContents();
            _game.OnMoveComplete();


            var storyboard = (Storyboard)frameworkElement.FindResource("MoveStoryboard");
            frameworkElement.RenderTransform = new TranslateTransform
            {
                X = frameworkElement.ActualWidth * (originalPosition.Column - Cell.Position.Column),
                Y = frameworkElement.ActualHeight * (originalPosition.Row - Cell.Position.Row)
            };
            Storyboard.SetTarget(storyboard, frameworkElement);
            storyboard.Begin();

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

            if (IsMovable)
                return KnownColors.Movable;

            return KnownColors.NotMovable;
        }
    }
}