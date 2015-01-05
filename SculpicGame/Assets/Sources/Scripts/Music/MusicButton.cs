using Assets.Sources.Common;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Sources.Scripts.Music
{
    public class MusicButton : MonoBehaviour
    {
        public Image MusicOnImage;
        public Image MusicOffImage;

        void Awake()
        {
            SetImage();
        }

        public void OnClick()
        {
            Preferences.MusicOn = !Preferences.MusicOn;
            SoundManager.SetMusicVolume(Preferences.MusicOn);
            SetImage();
        }

        private void SetImage()
        {
            MusicOnImage.enabled = Preferences.MusicOn;
            MusicOffImage.enabled = !Preferences.MusicOn;
        }

    }
}
