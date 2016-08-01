using System.Collections.Generic;

namespace Stratego.Core
{
    public class Game
    {
        public readonly Board Board = Board.CreateStandard();

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

        public IReadOnlyCollection<Position> GetPossibleMoves(Position from)
        {
            var piece = Board[from].Piece;

            if (piece == null)
                return new Position[0];

            return piece.GetPossibleMoves(Board, from);
        }
    }
}