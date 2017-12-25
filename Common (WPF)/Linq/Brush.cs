using System.Windows.Media;

namespace Imagin.Common.Linq
{
    /// <summary>
    /// 
    /// </summary>
    public static class BrushExtensions
    {
        /// <summary>
        /// Creates Brush from specified Brush and it's values.
        /// </summary>
        public static Brush Duplicate(this Brush Value)
        {
            if (Value is LinearGradientBrush)
            {
                var Linear = Value as LinearGradientBrush;

                return new LinearGradientBrush(Linear.GradientStops)
                {
                    StartPoint = Linear.StartPoint,
                    EndPoint = Linear.EndPoint,
                    MappingMode = Linear.MappingMode,
                    Opacity = Linear.Opacity,
                    ColorInterpolationMode = Linear.ColorInterpolationMode,
                    SpreadMethod = Linear.SpreadMethod,
                    RelativeTransform = Linear.RelativeTransform,
                    Transform = Linear.Transform
                };
            }
            else if (Value is RadialGradientBrush)
            {
                var Radial = Value as RadialGradientBrush;

                return new RadialGradientBrush(Radial.GradientStops)
                {
                    MappingMode = Radial.MappingMode,
                    Opacity = Radial.Opacity,
                    ColorInterpolationMode = Radial.ColorInterpolationMode,
                    SpreadMethod = Radial.SpreadMethod,
                    RelativeTransform = Radial.RelativeTransform,
                    Transform = Radial.Transform
                };
            }

            return default(Brush);
        }
    }
}
