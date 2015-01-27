using System.Collections.Generic;
using Assets.Sources.Scripts.GameRoom;
using Assets.Sources.Scripts.GameRoom.SolidManagement;
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
        public Text PhraseTextField;
        public Text TimerTextField;
        public Toggle RotateToggle;
        public Toggle MoveToggle;
        private static readonly List<Object> InstantiatedSolids = new List<Object>();
        public static bool IsSendingScene;
        private static int _synchronizedObjectsCounter;
        private static readonly Object SynchronizedObjectsCounterLock = new Object();
        
        public static void SynchronizeNextObject()
        {
            lock (SynchronizedObjectsCounterLock)
            {
                if (++_synchronizedObjectsCounter == InstantiatedSolids.Count)
                    IsSendingScene = false;
            }
        }

        void Start()
        {
            Debug.Log("Method DrawerGUI.Start: Room.CurrentPhrase == " + ClientSide.Phrase);
            PhraseTextField.text = ClientSide.Phrase;
            ClientSide.OnNewScreenLoad(ChatTextField);
        }

        void Update()
        {
            TimerTextField.text = ClientSide.RemainingTime;
        }

        public void AddSolidClick(GameObject solidToInstantiate)
        {
            Debug.Log("Method DrawerGUI.AddSolidClick");
            InstantiatedSolids.Add(SolidNetworkManager.SpawnSolid(solidToInstantiate, solidToInstantiate.gameObject.transform.position, solidToInstantiate.gameObject.transform.rotation, SculptorCurrentSettings.MaterialColor));
        }

        public void UpdateClick()
        {
            Debug.Log("Method DrawerGUI.UpdateClick");
            if (!IsSendingScene)
            {
                _synchronizedObjectsCounter = 0;
                IsSendingScene = true;
            }
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

        public void OnMoveToggleValueChanged(Toggle callingObject)
        {
            Debug.Log("Method DrawerGUI.OnMoveToggleValueChanged: callingObject.isOn == " + callingObject.isOn);
            SculptorCurrentSettings.Move = callingObject.isOn;
            if (SculptorCurrentSettings.Move)
            {
                RotateToggle.isOn = false;
                SculptorCurrentSettings.Rotate = false;
            }
        }

        public void OnRotateToggleValueChanged(Toggle callingObject)
        {
            Debug.Log("Method DrawerGUI.OnRotateToggleValueChanged: callingObject.isOn == " + callingObject.isOn);
            SculptorCurrentSettings.Rotate = callingObject.isOn;
            if (SculptorCurrentSettings.Rotate)
            {
                MoveToggle.isOn = false;
                SculptorCurrentSettings.Move = false;
            }
        }

        public void ClearClick()
        {
            foreach (var solid in InstantiatedSolids)
                Network.Destroy(solid as GameObject);
            InstantiatedSolids.Clear();
        }

        void OnDestroy()
        {
            Debug.Log("Method DrawerGUI.OnDestroy");
            ClearClick();
        }
    }
}
