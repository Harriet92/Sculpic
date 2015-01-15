using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Sources.DatabaseClient.Services
{
    public static class ConnectionEstablisher
    {
        private static UserService userService = new UserService();

        public static void EstablishConnection()
        {
            userService.PingService();
        }
    }
}
