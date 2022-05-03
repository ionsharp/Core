using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media.Animation;

namespace Imagin.Common.Linq
{
    [Extends(typeof(UIElement))]
    public static class XUIElement
    {
        public static T AddAdorner<T>(this UIElement input, Func<T> create) where T : Adorner
        {
            var layer = AdornerLayer.GetAdornerLayer
            (
                input is Window window
                ? window.GetChild<Border>(XWindow.BorderKey)
                : input
            );

            if (layer != null)
            {
                var result = create();
                layer.Add(result);
                if (result is ISubscribe i)
                    i.Subscribe();

                return result;
            }

            return default;
        }

        public static void RemoveAdorners<T>(this UIElement input) where T : Adorner
        {
            if (AdornerLayer.GetAdornerLayer(input) is AdornerLayer layer)
            {
                layer.GetAdorners(input)?.ForEach(i =>
                {
                    if (i is T)
                    {
                        if (i is IUnsubscribe j)
                            j.Unsubscribe();

                        layer.Remove(i);
                    }
                });
            }
        }

        //...

        public static void AddOnce(this UIElement input, CommandBinding binding)
        {
            if (!input.CommandBindings.Contains<CommandBinding>(i => i.Command == binding.Command))
                input.CommandBindings.Add(binding);
        }

        public static Task Animate(this UIElement input, Storyboard story)
        {
            var result = new TaskCompletionSource<object>();

            EventHandler handler = default;
            handler = (s, e) =>
            {
                story.Completed -= handler;
                result.TrySetResult(null);
            };

            story.Completed += handler;
            story.Begin();
            return result.Task;
        }

        public async static Task FadeIn(this UIElement input, Duration duration = default)
        {
            var animation = new DoubleAnimation()
            {
                From = 0.0,
                To = 1.0,
                Duration = duration == default ? 0.3.Seconds().Duration() : duration
            };
            Storyboard.SetTarget(animation, input);
            Storyboard.SetTargetProperty(animation, new PropertyPath(nameof(UIElement.Opacity)));

            var result = new Storyboard();
            result.Children.Add(animation);
            await input.Animate(result);
        }

        public async static Task FadeOut(this UIElement input, Duration duration = default, EventHandler Callback = null)
        {
            var animation = new DoubleAnimation()
            {
                From = 1.0,
                To = 0.0,
                Duration = duration == default ? 0.5.Seconds().Duration() : duration
            };
            Storyboard.SetTarget(animation, input);
            Storyboard.SetTargetProperty(animation, new PropertyPath(nameof(UIElement.Opacity)));

            var result = new Storyboard();
            result.Children.Add(animation);
            await input.Animate(result);
        }
    }
}