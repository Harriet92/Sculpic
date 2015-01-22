using System;
using System.Collections;
using System.Text;
using System.Threading;
using Assets.Sources.DatabaseClient.REST;
using Assets.Sources.DatabaseServer.JsonFx;
using Assets.Sources.DatabaseServer.Models;
//using Assets.Sources.DatabaseServer.REST;
using UnityEngine;

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
            return SendAndRecieve<User>(url);
        }

        public User AddNewUser(string username, string password)
        {
            var url = ServiceEndpoint + "/AddNewUser/"+username+"/"+password;
            return SendAndRecieve<User>(url);
        }

        public bool UpdateRanking(string usernames, string scores)
        {
            var url = ServiceEndpoint + "/UpdateRanking/" + usernames + "/" + scores;
            return SendAndRecieve<bool>(url);
        }

        private T SendAndRecieve<T>(string url)
        {
            var response = restCommunication.SendAndReceive(url);
            Debug.Log("Response: " + response);
            if (String.IsNullOrEmpty(response)) return default(T);
            var reader = new JsonReader(response);
            var result = (T)reader.Deserialize(typeof(User));
            return result;
        }

        public string PingService()
        {
            var url = ServiceEndpoint + "/PingService";
            string response = restCommunication.SendAndReceive(url);
            Debug.Log("Response: " + response);
            if (String.IsNullOrEmpty(response)) return null;
            return response;
        }
    }
}
