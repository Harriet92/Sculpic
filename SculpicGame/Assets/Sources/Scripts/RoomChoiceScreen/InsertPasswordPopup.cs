using System.Threading;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Sources.Scripts.RoomChoiceScreen
{
    public class InsertPasswordPopup: MenuBase
    {
        public HostData hostData;
        public InputField PasswordInput;
        public void EnterRoomClick()
        {
            Network.Connect(hostData, PasswordInput.text);
        }
        void OnFailedToConnect(NetworkConnectionError error)
        {
            if (error == NetworkConnectionError.InvalidPassword)
                DisplayInfoPopup("Incorrect password, try again!");
            else
                Debug.Log("Game server error, try again later.");
        }

        public void BackButtonClick()
        {
            Destroy(this);
        }
    }
}
