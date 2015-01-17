﻿using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Sources.Scripts.GameRoom
{
    public class ClientSide
    {
        private Text _chatTextField;
        private readonly StringBuilder _chatHistory = new StringBuilder();
        private Toggle _wantToDrawToggle;
        public bool WantToDraw { get; set; }
        public bool IsDrawer { get; set; }
        public readonly ActivePlayers ConnectedPlayers = new ActivePlayers();
        public bool IsRegistered { get; set; }
        private bool _isActive;

        public bool CanRegister
        {
            get { return !IsRegistered && _isActive; }
        }
        public string Phrase { get; set; }

        public void OnNewScreenLoad(Text chatTextField, Toggle wantToDrawToggle = null)
        {
            _chatTextField = chatTextField;
            if (wantToDrawToggle != null)
            {
                _wantToDrawToggle = wantToDrawToggle;
                wantToDrawToggle.isOn = WantToDraw;
            }
            RefreshChat();
            _isActive = true;
        }

        private void RefreshChat()
        {
            _chatTextField.text = _chatHistory.ToString();
        }

        public void DisplayMessage(string message)
        {
            if (_chatTextField == null)
            {
                Debug.Log("Room.ChatTextField is null.");
                return;
            }
            _chatHistory.AppendLine(message);
            RefreshChat();
        }

        public void SetDrawer(string phrase)
        {
            Debug.Log("Method ClientSide.SetDrawer");
            IsDrawer = true;
            Phrase = phrase;
        }
    }
}
