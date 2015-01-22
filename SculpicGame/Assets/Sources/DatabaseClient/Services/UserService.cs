using System.Collections.Generic;
using Assets.Sources.DatabaseClient.Models;
using Assets.Sources.DatabaseClient.REST;

namespace Assets.Sources.DatabaseClient.Services
{
    public class UserService
    {
        public static string LastError;
        private static string ServiceEndpoint = RestCommunication.BaseURL + "/UserService";
        private readonly RestCommunication restCommunication;
        public UserService()
        {
            LastError = string.Empty;
            restCommunication = new RestCommunication();
        }

        public User LoginUser(string username, string password)
        {
            var url = ServiceEndpoint + "/LoginUser/" + username + "/" + password;
            return restCommunication.SendAndReceive<User>(url);
        }

        public User AddNewUser(string username, string password)
        {
            var url = ServiceEndpoint + "/AddNewUser/"+username+"/"+password;
            return restCommunication.SendAndReceive<User>(url);
        }

        public bool UpdateRanking(string usernames, string scores)
        {
            var url = ServiceEndpoint + "/UpdateRanking/" + usernames + "/" + scores;
            return restCommunication.SendAndReceive<bool>(url);
        }

        public User[] GetTopRanking(string count)
        {
            var url = ServiceEndpoint + "/GetTopRanking/" + count;
            return restCommunication.SendAndReceive<User[]>(url);
        }

        public bool PingService()
        {
            var url = ServiceEndpoint + "/PingService";
            return  restCommunication.SendAndReceive<bool>(url);
        }
    }
}
