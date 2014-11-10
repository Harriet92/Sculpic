using System;
using Assets.Sources.DatabaseClient.Security;
using Assets.Sources.DatabaseClient.Services;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Sources.Scripts
{
    public class RegisterMenu : MonoBehaviour
    {
        public InputField LoginField;
        public InputField PasswordField;
        public InputField PasswordField2;

        public void RegisterClick()
        {
            var login = LoginField.text;
            var password = PasswordField.text;
            var password2 = PasswordField2.text;
            if (String.IsNullOrEmpty(login) || String.IsNullOrEmpty(password) || String.IsNullOrEmpty(password2) ||
                password!=password2)
                return;
            UserService userService = new UserService();
            var result = userService.AddNewUser(login, SecureString.GetBase64Hash(password));
            Debug.Log(result);
            if (result != null)
            {
                Player.LogIn(result);
                Application.LoadLevel("GameScreen");
            }
        }
    }
}
