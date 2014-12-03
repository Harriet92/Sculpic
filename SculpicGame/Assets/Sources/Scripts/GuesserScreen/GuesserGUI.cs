using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Sources.Common;
using Assets.Sources.Scripts.Sculptor;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Sources.Scripts.DrawerScreen
{
    public class GuesserGUI : MenuBase
    {
        public Text ChatTextField;
        public InputField ChatInputField;
        public void OnTextFieldEditEnd(string value)
        {
            StringBuilder builder = new StringBuilder(ChatTextField.text);
            ChatInputField.text = String.Empty;
            ChatTextField.text = builder.AppendLine(CreateUserMessageLine(value)).ToString();
        }

        private string CreateUserMessageLine(string value)
        {
            return (Player.Current == null ? "Stranger: " : Player.Current.Username + " :") + value;
        }

    }
}
