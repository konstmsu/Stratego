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

        protected static IReadOnlyCollection<Position> GetPossibleMoves2(Board board, Position from)
        {
            return new[]
            {
                new Position(from.Row - 1, from.Column),
                new Position(from.Row + 1, from.Column),
                new Position(from.Row, from.Column - 1),
                new Position(from.Row, from.Column + 1)
            };
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
            return GetPossibleMoves2(board, from);
        }
    }

    public class Scout : Piece
    {
        public Scout(Player owner)
            : base(owner, 2, "Scout")
        {
        }

        public override IReadOnlyCollection<Position> GetPossibleMoves(Board board, Position from)
        {
            var possibleMoves = new List<Position>();

            foreach (var direction in new[] { new Position(1, 0), new Position(-1, 0), new Position(0, 1), new Position(0, -1) })
                for (var distance = 1;; distance++)
                {
                    var position = from + direction * distance;

                    var cell = board[position];

                    if (cell == null || cell.Piece != null || cell.IsLake)
                        break;

                    possibleMoves.Add(position);
                }

            return possibleMoves;
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
            return GetPossibleMoves2(board, from);
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
            }

            throw new ArgumentOutOfRangeException(nameof(rank));
        }

        public override IReadOnlyCollection<Position> GetPossibleMoves(Board board, Position from)
        {
            return GetPossibleMoves2(board, from);
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