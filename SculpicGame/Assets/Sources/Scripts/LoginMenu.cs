using System;
using Assets.Sources.DatabaseClient.Security;
using Assets.Sources.DatabaseClient.Services;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Sources.Scripts
{
    public class LoginMenu : MonoBehaviour
    {
        public InputField LoginField;
        public InputField PasswordField;
        public void LoginClick()
        {
            var login = LoginField.text;
            var password = PasswordField.text;
            if (String.IsNullOrEmpty(login) || String.IsNullOrEmpty(password))
                return;
            
            UserService userService = new UserService();
            var result = userService.LoginUser(login,  SecureString.GetBase64Hash(password)); 
            Debug.Log(result);
            if (result != null)
            {
                Player.LogIn(result);
                Application.LoadLevel("MainScene");
            }

        }
    }
}
