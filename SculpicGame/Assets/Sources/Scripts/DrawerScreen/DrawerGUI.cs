using System;
using System.Collections.Generic;
using System.Text;
using Assets.Sources.Enums;
using Assets.Sources.Scripts.GameServer;
using Assets.Sources.Scripts.GameServer.SolidManagement;
using Assets.Sources.Scripts.Sculptor;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace Assets.Sources.Scripts.DrawerScreen
{
    // TODO: add leave and music buttons
    public class DrawerGUI : MenuBase
    {
        public Text ChatTextField;
        private List<Object> instantiatedSolids = new List<Object>();
        public static bool IsSendingScene;

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape)) { Application.LoadLevel(SceneName.RoomChoiceScreen.ToString()); }
            if (!ChatterState.DisplayQueueEmpty && !String.IsNullOrEmpty(ChatterState.PendingMessageToDisplay.Peek()))
                DisplayNewMessage(ChatterState.PendingMessageToDisplay.Dequeue());
        }

        public void AddSolidClick(GameObject solidToInstantiate)
        {
            Debug.Log("Method DrawerGUI.AddSolidClick");
            instantiatedSolids.Add(SolidNetworkManager.SpawnSolid(solidToInstantiate, solidToInstantiate.gameObject.transform.position, solidToInstantiate.gameObject.transform.rotation));
        }

        public void UpdateClick()
        {
            Debug.Log("Method DrawerGUI.UpdateClick");
            if (!IsSendingScene)
            {
                IsSendingScene = true;
            }
        }

        public void MenuClick()
        {
            base.DisplayInfoPopup("Menu will be shown here");
        }

        public void OnRadiusSliderValueChanged(Slider callingObject)
        {
            SculptorCurrentSettings.Radius = callingObject.value;
        }

        public void OnPullSliderValueChanged(Slider callingObject)
        {
            SculptorCurrentSettings.Pull = callingObject.value;
        }

        public void OnCarveToggleValueChanged(Toggle callingObject)
        {
            SculptorCurrentSettings.Carve = callingObject.isOn;
        }

        public void ColorClick()
        {
            // TODO: change
            foreach (var instantiatedSolid in instantiatedSolids)
            {
                var solid = instantiatedSolid as GameObject;
                if (solid != null)
                {
                    solid.renderer.material.color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
                }
            }
        }

        public void ClearClick()
        {
            foreach(var solid in instantiatedSolids)
                Network.Destroy(solid as GameObject);
            instantiatedSolids.Clear();
        }

        private void DisplayNewMessage(string message)
        {
            StringBuilder builder = new StringBuilder(ChatTextField.text);
            ChatTextField.text = builder.AppendLine(message).ToString();
        }
    }
}
