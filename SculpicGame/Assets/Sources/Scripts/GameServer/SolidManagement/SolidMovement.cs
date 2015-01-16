using Assets.Sources.Scripts.Sculptor;
using UnityEngine;

namespace Assets.Sources.Scripts.GameServer.SolidManagement
{
    public class SolidMovement : MonoBehaviour
    {
        private bool _isMoving;
        private const int Speed = 100;

        private void Update()
        {
            if (!SculptorCurrentSettings.Move)
                return;
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
                                if (hit.collider == collider )
                                {
                                    var plane = new Plane(Vector3.up, transform.position);
                                    float dist;
                                    if (plane.Raycast(ray, out dist))
                                        transform.position = Vector3.MoveTowards(transform.position, ray.GetPoint(dist), Time.deltaTime * Speed);
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