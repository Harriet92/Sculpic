using System.Collections.Generic;
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
        public Text PhraseTextField;
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
            Debug.Log("Method DrawerGUI.Start: Room.CurrentPhrase == " + Room.CurrentPhrase);
            PhraseTextField.text = Room.CurrentPhrase;
            Room.ChatTextField = ChatTextField;
        }

        public void AddSolidClick(GameObject solidToInstantiate)
        {
            Debug.Log("Method DrawerGUI.AddSolidClick");
            InstantiatedSolids.Add(SolidNetworkManager.SpawnSolid(solidToInstantiate, solidToInstantiate.gameObject.transform.position, solidToInstantiate.gameObject.transform.rotation));
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
            foreach (var instantiatedSolid in InstantiatedSolids)
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
            foreach (var solid in InstantiatedSolids)
                Network.Destroy(solid as GameObject);
            InstantiatedSolids.Clear();
        }
    }
}
