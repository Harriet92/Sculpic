using UnityEngine;
using UnityEngine.UI;

namespace Assets.Sources.Scripts.RoomChoiceScreen
{
    public class RoomButton : MonoBehaviour
    {
        public Text RoomName;
        public Text NumberOfPlayers;
        public Image LockImage;
        public InsertPasswordPopup PasswordCanvas;
        private HostData hostData;
        private RoomChoiceMenu roomChoiceMenu;

        void Start()
        {
            roomChoiceMenu = FindObjectOfType<RoomChoiceMenu>();
        }

        public void JoinRoom()
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

        public void SetRoomData(HostData hostData, GameObject parentPanel)
        {
            this.hostData = hostData;
            RoomName.text = hostData.gameName;
            NumberOfPlayers.text = (hostData.connectedPlayers - 1) + "/" + (hostData.playerLimit - 1); // 1 is for server who isn't a player
            transform.parent = parentPanel.transform;
            LockImage.gameObject.SetActive(hostData.passwordProtected);
        }
    }
}
