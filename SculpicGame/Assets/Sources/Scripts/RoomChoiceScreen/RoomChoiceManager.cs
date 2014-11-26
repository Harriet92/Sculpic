using Assets.Sources.Common;
using UnityEngine;

namespace Assets.Sources.Scripts.RoomChoiceScreen
{
    public class RoomChoiceManager : MonoBehaviour
    {

        #region HostRoom
        public static void HostRoom(int roomPort, int connectionsNo, string gameName)
        {
            Debug.Log("Method RoomChoiceManager.HostRoom");
            Network.InitializeServer(connectionsNo, roomPort, !Network.HavePublicAddress());
            MasterServerConnectionManager.RegisterHost(gameName);
        }

        void OnServerInitialized()
        {
            Debug.Log("Method RoomChoiceManager.OnServerInitialized");
            //TODO: uncomment
            //Player.Current.IsHost = true;
            Application.LoadLevel("GameScreen");
        }
        #endregion

        #region JoinRoom

        public static void JoinFirstRoom()
        {
            Debug.Log("Method RoomChoiceManager.JoinFirstRoom");
            var hostData = GetFirstRoom();
            if (hostData != null)
            {
                Debug.Log("Host gameName: " + hostData.gameName);
                Network.Connect(hostData);
            }
        }

        private static HostData GetFirstRoom()
        {
            Debug.Log("Method RoomChoiceManager.GetFirstRoom");
            var hostList = MasterServerConnectionManager.HostList;
            Debug.Log("Host list length: " + (hostList == null ? "null" : hostList.Length.ToString()));
            if (hostList != null && hostList.Length >= 1)
            {
                return hostList[0];
            }
            return null;
        }

        void OnConnectedToServer()
        {
            Debug.Log("Method RoomChoiceManager.OnConnectedToServer");
            // TODO: uncomment
            //Player.Current.IsHost = false;
            Application.LoadLevel("GameScreen");
        }

        #endregion JoinRoom
        }
}