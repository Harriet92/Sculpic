using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Sources.DatabaseServer.Models;

namespace Assets.Sources.Scripts
{
    public class Player
    {
        public static Player Current { get; private set; }

        public string Username { get; private set; }
        public bool IsLoggedIn { get; private set; }

        public static void LogIn(User dbUser)
        {
            Current = new Player()
            {
                IsLoggedIn = true,
                Username = dbUser.Username
            };
        }
    }
}
