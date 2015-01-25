using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Sources.Scripts.GameRoom
{
    public static class Chat
    {
        public const string System = "System";
        public const string NextDrawerMessage = "{0}'s drawing...";
        public const string PlayerHasJoinedMessage = "{0}'s joined.";
        public const string YouHaveJoinedMessage = "You've joined the room.";
        public const string PlayerHasLeftMessage = "{0}'s left.";
        public const string TimeIsUp =  "{0}'s time for drawing is up...";

        private static readonly Queue<MessageToDisplay> PendingMessageToDisplay = new Queue<MessageToDisplay>();
        private static readonly Queue<MessageToSend> PendingMessageToSend = new Queue<MessageToSend>();

        public static void AddMessageToDisplay(string message, string login, NetworkPlayer player)
        {
            PendingMessageToDisplay.Enqueue(new MessageToDisplay { Message = message, SenderLogin = login, SenderNetworkPlayer = player });
        }

        public static MessageToDisplay GetMessageToDisplay()
        {
            return PendingMessageToDisplay.Dequeue();
        }

        public static void AddMessageToSend(string message, string login)
        {
            PendingMessageToSend.Enqueue(new MessageToSend { Message = message, SenderLogin = login });
        }

        public static MessageToSend GetMessageToSend()
        {
            return PendingMessageToSend.Dequeue();
        }

        public static bool HasMessageToDisplay
        {
            get { return PendingMessageToDisplay.Any() && !IsChatMessageEmpty(PendingMessageToDisplay.Peek()); }
        }

        public static bool HasMessageToSend
        {
            get { return PendingMessageToSend.Any() && !IsMessageToSendEmpty(PendingMessageToSend.Peek()); }
        }

        private static bool IsChatMessageEmpty(MessageToDisplay message)
        {
            return message == null || String.IsNullOrEmpty(message.Message);
        }

        private static bool IsMessageToSendEmpty(MessageToSend message)
        {
            return message == null || String.IsNullOrEmpty(message.Message);
        }
    }

    public class MessageToDisplay
    {
        private const string GameWon = "{0}'s guessed! The phrase was {1}";
        public NetworkPlayer SenderNetworkPlayer { get; set; }
        public string SenderLogin { get; set; }
        public string Message { get; set; }

        public string FullMessage { get { return (SenderLogin + ": " + Message).Replace(Environment.NewLine, ""); } }
        public string WinningMessage { get { return (String.Format(GameWon, SenderLogin, Message)).Replace(Environment.NewLine, ""); } }
    }

    public class MessageToSend
    {
        public string SenderLogin { get; set; }
        public string Message { get; set; }
    }
}
