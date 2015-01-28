using UnityEngine;

namespace Assets.Sources.Scripts.TouchLogic
{
    public class TouchHandling : MonoBehaviour
    {
        void Update()
        {
            for (int i = 0; i < Input.touchCount; i++)
            {
                switch (Input.GetTouch(i).phase)
                {
                    case TouchPhase.Began:
                        SendMessage("OnTouch");
                        break;
                    case TouchPhase.Moved:
                        SendMessage("OnTouch");
                        break;
                    case TouchPhase.Stationary:
                        SendMessage("OnTouch");
                        break;
                    case TouchPhase.Ended:
                        SendMessage("OnTouch");
                        break;
                }
            }
        }
    }
}
