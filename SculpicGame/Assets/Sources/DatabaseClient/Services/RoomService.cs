using System;
using System.IO;
using System.Text;
using Assets.Sources.DatabaseClient.Models;
using Assets.Sources.DatabaseClient.REST;
using Assets.Sources.DatabaseServer.JsonFx;
using UnityEngine;

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
            Debug.Log(ServiceEndpoint + "/SetUpNewRoom/" + userId + "/" + gameName + "/" + password + "/" + usersLimit);
            var response = restCommunication.SendAndReceive(url);
            Debug.Log("Response: " + response);
            if (String.IsNullOrEmpty(response)) return false;
            JsonReader reader = new JsonReader(response);
            bool result = (bool)reader.Deserialize(typeof(bool));
            return result;
        }
    }
}
