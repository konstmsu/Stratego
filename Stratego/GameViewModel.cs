﻿using System.Collections.ObjectModel;
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
            Board = new BoardViewModel(Game.Board.RowCount, Game.Board.ColumnCount);

            Place(new Flag(Game.Players[0]), new Position(1, 1));
            Place(new Flag(Game.Players[1]), new Position(8, 8));
        }

        public BoardViewModel Board { get; }

        public void Place(Piece piece, Position position)
        {
            Game.Board[position].Piece = piece;
            UpdateContents();
        }

        void UpdateContents()
        {
            for(var row = 0; row < Game.Board.RowCount; row++)
                for (var column = 0; column < Game.Board.ColumnCount; column++)
                {
                    var piece = Game.Board[new Position(row, column)].Piece;
                    var cellViewModel = Board.Rows[row].Cells[column];

                    if (piece != null)
                    {
                        cellViewModel.Content = piece.Name;
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
        public ObservableCollection<BoardRowViewModel> Rows { get; } = new ObservableCollection<BoardRowViewModel>();

        public BoardViewModel(int rowCount, int columnCount)
        {
            foreach (var row in Enumerable.Range(0, rowCount))
                Rows.Add(new BoardRowViewModel(columnCount));
        }
    }

    public class BoardRowViewModel
    {
        public ObservableCollection<CellViewModel> Cells { get; } = new ObservableCollection<CellViewModel>();

        public BoardRowViewModel(int columnCount)
        {
            foreach (var column in Enumerable.Range(0, columnCount))
                Cells.Add(new CellViewModel());
        }
    }

    public class CellViewModel : INotifyPropertyChanged
    {
        string _content;
        SolidColorBrush _color;

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

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}