using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Sources.Scripts.GameRoom
{
    public class ActivePlayers
    {
        private readonly List<PlayerData> _players = new List<PlayerData>();
        private readonly int _maxScore;

        public bool HasChanged { get; set; }
        public int Count { get { return _players.Count; } }

        public bool GameEnds { get; private set; }

        public ActivePlayers(int maxScore)
        {
            HasChanged = true;
            _maxScore = maxScore;
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
                if (data.Score > _maxScore)
                    GameEnds = true;
                HasChanged = true;
            }
        }

        public void GetRankingData(out string usernames, out string scores)
        {
            usernames = String.Empty;
            scores = String.Empty;
            foreach (var playerData in _players)
            {
                const string separator = ";";
                usernames += playerData.Login + separator;
                scores += playerData.Score + separator;
            }
        }
    }
}
