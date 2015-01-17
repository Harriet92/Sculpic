using UnityEngine;

namespace Assets.Sources.Scripts.GameRoom
{
    public class PlayerData
    {
        public NetworkPlayer NetworkPlayer { get; set; }
        public string Login { get; set; }
        public int Score { get; set; }
    }
}