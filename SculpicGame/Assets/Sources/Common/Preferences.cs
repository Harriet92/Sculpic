using Assets.Sources.DatabaseClient.Security;
using UnityEngine;

namespace Assets.Sources.Common
{
    public static class Preferences
    {
        private const string RememberLoginDataKey = "RememberLoginData";
        private const string SavedLoginKey = "SavedLogin";
        private const string SavedPasswordKey = "SavedPassword";
        private const string MusicOnKey = "MusicOn";

        public static bool RememberLogin
        {
            get { return PlayerPrefs.GetInt(RememberLoginDataKey) > 0; }
            set { PlayerPrefs.SetInt(RememberLoginDataKey, value ? 1 : 0); }
        }

        public static string SavedLogin
        {
            get { return PlayerPrefs.GetString(SavedLoginKey); }
            set { PlayerPrefs.SetString(SavedLoginKey, value); }
        }
        public static string SavedPassword
        {
            get { return PlayerPrefs.GetString(SavedPasswordKey); }
            set { PlayerPrefs.SetString(SavedPasswordKey, value); }
        }

        public static void SaveLoginData(string login, string password)
        {
            if (RememberLogin)
            {
                SavedLogin = login;
                SavedPassword = SecureString.GetBase64Hash(password);
            }
        }
        public static bool MusicOn
        {
            get { return PlayerPrefs.GetInt(MusicOnKey) > 0; }
            set { PlayerPrefs.SetInt(MusicOnKey, value ? 1 : 0); }
        }
    }
}
