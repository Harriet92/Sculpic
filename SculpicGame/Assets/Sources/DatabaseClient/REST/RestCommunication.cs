using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Assets.Sources.DatabaseClient.Services;
using UnityEngine;

namespace Assets.Sources.DatabaseClient.REST
{//TODO: Add logging
    public class RestCommunication : MonoBehaviour
    {
        private const bool isLocalhost = false;
        private const int retryCount = 5;
        private const int sleepTimeout = 500;
        public const string BaseURL = isLocalhost ? "http://localhost:8733" : "http://deemi.ddns.net:8733";


        void Start() { }

        public WWW GET(string url)
        {

            WWW www = new WWW(url);
            StartCoroutine(WaitForRequest(www));
            return www;
        }

        public WWW POST(string url, Dictionary<string, string> post)
        {
            WWWForm form = new WWWForm();
            foreach (KeyValuePair<String, String> post_arg in post)
            {
                form.AddField(post_arg.Key, post_arg.Value);
            }
            WWW www = new WWW(url, form);

            StartCoroutine(WaitForRequest(www));
            return www;
        }

        private IEnumerator WaitForRequest(WWW www)
        {
            yield return www;

            // check for errors
            if (www.error == null)
            {
                Debug.Log("WWW Ok!: " + www.text);
            }
            else
            {
                Debug.Log("WWW Error: " + www.error);
            }
        }

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
                else if (www.isDone)
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
