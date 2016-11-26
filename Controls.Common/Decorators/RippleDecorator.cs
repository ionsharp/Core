using Imagin.Common.Extensions;
using Imagin.Common.Input;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Imagin.Controls.Common
{
    /// <summary>
    /// A container that applies a ripple effect to content.
    /// </summary>
    /// <remarks>
    /// If (RippleMouseEvent = MouseEvent.Default) 
    ///     RepeatBehavior = Forever
    ///     Animation begins without input
    /// </remarks>
    [TemplatePart(Name = "PART_Canvas", Type = typeof(Canvas))]
    [TemplatePart(Name = "PART_Ellipse", Type = typeof(Ellipse))]
    public class RippleDecorator : ContentControl
    {
        #region Properties

        Canvas PART_Canvas
        {
            get; set;
        }

        Ellipse PART_Ellipse
        {
            get; set;
        }

        bool PropertyChangeHandled = false;

        public static DependencyProperty IsRippleEnabledProperty = DependencyProperty.Register("IsRippleEnabled", typeof(bool), typeof(RippleDecorator), new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public bool IsRippleEnabled
        {
            get
            {
                return (bool)GetValue(IsRippleEnabledProperty);
            }
            set
            {
                SetValue(IsRippleEnabledProperty, value);
            }
        }

        public static DependencyProperty MaximumOpacityProperty = DependencyProperty.Register("MaximumOpacity", typeof(double), typeof(RippleDecorator), new FrameworkPropertyMetadata(0.8, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnPropertyChanged));
        public double MaximumOpacity
        {
            get
            {
                return (double)GetValue(MaximumOpacityProperty);
            }
            set
            {
                SetValue(MaximumOpacityProperty, value);
            }
        }

        public static DependencyProperty MaximumRadiusProperty = DependencyProperty.Register("MaximumRadius", typeof(double), typeof(RippleDecorator), new FrameworkPropertyMetadata(100.0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnPropertyChanged));
        public double MaximumRadius
        {
            get
            {
                return (double)GetValue(MaximumRadiusProperty);
            }
            set
            {
                SetValue(MaximumRadiusProperty, value);
            }
        }

        public static DependencyProperty ToStrokeThicknessProperty = DependencyProperty.Register("ToStrokeThickness", typeof(double), typeof(RippleDecorator), new FrameworkPropertyMetadata(3.0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnPropertyChanged));
        public double ToStrokeThickness
        {
            get
            {
                return (double)GetValue(ToStrokeThicknessProperty);
            }
            set
            {
                SetValue(ToStrokeThicknessProperty, value);
            }
        }

        public static DependencyProperty FromStrokeThicknessProperty = DependencyProperty.Register("FromStrokeThickness", typeof(double), typeof(RippleDecorator), new FrameworkPropertyMetadata(15.0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnPropertyChanged));
        public double FromStrokeThickness
        {
            get
            {
                return (double)GetValue(FromStrokeThicknessProperty);
            }
            set
            {
                SetValue(FromStrokeThicknessProperty, value);
            }
        }

        public static DependencyProperty RippleMouseEventProperty = DependencyProperty.Register("RippleMouseEvent", typeof(MouseEvent), typeof(RippleDecorator), new FrameworkPropertyMetadata(MouseEvent.MouseDown, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnPropertyChanged));
        public MouseEvent RippleMouseEvent
        {
            get
            {
                return (MouseEvent)GetValue(RippleMouseEventProperty);
            }
            set
            {
                SetValue(RippleMouseEventProperty, value);
            }
        }
        
        public static DependencyProperty RippleAnimationProperty = DependencyProperty.Register("RippleAnimation", typeof(Storyboard), typeof(RippleDecorator), new FrameworkPropertyMetadata(default(Storyboard), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public Storyboard RippleAnimation
        {
            get
            {
                return (Storyboard)GetValue(RippleAnimationProperty);
            }
            private set
            {
                SetValue(RippleAnimationProperty, value);
            }
        }

        public static DependencyProperty RippleAccelerationProperty = DependencyProperty.Register("RippleAcceleration", typeof(double), typeof(RippleDecorator), new FrameworkPropertyMetadata(0.6, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnPropertyChanged));
        public double RippleAcceleration
        {
            get
            {
                return (double)GetValue(RippleAccelerationProperty);
            }
            set
            {
                SetValue(RippleAccelerationProperty, value);
            }
        }

        public static DependencyProperty RippleDecelerationProperty = DependencyProperty.Register("RippleDeceleration", typeof(double), typeof(RippleDecorator), new FrameworkPropertyMetadata(0.4, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnPropertyChanged));
        public double RippleDeceleration
        {
            get
            {
                return (double)GetValue(RippleDecelerationProperty);
            }
            set
            {
                SetValue(RippleDecelerationProperty, value);
            }
        }

        public static DependencyProperty RippleDelayProperty = DependencyProperty.Register("RippleDelay", typeof(Duration), typeof(RippleDecorator), new FrameworkPropertyMetadata(default(Duration), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnPropertyChanged));
        /// <summary>
        /// Animation delay in seconds.
        /// </summary>
        public Duration RippleDelay
        {
            get
            {
                return (Duration)GetValue(RippleDelayProperty);
            }
            set
            {
                SetValue(RippleDelayProperty, value);
            }
        }

        public static DependencyProperty RippleDurationProperty = DependencyProperty.Register("RippleDuration", typeof(Duration), typeof(RippleDecorator), new FrameworkPropertyMetadata(default(Duration), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnPropertyChanged));
        public Duration RippleDuration
        {
            get
            {
                return (Duration)GetValue(RippleDurationProperty);
            }
            set
            {
                SetValue(RippleDurationProperty, value);
            }
        }

        static void OnPropertyChanged(DependencyObject Object, DependencyPropertyChangedEventArgs e)
        {
            Object.As<RippleDecorator>().OnPropertyChanged(e.NewValue);
        }

        #endregion

        #region RippleDecorator

        public RippleDecorator()
        {
            this.DefaultStyleKey = typeof(RippleDecorator);

            this.PropertyChangeHandled = true;
            SetCurrentValue(RippleDelayProperty, new Duration(TimeSpan.FromSeconds(0.0)));
            SetCurrentValue(RippleDurationProperty, new Duration(TimeSpan.FromSeconds(1.0)));
            this.PropertyChangeHandled = false;

            this.RippleAnimation = this.GetAnimation();
        }

        #endregion

        #region Methods

        void BeginAnimation()
        {
            this.PART_Ellipse.BeginStoryboard(this.RippleAnimation);
        }

        Storyboard GetAnimation()
        {
            var Result = new Storyboard();
            Result.Duration = this.RippleDuration;

            var WidthAnimation = new DoubleAnimationUsingKeyFrames()
            {
                Duration = this.RippleDuration,
                AccelerationRatio = this.RippleAcceleration,
                DecelerationRatio = this.RippleDeceleration
            };
            var HeightAnimation = new DoubleAnimationUsingKeyFrames()
            {
                Duration = this.RippleDuration,
                AccelerationRatio = this.RippleAcceleration,
                DecelerationRatio = this.RippleDeceleration
            };
            var StrokeThicknessAnimation = new DoubleAnimationUsingKeyFrames()
            {
                Duration = this.RippleDuration,
                AccelerationRatio = this.RippleAcceleration,
                DecelerationRatio = this.RippleDeceleration
            };
            var OpacityAnimation = new DoubleAnimationUsingKeyFrames()
            {
                Duration = this.RippleDuration,
                AccelerationRatio = this.RippleAcceleration,
                DecelerationRatio = this.RippleDeceleration
            };

            Storyboard.SetTargetName(WidthAnimation, "PART_Ellipse");
            Storyboard.SetTargetName(HeightAnimation, "PART_Ellipse");
            Storyboard.SetTargetName(StrokeThicknessAnimation, "PART_Ellipse");
            Storyboard.SetTargetName(OpacityAnimation, "PART_Ellipse");

            Storyboard.SetTargetProperty(WidthAnimation, new PropertyPath("Width"));
            Storyboard.SetTargetProperty(HeightAnimation, new PropertyPath("Height"));
            Storyboard.SetTargetProperty(StrokeThicknessAnimation, new PropertyPath("StrokeThickness"));
            Storyboard.SetTargetProperty(OpacityAnimation, new PropertyPath("Opacity"));

            WidthAnimation.KeyFrames.Add(new DiscreteDoubleKeyFrame(0, KeyTime.FromTimeSpan(this.RippleDelay.TimeSpan)));
            WidthAnimation.KeyFrames.Add(new LinearDoubleKeyFrame(this.MaximumRadius, KeyTime.FromTimeSpan(this.RippleDuration.TimeSpan)));
            WidthAnimation.KeyFrames.Add(new DiscreteDoubleKeyFrame(0, KeyTime.FromTimeSpan(this.RippleDuration.TimeSpan)));

            HeightAnimation.KeyFrames.Add(new DiscreteDoubleKeyFrame(0, KeyTime.FromTimeSpan(this.RippleDelay.TimeSpan)));
            HeightAnimation.KeyFrames.Add(new LinearDoubleKeyFrame(this.MaximumRadius, KeyTime.FromTimeSpan(this.RippleDuration.TimeSpan)));
            HeightAnimation.KeyFrames.Add(new DiscreteDoubleKeyFrame(0, KeyTime.FromTimeSpan(this.RippleDuration.TimeSpan)));

            var Offset = (this.RippleDuration.TimeSpan.Ticks / 4) * 3;

            StrokeThicknessAnimation.KeyFrames.Add(new DiscreteDoubleKeyFrame(this.FromStrokeThickness, KeyTime.FromTimeSpan(this.RippleDelay.TimeSpan)));
            StrokeThicknessAnimation.KeyFrames.Add(new LinearDoubleKeyFrame(this.ToStrokeThickness, KeyTime.FromTimeSpan(this.RippleDuration.TimeSpan)));

            OpacityAnimation.KeyFrames.Add(new DiscreteDoubleKeyFrame(this.MaximumOpacity, KeyTime.FromTimeSpan(this.RippleDelay.TimeSpan)));
            OpacityAnimation.KeyFrames.Add(new LinearDoubleKeyFrame(this.MaximumOpacity / 2.0, KeyTime.FromTimeSpan(new TimeSpan(Offset))));
            OpacityAnimation.KeyFrames.Add(new LinearDoubleKeyFrame(0.0, KeyTime.FromTimeSpan(this.RippleDuration.TimeSpan)));

            Result.Children.Add(WidthAnimation);
            Result.Children.Add(HeightAnimation);
            Result.Children.Add(StrokeThicknessAnimation);
            Result.Children.Add(OpacityAnimation);

            if (this.RippleMouseEvent == MouseEvent.Default)
            {
                Result.RepeatBehavior = RepeatBehavior.Forever;
                this.BeginAnimation();
            }

            return Result;
        }

        void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            Canvas.SetTop(PART_Ellipse, (PART_Canvas.ActualHeight / 2.0) - (e.NewSize.Height / 2.0));
            Canvas.SetLeft(PART_Ellipse, (PART_Canvas.ActualWidth / 2.0) - (e.NewSize.Width / 2.0));
        }

        protected override void OnPreviewMouseDown(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseDown(e);

            if (this.RippleAnimation != null && this.IsRippleEnabled && this.RippleMouseEvent == MouseEvent.MouseDown)
                this.BeginAnimation();
        }

        public override void OnApplyTemplate()
        {
            base.ApplyTemplate();

            this.PART_Canvas = this.Template.FindName("PART_Canvas", this).As<Canvas>();

            this.PART_Ellipse = this.Template.FindName("PART_Ellipse", this).As<Ellipse>();
            this.PART_Ellipse.SizeChanged += OnSizeChanged; 
        }

        protected virtual void OnPropertyChanged(object Value)
        {
            if (this.PropertyChangeHandled) return;
            this.RippleAnimation = this.GetAnimation();
        }

        #endregion
    }
}
