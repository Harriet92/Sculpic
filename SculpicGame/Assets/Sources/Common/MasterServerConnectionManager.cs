using System;
using System.Linq;
using UnityEngine;

namespace Assets.Sources.Common
{
    public class MasterServerConnectionManager : MonoBehaviour
    {
        //TODO: Config file
        private const string MasterServerIp = "sculpicserver.cloudapp.net";
        private const int MasterServerPort = 23466;

        public const string GameTypeName = "Sculpic";
        public const int RoomPort = 25001;
        public const int ConnectionsNo = 4;
        public static bool HasHosts { get { return HostList != null && HostList.Length > 0; } }
        public static bool HostsRefreshed { get; private set; }
        public static HostData[] HostList { get; private set; }

        static MasterServerConnectionManager()
        {
            HostList = new HostData[0];
        }
        public static void SetMasterServerLocation()
        {
            Debug.Log("Method MasterServerConnectionManager.SetMasterServerLocation");
            MasterServer.ipAddress = MasterServerIp;
            MasterServer.port = MasterServerPort;

        }

        public static HostData GetHostDataByGameName(string gameName)
        {
            return HostList.FirstOrDefault(x => x.gameName == gameName);
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