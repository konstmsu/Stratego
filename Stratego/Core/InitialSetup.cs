using System;
using System.Collections.Generic;
using System.Linq;

namespace Stratego.Core
{
    public class InitialSetup
    {
        static IEnumerable<T> Repeat<T>(int count, Func<T> factory)
        {
            for (var i = 0; i < count; i++)
                yield return factory();
        }

        public IEnumerable<Piece> GenerateSinglePlayerPieces(Player owner)
        {
            return new IEnumerable<Piece>[]
            {
                Repeat(1, () => new Flag(owner)),
                Repeat(1, () => new Spy(owner)),
                Repeat(8, () => new Scout(owner)),
                Repeat(5, () => new Miner(owner)),
                Repeat(4, () => new OtherPiece(4, owner)),
                Repeat(4, () => new OtherPiece(5, owner)),
                Repeat(4, () => new OtherPiece(6, owner)),
                Repeat(3, () => new OtherPiece(7, owner)),
                Repeat(2, () => new OtherPiece(8, owner)),
                Repeat(1, () => new OtherPiece(9, owner)),
                Repeat(1, () => new OtherPiece(10, owner)),
                Repeat(6, () => new Bomb(owner))
            }.SelectMany(v => v);
        }
    }

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
    }

    public class Flag : Piece
    {
        public Flag(Player owner)
            : base(owner, 0, "Flag")
        {
        }
    }

    public class Spy : Piece
    {
        public Spy(Player owner)
            : base(owner, 1, "Spy")
        {
        }
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

        /// <param name="rank">[4, 10]</param>
        /// <param name="owner"></param>
        public OtherPiece(int rank, Player owner)
            : base(owner, rank, GetNameFromRank(rank))
        {
        }
    }

    public class Bomb : Piece
    {
        public Bomb(Player owner)
            : base(owner, 11, "Bomb")
        {
        }
    }
}