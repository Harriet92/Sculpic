using Assets.Sources.Enums;
using UnityEngine;

namespace Assets.Sources.Scripts.RankingScreen
{
    public class RankingButton : MonoBehaviour
    {
        public void OnClick()
        {
            Application.LoadLevel(SceneName.RankingScreen.ToString());
        }
    }
}
