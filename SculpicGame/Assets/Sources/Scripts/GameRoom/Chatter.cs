using Assets.Sources.Scripts.Music;
using UnityEngine;

namespace Assets.Sources.Scripts.GameRoom
{
    [RequireComponent(typeof(NetworkView))]
    public class Chatter : MonoBehaviour
    {
        public SoundManager SoundManager;
        void Update()
        {
            if (Chat.HasMessageToSend)
            {
                var message = Chat.GetMessageToSend();
                var rpcMode = message.ToSelf ? RPCMode.All : RPCMode.Others;
                networkView.RPC("LogMessage", rpcMode, message.Message, message.SenderLogin, Network.player);
            }
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

        void SystemMessage(string message, NetworkPlayer player)
        {
            Chat.AddMessageToDisplay(message, Chat.System, player);
        }

        [RPC]
        void LogMessage(string message, string sender, NetworkPlayer player)
        {
            Chat.AddMessageToDisplay(message, sender, player);
            SoundManager.PlayChatMessageSound();
        }
    }
}
