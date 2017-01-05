using System;
using System.Windows;
using System.Windows.Media.Animation;
using System.Threading.Tasks;

namespace Imagin.Common.Extensions
{
    public static class UIElementExtensions
    {
        public static Task BeginAnimationAsync(this UIElement Element, Storyboard Storyboard)
        {
            var Source = new TaskCompletionSource<object>();

            var i = default(EventHandler);

            i = (s, e) =>
            {
                Storyboard.Completed -= i;
                Source.TrySetResult(null);
            };

            Storyboard.Completed += i;
            Storyboard.Begin();

            return Source.Task;
        }

        public static Task FadeIn(this UIElement Element, Duration Duration = default(Duration))
        {
            var Child = new DoubleAnimation()
            {
                From = 0.0,
                To = 1.0,
                Duration = Duration == default(Duration) ? new Duration(TimeSpan.FromSeconds(0.3)) : Duration
            };
            Storyboard.SetTarget(Child, Element);
            Storyboard.SetTargetProperty(Child, new PropertyPath("Opacity"));

            var Animation = new Storyboard();
            Animation.Children.Add(Child);

            return Element.BeginAnimationAsync(Animation);
        }

        public static Task FadeOut(this UIElement Element, Duration Duration = default(Duration), EventHandler Callback = null)
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

            return Element.BeginAnimationAsync(Animation);
        }
    }
}
