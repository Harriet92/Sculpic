using System;
using Assets.Sources.Common;
using Assets.Sources.Scripts.GameServer;
using UnityEngine.UI;

namespace Assets.Sources.Scripts.GuesserScreen
{
    // TODO: add leave and music buttons
    public class GuesserGUI : MenuBase
    {
        public Text ChatTextField;
        public InputField ChatInputField;
        public Toggle WantToDrawToggle;

        void Start()
        {
            Room.KeepState(ChatTextField, WantToDrawToggle);
        }
        
        public void OnTextFieldEditEnd(string message)
        {
            if (String.IsNullOrEmpty(message)) return;
            ChatInputField.text = String.Empty;
            Chat.AddMessageToSend(message, Player.Current == null ? "Stranger" : Player.Current.Username);
        }
    }
}
