using System;

namespace ElectricC.Core.MathUtils
{
    public static class MathExtensions
    {
        public static float ToRadians(this float value)
        {
            return (float)(Math.PI / 180.0f) * value;
        }

        public static float ToDegrees(this float value)
        {
            return (float)(value * (180.0f / Math.PI));
        }
    }
}
