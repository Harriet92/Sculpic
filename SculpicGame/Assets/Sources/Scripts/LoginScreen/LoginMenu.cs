using System;
using Assets.Sources.Common;
using Assets.Sources.DatabaseClient.Security;
using Assets.Sources.DatabaseClient.Services;
using System.Collections;
using Assets.Sources.Enums;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Sources.Scripts.LoginScreen
{
    public class LoginMenu : MenuBase
    {
        public InputField LoginField;
        public InputField PasswordField;
        private UserService userService;
        private Color warnColor;
        void Start()
        {
            warnColor = LoginField.GetComponentInChildren<Text>().color;
            ClearValidationMessages();
        }
        public void LoginClick()
        {
            var login = LoginField.text;
            var password = PasswordField.text;
            if (InputNotValid(login, password)) return;
            DisplayLoadingPopup();
            StartCoroutine(InvokeLoginUser(login, password));
        }

        private IEnumerator InvokeLoginUser(string login, string password)
        {
            yield return null;
            userService = new UserService();
            var result = userService.LoginUser(login, SecureString.GetBase64Hash(password));
            Debug.Log(result);
            if (result != null)
            {
                Player.LogIn(result);
                Application.LoadLevel(SceneName.RoomChoiceScreen.ToString());
            }
            else if (!string.IsNullOrEmpty(UserService.LastError))
            {
                DisplayInfoPopup("Internal server error, try again later.");
            }
            else
            {
                DisplayInfoPopup("Incorrect login or password!");
            }
            DismissLoadingPopup();
        }

        private bool InputNotValid(string login, string password)
        {
            if (String.IsNullOrEmpty(login) || String.IsNullOrEmpty(password))
            {
                if (String.IsNullOrEmpty(login))
                    LoginField.GetComponentInChildren<Text>().color = warnColor;

                if (String.IsNullOrEmpty(password))
                    PasswordField.GetComponentInChildren<Text>().color = warnColor;
                return true;
            }
            return false;
        }

        public void RegisterClick()
        {
            Application.LoadLevel(SceneName.RegisterScreen.ToString());
        }

        public void ClearValidationMessages()
        {
            LoginField.GetComponentInChildren<Text>().color = new Color(131, 1, 1, 0);
            PasswordField.GetComponentInChildren<Text>().color = new Color(131, 1, 1, 0);
        }
    }
}
