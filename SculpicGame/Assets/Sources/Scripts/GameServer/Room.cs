using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Sources.Scripts.GameServer
{
    [RequireComponent(typeof(NetworkView))]
    class Room : MonoBehaviour
    {
        // RoomOwner
        private readonly List<NetworkPlayer> _drawers = new List<NetworkPlayer>();

        [RPC]
        public void SignUpForDrawing(NetworkPlayer player)
        {
            Debug.Log("Method RoomOwner.SignUpForDrawing");
            _drawers.Add(player);
            Debug.Log("Drawers count: " + _drawers.Count);
        }

        [RPC]
        public void SignOffFromDrawing(NetworkPlayer player)
        {
            Debug.Log("Method RoomOwner.SignOffFromDrawing");
            if (_drawers.Remove(player))
                Debug.Log("Removed player from drawing queue.");
            Debug.Log("Drawers count: " + _drawers.Count);
        }

        public void OnWantToDrawValueChanged(Toggle callingObject)
        {
            bool wantToDraw = callingObject.isOn;
            Debug.Log("Method RoomOwner.WantToDrawToggle: wantToDraw == " + wantToDraw);
            networkView.RPC(wantToDraw ? "SignUpForDrawing" : "SignOffFromDrawing", RPCMode.Server, Network.player);
        }

        // TODO: checking if phrase matches
        // TODO: assigning drawer and phrase


        // TODO: player dictionary

        // TODO: recieving data (winner, points, next drawer)
        // TODO: adding points to winner
        // TODO: load screen (drawer/guesser)

        // TODO: add self to drawing queue
    }
}
