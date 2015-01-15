using System;
using Assets.Sources.Common;
using Assets.Sources.DatabaseClient.Services;
using Assets.Sources.Enums;
using Assets.Sources.Scripts.Music;
using UnityEngine;
using UnityEngine.UI;
using Assets.Sources.Scripts.Authentication;

namespace Assets.Sources.Scripts.LoginScreen
{
    public class LoginMenu : MenuBase
    {
        public InputField LoginField;
        public InputField PasswordField;
        public AuthenticationManager AuthenticationManager;
        private Color warnColor;

        void Start()
        {
            ConnectionEstablisher.EstablishConnection();
            warnColor = LoginField.GetComponentInChildren<Text>().color;
            ClearValidationMessages();
        }
        void Awake()
        {
            SoundManager.SetMusicVolume(Preferences.MusicOn);
            AuthenticationManager.LoginByPrefsData();
        }
        
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape)) { Application.Quit(); }
        }
        public void LoginClick()
        {
            var login = LoginField.text;
            var password = PasswordField.text;
            if (InputNotValid(login, password)) return;
            AuthenticationManager.LoginByProvidedData(login, password);
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

        public void RememberMeChecked(bool value)
        {
            Preferences.RememberLogin = value;
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
