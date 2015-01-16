using UnityEngine;

namespace Assets.Sources.Scripts.GameServer.SolidManagement
{
    public class SolidMovement : MonoBehaviour
    {
        private bool _isMoving;

        private void Update()
        {
            if (Input.touchCount == 1)
            {
                var touch = Input.GetTouch(0);
                switch (touch.phase)
                {
                    case TouchPhase.Stationary:
                        _isMoving = true;
                        break;
                    case TouchPhase.Moved:
                        if (_isMoving)
                        {
                            var ray = Camera.main.ScreenPointToRay(touch.position);
                            RaycastHit hit;
                            if (Physics.Raycast(ray, out hit))
                            {
                                if (hit.collider == collider)
                                {
                                    var oldPosition = transform.position;
                                    transform.position = new Vector3(oldPosition.x + touch.deltaPosition.x/100,
                                        oldPosition.y + touch.deltaPosition.y/100, oldPosition.z);
                                }
                            }
                        }
                        break;
                    case TouchPhase.Canceled:
                        _isMoving = false;
                        break;
                    case TouchPhase.Ended:
                        _isMoving = false;
                        break;
                }
            }
        }
    }
}