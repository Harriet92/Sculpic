using System;
using System.Collections.Generic;
using Kalambury.Database.Mongo;
using Kalambury.Mongo.Interfaces;
using Kalambury.WcfServer.Helpers;
using Kalambury.WcfServer.Interfaces;
using Kalambury.WcfServer.Models;
using Kalambury.WcfServer.Repositories;
using Kalambury.WcfServer.Validators;

namespace Kalambury.WcfServer.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository userRepository;
        //For testing purposes
        public UserService()
        {
            IDatabaseServer serverConnection = new MongoDatabaseServer(
                new MongoConnectionSettings()
                {
                    DatabaseName = "TestUserDB",
                    Ip = "127.0.0.1",
                    Port = "27017"
                });
            userRepository = new UserMongoRepository(serverConnection);
        }

        public UserService(IDatabaseServer serverConnection)
        {
            userRepository = new UserMongoRepository(serverConnection);
        }

        public User LoginUser(string username, string password)
        {
            User user = userRepository.GetUserByUsername(username);
            if (user == null || user.Password != password)
                return null;
            user.LastLoginAt = DateTime.Now;
            return userRepository.Save(user);
        }

        public User AddNewUser(string username, string password)
        {
            if (!IsNewUserDataValid(username, password)) return null;
            return userRepository.Insert(new User
            {
                UserId = userRepository.CountAll(),
                Username = username,
                Password = password,
                CreatedAt = DateTime.UtcNow,
                LastLoginAt = DateTime.UtcNow,
                Ranking = EloRanking.BaseRanking
            });
        }

        public bool UpdateRanking(string usernames, string points)
        {
            if (usernames == null && points == null)
                return true;

            if (usernames == null || points == null)
                return false;

            List<EloRanking.UserScore> userScores;
            if (!GetUserScores(usernames, points, out userScores)) return false;

            var eloRanking = new EloRanking(userScores);
            var users = eloRanking.CountNewRankings();
            UpdateUsers(users);

            return true;
        }

        private bool GetUserScores(string usernames, string points, out List<EloRanking.UserScore> userScores)
        {
            userScores = new List<EloRanking.UserScore>();
            var tempUsernames = usernames.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            var tempPoints = points.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

            return tempPoints.Length == tempUsernames.Length && ConvertUserScores(tempPoints, tempUsernames, out userScores);
        }

        private bool ConvertUserScores(string[] points, string[] usernames, out List<EloRanking.UserScore> userScores)
        {
            userScores = new List<EloRanking.UserScore>();
            for (var i = 0; i < points.Length; i++)
            {
                int score;
                if (!int.TryParse(points[i], out score))
                    return false;

                var user = userRepository.GetUserByUsername(usernames[i]);
                if (user == null)
                    return false;

                userScores.Add(new EloRanking.UserScore { User = user, Score = score });
            }
            return true;
        }

        private void UpdateUsers(List<User> users)
        {
            users.ForEach(user => userRepository.Save(user));
        }

        private bool IsNewUserDataValid(string username, string password)
        {
            return !String.IsNullOrEmpty(password) && !String.IsNullOrEmpty(username) &&
                   UsernameValidator.IsUsernameValid(username) && userRepository.IsUsernameUnique(username);
        }

        public bool PingService()
        {
            return true;
        }

        public List<User> GetTopRanking(string count)
        {
            int _count;
            if(!Int32.TryParse(count, out _count))
                return null;

            return userRepository.GetUsersByRanking(_count);
        }
    }
}
