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
