using System.Collections;
using Assets.Sources.Enums;
using UnityEngine;

namespace Assets.Sources.Scripts.GameRoom
{
    public static class ScreenHelper
    {
        public static IEnumerator LoadLevel(SceneName sceneName)
        {
            Debug.Log("Method ScreenHelper.LoadLevel");
            Network.SetSendingEnabled(0, false);
            Network.isMessageQueueRunning = false;

            Debug.Log("Loading level: " + sceneName);
            Application.LoadLevel(sceneName.ToString());

            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();

            Network.isMessageQueueRunning = true;
            Network.SetSendingEnabled(0, true);

            Debug.Log("End of ScreenHelper.LoadLevel method");
        }
    }
}
