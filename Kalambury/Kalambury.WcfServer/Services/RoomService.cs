using System;
using Kalambury.WcfServer.Helpers;
using Kalambury.WcfServer.Interfaces;
using Kalambury.WcfServer.Models;

namespace Kalambury.WcfServer.Services
{
    public class RoomService : IRoomService
    {
        public bool SetUpNewRoom(string userId, string gameName, string password, string usersLimit)
        {
            RoomManager.CreateNewRoom(Int32.Parse(userId), new RoomSettings
            {
                GameName = gameName,
                Password = password,
                UsersLimit = Int32.Parse(usersLimit)
            });
            return true;
        }
    }
}
