using Assets.Sources.Enums;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Sources.Scripts.RoomSettingsScreen
{
    public class RoomSettingsMenu : MonoBehaviour
    {
        public InputField NameField;
        public InputField PasswordField;
        public InputField UsersLimitField;

        private void Start()
        {

        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape)) { Application.LoadLevel(SceneName.RoomChoiceScreen.ToString()); }
        }

        public void BackButtonClick()
        {
            Application.LoadLevel(SceneName.RoomChoiceScreen.ToString());
        }

        public void StartGameClick()
        {//Mock behaviour
            Application.LoadLevel(SceneName.RoomChoiceScreen.ToString());
        }
    }
}