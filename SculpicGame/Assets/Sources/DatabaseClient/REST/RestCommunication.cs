using System.Threading;
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
            WWW www = new WWW(url);
            //Thread thread = new Thread(() =>
            //{
                Debug.Log("Sending:" + www.url);
                int noOfRetry = 0;
                while (!www.isDone && noOfRetry++ < retryCount)
                {
                    Thread.Sleep(sleepTimeout);
                }
            //});
            //thread.Start();
            //thread.Join();
            return www.isDone ? www.text : null;
        }
    }
}
