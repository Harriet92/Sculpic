using System;
using UnityEngine;

namespace Assets.Sources.Common
{
    public class MasterServerConnectionManager : MonoBehaviour
    {
        private const string MasterServerIp = "127.0.0.1";
        private const int MasterServerPort = 23466;

        public const string GameTypeName = "Sculpic";
        public static HostData[] HostList { get; private set; }

        public static void SetMasterServerLocation()
        {
            Debug.Log("Method MasterServerConnectionManager.SetMasterServerLocation");
            MasterServer.ipAddress = MasterServerIp;
            MasterServer.port = MasterServerPort;
        }

        public static void RefreshHostList()
        {
            Debug.Log("Method MasterServerConnectionManager.RefreshHostList");
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