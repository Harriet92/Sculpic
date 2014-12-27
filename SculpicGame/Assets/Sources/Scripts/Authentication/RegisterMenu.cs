using System;
using System.Text.RegularExpressions;
using Assets.Sources.Common;
using Assets.Sources.DatabaseClient.Security;
using Assets.Sources.DatabaseClient.Services;
using System.Collections;
using Assets.Sources.DatabaseServer.Models;
using Assets.Sources.Enums;
using UnityEngine;
using UnityEngine.UI;
using Assets.Sources.Scripts.Authentication;

namespace Assets.Sources.Scripts.RegisterScreen
{
    public class RegisterMenu : MenuBase
    {
        public InputField LoginField;
        public InputField PasswordField;
        public InputField PasswordField2;
        public AuthenticationManager AuthenticationManager;
        private Color warnColor;
        private Regex regexSpecialCharactersCheck;
        void Start()
        {
            warnColor = LoginField.GetComponentInChildren<Text>().color;
            ClearValidationMessages();
            regexSpecialCharactersCheck = new Regex("^[a-zA-Z0-9]*$");
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape)) { Application.LoadLevel(SceneName.LoginScreen.ToString()); }
        }

        public void RegisterClick()
        {
            var login = LoginField.text;
            var password = PasswordField.text;
            var password2 = PasswordField2.text;
            if (InputNotValid(login, password, password2)) return;
            AuthenticationManager.RegisterNewUser(login, password);
        }

        public void BackClick()
        {
            Application.LoadLevel(SceneName.LoginScreen.ToString());
        }

        public void RememberMeChecked(bool value)
        {
            Preferences.RememberLogin = value;
        }

        private bool InputNotValid(string login, string password, string password2)
        {
            if (String.IsNullOrEmpty(login) || String.IsNullOrEmpty(password) || String.IsNullOrEmpty(password2))
            {
                if (String.IsNullOrEmpty(login))
                    LoginField.GetComponentInChildren<Text>().color = warnColor;

                if (String.IsNullOrEmpty(password))
                    PasswordField.GetComponentInChildren<Text>().color = warnColor;

                if (String.IsNullOrEmpty(password2))
                    PasswordField2.GetComponentInChildren<Text>().color = warnColor;
                return true;
            }
            if (password != password2)
            {
                PasswordField.GetComponentInChildren<Text>().text = "Passwords don't match!";
                PasswordField.GetComponentInChildren<Text>().color = warnColor;
                return true;
            }
            if (login.Length < User.MIN_USERNAME_LEN)
            {
                LoginField.GetComponentInChildren<Text>().text = "Username is too short!";
                LoginField.GetComponentInChildren<Text>().color = warnColor;
                return true;
            }
            if(!regexSpecialCharactersCheck.IsMatch(login))
            {
                LoginField.GetComponentInChildren<Text>().text = "Only letters and numbers allowed!";
                LoginField.GetComponentInChildren<Text>().color = warnColor;
                return true;
            }
            return false;
        }

        public void ClearValidationMessages()
        {
            LoginField.GetComponentInChildren<Text>().color = 
            PasswordField.GetComponentInChildren<Text>().color = 
            PasswordField2.GetComponentInChildren<Text>().color = new Color(131, 1, 1, 0);

            LoginField.GetComponentInChildren<Text>().text = 
            PasswordField.GetComponentInChildren<Text>().text = 
            PasswordField2.GetComponentInChildren<Text>().text = "Field cannot be empty!";
        }
    }
}
