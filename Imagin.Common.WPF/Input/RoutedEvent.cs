using System.Windows;

namespace Imagin.Common.Input
{
    public delegate void RoutedEventHandler<T>(DependencyObject sender, RoutedEventArgs<T> e);

    public class RoutedEventArgs<T> : RoutedEventArgs
    {
        public readonly T Value;

        public RoutedEventArgs(RoutedEvent input, T value, object source = null) : base(input, source)
        {
            Value = value;
        }
    }
}