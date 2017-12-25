using System;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Animation;

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
        /// <param name="Element"></param>
        /// <param name="Storyboard"></param>
        /// <returns></returns>
        public static Task BeginAnimationAsync(this UIElement Element, Storyboard Storyboard)
        {
            var Source = new TaskCompletionSource<object>();

            var i = default(EventHandler<object>);

            i = (s, e) =>
            {
                Storyboard.Completed -= i;
                Source.TrySetResult(null);
            };

            Storyboard.Completed += i;
            Storyboard.Begin();

            return Source.Task;
        }

        /// <summary>
        /// Fades a <see cref="UIElement"/> in by increasing <see cref="UIElement.Opacity"/>.
        /// </summary>
        /// <param name="Element"></param>
        /// <param name="Duration">Duration of animation in seconds.</param>
        /// <param name="Callback"></param>
        public static async Task BeginFadeIn(this UIElement Element, double Duration = 0.5)
        {
            await Element.BeginFadeIn(new Duration(TimeSpan.FromSeconds(Duration)));
        }

        /// <summary>
        /// Fades a <see cref="UIElement"/> out by descreasing <see cref="UIElement.Opacity"/>.
        /// </summary>
        /// <param name="Element"></param>
        /// <param name="Duration">Duration of animation in seconds.</param>
        /// <param name="Callback"></param>
        public static async Task BeginFadeOut(this UIElement Element, double Duration = 0.5)
        {
            await Element.BeginFadeOut(new Duration(TimeSpan.FromSeconds(Duration)));
        }

        /// <summary>
        /// Fades a <see cref="UIElement"/> in by increasing <see cref="UIElement.Opacity"/>.
        /// </summary>
        /// <param name="Element"></param>
        /// <param name="Duration"></param>
        /// <param name="Callback"></param>
        public static async Task BeginFadeIn(this UIElement Element, Duration Duration)
        {
            var Child = new DoubleAnimation()
            {
                From = 0.0,
                To = 1.0,
                Duration = Duration == default(Duration) ? new Duration(TimeSpan.FromSeconds(0.3)) : Duration
            };
            Storyboard.SetTarget(Child, Element);
            Storyboard.SetTargetProperty(Child, "Opacity");

            var s = new Storyboard();
            s.Children.Add(Child);

            await Element.BeginAnimationAsync(s);
        }

        /// <summary>
        /// Fades a <see cref="UIElement"/> out by descreasing <see cref="UIElement.Opacity"/>.
        /// </summary>
        /// <param name="Element"></param>
        /// <param name="Duration"></param>
        /// <param name="Callback"></param>
        public static async Task BeginFadeOut(this UIElement Element, Duration Duration)
        {
            var Child = new DoubleAnimation()
            {
                From = 1.0,
                To = 0.0,
                Duration = Duration
            };
            Storyboard.SetTarget(Child, Element);
            Storyboard.SetTargetProperty(Child, "Opacity");

            var s = new Storyboard();
            s.Children.Add(Child);

            await Element.BeginAnimationAsync(s);
        }
    }
}
