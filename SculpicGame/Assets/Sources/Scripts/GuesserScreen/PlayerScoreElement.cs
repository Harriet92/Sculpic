using Assets.Sources.Scripts.GameServer;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Sources.Scripts.GuesserScreen
{
    public class PlayerScoreElement: MonoBehaviour
    {
        public Text UsernameText;
        public Text ScoreText;
        private PlayerData playerData;
        void Update()
        {
            ScoreText.text = playerData.Score.ToString();
        }

        public void SetPlayerData(PlayerData _playerData, GameObject parentPanel)
        {
            playerData = _playerData;
            UsernameText.text = playerData.Login;
            transform.parent = parentPanel.transform;
        }
    }
}
