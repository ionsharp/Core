using System.Windows;
using System.Windows.Interactivity;

namespace Imagin.Common.Behavior
{
    public class DynamicResourceBehavior : Behavior<FrameworkElement>
    {
        public static readonly DependencyProperty KeyProperty = DependencyProperty.Register(nameof(Key), typeof(string), typeof(DynamicResourceBehavior), new FrameworkPropertyMetadata(null));
        public string Key
        {
            get => (string)GetValue(KeyProperty);
            set => SetValue(KeyProperty, value);
        }

        public static readonly DependencyProperty PropertyProperty = DependencyProperty.Register(nameof(Property), typeof(DependencyProperty), typeof(DynamicResourceBehavior), new FrameworkPropertyMetadata(null));
        public DependencyProperty Property
        {
            get => (DependencyProperty)GetValue(PropertyProperty);
            set => SetValue(PropertyProperty, value);
        }

        protected override void OnAttached()
        {
            if (AssociatedObject != null)
            {
                if (Property != null)
                {
                    if (Key != null)
                        AssociatedObject.SetResourceReference(Property, Key);
                }
            }
        }

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
            OnAttached();
        }
    }
}