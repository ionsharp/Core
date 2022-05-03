using System;

namespace Imagin.Common.Media
{
    [Serializable]
    public enum GradientType : int
    {
        [Icon(Images.GradientAngle)]
        Angle = 0,
        [DisplayName("Radial")]
        [Icon(Images.GradientRadial)]
        Circle = 1,
        [Icon(Images.GradientDiamond)]
        Diamond = 2,
        [Icon(Images.Gradient)]
        Linear = 3,
    }
}