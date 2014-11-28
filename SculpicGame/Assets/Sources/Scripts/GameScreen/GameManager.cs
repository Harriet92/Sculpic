using UnityEngine;

namespace Assets.Sources.Scripts.GameScreen
{
    public class GameManager : MonoBehaviour
    {

        public static void SpawnSolid(GameObject solidprefab, Vector3 position)
        {
            Debug.Log("Method GameManager.SpawnSolid");
            Network.Instantiate(solidprefab, position, Quaternion.identity, 0);
        }
    }
}
