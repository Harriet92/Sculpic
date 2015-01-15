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
            var response = restCommunication.SendAndReceive(url);
            Debug.Log("Response: " + response);
            if (String.IsNullOrEmpty(response)) return null;
            JsonReader reader = new JsonReader(response);
            User result = (User)reader.Deserialize(typeof(User));
            return result;
        }

        public User AddNewUser(string username, string password)
        {
            var url = ServiceEndpoint + "/AddNewUser/"+username+"/"+password;
            string response = restCommunication.SendAndReceive(url);
            Debug.Log("Response: " + response);
            if (String.IsNullOrEmpty(response)) return null;
            JsonReader reader = new JsonReader(response);
            User result = (User)reader.Deserialize(typeof(User));
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
