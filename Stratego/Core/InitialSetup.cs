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

        public IEnumerable<Piece> GeneratePieces()
        {
            return new IEnumerable<Piece>[]
            {
                Repeat(1, () => new Flag()),
                Repeat(1, () => new Spy()),
                Repeat(8, () => new Scout()),
                Repeat(5, () => new Miner()),
                Repeat(4, () => new OtherPiece(4, "Sergeant")),
                Repeat(4, () => new OtherPiece(5, "Lieutenant")),
                Repeat(4, () => new OtherPiece(6, "Captain")),
                Repeat(3, () => new OtherPiece(7, "Major")),
                Repeat(2, () => new OtherPiece(8, "Colonel")),
                Repeat(1, () => new OtherPiece(9, "General")),
                Repeat(1, () => new OtherPiece(10, "Marshal")),
                Repeat(6, () => new Bomb()),
            }.SelectMany(v => v);
        }
    }

    public abstract class Piece
    {
        public readonly int Rank;
        public readonly string Name;

        protected Piece(int rank, string name)
        {
            Rank = rank;
            Name = name;
        }
    }

    public class Flag : Piece
    {
        public Flag()
            : base(0, "Flag")
        {
        }
    }

    public class Spy : Piece
    {
        public Spy()
            : base(1, "Spy")
        {
        }
    }

    public class Scout : Piece
    {
        public Scout()
            : base(2, "Scout")
        {
        }
    }

    public class Miner : Piece
    {
        public Miner()
            : base(3, "Miner")
        {
        }
    }

    public class OtherPiece : Piece
    {
        public OtherPiece(int rank, string name)
            : base(rank, name)
        {
        }
    }

    public class Bomb : Piece
    {
        public Bomb()
            : base(11, "Bomb")
        {
        }
    }
}