using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Sources.Scripts.GameServer
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

       public void KeepState(Text chatTextField, Toggle wantToDrawToggle = null)
       {
           _chatTextField = chatTextField;
           if (wantToDrawToggle != null)
           {
               _wantToDrawToggle = wantToDrawToggle;
               wantToDrawToggle.isOn = WantToDraw;
           }
           RefreshChat();
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

       public void Clear()
       {
           _wantToDrawToggle.isOn = false;
       }
    }
}
