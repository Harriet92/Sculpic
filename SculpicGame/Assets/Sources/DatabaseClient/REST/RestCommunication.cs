using System;
using System.Threading;
using Assets.Sources.DatabaseClient.Services;
using UnityEngine;

namespace Assets.Sources.DatabaseClient.REST
{//TODO: Add logging
    public class RestCommunication
    {
        private const bool isLocalhost = false;
        private const int retryCount = 5;
        private const int sleepTimeout = 500;
        public const string BaseURL = isLocalhost ? "http://localhost:8733" : "http://deemi.ddns.net:8733";
        public string SendAndReceive(string url)
        {
            WWW www = null;
            int noOfConnections = 0;
            bool connectionSucceeded = false;
            do
            {
                www = new WWW(url);
                Debug.Log("Sending:" + www.url);
                int noOfRetry = 0;
                while (!www.isDone && noOfRetry++ < retryCount)
                {
                    Debug.Log("Retry no: " + noOfRetry);
                    Thread.Sleep(sleepTimeout);
                }
                if (www.isDone && String.IsNullOrEmpty(www.error))
                {
                    connectionSucceeded = true;
                    Debug.Log(www.text);
                }
                else if(www.isDone)
                    noOfConnections++;
            } while (!connectionSucceeded && noOfConnections < retryCount);
            UserService.LastError = www.error;
            if (!www.isDone && String.IsNullOrEmpty(www.error))
            {
                UserService.LastError = "WWW request timed out.";
                Debug.Log("Server error: " + UserService.LastError);
            }
            return www.isDone ? www.text : null;
        }
    }
}
