using System;

namespace Imagin.Common.Media
{
    [Serializable]
    public struct Argb
    {
        public readonly byte A;

        public readonly byte R;

        public readonly byte G;

        public readonly byte B;

        public Argb(byte a, byte r, byte g, byte b)
        {
            A = a;
            R = r;
            G = g;
            B = b;
        }
    }
}