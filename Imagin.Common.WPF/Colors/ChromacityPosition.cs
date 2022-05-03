using System;

namespace Imagin.Common.Colors
{
    [Serializable]
    public struct ChromacityPosition
    {
        public ChromacityPoint R { get; private set; }

        public ChromacityPoint G { get; private set; }

        public ChromacityPoint B { get; private set; }

        public ChromacityPosition(ChromacityPoint r, ChromacityPoint g, ChromacityPoint b)
        {
            R = r; G = g; B = b;
        }
    }
}