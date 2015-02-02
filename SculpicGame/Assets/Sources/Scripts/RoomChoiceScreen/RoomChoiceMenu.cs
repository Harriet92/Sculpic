using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Sources.Common;
using Assets.Sources.Enums;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

namespace Assets.Sources.Scripts.RoomChoiceScreen
{

    public class RoomChoiceMenu : MenuBase
    {
        public string GameName = "First game";
        public GameObject RoomButtonsPanel;
        public Button RoomButton;
        public Text PlayerNameText;
        public Text PlayerRankingText;
        public InsertPasswordPopup PasswordCanvas;
        private Random random;

        private void Awake()
        {
            Debug.Log("Method RoomChoiceMenu.Awake");
            random = new Random();
            MasterServerConnectionManager.SetMasterServerLocation();
            PlayerNameText.text = Player.Name;
            PlayerRankingText.text = Player.GetRanking.ToString();
            RefreshRoomHosts();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape)) { Application.Quit(); }
        }

        public void HostRoom()
        {
            //For local room tests
            //MasterServerConnectionManager.RefreshHostList();
            //StartCoroutine(InitServerAndHostRoom(MasterServerConnectionManager.RoomPort, MasterServerConnectionManager.ConnectionsNo, GameName));
            Application.LoadLevel(SceneName.RoomSettingsScreen.ToString());
        }

        void OnFailedToConnect(NetworkConnectionError error)
        {
            if (error == NetworkConnectionError.TooManyConnectedPlayers)
                DisplayInfoPopup("Maximum player limit reached, try with a different room");
            else
                Debug.Log("Game server error, try again later.");
        }

        private IEnumerator InitServerAndHostRoom(int roomPort, int connectionsNo, string gameName)
        {
            while (!MasterServerConnectionManager.HostsRefreshed)
                yield return null;

            Network.InitializeServer(connectionsNo, roomPort + MasterServerConnectionManager.HostList.Length, true);
            MasterServerConnectionManager.RegisterHost(gameName);
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
            var button = (Button)Instantiate(RoomButton);
            var roomButtonScript = button.GetComponentInChildren<RoomButton>();
            roomButtonScript.SetRoomData(hostData, RoomButtonsPanel);
        }

        public void JoinRoom(HostData hostData)
        {
            Debug.Log("Host gameName: " + hostData.gameName);
            if (!hostData.passwordProtected)
                Network.Connect(hostData);
            else
            {
                var passPopup = Instantiate(PasswordCanvas) as InsertPasswordPopup;
                passPopup.hostData = hostData;
            }
        }

        public void JoinRandomRoom()
        {
            int passFreeCount = MasterServerConnectionManager.HostList.Count(x => !x.passwordProtected);
            if (passFreeCount == 0)
                DisplayInfoPopup("There are no open rooms.");
            else
                JoinRoom(MasterServerConnectionManager.HostList[random.Next(0, MasterServerConnectionManager.HostList.Count(x => !x.passwordProtected))]);
        }

        public void LogOffClick()
        {
            Preferences.RememberLogin = false;
            Application.LoadLevel(SceneName.LoginScreen.ToString());
        }
    }
}