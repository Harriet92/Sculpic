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
            int userIdparsed;
            int usersLimitparsed;
            if (String.IsNullOrEmpty(userId) || String.IsNullOrEmpty(gameName) ||
                !Int32.TryParse(userId, out userIdparsed) || !Int32.TryParse(usersLimit, out usersLimitparsed))
                return false;
            RoomManager.CreateNewRoom(userIdparsed, new RoomSettings
            {
                GameName = gameName,
                Password = password,
                UsersLimit = usersLimitparsed
            });
            return true;
        }
    }
}
