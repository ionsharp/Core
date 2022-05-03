using Imagin.Common.Input;
using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Imagin.Apps.Paint
{
    public class ToolButton : UserControl
    {
        static readonly DependencyPropertyKey ArrowVisibilityKey = DependencyProperty.RegisterReadOnly(nameof(ArrowVisibility), typeof(Visibility), typeof(ToolButton), new FrameworkPropertyMetadata(Visibility.Collapsed, FrameworkPropertyMetadataOptions.None));
        public static readonly DependencyProperty ArrowVisibilityProperty = ArrowVisibilityKey.DependencyProperty;
        public Visibility ArrowVisibility
        {
            get => (Visibility)GetValue(ArrowVisibilityProperty);
            private set => SetValue(ArrowVisibilityKey, value);
        }

        public static readonly DependencyProperty CountProperty = DependencyProperty.Register(nameof(Count), typeof(int), typeof(ToolButton), new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.None, OnCountChanged));
        public int Count
        {
            get => (int)GetValue(CountProperty);
            set => SetValue(CountProperty, value);
        }
        static void OnCountChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as ToolButton).ArrowVisibility = (int)e.NewValue > 1 ? Visibility.Visible : Visibility.Collapsed;

        public static readonly DependencyProperty IsCheckedProperty = DependencyProperty.Register(nameof(IsChecked), typeof(bool), typeof(ToolButton), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.None));
        public bool IsChecked
        {
            get => (bool)GetValue(IsCheckedProperty);
            set => SetValue(IsCheckedProperty, value);
        }

        public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register(nameof(ItemsSource), typeof(IList), typeof(ToolButton), new FrameworkPropertyMetadata(default(IList), FrameworkPropertyMetadataOptions.None));
        public IList ItemsSource
        {
            get => (IList)GetValue(ItemsSourceProperty);
            set => SetValue(ItemsSourceProperty, value);
        }

        public static readonly DependencyProperty ItemTemplateProperty = DependencyProperty.Register(nameof(ItemTemplate), typeof(DataTemplate), typeof(ToolButton), new FrameworkPropertyMetadata(default(DataTemplate), FrameworkPropertyMetadataOptions.None));
        public DataTemplate ItemTemplate
        {
            get => (DataTemplate)GetValue(ItemTemplateProperty);
            set => SetValue(ItemTemplateProperty, value);
        }

        public static readonly DependencyProperty IsMenuVisibleProperty = DependencyProperty.Register(nameof(IsMenuVisible), typeof(bool), typeof(ToolButton), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.None));
        public bool IsMenuVisible
        {
            get => (bool)GetValue(IsMenuVisibleProperty);
            set => SetValue(IsMenuVisibleProperty, value);
        }

        public static readonly DependencyProperty SelectedItemTemplateProperty = DependencyProperty.Register(nameof(SelectedItemTemplate), typeof(DataTemplate), typeof(ToolButton), new FrameworkPropertyMetadata(default(DataTemplate), FrameworkPropertyMetadataOptions.None));
        public DataTemplate SelectedItemTemplate
        {
            get => (DataTemplate)GetValue(SelectedItemTemplateProperty);
            set => SetValue(SelectedItemTemplateProperty, value);
        }

        public static readonly DependencyProperty SelectedItemProperty = DependencyProperty.Register(nameof(SelectedItem), typeof(object), typeof(ToolButton), new FrameworkPropertyMetadata(default, FrameworkPropertyMetadataOptions.None));
        public object SelectedItem
        {
            get => GetValue(SelectedItemProperty);
            set => SetValue(SelectedItemProperty, value);
        }

        public ToolButton() : base() { }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            SetCurrentValue(IsCheckedProperty, true);
        }

        protected override void OnMouseRightButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseRightButtonDown(e);
            if (Count > 1)
                SetCurrentValue(IsMenuVisibleProperty, !IsMenuVisible);
        }

        ICommand selectCommand;
        public ICommand SelectCommand => selectCommand ??= new RelayCommand<Tool>(i =>
        {
            i.IsSelected = true;
            SetCurrentValue(SelectedItemProperty, i);
            SetCurrentValue(IsMenuVisibleProperty, false);

        },
        i => i != null);
    }
}