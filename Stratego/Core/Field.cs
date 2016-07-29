using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stratego.Core
{
    public class Field
    {
        public List<List<Cell>> Cells = new List<List<Cell>>();

        public Field()
        {
            var width = 10;
            var height = 10;

            for (var lineIndex = 0; lineIndex < height; lineIndex++)
            {
                var l = new List<Cell>();

                for (var column = 0; column < width; column++)
                    l.Add(new Cell());

                Cells.Add(l);
            }
        }
    }

    public class Cell
    {
        public Piece Piece;
    }
}
