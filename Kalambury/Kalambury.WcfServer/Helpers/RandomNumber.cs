using System;

namespace Kalambury.WcfServer.Helpers
{
    public static class RandomNumber
    {
        private static readonly Random random;

        static RandomNumber()
        {
            random = new Random(DateTime.Now.Millisecond);
        }

        public static int Get()
        {
            return random.Next();
        }

        public static double GetDouble()
        {
            return random.NextDouble();
        }
    }
}
