using Assets.Sources.Scripts.Sculptor;
using UnityEngine;

namespace Assets.Sources.Scripts.TouchLogic
{
    public class SwipeCameraRotate : TouchHandling
    {
        public Transform target;
        public float distanceMin = 10.0f;
        public float distanceMax = 15.0f;
        public float distanceInitial = 12.5f;
        public float scrollSpeed = 1.0f;

        public float xSpeed = 5.0f;
        public float ySpeed = 5.0f;

        public float yMinLimit = -80f;
        public float yMaxLimit = 80f;

        private float _x;
        private float _y;
        private float _distanceCurrent;


        void Start()
        {
            Debug.Log("Method TouchRotate.Start");
            var angles = transform.eulerAngles;
            _x = angles.y;
            _y = angles.x;

            _distanceCurrent = distanceInitial;

            // Make the rigid body not change rotation
            if (rigidbody)
                rigidbody.freezeRotation = true;
        }

        void OnTouchMoved()
        {
            Debug.Log("Method TouchRotate.OnTouchMoved");
            if (SculptorCurrentSettings.Move || !SculptorCurrentSettings.Rotate)
                return;
            if (target)
            {
                _x += Input.GetTouch(0).deltaPosition.x * xSpeed * 0.02f;
                _y -= Input.GetTouch(0).deltaPosition.y * ySpeed * 0.02f;
                _distanceCurrent -= Input.GetAxis("Mouse ScrollWheel") * scrollSpeed;

                _distanceCurrent = Mathf.Clamp(_distanceCurrent, distanceMin, distanceMax);
                _y = ClampAngle(_y, yMinLimit, yMaxLimit);

                var rotation = Quaternion.Euler(_y, _x, 0);
                var position = rotation * new Vector3(0.0f, 0.0f, -_distanceCurrent) + target.position;

                transform.rotation = rotation;
                transform.position = position;
            }
        }

        static float ClampAngle(float angle, float min, float max)
        {
            if (angle < -360)
                angle += 360;
            if (angle > 360)
                angle -= 360;
            return Mathf.Clamp(angle, min, max);
        }
    }
}
