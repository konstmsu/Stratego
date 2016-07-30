using System;
using System.Linq;
using FluentAssertions;
using FluentAssertions.Specialized;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Stratego.Core;

namespace Stratego
{
    [TestClass]
    public class InitialSetupTests
    {
        [TestMethod]
        public void ShouldGenerateCorrectPieceSet()
        {
            var setup = new InitialSetup();
            var counts = setup.GenerateSinglePlayerPieces(null)
                .GroupBy(p => p.Rank)
                .OrderBy(g => g.Key)
                .Select(g => g.Count())
                .ToList();

            var expectedCounts = new[] { 1, 1, 8, 5, 4, 4, 4, 3, 2, 1, 1, 6 };
            counts.Sum().Should().Be(40);
            counts.Should().Equal(expectedCounts);
        }

        [TestMethod]
        public void ShouldGenerateCorrectNames()
        {
            var setup = new InitialSetup();
            var names = setup.GenerateSinglePlayerPieces(null).OrderByDescending(p => p.Rank).GroupBy(p => p.Rank).Select(g => g.Select(p => p.Name).Distinct().Single());
            names.Should().Equal("Bomb", "Marshal", "General", "Colonel", "Major", "Captain", "Lieutenant", "Sergeant", "Miner", "Scout", "Spy", "Flag");
        }
    }
}