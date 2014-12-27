using Assets.Sources.Common;
using Assets.Sources.DatabaseClient.Security;
using Assets.Sources.DatabaseClient.Services;
using Assets.Sources.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Sources.Scripts.Authentication
{
    public class AuthenticationManager : MenuBase
    {
        private UserService userService;
        public AuthenticationManager()
        {
            userService = new UserService();
        }

        public void LoginByPrefsData()
        {
            if (Preferences.RememberLogin &&
                !String.IsNullOrEmpty(Preferences.SavedLogin) &&
                !String.IsNullOrEmpty(Preferences.SavedPassword))
            {
                DisplayLoadingPopup();
                StartCoroutine(InvokeLoginUser(Preferences.SavedLogin, Preferences.SavedPassword, true));
            }
        }

        public void LoginByProvidedData(string login, string password)
        {
            Preferences.SaveLoginData(login, password);
            DisplayLoadingPopup();
            StartCoroutine(InvokeLoginUser(login, password));
        }

        public void RegisterNewUser(string login, string password)
        {
            Preferences.SaveLoginData(login, password);
            DisplayLoadingPopup();
            StartCoroutine(InvokeAddNewUser(login, password));
        }

        private IEnumerator InvokeLoginUser(string login, string password, bool passwordAlreadyHashed = false)
        {
            yield return null;
            var result = userService.LoginUser(login, passwordAlreadyHashed ? password : SecureString.GetBase64Hash(password));
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

        private IEnumerator InvokeAddNewUser(string login, string password)
        {
            yield return null;
            var result = userService.AddNewUser(login, SecureString.GetBase64Hash(password));
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
                DisplayInfoPopup("Username is already taken");
            }
            DismissLoadingPopup();
        }
    }
}
