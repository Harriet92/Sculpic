using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Sources.Scripts.GameServer
{
    public class PlayerData
    {
        public NetworkPlayer NetworkPlayer { get; set; }
        public string Login { get; set; }
        public int Points { get; set; }
    }
}