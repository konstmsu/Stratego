using System;
using System.Collections.ObjectModel;
using System.Linq;
using Stratego.Annotations;

namespace Stratego.Core
{
    public class Board
    {
        readonly ReadOnlyCollection<ReadOnlyCollection<Cell>> _cells;

        public Board(int rowCount, int columnCount, Func<Position, bool> isLake)
        {
            _cells = Enumerable.Range(0, rowCount)
                .Select(r => Enumerable.Range(0, columnCount)
                    .Select(c =>
                    {
                        var position = new Position(r, c);
                        return new Cell(position, isLake(position));
                    })
                    .ToList().AsReadOnly())
                .ToList().AsReadOnly();
        }

        public Cell this[Position position]
        {
            [NotNull]
            get
            {
                if (!ContainsPosition(position))
                    throw new ArgumentOutOfRangeException($"{position} is outside of the board");

                return _cells[position.Row][position.Column];
            }
        }

        public int RowCount => _cells.Count;
        public int ColumnCount => _cells[0].Count;

        public static Board CreateStandard()
        {
            return new Board(10, 10, p => (p.Row == 4 || p.Row == 5) && (p.Column == 2 || p.Column == 3 || p.Column == 6 || p.Column == 7));
        }

        public bool ContainsPosition(Position position) => position.Row >= 0 && position.Column >= 0 && position.Row < RowCount && position.Column < ColumnCount;
    }

    public class Cell
    {
        public readonly bool IsLake;
        public readonly Position Position;
        Piece _piece;

        public Cell(Position position, bool isLake)
        {
            IsLake = isLake;
            Position = position;
        }

        [CanBeNull]
        public Piece Piece
        {
            get { return _piece; }

            set
            {
                if (IsLake)
                    throw new InvalidOperationException($"Can't put '{value?.Name}' on a lake {Position}");

                _piece = value;
            }
        }
    }
}