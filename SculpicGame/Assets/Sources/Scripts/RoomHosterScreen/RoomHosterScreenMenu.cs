using System.Collections;
using System.Linq;
using Assets.Sources.Common;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Sources.Scripts.RoomHosterScreen
{
    public class RoomHosterScreenMenu : MonoBehaviour
    {
        private string gameName = "Test Game";
        public Text GameNameTextField;
        public Text PlayersNumberTextField;

        void Start ()
        {
            MasterServerConnectionManager.SetMasterServerLocation(); 
            MasterServerConnectionManager.RefreshHostList();
            StartCoroutine(InitServerAndHostRoom(MasterServerConnectionManager.RoomPort, MasterServerConnectionManager.ConnectionsNo, gameName));
        }

        private IEnumerator InitServerAndHostRoom(int roomPort, int connectionsNo, string gameName)
        {
            while (!MasterServerConnectionManager.HostsRefreshed)
                yield return null;

            Network.InitializeServer(connectionsNo, roomPort + MasterServerConnectionManager.HostList.Length, true);
            MasterServerConnectionManager.RegisterHost(gameName);
            GameNameTextField.text = gameName;
        }
	
        void Update ()
        {
            PlayersNumberTextField.text = Network.connections.Count() + "/" + Network.maxConnections;
        }
    }
}
