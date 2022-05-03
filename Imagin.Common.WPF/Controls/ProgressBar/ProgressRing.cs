using Imagin.Common.Converters;
using Imagin.Common.Linq;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Imagin.Common.Controls
{
    public class ProgressRing : ProgressBar
    {
        #region Converters

        public static readonly IMultiValueConverter AngleToPointConverter = new MultiConverter<Point>(i =>
        {
            if (i.Values[0] is double angle)
            {
                if (i.Values[1] is double radius)
                {
                    if (i.Values[2] is double stroke)
                    {
                        double piang = angle * Math.PI / 180;

                        double px = Math.Sin(piang) * (radius - stroke / 2) + radius;
                        double py = -Math.Cos(piang) * (radius - stroke / 2) + radius;
                        return new Point(px, py);
                    }
                }
            }
            return default;
        });

        public static readonly IMultiValueConverter RadiusToSizeConverter 
            = new MultiConverter<Size>(i => i.Values[0] is double radius && i.Values[1] is double stroke ? new Size(radius - stroke / 2, radius - stroke / 2) : default);

        public static readonly IMultiValueConverter StrokeToStartPointConverter 
            = new MultiConverter<Point>(i => i.Values[0] is double radius && i.Values[1] is double stroke ? new Point(radius, stroke / 2) : default);

        #endregion

        #region Properties

        public static readonly DependencyProperty AngleProperty = DependencyProperty.Register(nameof(Angle), typeof(double), typeof(ProgressRing), new FrameworkPropertyMetadata(0.0));
        public double Angle
        {
            get => (double)GetValue(AngleProperty);
            set => SetValue(AngleProperty, value);
        }

        public static readonly DependencyProperty BackgroundStrokeProperty = DependencyProperty.Register(nameof(BackgroundStroke), typeof(Brush), typeof(ProgressRing), new FrameworkPropertyMetadata(Brushes.LightGray));
        public Brush BackgroundStroke
        {
            get => (Brush)GetValue(BackgroundStrokeProperty);
            set => SetValue(BackgroundStrokeProperty, value);
        }

        public static readonly DependencyProperty BackgroundStrokeThicknessProperty = DependencyProperty.Register(nameof(BackgroundStrokeThickness), typeof(double), typeof(ProgressRing), new FrameworkPropertyMetadata(2.0));
        public double BackgroundStrokeThickness
        {
            get => (double)GetValue(BackgroundStrokeThicknessProperty);
            set => SetValue(BackgroundStrokeThicknessProperty, value);
        }

        public static readonly DependencyProperty InnerRadiusProperty = DependencyProperty.Register(nameof(InnerRadius), typeof(double), typeof(ProgressRing), new FrameworkPropertyMetadata(32.0));
        public double InnerRadius
        {
            get => (double)GetValue(InnerRadiusProperty);
            set => SetValue(InnerRadiusProperty, value);
        }

        public static readonly DependencyProperty RadiusProperty = DependencyProperty.Register(nameof(Radius), typeof(double), typeof(ProgressRing), new FrameworkPropertyMetadata(42.0));
        public double Radius
        {
            get => (double)GetValue(RadiusProperty);
            set => SetValue(RadiusProperty, value);
        }

        public static readonly DependencyProperty StrokeThicknessProperty = DependencyProperty.Register(nameof(StrokeThickness), typeof(double), typeof(ProgressRing), new FrameworkPropertyMetadata(10.0));
        public double StrokeThickness
        {
            get => (double)GetValue(StrokeThicknessProperty);
            set => SetValue(StrokeThicknessProperty, value);
        }

        #endregion

        #region ProgressRing

        public ProgressRing() : base() { }

        #endregion

        #region Methods

        void Update()
        {
            double currentAngle
                = Angle;
            double targetAngle
                = Value / Maximum * 359.999;
            double duration
                = (currentAngle - targetAngle).Absolute() / 359.999 * 500;

            var animation = new DoubleAnimation(currentAngle, targetAngle, TimeSpan.FromMilliseconds(duration > 0 ? duration : 10));
            BeginAnimation(AngleProperty, animation, HandoffBehavior.Compose);
        }

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            if (e.Property == RadiusProperty)
                Width = Height = Radius * 2;

            base.OnPropertyChanged(e);
        }

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);
            Radius = Math.Min(ActualWidth, ActualHeight) / 2;
        }

        protected override void OnMaximumChanged(double oldMaximum, double newMaximum)
        {
            base.OnMaximumChanged(oldMaximum, newMaximum);
            Update();
        }

        protected override void OnMinimumChanged(double oldMinimum, double newMinimum)
        { 
            base.OnMinimumChanged(oldMinimum, newMinimum);
            Update();
        }

        protected override void OnValueChanged(double oldValue, double newValue)
        {
            base.OnValueChanged(oldValue, newValue);
            Update();
        }

        #endregion
    }
}