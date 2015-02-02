using UnityEngine;

namespace Assets.Sources.Scripts.Sculptor
{
    public static class SculptorCurrentSettings
    {
        public const float DefaultRadius = 1.0f;
        public const float DefaultPull = 6.0f;
        public static bool Carve { get; set; }
        public static bool Move { get; set; }
        public static bool Rotate { get; set; }
        public static float Radius { get; set; }
        public static float Pull { get; set; }
        public static Color MaterialColor { get; set; }
        public static string MaterialColorName { get; set; }

        static SculptorCurrentSettings()
        {
            ResetValues();
        }

        public static void ResetValues()
        {
            Radius = DefaultRadius;
            Pull = DefaultPull;
            Carve = false;
            Move = false;
            Rotate = false;
        }
    }
}
