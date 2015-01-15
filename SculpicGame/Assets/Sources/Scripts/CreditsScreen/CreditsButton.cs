using Assets.Sources.Enums;
using UnityEngine;

namespace Assets.Sources.Scripts.CreditsScreen
{
    public class CreditsButton : MonoBehaviour
    {
        public void OnClick()
        {
            Application.LoadLevel(SceneName.CreditsScreen.ToString());
        }
    }
}
