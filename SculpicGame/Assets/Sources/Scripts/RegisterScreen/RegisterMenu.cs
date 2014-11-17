using System;
using Assets.Sources.Common;
using Assets.Sources.DatabaseClient.Security;
using Assets.Sources.DatabaseClient.Services;
using Assets.Sources.Scripts.GameScreen;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Sources.Scripts.RegisterScreen
{
    public class RegisterMenu : MonoBehaviour
    {
        public InputField LoginField;
        public InputField PasswordField;
        public InputField PasswordField2;
        public Canvas IncorrectLoginCanvas;
        private Canvas popUpCanvas;
        private Color color;

        void Start()
        {
            color = LoginField.GetComponentInChildren<Text>().color;
            ClearValidationMessages();
        }
        public void RegisterClick()
        {
            var login = LoginField.text;
            var password = PasswordField.text;
            var password2 = PasswordField2.text;
            if (InputNotValid(login, password, password2)) return;
            UserService userService = new UserService();
            var result = userService.AddNewUser(login, SecureString.GetBase64Hash(password));
            Debug.Log(result);
            if (result != null)
            {
                Player.LogIn(result);
                Application.LoadLevel("RoomChoiceScreen");
            }
            else if (!string.IsNullOrEmpty(UserService.LastError))
            {
                DisplayPopup("Internal server error, try again later.");
            }
            else
            {
                DisplayPopup("Username is already taken");
            }
        }
        private bool InputNotValid(string login, string password, string password2)
        {
            if (String.IsNullOrEmpty(login) || String.IsNullOrEmpty(password) || String.IsNullOrEmpty(password2))
            {
                if (String.IsNullOrEmpty(login))
                    LoginField.GetComponentInChildren<Text>().color = color;

                if (String.IsNullOrEmpty(password))
                    PasswordField.GetComponentInChildren<Text>().color = color;

                if (String.IsNullOrEmpty(password2))
                    PasswordField2.GetComponentInChildren<Text>().color = color;
                return true;
            }
            if (password != password2)
            {
                PasswordField.GetComponentInChildren<Text>().text = "Passwords don't match!";
                PasswordField.GetComponentInChildren<Text>().color = color;
                return true;
            }
            return false;
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
            LoginField.GetComponentInChildren<Text>().color = 
            PasswordField.GetComponentInChildren<Text>().color = 
            PasswordField2.GetComponentInChildren<Text>().color = new Color(131, 1, 1, 0);

            LoginField.GetComponentInChildren<Text>().text = 
            PasswordField.GetComponentInChildren<Text>().text = 
            PasswordField2.GetComponentInChildren<Text>().text = "Field cannot be empty!";
        }
    }
}
