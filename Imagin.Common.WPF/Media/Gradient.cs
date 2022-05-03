using Imagin.Common.Linq;
using Imagin.Common.Numbers;
using System;
using System.Windows;
using System.Windows.Media;

namespace Imagin.Common.Media
{
    [Icon(Images.Gradient)]
    [Serializable]
    public class Gradient : Base, ICloneable
    {
        public static Gradient Default => new(new GradientStep(0, System.Windows.Media.Colors.White), new GradientStep(1, System.Windows.Media.Colors.Black));

        public static LinearGradientBrush DefaultBrush => new(System.Windows.Media.Colors.White, System.Windows.Media.Colors.Black, Horizontal.X, Horizontal.Y);

        public static Vector2<Point> Horizontal => new(new Point(0, 0.5), new Point(1, 0.5));

        public static Vector2<Point> Vertical => new(new Point(0.5, 0), new Point(0.5, 1));

        //...

        GradientStepCollection steps = new();
        public GradientStepCollection Steps
        {
            get => steps;
            private set => this.Change(ref steps, value);
        }

        //...

        public Gradient() : base() { }

        public Gradient(params GradientStep[] input) : this()
        {
            input?.ForEach(i => Steps.Add(i));
        }

        public Gradient(GradientStepCollection input) : this()
        {
            input?.ForEach(i => steps.Add(new GradientStep(i.Offset, new StringColor(i.Color))));
        }

        public Gradient(GradientStopCollection input) : this()
        {
            input?.ForEach(i => steps.Add(new GradientStep(i.Offset, new StringColor(i.Color))));
        }

        public Gradient(LinearGradientBrush input) : this(input.GradientStops) { }

        public Gradient(RadialGradientBrush input) : this(input.GradientStops) { }

        //...

        object ICloneable.Clone() => Clone();
        public virtual Gradient Clone()
        {
            var result = new Gradient();
            result.CopyFrom(this);
            return result;
        }

        public LinearGradientBrush LinearBrush()
        {
            var result = new LinearGradientBrush()
            {
                EndPoint = Horizontal.Y,
                StartPoint = Horizontal.X,
                Opacity = 1,
            };

            Steps.ForEach(i => result.GradientStops.Add(new GradientStop(i.Color.Value, i.Offset)));
            return result;
        }

        public RadialGradientBrush RadialBrush()
        {
            var result = new RadialGradientBrush()
            {
                RadiusX = 0.5,
                RadiusY = 0.5,
                Opacity = 1,
            };

            Steps.ForEach(i => result.GradientStops.Add(new GradientStop(i.Color.Value, i.Offset)));
            return result;
        }

        public void CopyFrom(Gradient input)
        {
            Steps.Clear();
            foreach (var i in input.Steps)
                Steps.Add(new GradientStep(i.Offset, new StringColor(i.Color)));
        }

        public virtual void Reset() => CopyFrom(Default);
    }
}