using System;
using System.Text;
using Assets.Sources.Common;
using Assets.Sources.Enums;
using Assets.Sources.Scripts.GameServer;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Sources.Scripts.GuesserScreen
{
    // TODO: add leave and music buttons
    // TODO: add "want to draw" checkbox
    public class GuesserGUI : MenuBase
    {
        public Text ChatTextField;
        public InputField ChatInputField;

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape)) { Application.LoadLevel(SceneName.RoomChoiceScreen.ToString()); }
            if(!ChatterState.DisplayQueueEmpty && !String.IsNullOrEmpty(ChatterState.PendingMessageToDisplay.Peek()))
                DisplayNewMessage(ChatterState.PendingMessageToDisplay.Dequeue());
        }

        public void OnTextFieldEditEnd(string value)
        {
            ChatInputField.text = String.Empty;
            ChatterState.PendingMessageToSend.Enqueue(CreateUserMessageLine(value));
        }

        private void DisplayNewMessage(string message)
        {
            StringBuilder builder = new StringBuilder(ChatTextField.text);
            ChatTextField.text = builder.AppendLine(message).ToString();
        }

        private string CreateUserMessageLine(string value)
        {
            return (Player.Current == null ? "Stranger: " : Player.Current.Username + ": ") + value.Replace(Environment.NewLine, "" );
        }

    }
}
