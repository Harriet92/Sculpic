using UnityEngine;
using UnityEngine.UI;

namespace Assets.Sources.Scripts.RoomChoiceScreen
{
    public class RoomButton : MonoBehaviour
    {
        public Text RoomName;
        public Text NumberOfPlayers;
        private HostData _hostData;

        public void JoinRoom()
        {
            Debug.Log("Host gameName: " + _hostData.gameName);
            Network.Connect(_hostData);
        }

        public void SetRoomData(HostData hostData, GameObject parentPanel)
        {
            _hostData = hostData;
            RoomName.text = hostData.gameName;
            NumberOfPlayers.text = (hostData.connectedPlayers - 1) + "/" + (hostData.playerLimit - 1); // 1 is for server who isn't a player
            transform.parent = parentPanel.transform;
        }
    }
}
