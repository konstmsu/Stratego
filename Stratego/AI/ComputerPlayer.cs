using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

    public class ComputerPlayer
    {
        Random random = new Random();
        public readonly Player Player;

        public ComputerPlayer(Player player)
        {
            this.Player = player;
        }

        public SuggestedMove SuggestMove(Game game)
        {
            var moves = game.Board.Cells.Where(c => c.Piece?.Owner == Player)
                .SelectMany(c => game.GetPossibleMoves(c.Position).Select(m => new SuggestedMove(c.Position, m)))
                .ToList();

            return random.NextItem(moves);
        }
    }
}
