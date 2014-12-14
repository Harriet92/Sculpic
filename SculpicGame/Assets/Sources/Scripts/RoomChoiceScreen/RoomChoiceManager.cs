using System.Collections;
using System.Threading;
using Assets.Sources.Common;
using Assets.Sources.Enums;
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
            StartCoroutine(LoadLevel(SceneName.DrawerScreen.ToString()));
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
            StartCoroutine(LoadLevel(SceneName.GuesserScreen.ToString()));
        }

        #endregion JoinRoom

        private IEnumerator LoadLevel(string level)
        {
            Debug.Log("Method RoomChoiceManager.LoadLevel");
            Network.SetSendingEnabled(0, false);
            Network.isMessageQueueRunning = false;

            Debug.Log("Loading level: " + level);
            Application.LoadLevel(level);

            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();

            Network.isMessageQueueRunning = true;
            Network.SetSendingEnabled(0, true);

            Debug.Log("End of RoomChoiceManager.LoadLevel method");

        }
    }
}