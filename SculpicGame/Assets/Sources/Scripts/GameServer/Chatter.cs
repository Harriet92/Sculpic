using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Sources.Scripts.GameServer
{
    [RequireComponent(typeof(NetworkView))]
    public class Chatter : MonoBehaviour
    {
        void Update()
        {
            if (!ChatterState.SendQueueEmpty && !String.IsNullOrEmpty(ChatterState.PendingMessageToSend.Peek()))
                networkView.RPC("LogMessage", RPCMode.All, ChatterState.PendingMessageToSend.Dequeue(), Network.player);
        }


        void OnDisconnectedFromServer(NetworkDisconnection info)
        {
            if (Network.isServer)
                SystemMessage("Server connection lost!", Network.player);
            else
                if (info == NetworkDisconnection.LostConnection)
                    SystemMessage("Lost connection to server!", Network.player);
                else
                    SystemMessage("Successfully diconnected from server.", Network.player);
        }
        

        [RPC]
        void SystemMessage(string message, NetworkPlayer player)
        {
            ChatterState.PendingMessageToDisplay.Enqueue("System: " + message);
        }

        [RPC]
        void LogMessage(string message, NetworkPlayer player)
        {
            ChatterState.PendingMessageToDisplay.Enqueue(message);
        }
    }
}
