using UnityEngine;

namespace Assets.Sources.Scripts.GameScreen
{
    public class CameraOrbit : MonoBehaviour
    {
        public Transform target;
        public float distanceMin = 10.0f;
        public float distanceMax = 15.0f;
        public float distanceInitial = 12.5f;
        public float scrollSpeed = 1.0f;

        public float xSpeed = 250.0f;
        public float ySpeed = 120.0f;

        public float yMinLimit = -20f;
        public float yMaxLimit = 80f;

        private float _x;
        private float _y;
        private float _distanceCurrent;

        void Start()
        {
            var angles = transform.eulerAngles;
            _x = angles.y;
            _y = angles.x;

            _distanceCurrent = distanceInitial;

            // Make the rigid body not change rotation
            if (rigidbody)
                rigidbody.freezeRotation = true;
        }

        void LateUpdate()
        {
            //if (target)
            //{
            //    _x += Input.GetAxis("Horizontal") * xSpeed * 0.02f;
            //    _y -= Input.GetAxis("Vertical") * ySpeed * 0.02f;
            //    _distanceCurrent -= Input.GetAxis("Mouse ScrollWheel") * scrollSpeed;

            //    _distanceCurrent = Mathf.Clamp(_distanceCurrent, distanceMin, distanceMax);
            //    _y = ClampAngle(_y, yMinLimit, yMaxLimit);

            //    var rotation = Quaternion.Euler(_y, _x, 0);
            //    var position = rotation * new Vector3(0.0f, 0.0f, -_distanceCurrent) + target.position;

            //    transform.rotation = rotation;
            //    transform.position = position;
            //}
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
