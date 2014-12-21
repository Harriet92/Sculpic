using System.Collections;
using System.Collections.Generic;
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
        public GameObject RoomButtonsPanel;
        public Button RoomButton;

        private void Awake()
        {
            Debug.Log("Method RoomChoiceMenu.Awake");
            MasterServerConnectionManager.SetMasterServerLocation();
            RefreshRoomHosts();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape)) { Application.LoadLevel(SceneName.LoginScreen.ToString()); }
        }

        public void HostRoom()
        {
            MasterServerConnectionManager.RefreshHostList();
            StartCoroutine(InitServerAndHostRoom(RoomPort, ConnectionsNo, GameName));
        }

        private IEnumerator InitServerAndHostRoom(int roomPort, int connectionsNo, string gameName)
        {
            while (!MasterServerConnectionManager.HostsRefreshed)
                yield return null;

            Network.InitializeServer(connectionsNo, roomPort + MasterServerConnectionManager.HostList.Length, true);
            MasterServerConnectionManager.RegisterHost(gameName);
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
            StartCoroutine(DisplayHostList());
        }

        private IEnumerator DisplayHostList()
        {
            while (!MasterServerConnectionManager.HostsRefreshed)
                yield return null;
            ClearRoomPanel();
            foreach (var hostData in MasterServerConnectionManager.HostList)
                AddRoomButton(hostData);
        }

        private void ClearRoomPanel()
        {
            var children = new List<GameObject>();
            foreach (Transform child in RoomButtonsPanel.transform) children.Add(child.gameObject);
            children.ForEach(Destroy);
        }
        private void AddRoomButton(HostData hostData)
        {
            var button = (Button) Instantiate(RoomButton);
            var roomButtonScript = button.GetComponentInChildren<RoomButton>();
            roomButtonScript.SetRoomData(hostData, RoomButtonsPanel);
        }
    }
}