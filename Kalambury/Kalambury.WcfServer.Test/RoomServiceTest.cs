using System;
using FluentAssertions;
using Kalambury.WcfServer.Models;
using Kalambury.WcfServer.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Kalambury.WcfServer.Test
{
    [TestClass]
    public class RoomServiceTest
    {
        private RoomService roomService;
        private RoomSettings testRoomSettings;
        [TestInitialize]
        public void Initialize()
        {
            roomService = new RoomService();
            CreateTestData();
        }

        private void CreateTestData()
        {
            testRoomSettings = new RoomSettings()
            {
                GameName = "Sculpic",
                Password = "Password",
                UsersLimit = 5
            };
        }

        [TestMethod]
        [TestCategory("RoomService SetUpNewRoom")]
        public void SetUpNewRoom_BasicTest_ShouldSucceed()
        {
            roomService.SetUpNewRoom("1", testRoomSettings.GameName, testRoomSettings.Password, testRoomSettings.UsersLimit.ToString()).Should().BeTrue();
        }

        [TestMethod]
        [TestCategory("RoomService SetUpNewRoom")]
        public void SetUpNewRoom_UserIdNotANumber_ShouldReturnFalse()
        {
            roomService.SetUpNewRoom("user", testRoomSettings.GameName, testRoomSettings.Password, testRoomSettings.UsersLimit.ToString()).Should().BeFalse();
        }

        [TestMethod]
        [TestCategory("RoomService SetUpNewRoom")]
        public void SetUpNewRoom_EmptyUserId_ShouldReturnFalse()
        {
            roomService.SetUpNewRoom(String.Empty, testRoomSettings.GameName, testRoomSettings.Password, testRoomSettings.UsersLimit.ToString()).Should().BeFalse();
        }

        [TestMethod]
        [TestCategory("RoomService SetUpNewRoom")]
        public void SetUpNewRoom_EmptyGameName_ShouldReturnFalse()
        {
            roomService.SetUpNewRoom("1", String.Empty, testRoomSettings.Password, testRoomSettings.UsersLimit.ToString()).Should().BeFalse();
        }

        [TestMethod]
        [TestCategory("RoomService SetUpNewRoom")]
        public void SetUpNewRoom_UsersLimitNotANumber_ShouldReturnFalse()
        {
            roomService.SetUpNewRoom("1", testRoomSettings.GameName, testRoomSettings.Password, "limit").Should().BeFalse();
        }
    }
}
