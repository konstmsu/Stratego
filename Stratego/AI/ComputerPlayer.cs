using System;
using System.Linq;
using Stratego.Core;
using Stratego.Core.Utility;

namespace Stratego.AI
{
    public class SuggestedMove
    {
        public readonly Position From;
        public readonly Position To;

        public SuggestedMove(Position from, Position to)
        {
            From = from;
            To = to;
        }
    }

    public interface IPlayer
    {
        SuggestedMove SuggestMove(Game game, Player player);
    }

    public class HumanPlayer : IPlayer
    {
        public SuggestedMove SuggestMove(Game game, Player player) => null;
    }

    public class ComputerPlayer : IPlayer
    {
        readonly Random _random = new Random();

        public ComputerPlayer()
        {
        }

        public SuggestedMove SuggestMove(Game game, Player player)
        {
            var moves = game.Board.Cells.Where(c => c.Piece?.Owner == player)
                .SelectMany(c => game.GetPossibleMoves(c.Position).Select(m => new SuggestedMove(c.Position, m)))
                .ToList();

            return _random.NextItem(moves);
        }
    }
}
