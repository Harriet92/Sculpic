namespace Assets.Sources.Scripts.Sculptor
{
    public static class SculptorCurrentSettings
    {
        public const float DefaultRadius = 1.0f;
        public const float DefaultPull = 6.0f;
        public static bool Carve { get; set; }
        public static float Radius { get; set; }
        public static float Pull { get; set; }

        static SculptorCurrentSettings()
        {
            ResetValues();
        }

        public static void ResetValues()
        {
            Radius = DefaultRadius;
            Pull = DefaultPull;
            Carve = false;
        }
    }
}
