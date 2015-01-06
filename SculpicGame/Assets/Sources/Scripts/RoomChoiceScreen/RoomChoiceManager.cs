using Assets.Sources.Common;
using Assets.Sources.Enums;
using Assets.Sources.Scripts.GameServer;
using UnityEngine;

namespace Assets.Sources.Scripts.RoomChoiceScreen
{
    public class RoomChoiceManager : MonoBehaviour
    {
        void Awake()
        {
            DontDestroyOnLoad(this);
        }

        private void OnServerInitialized()
        {
            Debug.Log("Method RoomChoiceManager.OnServerInitialized");
            StartCoroutine(ScreenHelper.LoadLevel(SceneName.GuesserScreen));
        }

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

        private void OnConnectedToServer()
        {
            Debug.Log("Method RoomChoiceManager.OnConnectedToServer");
            StartCoroutine(ScreenHelper.LoadLevel(SceneName.GuesserScreen));
        }

        #endregion JoinRoom
    }
}