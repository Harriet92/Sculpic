using System;
using FluentAssertions;
using Kalambury.Database.Mongo;
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
    }
}
