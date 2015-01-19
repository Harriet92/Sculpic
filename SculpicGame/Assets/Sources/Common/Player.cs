using System;
using Assets.Sources.DatabaseServer.Models;

namespace Assets.Sources.Common
{
    public class Player
    {
        public static Player Current { get; private set; }
        public string Username { get; private set; }
        public bool IsLoggedIn { get; private set; }
        public Int64 UserId { get; set; }

        public static void LogIn(User dbUser)
        {
            Current = new Player
            {
                IsLoggedIn = true,
                Username = dbUser.Username,
                UserId = dbUser.UserId
            };
        }

        public static string Name
        {
            get
            {
                return Current != null ? Current.Username : "Stranger";
            }
        }
    }
}
