using System;
using System.Collections.Generic;
using System.Linq;

namespace Stratego.Core
{
    public class Board
    {
        readonly List<List<Cell>> _cells = new List<List<Cell>>();

        public Cell this[Position position] => _cells[position.Row][position.Column];

        public Board()
        {
            var lakeRows = new[] { 4, 5 };
            var lakeColumns = new[] { 2, 3, 6, 7 };

            for (var r = 0; r < 10; r++)
            {
                var row = new List<Cell>();

                for (var c = 0; c < 10; c++)
                    row.Add(new Cell(lakeRows.Contains(r) && lakeColumns.Contains(c)));

                _cells.Add(row);
            }
        }

        public int RowCount => _cells.Count;
        public int ColumnCount => _cells[0].Count;
    }

    public class Cell
    {
        Piece _piece;
        public readonly bool IsLake;

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

        public Cell(bool isLake)
        {
            IsLake = isLake;
        }
    }
}