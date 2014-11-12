using System;
using System.Threading;
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
        public Canvas IncorrectLoginCanvas;

        public Image LoadingIndicator;
        public Text LoginEmpty;
        public Text PassEmpty;
        private Color color;
        void Start()
        {
            color = LoginField.GetComponentInChildren<Text>().color;
            LoginField.GetComponentInChildren<Text>().color = new Color(131, 1, 1, 0);
            PasswordField.GetComponentInChildren<Text>().color = new Color(131, 1, 1, 0);
        }
        public void LoginClick()
        {
            var login = LoginField.text;
            var password = PasswordField.text;
            if (String.IsNullOrEmpty(login) || String.IsNullOrEmpty(password))
            {
                if (String.IsNullOrEmpty(login))
                    LoginField.GetComponentInChildren<Text>().color = color;

                if (String.IsNullOrEmpty(password))
                    PasswordField.GetComponentInChildren<Text>().color = color;
                return;
            }
            UserService userService = new UserService();
            var result = userService.LoginUser(login, SecureString.GetBase64Hash(password));
            Debug.Log(result);
            if (result != null)
            {
                Player.LogIn(result);
                Application.LoadLevel("GameScreen");
            }
            else
            {
                Instantiate(IncorrectLoginCanvas);
            }
        }

        public void RegisterClick()
        {
            Application.LoadLevel("RegisterScreen");
        }

        public void IncorrectInputButtonClick()
        {
            IncorrectLoginCanvas.gameObject.SetActive(false);
        }
    }
}
