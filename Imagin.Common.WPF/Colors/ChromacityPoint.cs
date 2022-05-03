using System;

namespace Imagin.Common.Colors
{
    [Serializable]
    public struct ChromacityPoint
    {
        public double X { get; private set; }

        public double Y { get; private set; }

        public ChromacityPoint(double x, double y)
        {
           X = x; Y = y;
        }
    }
}