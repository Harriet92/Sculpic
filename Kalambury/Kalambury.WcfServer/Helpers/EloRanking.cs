using System;
using System.Collections.Generic;
using System.Linq;
using Kalambury.WcfServer.Models;

namespace Kalambury.WcfServer.Helpers
{
    public class EloRanking
    {
        public const int BaseRanking = 1200;

        private const double WinScore = 1.0;
        private const double TieScore = 0.5;
        private const double LoseScore = 0.0;

        private readonly List<UserScore> _userScores;

        public EloRanking(List<UserScore> userScores)
        {
            _userScores = userScores;
        }

        public List<User> CountNewRankings()
        {
            foreach (var playerScore in _userScores)
            {
                var rankingDifference =
                    _userScores.Where(opponentScore => playerScore != opponentScore)
                        .Sum(
                            opponentScore =>
                                CountRankingDifference(playerScore.User.Ranking, playerScore.User.Ranking,
                                    GetGameResult(playerScore.Score, opponentScore.Score)));
                playerScore.User.Ranking += rankingDifference;
            }
            return _userScores.Select(userScore => userScore.User).ToList();
        }

        private static int CountRankingDifference(int playerRanking, int opponentRanking, double gameResult)
        {
            var rankingDifference = opponentRanking - playerRanking;
            var expectedResult = 1 / (1 + Math.Pow(10f, rankingDifference / 400f));
            var absoluteRankingDifference = gameResult - expectedResult;
            var newRanking = (playerRanking + (32 * absoluteRankingDifference));
            return (int)newRanking;
        }

        private static double GetGameResult(int playerRanking, int opponentRanking)
        {
            if (playerRanking > opponentRanking)
                return WinScore;
            if (playerRanking == opponentRanking)
                return TieScore;
            return LoseScore;
        }

        public class UserScore
        {
            public User User { get; set; }
            public int Score { get; set; }
        }
    }
}