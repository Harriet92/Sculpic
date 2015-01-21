using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Kalambury.WcfServer.Helpers;
using Kalambury.WcfServer.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Kalambury.WcfServer.Test
{
    [TestClass]
    public class EloRankingTest
    {
        [TestMethod]
        [TestCategory("EloRanking")]
        public void CountNewRankings_TwoPlayersWorseRankingWins_ShouldChangeRanking()
        {
            const int playerOneBaseRanking = 1500;
            const int playerTwoBaseRanking = 1700;

            var playerOne = new User { Ranking = playerOneBaseRanking };
            var playerTwo = new User { Ranking = playerTwoBaseRanking };

            var userScores = new List<EloRanking.UserScore>
            {
                new EloRanking.UserScore {User = playerOne, Score = 600},
                new EloRanking.UserScore {User = playerTwo, Score = 300}
            };

            var eloRanking = new EloRanking(userScores);
            eloRanking.CountNewRankings();

            playerOne.Ranking.Should().BeGreaterThan(playerOneBaseRanking);
            playerTwo.Ranking.Should().BeLessThan(playerTwoBaseRanking);
        }

        [TestMethod]
        [TestCategory("EloRanking")]
        public void CountNewRankings_TwoPlayersBetterRankingWins_ShouldChangeRanking()
        {
            const int playerOneBaseRanking = 1500;
            const int playerTwoBaseRanking = 1700;

            var playerOne = new User { Ranking = playerOneBaseRanking };
            var playerTwo = new User { Ranking = playerTwoBaseRanking };

            var userScores = new List<EloRanking.UserScore>
            {
                new EloRanking.UserScore {User = playerOne, Score = 300},
                new EloRanking.UserScore {User = playerTwo, Score = 600}
            };

            var eloRanking = new EloRanking(userScores);
            eloRanking.CountNewRankings();

            playerOne.Ranking.Should().BeLessThan(playerOneBaseRanking);
            playerTwo.Ranking.Should().BeGreaterThan(playerTwoBaseRanking);
        }

        [TestMethod]
        [TestCategory("EloRanking")]
        public void CountNewRankings_TwoPlayersSameRankingTie_ShouldChangeRanking()
        {
            var playerOne = new User { Ranking = EloRanking.BaseRanking };
            var playerTwo = new User { Ranking = EloRanking.BaseRanking };

            var userScores = new List<EloRanking.UserScore>
            {
                new EloRanking.UserScore {User = playerOne, Score = 300},
                new EloRanking.UserScore {User = playerTwo, Score = 300}
            };

            var eloRanking = new EloRanking(userScores);
            eloRanking.CountNewRankings();

            playerOne.Ranking.Should().Be(playerTwo.Ranking);
        }

        [TestMethod]
        [TestCategory("EloRanking")]
        public void CountNewRankings_TwoPlayersDifferentRankingTie_ShouldChangeRanking()
        {
            const int playerOneBaseRanking = 1500;
            const int playerTwoBaseRanking = 1700;

            var playerOne = new User { Ranking = playerOneBaseRanking };
            var playerTwo = new User { Ranking = playerTwoBaseRanking };

            var userScores = new List<EloRanking.UserScore>
            {
                new EloRanking.UserScore {User = playerOne, Score = 300},
                new EloRanking.UserScore {User = playerTwo, Score = 300}
            };

            var eloRanking = new EloRanking(userScores);
            eloRanking.CountNewRankings();

            playerOne.Ranking.Should().BeGreaterThan(playerOneBaseRanking);
            playerTwo.Ranking.Should().BeLessThan(playerTwoBaseRanking);
        }

        [TestMethod]
        [TestCategory("EloRanking")]
        public void CountNewRankings_ThreePlayersSameRanking_ShouldSortByScore()
        {
            var playerOne = new User { Ranking = EloRanking.BaseRanking };
            var playerTwo = new User { Ranking = EloRanking.BaseRanking };
            var playerThree = new User { Ranking = EloRanking.BaseRanking };

            var userScores = new List<EloRanking.UserScore>
            {
                new EloRanking.UserScore {User = playerOne, Score = 300}, // 3rd place
                new EloRanking.UserScore {User = playerTwo, Score = 900}, // 1st place
                new EloRanking.UserScore {User = playerThree, Score = 600} // 2nd place
            };

            var eloRanking = new EloRanking(userScores);
            var newRankings = eloRanking.CountNewRankings();
            var orderedNewRankings = newRankings.OrderByDescending(user => user.Ranking).ToArray();

            orderedNewRankings[0].Should().Be(playerTwo);
            orderedNewRankings[1].Should().Be(playerThree);
            orderedNewRankings[2].Should().Be(playerOne);
        }
    }
}
