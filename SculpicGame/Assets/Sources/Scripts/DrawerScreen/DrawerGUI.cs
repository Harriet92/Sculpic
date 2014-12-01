using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Sources.Scripts.Sculptor;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Sources.Scripts.DrawerScreen
{
    public class DrawerGUI : MenuBase
    {
        public void AddSolidClick(GameObject solidToInstantiate)
        {
            Debug.Log("AddSolidClick");
            Instantiate(solidToInstantiate, solidToInstantiate.gameObject.transform.position, solidToInstantiate.gameObject.transform.rotation);
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


    }
}
