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

        public static Position operator +(Position left, Position right)
        {
            return new Position(left.Row + right.Row, left.Column + right.Column);
        }

        public static Position operator *(Position left, int right)
        {
            return new Position(left.Row * right, left.Column * right);
        }

        public override string ToString()
        {
            return $"({Row},{Column})";
        }
    }
}