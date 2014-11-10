using System;
using System.Collections;
using System.Collections.Generic;
using HTTP;

namespace Assets.Sources.DatabaseServer.REST
{//TODO: Add logging
    public class RestCommunication
    {
        private string URL;
        private string response;
        //private Logging.LoggingHelper loger;
        private int retryCount = 0;

        public RestCommunication()
        {
            //loger = new Logging.LoggingHelper();
        }
        public RestCommunication(string postUrl)
        {
            //loger = new Logging.LoggingHelper();
            URL = postUrl;
        }

        public void Send(string json, string password = null)
        {
            try
            {
                //loger.WriteLog(String.Format("Start sending Json = {0} to {1}", json, this.URL), Logging.LogLevel.Debug);

                Hashtable data = new Hashtable();
                data.Add("", "");
                Request someRequest = new Request("POST", URL);


                someRequest.Text = json;
                someRequest.SetHeader("Content-Type", "application/json");
                someRequest.SetHeader("charset", "UTF-8");
                //AddSessionKeyToHeader(someRequest);
                if (!String.IsNullOrEmpty(password))
                {
                    someRequest.AddHeader("OTPassword-Type", password);
                }
                someRequest.Send();
                int failCounter = 0;

                while ((!someRequest.isDone || someRequest.response == null) && failCounter < 300)
                {
                    System.Threading.Thread.Sleep(25);
                    failCounter++;
                }
                if (someRequest.exception != null)
                {
                    //loger.WriteLog(String.Format("{0} - Communication exception: {1}",URL, someRequest.exception.Message), Logging.LogLevel.Warning);
                    throw someRequest.exception;
                }
                if (someRequest.response == null)
                {
                    //loger.WriteLog(String.Format("{0} - Response is null",URL), Logging.LogLevel.Warning);
                    throw new Exception("Response is null");
                }

                response = someRequest.response.Text;
                //SetOtpPassword(someRequest);
                //SessionHelper.SetSessionKey(someRequest.response.GetHeader(SessionHelper.ErrorHeaderCode),
                //someRequest.response.GetHeader(SessionHelper.SessionHeaderCode));

                //loger.WriteLog(String.Format("Successful send to {0}", this.URL), Logging.LogLevel.Info);
            }
            catch (Exception)
            {
                if (retryCount <= 3)
                {
                    //loger.WriteLog(String.Format("{0} - Retry request",URL), Logging.LogLevel.Warning);
                    retryCount++;
                    Send(json, password);
                }
                //loger.WriteLog(String.Format("{0} - COMMUNICATION ERROR!", URL), Logging.LogLevel.Error);
            }
        }


        public string Recive()
        {
            //loger.WriteLog(String.Format("Successful recive from {0}", URL), Logging.LogLevel.Info);
            //loger.WriteLog(String.Format("Recive: {0} from {1}", response, URL), Logging.LogLevel.Debug);
            return response;
        }

        public string SendAndRecive(string json, string password)
        {
            Send(json, password);
            return Recive();;
        }

        public void SetUrl(string url)
        {
            URL = url;
            //loger.WriteLog("SET URL = " + this.URL + " in [WebPlayerRestCommunication]", Logging.LogLevel.Debug);
        }
    }
}
