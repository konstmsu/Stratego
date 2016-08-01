namespace Stratego.Core
{
    public class Position
    {
        public readonly int Column;
        public readonly int Row;

        public Position(int row, int column)
        {
            Row = row;
            Column = column;
        }
    }
}