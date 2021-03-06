﻿using Assets.Sources.Scripts.GameRoom.SolidManagement;
using UnityEngine;

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
            SolidNetworkManager.SpawnSolid(SpherePrefab,
                new Vector3(Random.Range(MinRand, MaxRand), Random.Range(MinRand, MaxRand)), Quaternion.identity);
            _spheresCount++;
        }

        public void RandomizeColors()
        {
            SpheresColorToChange = _spheresCount;
        }

    }
}
