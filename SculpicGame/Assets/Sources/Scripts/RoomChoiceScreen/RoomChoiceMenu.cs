using Assets.Sources.Common;
using UnityEngine;

namespace Assets.Sources.Scripts.RoomChoiceScreen
{

    public class RoomChoiceMenu : MonoBehaviour
    {
        public const int RoomPort = 25001;
        public const int ConnectionsNo = 4;
        public string GameName = "First game";

        private void Awake()
        {
            Debug.Log("Method RoomChoiceMenu.Awake");
            MasterServerConnectionManager.SetMasterServerLocation();
        }

        public void HostRoom()
        {
            Debug.Log("Method RoomChoiceMenu.HostRoom");
            RoomChoiceManager.HostRoom(RoomPort, ConnectionsNo, GameName);
        }

        public void JoinFirstRoom()
        {
            Debug.Log("Method RoomChoiceMenu.JoinFirstRoom");
            RoomChoiceManager.JoinFirstRoom();
        }

        public void RefreshRoomHosts()
        {
            Debug.Log("Method RoomChoiceMenu.RefreshRoomHosts");
            MasterServerConnectionManager.RefreshHostList();
        }
    }
}