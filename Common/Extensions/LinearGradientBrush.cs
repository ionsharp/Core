using System.Windows.Media;

namespace Imagin.Common.Extensions
{
    public static class LinearGradientBrushExtensions
    {
        /// <summary>
        /// Creates LinearGradientBrush from specified LinearGradientBrush and it's values.
        /// </summary>
        public static LinearGradientBrush Duplicate(this LinearGradientBrush ToClone)
        {
            return new LinearGradientBrush(ToClone.GradientStops)
            {
                StartPoint = ToClone.StartPoint,
                EndPoint = ToClone.EndPoint,
                MappingMode = ToClone.MappingMode,
                Opacity = ToClone.Opacity,
                ColorInterpolationMode = ToClone.ColorInterpolationMode,
                SpreadMethod = ToClone.SpreadMethod,
                RelativeTransform = ToClone.RelativeTransform,
                Transform = ToClone.Transform
            };
        }

    }
}
