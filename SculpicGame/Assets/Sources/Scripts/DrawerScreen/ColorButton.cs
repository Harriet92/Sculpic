﻿using System.Collections.Generic;
using Assets.Sources.Scripts.Sculptor;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Sources.Scripts.DrawerScreen
{
    public class ColorButton: MonoBehaviour
    {
        public Image ButtonImage;
        private int selectedColor;
        private static List<Color> Colors = new List<Color>
        {
            Color.blue,
            Color.green,
            Color.yellow,
            Color.white,
            Color.red,
            Color.magenta,
            Color.black
        };

        void Start()
        {
            SetColor();
        }

        public void Click()
        {
            selectedColor = (++selectedColor)%Colors.Count;
            SetColor();
            
        }

        private void SetColor()
        {
            SculptorCurrentSettings.MaterialColor = Colors[selectedColor];
            ButtonImage.color = Colors[selectedColor];
        }
    }
}