using UnityEngine;

namespace Assets.Sources.Scripts.GameScreen
{
    public class GameManager : MonoBehaviour
    {

        public GameObject SpherePrefab;

        void Update()
        {
            if (Input.GetKey(KeyCode.Space))
                SpawnSphere();
        }

        private void SpawnSphere()
        {
            Debug.Log("Method GameManager.SpawnSphere");
            Network.Instantiate(SpherePrefab, Vector3.zero, Quaternion.identity, 0);
        }
    }
}
