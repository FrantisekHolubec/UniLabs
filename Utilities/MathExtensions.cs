// Author: František Holubec
// Copyright (c) UniLabs

namespace UniLabs.Utilities
{
    public static class MathExtensions
    {
        public static int PositiveModulo(this int x, int m)
        {
            var r = x % m;
            return r < 0 ? r + m : r;
        }

        public static float PositiveModulo(this float x, float m)
        {
            var c = x % m;
            if ((c < 0 && m > 0) || (c > 0 && m < 0)) {
                c += m;
            }
            return c;
        }
    }
}
