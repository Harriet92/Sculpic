using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Sources.Scripts.GameServer
{
    public class ActivePlayers
    {
        private readonly List<PlayerData> _players = new List<PlayerData>();

        public bool HasChanged { get; set; }
        public int Count { get { return _players.Count; } }

        public ActivePlayers()
        {
            HasChanged = true;
        }

        public void Add(PlayerData playerData)
        {
            _players.Add(playerData);
            HasChanged = true;
        }

        public void Remove(NetworkPlayer player)
        {
            var playerData = _players.FirstOrDefault(p => p.NetworkPlayer == player);
            if (_players.Remove(playerData))
            {
                Debug.Log("ActivePlayers.Remove");
                HasChanged = true;
            }
        }

        public IEnumerable<PlayerData> Sorted()
        {
            _players.Sort((x, y) => -(x.Score - y.Score));
            return _players.ToList();
        }

        public void AddPoints(NetworkPlayer player, int points)
        {
            var data = _players.FirstOrDefault(pd => pd.NetworkPlayer == player);
            if (data != null)
            {
                data.Score += points;
                HasChanged = true;
            }
        }

        public string GetLogin(NetworkPlayer player)
        {
            var playerData = _players.FirstOrDefault(p => p.NetworkPlayer == player);
            if (playerData != null)
                return playerData.Login;
            return null;
        }
    }
}
