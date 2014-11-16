using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Assets.Sources.Scripts
{
    public class MenuBase : MonoBehaviour
    {
        public Canvas InfoDialogCanvas;
        protected Canvas InfoPopUp;
        public Canvas LoadingIndicatorCanvas;
        protected Canvas LoadingPopup;

        protected void DisplayInfoPopup(string message)
        {
            if (InfoPopUp != null)
                InfoPopUp.gameObject.SetActive(true);
            else
                InfoPopUp = (Canvas)Instantiate(InfoDialogCanvas);
            InfoPopUp.GetComponentInChildren<Image>().GetComponentInChildren<Text>().text = message;
            InfoPopUp.GetComponentInChildren<Image>().GetComponentInChildren<Button>().onClick.AddListener(DismissInfoPopup);
        }

        protected void DisplayLoadingPopup()
        {
            if (LoadingPopup != null)
                LoadingPopup.gameObject.SetActive(true);
            else
                LoadingPopup = (Canvas)Instantiate(LoadingIndicatorCanvas);
        }

        private void DismissInfoPopup()
        {
            InfoPopUp.gameObject.SetActive(false);
        }

        protected void DismissLoadingPopup()
        {
            LoadingPopup.gameObject.SetActive(false);
        }

    }
}
