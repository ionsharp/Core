using Imagin.Common.Collections;
using Imagin.Common.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Imagin.Common.Controls
{
    public partial class ComboWindow : Window
    {
        public static readonly DependencyProperty ItemConverterProperty = DependencyProperty.Register(nameof(ItemConverter), typeof(IValueConverter), typeof(ComboWindow), new FrameworkPropertyMetadata(null));
        public IValueConverter ItemConverter
        {
            get => (IValueConverter)GetValue(ItemConverterProperty);
            set => SetValue(ItemConverterProperty, value);
        }
        
        public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register(nameof(ItemsSource), typeof(object), typeof(ComboWindow), new FrameworkPropertyMetadata(null));
        public object ItemsSource
        {
            get => GetValue(ItemsSourceProperty);
            set => SetValue(ItemsSourceProperty, value);
        }

        public static readonly DependencyProperty PlaceholderProperty = DependencyProperty.Register(nameof(Placeholder), typeof(string), typeof(ComboWindow), new FrameworkPropertyMetadata(string.Empty));
        public string Placeholder
        {
            get => (string)GetValue(PlaceholderProperty);
            set => SetValue(PlaceholderProperty, value);
        }

        public static readonly DependencyProperty SelectedItemProperty = DependencyProperty.Register(nameof(SelectedItem), typeof(object), typeof(ComboWindow), new FrameworkPropertyMetadata(null));
        public object SelectedItem
        {
            get => GetValue(SelectedItemProperty);
            set => SetValue(SelectedItemProperty, value);
        }

        static readonly DependencyPropertyKey SelectedItemsKey = DependencyProperty.RegisterReadOnly(nameof(SelectedItems), typeof(IList), typeof(ComboWindow), new FrameworkPropertyMetadata(null));
        public static readonly DependencyProperty SelectedItemsProperty = SelectedItemsKey.DependencyProperty;
        public IList SelectedItems
        {
            get => (IList)GetValue(SelectedItemsProperty);
            private set => SetValue(SelectedItemsKey, value);
        }

        public static readonly DependencyProperty SelectionModeProperty = DependencyProperty.Register(nameof(SelectionMode), typeof(SelectionMode), typeof(ComboWindow), new FrameworkPropertyMetadata(SelectionMode.Single));
        public SelectionMode SelectionMode
        {
            get => (SelectionMode)GetValue(SelectionModeProperty);
            set => SetValue(SelectionModeProperty, value);
        }
        
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(nameof(Text), typeof(string), typeof(ComboWindow), new FrameworkPropertyMetadata(string.Empty));
        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        public ComboWindow() : base()
        {
            SelectedItems = new List<object>();

            this.RegisterHandler(OnLoaded, OnUnloaded);
            InitializeComponent();
        }

        public ComboWindow(string title, string message, object source, IValueConverter itemConverter, SelectionMode selectionMode = SelectionMode.Single, Button[] buttons = null) : this()
        {
            SetCurrentValue(TitleProperty,
                title);
            SetCurrentValue(TextProperty,
                message);
            SetCurrentValue(ItemsSourceProperty,
                source);
            SetCurrentValue(ItemConverterProperty,
                itemConverter);
            SetCurrentValue(SelectionModeProperty,
                selectionMode);

            XWindow.SetFooterButtons(this, buttons != null ? new Buttons(this, buttons) : new Buttons(this, Buttons.SaveCancel));
        }

        void OnSelectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            SelectedItems.Clear();
            sender.As<ICollectionChanged>().ForEach<object>(i => SelectedItems.Add(i));
        }

        void OnLoaded()
        {
            var selection = XComboBox.GetSelectedItems(ComboBox);
            OnSelectionChanged(selection, null);

            selection.CollectionChanged += OnSelectionChanged;
        }

        void OnUnloaded()
            => XComboBox.GetSelectedItems(ComboBox).CollectionChanged -= OnSelectionChanged;
    }
}