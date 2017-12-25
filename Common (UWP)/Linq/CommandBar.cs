    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;
    using Windows.UI.Xaml.Media;

    namespace Imagin.Common.Linq
    {
        public static class CommandBarExtensions
        {
            public static readonly DependencyProperty HideMoreButtonProperty = DependencyProperty.RegisterAttached("HideMoreButton", typeof(bool), typeof(CommandBarExtensions), new PropertyMetadata(false, OnHideMoreButtonChanged));
            public static bool GetHideMoreButton(CommandBar d)
            {
                return (bool)d.GetValue(HideMoreButtonProperty);
            }
            public static void SetHideMoreButton(CommandBar d, bool value)
            {
                d.SetValue(HideMoreButtonProperty, value);
            }
            static void OnHideMoreButtonChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
            {
                var CommandBar = d as CommandBar;

                if (CommandBar != null)
                {
                    var MoreButton = CommandBar.GetChild<Button>("MoreButton") as UIElement;
                    if (MoreButton != null)
                    {
                        MoreButton.Visibility = e.NewValue.As<bool>().Invert().ToVisibility();
                    }
                    else CommandBar.Loaded += OnCommandBarLoaded;
                }
            }

            static void OnCommandBarLoaded(object sender, RoutedEventArgs e)
            {
                var CommandBar = sender as CommandBar;

                var MoreButton = CommandBar?.GetChild<Button>("MoreButton") as UIElement;
                if (MoreButton != null)
                {
                    MoreButton.Visibility = GetHideMoreButton(CommandBar).Invert().ToVisibility();
                    CommandBar.Loaded -= OnCommandBarLoaded;
                }
            }

            public static T GetChild<T>(this DependencyObject Parent, string Name) where T : DependencyObject
            {
                if (Parent != null)
                {
                    for (int i = 0, Count = VisualTreeHelper.GetChildrenCount(Parent); i < Count; i++)
                    {
                        var Child = VisualTreeHelper.GetChild(Parent, i);

                        var Result = Child is T && !Name.IsNullOrEmpty() && Child.As<FrameworkElement>()?.Name == Name ? Child as T : Child.GetChild<T>(Name);
                        if (Result != null)
                            return Result;
                    }
                }
                return null;
            }
        }
    }
