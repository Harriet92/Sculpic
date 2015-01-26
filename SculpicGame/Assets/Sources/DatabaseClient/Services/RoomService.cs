using System;
using Assets.Sources.DatabaseClient.REST;

namespace Assets.Sources.DatabaseClient.Services
{
    public class RoomService
    {
        public static string LastError;
        private static string ServiceEndpoint = RestCommunication.BaseURL + "/RoomService";
        private readonly RestCommunication restCommunication;
        public const string noPasswordMessage = "nopassword";

        public RoomService()
        {
            LastError = String.Empty;
            restCommunication = new RestCommunication();
        }

        public bool SetUpNewRoom(string userId, string gameName, string password, string usersLimit)
        {
            var url = ServiceEndpoint + "/SetUpNewRoom/" + userId + "/" + gameName + "/" + (String.IsNullOrEmpty(password) ? noPasswordMessage : password) + "/" + usersLimit;
            return restCommunication.SendAndReceive<bool>(url);
        }
    }
}
