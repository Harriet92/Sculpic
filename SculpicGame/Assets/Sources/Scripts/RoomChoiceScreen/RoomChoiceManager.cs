using Assets.Sources.Common;
using Assets.Sources.Enums;
using Assets.Sources.Scripts.GameRoom;
using UnityEngine;

namespace Assets.Sources.Scripts.RoomChoiceScreen
{
    public class RoomChoiceManager : MonoBehaviour
    {
        void Awake()
        {
            Debug.Log("Method RoomChoiceManager.Awake");
            DontDestroyOnLoad(this);
            Room.Clear();
        }

        private void OnServerInitialized()
        {
            Debug.Log("Method RoomChoiceManager.OnServerInitialized");
            StartCoroutine(ScreenHelper.LoadLevel(SceneName.GuesserScreen));
        }

        #region JoinRoom

        private void OnConnectedToServer()
        {
            Debug.Log("Method RoomChoiceManager.OnConnectedToServer");
            StartCoroutine(ScreenHelper.LoadLevel(SceneName.GuesserScreen));
        }

        #endregion JoinRoom
    }
}