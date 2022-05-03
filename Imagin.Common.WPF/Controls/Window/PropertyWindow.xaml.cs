using Imagin.Common.Linq;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Imagin.Common.Controls
{
    public partial class PropertyWindow : Window
    {
        public static readonly ReferenceKey<PropertyGrid> PropertyGridKey = new();

        public static readonly DependencyProperty SourceProperty = DependencyProperty.Register(nameof(Source), typeof(object), typeof(PropertyWindow), new FrameworkPropertyMetadata(null));
        public object Source
        {
            get => (object)GetValue(SourceProperty);
            set => SetValue(SourceProperty, value);
        }

        //...

        public PropertyWindow() : this(Buttons.Done) { }

        public PropertyWindow(Button[] buttons) : base()
        {
            XWindow.SetFooterButtons(this, new Buttons(this, buttons));
            InitializeComponent();
        }

        //...

        public static T Show<T>(string title, T source, Action<PropertyGrid> action = null)
        {
            var result = new PropertyWindow();
            result.SetCurrentValue(SourceProperty, source);
            result.SetCurrentValue(TitleProperty, title.NullOrEmpty() ? source.GetType().Name : title);
            if (result.GetChild<PropertyGrid>(PropertyGridKey) is PropertyGrid i)
                action?.Invoke(i);

            result.Show();
            return (T)result.Source;
        }

        public static T ShowDialog<T>(string title, T source, Action<PropertyGrid> action = null)
            => ShowDialog(title, source, out int result, action);

        public static T ShowDialog<T>(string title, T source, out int result, Action<PropertyGrid> action, params Button[] buttons)
        {
            var window = new PropertyWindow(buttons);
            window.SetCurrentValue(SourceProperty, source);
            window.SetCurrentValue(TitleProperty, title.NullOrEmpty() ? source.GetType().Name : title);
            if (window.GetChild(PropertyGridKey) is PropertyGrid i)
                action?.Invoke(i);

            window.ShowDialog();
            result = XWindow.GetResult(window);
            return (T)window.Source;
        }
    }
}