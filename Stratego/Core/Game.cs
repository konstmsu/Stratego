using System.Collections.Generic;

namespace Stratego.Core
{
    public class Game
    {
        public readonly Board Board;

        public readonly IReadOnlyList<Player> Players = new[]
        {
            new Player(),
            new Player()
        };

        public Game(Board board)
        {
            Board = board;
        }

        public void Move(Position from, Position to)
        {
            var attacker = Board[from].Piece;
            var defender = Board[to].Piece;

            Board[from].Piece = null;

            Board[to].Piece = attacker.Move(defender);
        }

        public IReadOnlyCollection<Position> GetPossibleMoves(Position from)
        {
            var piece = Board[from].Piece;
            return piece == null ? new Position[0] : piece.GetPossibleMoves(Board, from);
        }
    }
}