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
        public string GameName = "First game";
        public GameObject RoomButtonsPanel;
        public Button RoomButton;
        public Text PlayerNameText;

        private void Awake()
        {
            Debug.Log("Method RoomChoiceMenu.Awake");
            MasterServerConnectionManager.SetMasterServerLocation();
            PlayerNameText.text = Player.Name;
            RefreshRoomHosts();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape)) { Application.Quit(); }
        }

        public void HostRoom()
        {
            //MasterServerConnectionManager.RefreshHostList();
            //StartCoroutine(InitServerAndHostRoom(MasterServerConnectionManager.RoomPort, MasterServerConnectionManager.ConnectionsNo, GameName));
            Application.LoadLevel(SceneName.RoomSettingsScreen.ToString());
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
            foreach (Transform child in RoomButtonsPanel.transform) 
                children.Add(child.gameObject);
            children.ForEach(Destroy);
        }

        private void AddRoomButton(HostData hostData)
        {
            var button = (Button) Instantiate(RoomButton);
            var roomButtonScript = button.GetComponentInChildren<RoomButton>();
            roomButtonScript.SetRoomData(hostData, RoomButtonsPanel);
        }

        public void LogOffClick()
        {
            Preferences.RememberLogin = false;
            Application.LoadLevel(SceneName.LoginScreen.ToString());
        }
    }
}