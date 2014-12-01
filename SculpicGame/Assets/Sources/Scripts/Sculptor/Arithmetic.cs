using System;
using UnityEngine;

namespace Assets.Sources.Scripts.Sculptor
{
    public class Arithmetic
    {
        public static float SegmentLength(Vector3 a, Vector3 b)
        {
            return (float)Math.Sqrt((b.x - a.x) * (b.x - a.x) + (b.y - a.y) * (b.y - a.y));
        }

        public static Vector3 CalculateIncenter(Vector3 A, Vector3 B, Vector3 C)
        {
            float a = SegmentLength(A, B);
            float b = SegmentLength(B, C);
            float c = SegmentLength(C, A);

            float P = a + b + c;
            return new Vector3((b * A.x + c * B.x + a * C.x) / P, (b * A.y + c * B.y + a * C.y) / P, (b*A.z + c* B.z +a * C.z) / P);
        }
    }
}
