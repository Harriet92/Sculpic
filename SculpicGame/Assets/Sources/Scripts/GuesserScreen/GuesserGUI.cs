using System;
using System.Collections.Generic;
using Assets.Sources.Common;
using Assets.Sources.Scripts.GameServer;
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
            Room.KeepState(ChatTextField, WantToDrawToggle);
            UpdatePlayersList();
        }
        
        public void OnTextFieldEditEnd(string message)
        {
            if (String.IsNullOrEmpty(message)) return;
            ChatInputField.text = String.Empty;
            Chat.AddMessageToSend(message, Player.Current == null ? "Stranger" : Player.Current.Username);
        }

        public void UpdatePlayersList()
        {
            Room.Players.Sort((x,y) => x.Points - y.Points);
            ClearPlayersScorePanel();
            foreach (var playerData in Room.Players)
                AddRoomButton(playerData);
            
        }
        private void ClearPlayersScorePanel()
        {
            var children = new List<GameObject>();
            foreach (Transform child in PlayersScorePanel.transform)
                children.Add(child.gameObject);
            children.ForEach(Destroy);
        }

        private void AddRoomButton(PlayerData playerData)
        {
            var button = (GameObject)Instantiate(PlayerDataElement);
            var playerScoreScript = button.GetComponentInChildren<PlayerScoreElement>();
            playerScoreScript.SetPlayerData(playerData, PlayersScorePanel);
        }
    }
}
