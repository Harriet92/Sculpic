using System;
using System.Collections.Generic;
using Assets.Sources.Common;
using Assets.Sources.Enums;
using Assets.Sources.Scripts.GameRoom;
using Assets.Sources.Scripts.Sculptor;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Sources.Scripts.GuesserScreen
{
    // TODO: add leave and music buttons
    public class GuesserGUI : MenuBase
    {
        public Text ChatTextField;
        public InputField ChatInputField;
        public Toggle WantToDrawToggle;
        public GameObject PlayersScorePanel;
        public GameObject PlayerDataElement;

        void Start()
        {
            SculptorCurrentSettings.Rotate = true;
            ClientSide.OnNewScreenLoad(ChatTextField, WantToDrawToggle);
            UpdatePlayersList();
        }

        void OnDestroy()
        {
            SculptorCurrentSettings.ResetValues();
        }

        void Update()
        {
            if (ClientSide.ConnectedPlayers.HasChanged)
            {
                ClientSide.ConnectedPlayers.HasChanged = false;
                UpdatePlayersList();
            }
        }
        
        public void OnTextFieldEditEnd(string message)
        {
            if (String.IsNullOrEmpty(message)) return;
            ChatInputField.text = String.Empty;
            Chat.AddMessageToSend(message, Player.Name);
        }

        private void UpdatePlayersList()
        {
            ClearPlayersScorePanel();
            foreach (var playerData in ClientSide.ConnectedPlayers.Sorted())
                AddPlayerScoreElement(playerData);
        }

        private void ClearPlayersScorePanel()
        {
            var children = new List<GameObject>();
            foreach (Transform child in PlayersScorePanel.transform)
                children.Add(child.gameObject);
            children.ForEach(Destroy);
        }

        private void AddPlayerScoreElement(PlayerData playerData)
        {
            var button = (GameObject)Instantiate(PlayerDataElement);
            var playerScoreScript = button.GetComponentInChildren<PlayerScoreElement>();
            playerScoreScript.SetPlayerData(playerData, PlayersScorePanel);
        }

        public void LeaveButtonClick()
        {
            Application.LoadLevel(SceneName.RoomChoiceScreen.ToString());
        }
    }
}
