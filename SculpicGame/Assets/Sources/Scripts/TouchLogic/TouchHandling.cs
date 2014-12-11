using UnityEngine;

namespace Assets.Sources.Scripts.TouchLogic
{
    public class TouchHandling : MonoBehaviour
    {
        void Update()
        {
            if (Input.touchCount > 0)
            {
                for (int i = 0; i < Input.touchCount; i++)
                {
                    switch (Input.GetTouch(i).phase)
                    {
                        //case TouchPhase.Began:
                        //    SendMessage("OnTouchBagan");
                        //    break;
                        case TouchPhase.Moved:
                            SendMessage("OnTouchMoved");
                            break;
                        //case TouchPhase.Stationary:
                        //    SendMessage("OnTouchStationary");
                        //    break;
                        //case TouchPhase.Ended:
                        //    SendMessage("OnTouchEnded");
                        //    break;
                        //case TouchPhase.Canceled:
                        //    SendMessage("OnTouchMoved");
                        //    break;
                        //default:
                        //    throw new ArgumentOutOfRangeException();
                    }
                }
            }
        }
    }
}
