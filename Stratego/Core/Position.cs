namespace Stratego.Core
{
    public struct Position
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