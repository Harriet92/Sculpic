using System;
using System.Collections;
using Assets.Sources.Common;
using Assets.Sources.DatabaseClient.Services;
using Assets.Sources.Enums;
using Assets.Sources.Scripts.GameRoom;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Sources.Scripts.RoomSettingsScreen
{
    public class RoomSettingsMenu : MenuBase
    {
        public InputField NameField;
        public InputField PasswordField;
        public InputField UsersLimitField;
        private const string defaultGameName = "SculpicGame";
        private const string defaultUsersLimit = "10";
        private RoomService roomService;
        private const int refreshHostListCount = 10;

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
        {
            int userId = Player.Current == null ? 0 : (int)Player.Current.UserId;
            var gameName = String.IsNullOrEmpty(NameField.text) ? defaultGameName : NameField.text;
            var usersLimit = String.IsNullOrEmpty(UsersLimitField.text) ? defaultUsersLimit : UsersLimitField.text;
            roomService.SetUpNewRoom(userId.ToString(), gameName, PasswordField.text, usersLimit);
            DisplayLoadingPopup();
            StartCoroutine(WaitForRoomLaunch(gameName));
        }

        private IEnumerator WaitForRoomLaunch(string gameName)
        {
            yield return null;
            HostData room;
            int counter = 0;
            do
            {

                MasterServerConnectionManager.RefreshHostList();
                while (!MasterServerConnectionManager.HostsRefreshed)
                    yield return null;

                room = MasterServerConnectionManager.GetHostDataByGameName(gameName);
            } while (room == null && ++counter != refreshHostListCount);
            if(room == null)
                Application.LoadLevel(SceneName.RoomChoiceScreen.ToString());
            else
            {
                Network.Connect(room, room.passwordProtected? PasswordField.text:null);
                ScreenHelper.LoadLevel(SceneName.GuesserScreen);
            }
        }
    }
}