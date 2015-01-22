using System;
using FluentAssertions;
using Kalambury.Database.Mongo;
using Kalambury.WcfServer.Helpers;
using Kalambury.WcfServer.Models;
using Kalambury.WcfServer.Repositories;
using Kalambury.WcfServer.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Kalambury.WcfServer.Test
{
    [TestClass]
    public class UserServiceTest
    {
        private UserService userService;
        private UserMongoRepository userRepository;
        private User defaultTestUser;
        private User userToBeRegistered;

        [TestInitialize]
        public void Initialize()
        {
            var settings = new MongoConnectionSettings { DatabaseName = "SculpicWcfTest", Ip = "localhost", Port = "27017" };
            var mongoServer = new MongoDatabaseServer(settings);

            userRepository = new UserMongoRepository(mongoServer);
            userService = new UserService(mongoServer);
            InitializeTestData();
        }

        [TestCleanup]
        private void CleanCollections()
        {
            userRepository.DeleteAll();
        }
        private void InitializeTestData()
        {
            CleanCollections();
            defaultTestUser = new User()
            {
                UserId = 0,
                Username = "Test",
                Password = "password"
            };
            userToBeRegistered = new User
            {
                Username = "RegisterTest",
                Password = "RegisterTestPassword"
            };
            userRepository.Insert(defaultTestUser);
        }

        #region UserService LoginUser
        [TestMethod]
        [TestCategory("UserService LoginUser")]
        public void LoginUser_BasicTest_ShouldSucceed()
        {
            var result = userService.LoginUser(defaultTestUser.Username, defaultTestUser.Password);
            result.Should().NotBeNull();
            result.Username.Should().Be(defaultTestUser.Username);
        }

        [TestMethod]
        [TestCategory("UserService LoginUser")]
        public void LoginUser_EmptyUsername_ShouldReturnNull()
        {
            var result = userService.LoginUser(string.Empty, defaultTestUser.Password);
            result.Should().BeNull();
        }

        [TestMethod]
        [TestCategory("UserService LoginUser")]
        public void LoginUser_NullUsername_ShouldReturnNull()
        {
            var result = userService.LoginUser(null, defaultTestUser.Password);
            result.Should().BeNull();
        }

        [TestMethod]
        [TestCategory("UserService LoginUser")]
        public void LoginUser_EmptyPassword_ShouldReturnNull()
        {
            var result = userService.LoginUser(defaultTestUser.Username, String.Empty);
            result.Should().BeNull();
        }

        [TestMethod]
        [TestCategory("UserService LoginUser")]
        public void LoginUser_NullPassword_ShouldReturnNull()
        {
            var result = userService.LoginUser(defaultTestUser.Username, null);
            result.Should().BeNull();
        }

        [TestMethod]
        [TestCategory("UserService LoginUser")]
        public void LoginUser_NonMatchingPassword_ShouldReturnNull()
        {
            var result = userService.LoginUser(defaultTestUser.Username, "SomeRandomPassword");
            result.Should().BeNull();
        }

        [TestMethod]
        [TestCategory("UserService LoginUser")]
        public void LoginUser_NonExistentUsername_ShouldReturnNull()
        {
            var result = userService.LoginUser("SomeRandomUsername", defaultTestUser.Password);
            result.Should().BeNull();
        }
        #endregion

        #region UserService AddNewUser
        [TestMethod]
        [TestCategory("UserService AddNewUser")]
        public void AddNewUser_BasicTest_ShouldSucceed()
        {
            var result = userService.AddNewUser(userToBeRegistered.Username, userToBeRegistered.Password);
            result.Should().NotBeNull();
            result.UserId.Should().Be(defaultTestUser.UserId + 1);
            userRepository.Delete(result);
        }

        [TestMethod]
        [TestCategory("UserService AddNewUser")]
        public void AddNewUser_UsernameWithNumber_ShouldSucceed()
        {
            var result = userService.AddNewUser(userToBeRegistered.Username + "92", userToBeRegistered.Password);
            result.Should().NotBeNull();
            result.UserId.Should().Be(defaultTestUser.UserId + 1);
            userRepository.Delete(result);
        }

        [TestMethod]
        [TestCategory("UserService AddNewUser")]
        public void AddNewUser_EmptyUsername_ShouldReturnNull()
        {
            var result = userService.AddNewUser(string.Empty, userToBeRegistered.Password);
            result.Should().BeNull();
        }

        [TestMethod]
        [TestCategory("UserService AddNewUser")]
        public void AddNewUser_NullUsername_ShouldReturnNull()
        {
            var result = userService.AddNewUser(null, userToBeRegistered.Password);
            result.Should().BeNull();
        }

        [TestMethod]
        [TestCategory("UserService AddNewUser")]
        public void AddNewUser_EmptyPassword_ShouldReturnNull()
        {
            var result = userService.AddNewUser(userToBeRegistered.Username, String.Empty);
            result.Should().BeNull();
        }

        [TestMethod]
        [TestCategory("UserService AddNewUser")]
        public void AddNewUser_NullPassword_ShouldReturnNull()
        {
            var result = userService.AddNewUser(userToBeRegistered.Username, null);
            result.Should().BeNull();
        }

        [TestMethod]
        [TestCategory("UserService AddNewUser")]
        public void AddNewUser_UsernameAlreadyExists_ShouldReturnNull()
        {
            var result = userService.AddNewUser(defaultTestUser.Username, userToBeRegistered.Password);
            result.Should().BeNull();
        }

        [TestMethod]
        [TestCategory("UserService AddNewUser")]
        public void AddNewUser_UsernameTooLong_ShouldReturnNull()
        {
            var result = userService.AddNewUser("SomeReallyReallyReallyLongUsername", userToBeRegistered.Password);
            result.Should().BeNull();
        }

        [TestMethod]
        [TestCategory("UserService AddNewUser")]
        public void AddNewUser_UsernameTooShort_ShouldReturnNull()
        {
            var result = userService.AddNewUser("S", userToBeRegistered.Password);
            result.Should().BeNull();
        }

        [TestMethod]
        [TestCategory("UserService AddNewUser")]
        public void AddNewUser_SpecialCharactersInUsername_ShouldReturnNull()
        {
            var result = userService.AddNewUser("Username#$", userToBeRegistered.Password);
            result.Should().BeNull();
        }
        #endregion

        #region UserService UpdateRanking

        [TestMethod]
        [TestCategory("UserService UpdateRanking")]
        public void UpdateRanking_EmptyArguments_ShoulReturnTrue()
        {
            var result = userService.UpdateRanking(String.Empty, String.Empty);
            result.Should().BeTrue();
        }

        [TestMethod]
        [TestCategory("UserService UpdateRanking")]
        public void UpdateRanking_NullArguments_ShoulReturnTrue()
        {
            var result = userService.UpdateRanking(null, null);
            result.Should().BeTrue();
        }

        [TestMethod]
        [TestCategory("UserService UpdateRanking")]
        public void UpdateRanking_OneNullArgument_ShoulReturnFalse()
        {
            var result = userService.UpdateRanking(null, "test");
            result.Should().BeFalse();
        }

        [TestMethod]
        [TestCategory("UserService UpdateRanking")]
        public void UpdateRanking_OneUser_ShouldNotChangeRanking()
        {
            const string username = "username";
            const string password = "password";

            userService.AddNewUser(username, password);

            var result = userService.UpdateRanking(username, "300");
            result.Should().BeTrue();

            var user = userRepository.GetUserByUsername(username);
            user.Ranking.Should().Be(EloRanking.BaseRanking);

            userRepository.Delete(user);
        }

        [TestMethod]
        [TestCategory("UserService UpdateRanking")]
        public void UpdateRanking_FiveUsers_ShouldChangeRanking()
        {
            const string username = "username";
            const string password = "password";
            const int usersCount = 5;

            var usernames = String.Empty;
            var scores = String.Empty;
            for (var i = 0; i < usersCount; i++)
            {
                var currentUsername = username + i;
                userService.AddNewUser(currentUsername, password);
                var separator = (i == usersCount - 1 ? "" : ";");
                usernames += currentUsername + separator;
                scores += i + separator;
            }

            var result = userService.UpdateRanking(usernames, scores);
            result.Should().BeTrue();

            for (var i = 0; i < usersCount; i++)
            {
                var currentUsername = username + i;
                var user = userRepository.GetUserByUsername(currentUsername);
                user.Ranking.Should().NotBe(EloRanking.BaseRanking);
                userRepository.Delete(user);
            }
        }

        [TestMethod]
        [TestCategory("UserService UpdateRanking")]
        public void UpdateRanking_FiveUsers_ShouldNotChangeRanking()
        {
            const string username = "username";
            const string password = "password";
            const int usersCount = 5;
            const int score = 10;

            var usernames = String.Empty;
            var scores = String.Empty;
            for (var i = 0; i < usersCount; i++)
            {
                var currentUsername = username + i;
                userService.AddNewUser(currentUsername, password);
                var separator = (i == usersCount - 1 ? "" : ";");
                usernames += currentUsername + separator;
                scores += score + separator;
            }

            var result = userService.UpdateRanking(usernames, scores);
            result.Should().BeTrue();

            for (var i = 0; i < usersCount; i++)
            {
                var currentUsername = username + i;
                var user = userRepository.GetUserByUsername(currentUsername);
                user.Ranking.Should().Be(EloRanking.BaseRanking);
                userRepository.Delete(user);
            }
        }

        #endregion
    }
}
