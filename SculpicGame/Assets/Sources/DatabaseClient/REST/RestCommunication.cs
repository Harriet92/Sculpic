using System;
using System.Threading;
using Assets.Sources.DatabaseClient.Services;
using UnityEngine;

namespace Assets.Sources.DatabaseClient.REST
{//TODO: Add logging
    public class RestCommunication
    {
        public const string BaseURL = isLocalhost ? "http://localhost:8733" : "http://deemi.ddns.net:8733";
        private const bool isLocalhost = false;
        private const int retryOnErrorCount = 5;
        private const int timeoutCount = 10;
        private const int sleepTimeout = 100;
        public string SendAndReceive(string url)
        {
            WWW www = null;
            int noOfConnections = 0;
            bool connectionSucceeded = false;
            do
            {
                www = new WWW(url);
                int noOfRetry = 0;
                while (!www.isDone && noOfRetry++ < timeoutCount)
                    Thread.Sleep(sleepTimeout);

                if (www.isDone && String.IsNullOrEmpty(www.error))
                    connectionSucceeded = true;
                else if(www.isDone)
                    noOfConnections++;

            } while (!connectionSucceeded && noOfConnections < retryOnErrorCount);
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
