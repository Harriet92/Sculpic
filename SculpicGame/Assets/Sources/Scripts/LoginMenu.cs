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
        private Canvas popUpCanvas;
        public Image LoadingIndicator;
        public Text LoginEmpty;
        public Text PassEmpty;
        private Color color;
        void Start()
        {
            color = LoginField.GetComponentInChildren<Text>().color;
            ClearValidationMessages();
        }
        public void LoginClick()
        {
            var login = LoginField.text;
            var password = PasswordField.text;
            if (InputNotValid(login, password)) return;
            UserService userService = new UserService();
            var result = userService.LoginUser(login, SecureString.GetBase64Hash(password));
            Debug.Log(result);
            if (result != null)
            {
                Player.LogIn(result);
                Application.LoadLevel("GameScreen");
            }
            else if (!string.IsNullOrEmpty(UserService.LastError))
            {
                DisplayPopup("Internal server error, try again later.");
            }
            else
            {
                DisplayPopup("Incorrect login or password!");
            }
        }

        private bool InputNotValid(string login, string password)
        {
            if (String.IsNullOrEmpty(login) || String.IsNullOrEmpty(password))
            {
                if (String.IsNullOrEmpty(login))
                    LoginField.GetComponentInChildren<Text>().color = color;

                if (String.IsNullOrEmpty(password))
                    PasswordField.GetComponentInChildren<Text>().color = color;
                return true;
            }
            return false;
        }

        public void RegisterClick()
        {
            Application.LoadLevel("RegisterScreen");
        }

        public void IncorrectInputButtonClick()
        {
            popUpCanvas.gameObject.SetActive(false);
        }

        private void DisplayPopup(string message)
        {
            if (popUpCanvas != null)
                popUpCanvas.gameObject.SetActive(true);
            else
                popUpCanvas = (Canvas)Instantiate(IncorrectLoginCanvas);
            popUpCanvas.GetComponentInChildren<Image>().GetComponentInChildren<Text>().text = message;
            popUpCanvas.GetComponentInChildren<Image>().GetComponentInChildren<Button>().onClick.AddListener(() => IncorrectInputButtonClick());
        }

        public void ClearValidationMessages()
        {
            LoginField.GetComponentInChildren<Text>().color = new Color(131, 1, 1, 0);
            PasswordField.GetComponentInChildren<Text>().color = new Color(131, 1, 1, 0);
        }
    }
}
