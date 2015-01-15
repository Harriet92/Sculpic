using Assets.Sources.Enums;
using UnityEngine;

namespace Assets.Sources.Scripts.CreditsScreen
{
    public class CreditsMenu : MonoBehaviour
    {
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape)) { Application.LoadLevel(SceneName.RoomChoiceScreen.ToString()); }
        }

        public void BackButtonClick()
        {
            Application.LoadLevel(SceneName.RoomChoiceScreen.ToString()); 
        }
    }
}