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
            roomService.SetUpNewRoom(1, testRoomSettings.GameName, testRoomSettings.Password, testRoomSettings.UsersLimit).Should().BeTrue();
        }
    }
}
