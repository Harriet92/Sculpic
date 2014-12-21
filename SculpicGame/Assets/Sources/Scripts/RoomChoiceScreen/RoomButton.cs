using System.Net.Mime;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class RoomButton : MonoBehaviour
{

    public Text RoomName;
    public Text NumberOfPlayers;
    private HostData hostData;

    public void JoinRoom()
    {
        Debug.Log("Host gameName: " + hostData.gameName);
        Network.Connect(hostData);
    }

    public void SetRoomData(HostData _hostData, GameObject parentPanel)
    {
        hostData = _hostData;
        RoomName.text = _hostData.gameName;
        NumberOfPlayers.text = _hostData.connectedPlayers + "/" + _hostData.playerLimit;
        transform.parent = parentPanel.transform;
    }
}
