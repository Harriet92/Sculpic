using System;
using System.Text;
using Assets.Sources.DatabaseServer.JsonFx;
using Assets.Sources.DatabaseServer.Models;
using Assets.Sources.DatabaseServer.REST;
using UnityEngine;

namespace Assets.Sources.DatabaseClient.Services
{
    public class UserService
    {
        private const string URL = "http://localhost:8733/UserService";
        private readonly JsonWriterSettings jsonWriterSettings;
        public UserService()
        {
            jsonWriterSettings = new JsonWriterSettings();
            jsonWriterSettings.DateTimeSerializer = (x, value) =>
            {
                DateTime date = (DateTime)value;
                DateTime Epoch = new DateTime(1970, 1, 1, 0, 0, 0);
                TimeSpan elapsedTime = new TimeSpan(date.ToUniversalTime().Ticks - Epoch.Ticks);
                long timestamp = (long)elapsedTime.TotalMilliseconds;
                x.Write("\\/Date(" + timestamp + ")\\/");
            };
        }

        public User LoginUser(string username, string password)
        {
            RestCommunication restCom = new RestCommunication();
            restCom.SetUrl(URL + "/LoginUser");
            StringBuilder parameters = new StringBuilder();
            JsonWriter writer = new JsonWriter(parameters, jsonWriterSettings);
            writer.Write(new { username, password });
            Debug.Log("Login user, sending: " + parameters.ToString());
            string response = restCom.SendAndRecive(parameters.ToString(), null);
            if (String.IsNullOrEmpty(response)) return null;
            JsonReader reader = new JsonReader(response);
            User result = (User)reader.Deserialize(typeof(User));
            return result;
        }

        public User AddNewUser(string username, string password)
        {
            RestCommunication restCom = new RestCommunication();
            restCom.SetUrl(URL + "/AddNewUser");
            StringBuilder parameters = new StringBuilder();
            JsonWriter writer = new JsonWriter(parameters, jsonWriterSettings);
            writer.Write(new { username, password });
            string response = restCom.SendAndRecive(parameters.ToString(), null);
            Debug.Log("Response: " + response);
            if (String.IsNullOrEmpty(response)) return null;
            JsonReader reader = new JsonReader(response);
            User result = (User)reader.Deserialize(typeof(User));
            return result;
        }
    }
}
