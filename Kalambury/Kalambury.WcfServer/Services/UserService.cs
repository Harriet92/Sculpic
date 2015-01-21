﻿using System;
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
        private const string StatusOk = "OK!";
        private const string StatusError = "ERROR!";
        private const int BaseRanking = 1200;

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
                Ranking = BaseRanking
            });
        }

        public string UpdateRanking(string usernames, string points)
        {
            try
            {
                var eloRanking = new EloRanking(usernames, points, userRepository);
                eloRanking.Compute();
            }
            catch (Exception)
            {
                return StatusError;
            }

            return StatusOk;
        }

        private bool IsNewUserDataValid(string username, string password)
        {
            return !String.IsNullOrEmpty(password) && !String.IsNullOrEmpty(username) &&
                   UsernameValidator.IsUsernameValid(username) && userRepository.IsUsernameUnique(username);
        }

        public string PingService()
        {
            return StatusOk;
        }
    }
}
