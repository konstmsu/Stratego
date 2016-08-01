using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;

namespace Stratego.Core
{
    public static class Validation
    {
        public static void ValidatePlayerSetup(Game game, IReadOnlyList<IReadOnlyList<int>> setup, int player)
        {
            var mapping = GetSetupToFieldMapping(player);

            var pieces = new List<Piece>();

            for (var row = 0; row < setup.Count; row++)
                for (var column = 0; column < setup[0].Count; column++)
                {
                    var piece = game.Board[mapping(row, column)].Piece;
                    pieces.Add(piece);
                    piece.Rank.Should().Be(setup[row][column]);
                    piece.Owner.Should().Be(game.Players[player]);
                }

            ValidateFullPieceSet(pieces);
        }

        static Func<int, int, Position> GetSetupToFieldMapping(int playerIndex)
        {
            if (playerIndex == 0)
                return (r, c) => new Position(3 - r, 9 - c);

            return (r, c) => new Position(6 + r, c);
        }

        static void ValidateFullPieceSet(ICollection<Piece> pieces)
        {
            var counts = pieces
                .OrderBy(g => g.Rank)
                .GroupBy(p => p.Rank)
                .Select(g => g.Count())
                .ToList();

            var expectedCounts = new[] { 1, 1, 8, 5, 4, 4, 4, 3, 2, 1, 1, 6 };
            counts.Sum().Should().Be(40);
            counts.Should().Equal(expectedCounts);

            var names = pieces
                .OrderByDescending(p => p.Rank)
                .GroupBy(p => p.Rank)
                .Select(g => g.Select(p => p.Name).Distinct().Single());

            names.Should().Equal("Bomb", "Marshal", "General", "Colonel", "Major", "Captain", "Lieutenant", "Sergeant", "Miner", "Scout", "Spy", "Flag");
        }
    }
}