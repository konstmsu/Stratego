using System.Collections.Generic;

namespace Stratego.Core
{
    public class Board
    {
        readonly List<List<Cell>> _cells = new List<List<Cell>>();

        public Cell this[Position position] => _cells[position.Row][position.Column];

        public Board()
        {
            var width = 10;
            var height = 10;

            for (var lineIndex = 0; lineIndex < height; lineIndex++)
            {
                var l = new List<Cell>();

                for (var column = 0; column < width; column++)
                    l.Add(new Cell());

                _cells.Add(l);
            }
        }
    }

    public class Cell
    {
        public Piece Piece;
    }
}