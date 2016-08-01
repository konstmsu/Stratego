using System;
using System.Collections.Generic;

namespace Stratego.Core
{
    public class Board
    {
        readonly List<List<Cell>> _cells = new List<List<Cell>>();

        public Board(int rowCount, int columnCount, Func<Position, bool> isLake)
        {
            for (var r = 0; r < rowCount; r++)
            {
                var row = new List<Cell>();

                for (var c = 0; c < columnCount; c++)
                    row.Add(new Cell(isLake(new Position(r, c))));

                _cells.Add(row);
            }
        }

        public Cell this[Position position] => position.Row < 0 || position.Column < 0 || position.Row >= RowCount || position.Column >= ColumnCount 
            ? null 
            : _cells[position.Row][position.Column];

        public int RowCount => _cells.Count;
        public int ColumnCount => _cells[0].Count;

        public static Board CreateStandard()
        {
            return new Board(10, 10, p => (p.Row == 4 || p.Row == 5) && (p.Column == 2 || p.Column == 3 || p.Column == 6 || p.Column == 7));
        }
    }

    public class Cell
    {
        public readonly bool IsLake;
        Piece _piece;

        public Cell(bool isLake)
        {
            IsLake = isLake;
        }

        public Piece Piece
        {
            get { return _piece; }

            set
            {
                if (IsLake)
                    throw new InvalidOperationException();

                _piece = value;
            }
        }
    }
}