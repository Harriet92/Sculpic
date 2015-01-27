using UnityEngine;

namespace Assets.Sources.Scripts.Music
{
    public class SoundManager : MonoBehaviour
    {
        public AudioSource ChatMessageSound;
        public AudioSource NewPlayerJoinedSound;
        public AudioSource YouGuessedSound;

        public static void SetMusicVolume(bool musicOn)
        {
            AudioListener.volume = musicOn ? 1 : 0;
        }
        public void PlayChatMessageSound()
        {
            ChatMessageSound.PlayOneShot(ChatMessageSound.clip);
        }
        public void PlayNewPlayerJoinedSound()
        {
            NewPlayerJoinedSound.PlayOneShot(NewPlayerJoinedSound.clip);
        }
        public void PlayYouGuessedSound()
        {
            YouGuessedSound.PlayOneShot(YouGuessedSound.clip);
        }
    }
}
