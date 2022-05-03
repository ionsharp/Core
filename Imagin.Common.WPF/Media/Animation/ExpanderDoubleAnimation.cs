using System.Windows;
using System.Windows.Media.Animation;

namespace Imagin.Common.Media.Animation
{
    /// <summary>
    /// Animates a double value.
    /// </summary>
    public class ExpanderDoubleAnimation : DoubleAnimationBase
    {
        #region Properties

        /// <summary>
        /// Dependency property for the From property
        /// </summary>
        public static readonly DependencyProperty FromProperty = DependencyProperty.Register("From", typeof(double?), typeof(ExpanderDoubleAnimation));
        /// <summary>
        /// CLR Wrapper for the From depenendency property
        /// </summary>
        public double? From
        {
            get
            {
                return (double?)GetValue(ExpanderDoubleAnimation.FromProperty);
            }
            set
            {
                SetValue(ExpanderDoubleAnimation.FromProperty, value);
            }
        }

        /// <summary>
        /// Dependency property for the To property
        /// </summary>
        public static readonly DependencyProperty ToProperty = DependencyProperty.Register("To", typeof(double?), typeof(ExpanderDoubleAnimation));
        /// <summary>
        /// CLR Wrapper for the To property
        /// </summary>
        public double? To
        {
            get
            {
                return (double?)GetValue(ExpanderDoubleAnimation.ToProperty);
            }
            set
            {
                SetValue(ExpanderDoubleAnimation.ToProperty, value);
            }
        }

        /// <summary>
        /// Sets the reverse value for the second animation
        /// </summary>
        public double? ReverseValue
        {
            get { return (double)GetValue(ReverseValueProperty); }
            set { SetValue(ReverseValueProperty, value); }
        }
        /// <summary>
        /// Sets the reverse value for the second animation
        /// </summary>
        public static readonly DependencyProperty ReverseValueProperty = DependencyProperty.Register("ReverseValue", typeof(double?), typeof(ExpanderDoubleAnimation), new FrameworkPropertyMetadata(0.0));

        #endregion

        #region Methods

        /// <summary>
        /// Creates an instance of the animation
        /// </summary>
        /// <returns></returns>
        protected override Freezable CreateInstanceCore()
        {
            return new ExpanderDoubleAnimation();
        }

        /// <summary>
        /// Animates the double value
        /// </summary>
        /// <param name="defaultOriginValue">The original value to animate</param>
        /// <param name="defaultDestinationValue">The final value</param>
        /// <param name="animationClock">The animation clock (timer)</param>
        /// <returns>Returns the new double to set</returns>
        protected override double GetCurrentValueCore(double defaultOriginValue, double defaultDestinationValue, AnimationClock animationClock)
        {
            double fromVal = this.From.Value;
            double toVal = this.To.Value;

            if (defaultOriginValue == toVal)
            {
                fromVal = toVal;
                toVal = this.ReverseValue.Value;
            }

            if (fromVal > toVal)
                return (1 - animationClock.CurrentProgress.Value) *
                    (fromVal - toVal) + toVal;
            else
                return (animationClock.CurrentProgress.Value *
                    (toVal - fromVal) + fromVal);
        }

        #endregion
    }
}
