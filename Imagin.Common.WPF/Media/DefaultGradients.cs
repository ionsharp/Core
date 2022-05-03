using Imagin.Common.Collections.Generic;
using System;
using System.Windows.Media;

namespace Imagin.Common.Media
{
    [Serializable]
    public sealed class DefaultGradients : GroupCollection<Gradient>
    {
        public DefaultGradients() : base("Default")
        {
            Add(new Gradient(new GradientStep(0, System.Windows.Media.Colors.Black),
                new GradientStep(1, System.Windows.Media.Colors.Transparent)));
            Add(new Gradient(new GradientStep(0, System.Windows.Media.Colors.Black),
                new GradientStep(1, System.Windows.Media.Colors.White)));

            Add(new Gradient(new GradientStep(0, System.Windows.Media.Colors.Transparent),
                new GradientStep(1, System.Windows.Media.Colors.Black)));
            Add(new Gradient(new GradientStep(0, System.Windows.Media.Colors.Transparent),
                new GradientStep(1, System.Windows.Media.Colors.White)));

            Add(new Gradient(new GradientStep(0, System.Windows.Media.Colors.White),
                new GradientStep(1, System.Windows.Media.Colors.Black)));
            Add(new Gradient(new GradientStep(0, System.Windows.Media.Colors.White),
                new GradientStep(1, System.Windows.Media.Colors.Transparent)));

            Add(new Gradient(new GradientStep(0, System.Windows.Media.Colors.Red),
                new GradientStep(1, System.Windows.Media.Colors.Black)));
            Add(new Gradient(new GradientStep(0, System.Windows.Media.Colors.Red),
                new GradientStep(1, System.Windows.Media.Colors.Transparent)));
            Add(new Gradient(new GradientStep(0, System.Windows.Media.Colors.Red),
                new GradientStep(1, System.Windows.Media.Colors.White)));

            Add(new Gradient(new GradientStep(0, System.Windows.Media.Colors.Green),
                new GradientStep(1, System.Windows.Media.Colors.Black)));
            Add(new Gradient(new GradientStep(0, System.Windows.Media.Colors.Green),
                new GradientStep(1, System.Windows.Media.Colors.Transparent)));
            Add(new Gradient(new GradientStep(0, System.Windows.Media.Colors.Green),
                new GradientStep(1, System.Windows.Media.Colors.White)));

            Add(new Gradient(new GradientStep(0, System.Windows.Media.Colors.Blue),
                new GradientStep(1, System.Windows.Media.Colors.Black)));
            Add(new Gradient(new GradientStep(0, System.Windows.Media.Colors.Blue),
                new GradientStep(1, System.Windows.Media.Colors.Transparent)));
            Add(new Gradient(new GradientStep(0, System.Windows.Media.Colors.Blue),
                new GradientStep(1, System.Windows.Media.Colors.White)));

            Add(new Gradient
            (
                new GradientStep(0.00, System.Windows.Media.Colors.Red),
                new GradientStep(0.15, Color.FromArgb(255, 255, 255, 0)),
                new GradientStep(0.30, System.Windows.Media.Colors.Blue),
                new GradientStep(0.5, Color.FromArgb(255, 0, 255, 255)),
                new GradientStep(0.70, System.Windows.Media.Colors.Green),
                new GradientStep(0.85, Color.FromArgb(255, 255, 255, 0)),
                new GradientStep(1.00, System.Windows.Media.Colors.Red)
            ));
        }
    }
}