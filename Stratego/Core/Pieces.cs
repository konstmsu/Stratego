using System;
using System.Collections.Generic;

namespace Stratego.Core
{
    public abstract class Piece
    {
        public readonly string Name;
        public readonly Player Owner;
        public readonly int Rank;

        protected Piece(Player owner, int rank, string name)
        {
            Owner = owner;
            Rank = rank;
            Name = name;
        }

        public virtual string ShortDisplayName => Rank.ToString();

        public abstract IReadOnlyCollection<Position> GetPossibleMoves(Board board, Position from);

        protected IReadOnlyCollection<Position> GetPossibleDirectMoves(Board board, Position from, int maxDistance)
        {
            var possibleMoves = new List<Position>();

            foreach (var direction in new[] { new Position(1, 0), new Position(-1, 0), new Position(0, 1), new Position(0, -1) })
                for (var distance = 1; distance <= maxDistance; distance++)
                {
                    var position = from + direction * distance;

                    if (!board.ContainsPosition(position))
                        break;

                    var cell = board[position];

                    if (cell.IsLake)
                        break;

                    if (cell.Piece != null && cell.Piece.Owner == Owner)
                        break;

                    possibleMoves.Add(position);

                    if (cell.Piece != null)
                        break;
                }

            return possibleMoves;
        }
    }

    public class Flag : Piece
    {
        public Flag(Player owner)
            : base(owner, 0, "Flag")
        {
        }

        public override string ShortDisplayName => "F";

        public override IReadOnlyCollection<Position> GetPossibleMoves(Board board, Position from)
        {
            return new Position[0];
        }
    }

    public class Spy : Piece
    {
        public Spy(Player owner)
            : base(owner, 1, "Spy")
        {
        }

        public override string ShortDisplayName => "S";

        public override IReadOnlyCollection<Position> GetPossibleMoves(Board board, Position from)
        {
            return GetPossibleDirectMoves(board, from, 1);
        }
    }

    public class Scout : Piece
    {
        public new const int Rank = 2;

        public Scout(Player owner)
            : base(owner, Rank, "Scout")
        {
        }

        public override IReadOnlyCollection<Position> GetPossibleMoves(Board board, Position from)
        {
            return GetPossibleDirectMoves(board, from, Math.Max(board.RowCount, board.ColumnCount));
        }
    }

    public class Miner : Piece
    {
        public Miner(Player owner)
            : base(owner, 3, "Miner")
        {
        }

        public override IReadOnlyCollection<Position> GetPossibleMoves(Board board, Position from)
        {
            return GetPossibleDirectMoves(board, from, 1);
        }
    }

    public class OtherPiece : Piece
    {
        /// <param name="rank">[4, 10]</param>
        /// <param name="owner"></param>
        public OtherPiece(int rank, Player owner)
            : base(owner, rank, GetNameFromRank(rank))
        {
        }

        static string GetNameFromRank(int rank)
        {
            switch (rank)
            {
                case 4:
                    return "Sergeant";
                case 5:
                    return "Lieutenant";
                case 6:
                    return "Captain";
                case 7:
                    return "Major";
                case 8:
                    return "Colonel";
                case 9:
                    return "General";
                case 10:
                    return "Marshal";
                default:
                    throw new ArgumentOutOfRangeException(nameof(rank));
            }
        }

        public override IReadOnlyCollection<Position> GetPossibleMoves(Board board, Position from)
        {
            return GetPossibleDirectMoves(board, from, 1);
        }
    }

    public class Bomb : Piece
    {
        public Bomb(Player owner)
            : base(owner, 11, "Bomb")
        {
        }

        public override string ShortDisplayName => "B";

        public override IReadOnlyCollection<Position> GetPossibleMoves(Board board, Position from)
        {
            return new Position[0];
        }
    }
}