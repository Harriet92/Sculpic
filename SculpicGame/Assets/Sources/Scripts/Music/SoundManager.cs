using Assets.Sources.Common;
using UnityEngine;

namespace Assets.Sources.Scripts.Music
{
    public static class SoundManager
    {
        public static void SetMusicVolume(bool musicOn)
        {
            AudioListener.volume = Preferences.MusicOn ? 1 : 0;
        }
    }
}
