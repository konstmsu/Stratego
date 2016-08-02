using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Stratego.Core
{
    public static class InitialSetup
    {
        public static void Setup(Game game, IReadOnlyList<IReadOnlyList<int>> setup, int player)
        {
            Func<int, int, Position> getPosition = (r, c) => player == 0 ? new Position(3 - r, 9 - c) : new Position(6 + r, c);

            for (var row = 0; row < setup.Count; row++)
            {
                Debug.Assert(setup[row].Count == setup[0].Count);
                for (var column = 0; column < setup[row].Count; column++)
                    game.Board[getPosition(row, column)].Piece = CreatePiece(setup[row][column], game.Players[player]);
            }
        }

        public static Piece CreatePiece(int rank, Player owner)
        {
            switch (rank)
            {
                case 0:
                    return new Flag(owner);
                case 1:
                    return new Spy(owner);
                case 2:
                    return new Scout(owner);
                case 3:
                    return new Miner(owner);
                case 11:
                    return new Bomb(owner);
                default:
                    return new OtherPiece(rank, owner);
            }
        }
    }
}