using System;
using System.Numerics;

namespace SimdDoors
{
    public static class Vector3Utility
    {
        public static Vector3 CreateRandomPosition(Random random, float range)
        {
            return new Vector3(
                (float) random.NextDouble() * range,
                0,
                (float) random.NextDouble() * range);
        }
    }
}
