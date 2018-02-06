using System;
using System.Windows;
using System.Windows.Media.Animation;
using System.Threading.Tasks;

namespace Imagin.Common.Linq
{
    /// <summary>
    /// 
    /// </summary>
    public static class UIElementExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        /// <param name="storyboard"></param>
        /// <returns></returns>
        public static Task AnimateAsync(this UIElement element, Storyboard storyboard)
        {
            var source = new TaskCompletionSource<object>();

            var handler = default(EventHandler);

            handler = (s, e) =>
            {
                storyboard.Completed -= handler;
                source.TrySetResult(null);
            };

            storyboard.Completed += handler;
            storyboard.Begin();

            return source.Task;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Element"></param>
        /// <param name="Duration"></param>
        /// <returns></returns>
        public static Task FadeInAsync(this UIElement Element, Duration Duration = default(Duration))
        {
            var DoubleAnimation = new DoubleAnimation()
            {
                From = 0.0,
                To = 1.0,
                Duration = Duration == default(Duration) ? new Duration(TimeSpan.FromSeconds(0.3)) : Duration
            };
            Storyboard.SetTarget(DoubleAnimation, Element);
            Storyboard.SetTargetProperty(DoubleAnimation, new PropertyPath("Opacity"));

            var s = new Storyboard();
            s.Children.Add(DoubleAnimation);

            return Element.AnimateAsync(s);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Element"></param>
        /// <param name="Duration"></param>
        /// <param name="Callback"></param>
        /// <returns></returns>
        public static Task FadeOutAsync(this UIElement Element, Duration Duration = default(Duration), EventHandler Callback = null)
        {
            var Child = new DoubleAnimation()
            {
                From = 1.0,
                To = 0.0,
                Duration = Duration == default(Duration) ? new Duration(TimeSpan.FromSeconds(0.5)) : Duration
            };
            Storyboard.SetTarget(Child, Element);
            Storyboard.SetTargetProperty(Child, new PropertyPath("Opacity"));

            var Animation = new Storyboard();
            Animation.Children.Add(Child);

            return Element.AnimateAsync(Animation);
        }
    }
}
