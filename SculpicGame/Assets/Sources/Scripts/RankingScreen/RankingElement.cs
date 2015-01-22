using Assets.Sources.DatabaseClient.Models;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Sources.Scripts.RankingScreen
{
    public class RankingElement: MonoBehaviour
    {
        public Text UsernameText;
        public Text ScoreText;

        public void SetPlayerData(User user, GameObject parentPanel)
        {
            UsernameText.text = user.Username;
            ScoreText.text = user.Ranking.ToString();
            transform.SetParent(parentPanel.transform, false);
        }
    }
}
