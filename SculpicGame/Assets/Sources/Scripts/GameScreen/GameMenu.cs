using UnityEngine;
using UnityEngine.UI;

namespace Assets.Sources.Scripts.GameScreen
{
    public class GameMenu : MonoBehaviour
    {
        private const float MinRand = -1.0f;
        private const float MaxRand = 1.0f;

        public static int SpheresColorToChange;
        private int _spheresCount;

        public GameObject SpherePrefab;

        private void Awake()
        {
            Debug.Log("Method GameMenu.Awake");
            if (Network.isClient)
                HideServerButtons();
        }

        private void HideServerButtons()
        {
            Debug.Log("Method GameMenu.HideServerButtons");
            var spawnSphereButton = GameObject.Find("SpawnSphereButton");
            spawnSphereButton.gameObject.SetActive(false);
            var changeColorsButton = GameObject.Find("ChangeColorsButton");
            changeColorsButton.gameObject.SetActive(false);
        }

        public void CreateRandomSphere()
        {
            GameManager.SpawnSolid(SpherePrefab,
                new Vector3(Random.Range(MinRand, MaxRand), Random.Range(MinRand, MaxRand), 0));
            _spheresCount++;
        }

        public void RandomizeColors()
        {
            SpheresColorToChange = _spheresCount;
        }

    }
}
