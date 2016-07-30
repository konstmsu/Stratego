namespace Stratego.Core
{
    public class Game
    {
        public readonly Player[] Players =
        {
            new Player(),
            new Player(),
        };

        public readonly Board Board = new Board();

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
    }
}