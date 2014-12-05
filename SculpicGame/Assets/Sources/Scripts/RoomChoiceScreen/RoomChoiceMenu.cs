using Assets.Sources.Common;
using Assets.Sources.Enums;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Sources.Scripts.RoomChoiceScreen
{

    public class RoomChoiceMenu : MonoBehaviour
    {
        public const int RoomPort = 25001;
        public const int ConnectionsNo = 4;
        public string GameName = "First game";
        private GameObject _joinRoomButton;
        private GameObject _hostButton;

        private void Awake()
        {
            Debug.Log("Method RoomChoiceMenu.Awake");
            MasterServerConnectionManager.SetMasterServerLocation();
            _joinRoomButton = GameObject.Find("JoinRoomButton");
            _hostButton = GameObject.Find("HostButton");
            _joinRoomButton.gameObject.SetActive(false);
        }

        private void Update()
        {

            if (Input.GetKeyDown(KeyCode.Escape)) { Application.LoadLevel(SceneName.LoginScreen.ToString()); }
            if (MasterServerConnectionManager.HasHosts && _hostButton.gameObject.activeSelf)
                _hostButton.gameObject.SetActive(false);

            if (MasterServerConnectionManager.HasHosts && !_joinRoomButton.gameObject.activeSelf)
                _joinRoomButton.gameObject.SetActive(true);
        }

        public void HostRoom()
        {
            Debug.Log("Method RoomChoiceMenu.HostRoom");
            RoomChoiceManager.HostRoom(RoomPort, ConnectionsNo, GameName);
        }

        public void JoinFirstRoom()
        {
            Debug.Log("Method RoomChoiceMenu.JoinFirstRoom");
            RoomChoiceManager.JoinFirstRoom();
        }

        public void RefreshRoomHosts()
        {
            Debug.Log("Method RoomChoiceMenu.RefreshRoomHosts");
            MasterServerConnectionManager.RefreshHostList();
        }
    }
}