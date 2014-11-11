using System;
using System.Collections;
using System.Text;
using System.Threading;
using Assets.Sources.DatabaseServer.JsonFx;
using Assets.Sources.DatabaseServer.Models;
//using Assets.Sources.DatabaseServer.REST;
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
            var url = URL + "/LoginUser/" + username + "/" + password;
            string response = Send(url);
            Debug.Log("Response: " + response);
            if (String.IsNullOrEmpty(response)) return null;
            JsonReader reader = new JsonReader(response);
            User result = (User)reader.Deserialize(typeof(User));
            return result;
        }

        public User AddNewUser(string username, string password)
        {
            var url = URL + "/AddNewUser/"+username+"/"+password;
            string response = Send(url);
            Debug.Log("Response: " + response);
            if (String.IsNullOrEmpty(response)) return null;
            JsonReader reader = new JsonReader(response);
            User result = (User)reader.Deserialize(typeof(User));
            return result;
        }
        string Send(string url)
        {
            WWW www = new WWW(url);
            Debug.Log(www.url);
            Thread.Sleep(2000);
            return www.text;
        }
    }
}
