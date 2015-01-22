using Assets.Sources.DatabaseClient.REST;

namespace Assets.Sources.DatabaseClient.Services
{
    public class RoomService
    {
        public static string LastError;
        private static string ServiceEndpoint = RestCommunication.BaseURL + "/RoomService";
        private readonly RestCommunication restCommunication;
        public RoomService()
        {
            LastError = string.Empty;
            restCommunication = new RestCommunication();
        }

        public bool SetUpNewRoom(string userId, string gameName, string password, string usersLimit)
        {
            var url = ServiceEndpoint + "/SetUpNewRoom/" + userId + "/" + gameName + "/" + "password" + "/" + usersLimit;
            return restCommunication.SendAndReceive<bool>(url);
        }
    }
}
