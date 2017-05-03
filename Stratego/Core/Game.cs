using System;
using System.Collections.Generic;
using System.Linq;

namespace Stratego.Core
{
    public class Game
    {
        public readonly Board Board;

        int _currentPlayerIndex;

        public int CurrentPlayerIndex => _currentPlayerIndex;

        public readonly IReadOnlyList<Player> Players = new[]
        {
            new Player(),
            new Player()
        };

        public Player CurrentPlayer => Players[_currentPlayerIndex];

        public Game(Board board)
        {
            Board = board;
        }

        public MoveResult Move(Position from, Position to)
        {
            var attacker = Board[from].Piece;

            if (attacker == null || attacker.Owner != CurrentPlayer)
                throw new InvalidOperationException();

            var defender = Board[to].Piece;

            if (!GetPossibleMoves(from).Contains(to))
                throw new InvalidOperationException();

            Board[from].Piece = null;
            Board[to].Piece = attacker.Move(defender);

            _currentPlayerIndex = (_currentPlayerIndex + 1) % Players.Count;

            return new MoveResult(attacker, defender, from, to, 
                hasAttackerDied: Board[to].Piece != attacker,
                hasDefenderDied: Board[to].Piece != defender);
        }

        public IReadOnlyCollection<Position> GetPossibleMoves(Position from)
        {
            var piece = Board[from].Piece;

            if (piece != null && piece.Owner == Players[_currentPlayerIndex])
                return piece.GetPossibleMoves(Board, from);

            return new Position[0];
        }

        public bool IsMovable(Position from) => GetPossibleMoves(from).Any();
    }
}