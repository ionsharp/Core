using System;
using System.Windows;
using System.Windows.Media.Animation;

namespace Imagin.Common.Extensions
{
    public static class UIElementExtensions
    {
        /// <summary>
        /// Imagin.Core
        /// </summary>
        public static void FadeIn(this UIElement Element, Duration Duration = default(Duration), EventHandler Callback = null)
        {
            DoubleAnimation Animation = new DoubleAnimation()
            {
                From = 0.0,
                To = 1.0,
                Duration = Duration == default(Duration) ? new Duration(TimeSpan.FromSeconds(0.3)) : Duration
            };
            if (Callback != null)
                Animation.Completed += Callback;
            Element.BeginAnimation(UIElement.OpacityProperty, Animation);
        }

        /// <summary>
        /// Imagin.Core
        /// </summary>
        public static void FadeOut(this UIElement Element, Duration Duration = default(Duration), EventHandler Callback = null)
        {
            DoubleAnimation Animation = new DoubleAnimation()
            {
                From = 1.0,
                To = 0.0,
                Duration = Duration == default(Duration) ? new Duration(TimeSpan.FromSeconds(0.3)) : Duration
            };
            if (Callback != null)
                Animation.Completed += Callback;
            Element.BeginAnimation(UIElement.OpacityProperty, Animation);
        }
    }
}
