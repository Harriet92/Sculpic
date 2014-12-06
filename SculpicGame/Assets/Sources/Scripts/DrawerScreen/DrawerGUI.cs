using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Sources.Enums;
using Assets.Sources.Scripts.GameScreen;
using Assets.Sources.Scripts.GameScreen.SolidManagement;
using Assets.Sources.Scripts.GameServer;
using Assets.Sources.Scripts.Sculptor;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace Assets.Sources.Scripts.DrawerScreen
{
    // TODO: add leave and music buttons
    public class DrawerGUI : MenuBase
    {
        public Text ChatTextField;
        private List<UnityEngine.Object> instantiatedSolids = new List<Object>();
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape)) { Application.LoadLevel(SceneName.RoomChoiceScreen.ToString()); }
            if (!ChatterState.DisplayQueueEmpty && !String.IsNullOrEmpty(ChatterState.PendingMessageToDisplay.Peek()))
                DisplayNewMessage(ChatterState.PendingMessageToDisplay.Dequeue());
        }

        public void AddSolidClick(GameObject solidToInstantiate)
        {
            Debug.Log("AddSolidClick");
            instantiatedSolids.Add(SolidNetworkManager.SpawnSolid(solidToInstantiate, solidToInstantiate.gameObject.transform.position, solidToInstantiate.gameObject.transform.rotation));
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

        public void ClearClick()
        {
            foreach(var solid in instantiatedSolids)
                Destroy(solid);
            instantiatedSolids.Clear();
        }
        private void DisplayNewMessage(string message)
        {
            StringBuilder builder = new StringBuilder(ChatTextField.text);
            ChatTextField.text = builder.AppendLine(message).ToString();
        }


    }
}
