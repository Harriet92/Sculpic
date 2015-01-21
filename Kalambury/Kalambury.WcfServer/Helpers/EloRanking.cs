using System;
using System.Collections.Generic;
using System.Linq;
using Kalambury.WcfServer.Interfaces;
using Kalambury.WcfServer.Models;

namespace Kalambury.WcfServer.Helpers
{
    public class EloRanking
    {
        private const double WinScore = 1.0;
        private const double TieScore = 0.5;
        private const double LoseScore = 0.0;

        private readonly List<UserScore> _userScores = new List<UserScore>();
        private readonly IUserRepository _userRepository;
        private readonly string[] _tempUsernames;
        private readonly string[] _tempPoints;

        public EloRanking(string usernames, string points, IUserRepository userRepository)
        {
            _userRepository = userRepository;
            _tempUsernames = usernames.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            _tempPoints = points.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

            if (_tempPoints.Length != _tempUsernames.Length)
                throw new ArgumentException("usernames and points have different number of elements after splitting.");
        }

        public void Compute()
        {
            ConvertUserPoints();
            CountNewRankings();
            UpdateUsers();
        }

        private void ConvertUserPoints()
        {
            for (var i = 0; i < _tempPoints.Length; i++)
            {
                int score;
                if (!int.TryParse(_tempPoints[i], out score))
                    throw new ArgumentException("One of the points isn't an integer.");

                var user = _userRepository.GetUserByUsername(_tempUsernames[i]);
                if (user == null)
                    throw new ArgumentException("One of the users doesn't exist.");

                _userScores.Add(new UserScore{User = user, Score = score});
            }
        }

        private void CountNewRankings()
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

        private void UpdateUsers()
        {
            _userScores.ForEach(userScore => _userRepository.Save(userScore.User));
        }


        private class UserScore
        {
            public User User { get; set; }
            public int Score { get; set; }
        }
    }
}