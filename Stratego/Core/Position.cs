using System;

namespace Stratego.Core
{
    public struct Position : IEquatable<Position>
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

        public bool Equals(Position other)
        {
            return Column == other.Column && Row == other.Row;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is Position && Equals((Position)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Column * 397) ^ Row;
            }
        }

        public static bool operator ==(Position left, Position right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Position left, Position right)
        {
            return !left.Equals(right);
        }
    }
}