using System.Collections.Generic;
using System.Net.Mail;

namespace Assets.Sources.Scripts.GameServer
{
    public static class ChatterState
    {
        public static Queue<string> PendingMessageToDisplay { get; set; }
        public static Queue<string> PendingMessageToSend { get; set; }
        public static bool SendQueueEmpty { get { return PendingMessageToSend.Count == 0; } }
        public static bool DisplayQueueEmpty { get { return PendingMessageToDisplay.Count == 0; } }
        static ChatterState()
        {
            PendingMessageToDisplay = new Queue<string>();
            PendingMessageToSend = new Queue<string>();
        }
    }
}
