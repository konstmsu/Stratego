using System.Collections.ObjectModel;

namespace Stratego.Core
{
    public class Game
    {
        public readonly Board Board = new Board();

        public readonly Player[] Players =
        {
            new Player(),
            new Player()
        };

        public void Move(Position from, Position to)
        {
            var attacker = Board[from].Piece;
            var defender = Board[to].Piece;

            Board[from].Piece = null;

            if (defender == null || defender.Rank < attacker.Rank)
                Board[to].Piece = attacker;
            else if (defender.Rank == attacker.Rank)
                Board[to].Piece = null;
        }

        public ReadOnlyCollection<Position> GetPossibleMoves(Position from)
        {
            return new[]
            {
                new Position(from.Row - 1, from.Column),
                new Position(from.Row + 1, from.Column),
                new Position(from.Row, from.Column - 1),
                new Position(from.Row, from.Column + 1)
            }.ToReadOnly();
        }
    }
}