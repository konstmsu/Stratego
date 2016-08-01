using System;

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
    }

    public class Flag : Piece
    {
        public Flag(Player owner)
            : base(owner, 0, "Flag")
        {
        }

        public override string ShortDisplayName => "F";
    }

    public class Spy : Piece
    {
        public Spy(Player owner)
            : base(owner, 1, "Spy")
        {
        }

        public override string ShortDisplayName => "S";
    }

    public class Scout : Piece
    {
        public Scout(Player owner)
            : base(owner, 2, "Scout")
        {
        }
    }

    public class Miner : Piece
    {
        public Miner(Player owner)
            : base(owner, 3, "Miner")
        {
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
    }

    public class Bomb : Piece
    {
        public Bomb(Player owner)
            : base(owner, 11, "Bomb")
        {
        }

        public override string ShortDisplayName => "B";
    }
}