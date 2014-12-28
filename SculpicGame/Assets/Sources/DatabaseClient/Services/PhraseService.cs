using System;
using Assets.Sources.DatabaseClient.REST;
using Assets.Sources.DatabaseServer.JsonFx;
using Assets.Sources.DatabaseServer.Models;
using UnityEngine;

namespace Assets.Sources.DatabaseClient.Services
{
    public class PhraseService
    {
        public static string LastError;
        private static string ServiceEndpoint = RestCommunication.BaseURL + "/PhraseService";
        private readonly RestCommunication restCommunication;
        public PhraseService()
        {
            LastError = string.Empty;
            restCommunication = new RestCommunication();
        }

        public string DrawPhrase()
        {
            var url = ServiceEndpoint + "/DrawPhrase";
            var response = restCommunication.SendAndReceive(url);
            Debug.Log("Response: " + response);
            if (String.IsNullOrEmpty(response)) return null;
            JsonReader reader = new JsonReader(response);
            string result = (string)reader.Deserialize(typeof(string));
            return result;
        }

    }
}
