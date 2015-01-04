using System;
using Assets.Sources.Common;
using Assets.Sources.DatabaseClient.Models;
using Assets.Sources.DatabaseClient.Services;
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
        private const string defaultGameName = "SculpicGame";
        private const string defaultUsersLimit = "10";
        private RoomService roomService;

        private void Start()
        {
            SetDefaultValues();
            roomService = new RoomService();
        }

        private void SetDefaultValues()
        {
            NameField.GetComponentInChildren<Text>().text = defaultGameName;
            UsersLimitField.GetComponentInChildren<Text>().text = defaultUsersLimit.ToString();
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
            int userId = Player.Current == null ? 0 : (int)Player.Current.UserId;
            roomService.SetUpNewRoom(userId.ToString(),  String.IsNullOrEmpty(NameField.text) ? defaultGameName : NameField.text, PasswordField.text,
                    String.IsNullOrEmpty(UsersLimitField.text) ? defaultUsersLimit : UsersLimitField.text);
            Application.LoadLevel(SceneName.RoomChoiceScreen.ToString());
        }
    }
}