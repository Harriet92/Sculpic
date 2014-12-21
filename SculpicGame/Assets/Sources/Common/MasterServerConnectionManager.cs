using System;
using UnityEngine;

namespace Assets.Sources.Common
{
    public class MasterServerConnectionManager : MonoBehaviour
    {
        private const string MasterServerIp = "localhost";//"deemi.ddns.net";
        private const int MasterServerPort = 23466;

        public const string GameTypeName = "Sculpic";
        public static bool HasHosts { get { return HostList != null && HostList.Length > 0; }}
        public static bool HostsRefreshed { get; private set; }
        public static HostData[] HostList { get; private set; }

        static MasterServerConnectionManager()
        {
            HostList = new HostData[0];
        }
        public static void SetMasterServerLocation()
        {
            Debug.Log("Method MasterServerConnectionManager.SetMasterServerLocation");
            //Network.natFacilitatorIP = 
                MasterServer.ipAddress = MasterServerIp;
            //Network.natFacilitatorPort = 8735;
            MasterServer.port = MasterServerPort;
         
        }
        public static void RefreshHostList()
        {
            Debug.Log("Method MasterServerConnectionManager.RefreshHostList set FALSE");
            HostsRefreshed = false;
            MasterServer.RequestHostList(GameTypeName);
        }

        void OnMasterServerEvent(MasterServerEvent masterServerEvent)
        {
            Debug.Log("Method MasterServerConnectionManager.OnMasterServerEvent: " + masterServerEvent);
            switch (masterServerEvent)
            {
                case MasterServerEvent.RegistrationFailedGameName:
                    break;
                case MasterServerEvent.RegistrationFailedGameType:
                    break;
                case MasterServerEvent.RegistrationFailedNoServer:
                    break;
                case MasterServerEvent.RegistrationSucceeded:
                    break;
                case MasterServerEvent.HostListReceived:
                    HostList = MasterServer.PollHostList();
                    HostsRefreshed = true;
                    Debug.Log("HostList.Length: " + HostList.Length);
                    break;
                default:
                    throw new ArgumentOutOfRangeException("masterServerEvent");
            }
        }

        public static void RegisterHost(string gameName)
        {
            Debug.Log("Method MasterServerConnectionManager.RegisterHost: " + gameName);
            MasterServer.RegisterHost(GameTypeName, gameName);
        }
    }
     
}