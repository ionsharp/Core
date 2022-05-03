using Imagin.Common.Linq;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Imagin.Common.Controls
{
    public abstract class TreeViewColumn : FrameworkElement
    {
        public event EventHandler<EventArgs> VisibilityChanged;

        public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register(nameof(Header), typeof(object), typeof(TreeViewColumn), new FrameworkPropertyMetadata(null));
        public object Header
        {
            get => GetValue(HeaderProperty);
            set => SetValue(HeaderProperty, value);
        }

        public static readonly DependencyProperty HeaderStringFormatProperty = DependencyProperty.Register(nameof(HeaderStringFormat), typeof(string), typeof(TreeViewColumn), new FrameworkPropertyMetadata(null));
        public string HeaderStringFormat
        {
            get => (string)GetValue(HeaderStringFormatProperty);
            set => SetValue(HeaderStringFormatProperty, value);
        }

        public static readonly DependencyProperty HeaderTemplateProperty = DependencyProperty.Register(nameof(HeaderTemplate), typeof(DataTemplate), typeof(TreeViewColumn), new FrameworkPropertyMetadata(null));
        public DataTemplate HeaderTemplate
        {
            get => (DataTemplate)GetValue(HeaderTemplateProperty);
            set => SetValue(HeaderTemplateProperty, value);
        }

        public static readonly DependencyProperty HeaderTemplateSelectorProperty = DependencyProperty.Register(nameof(HeaderTemplateSelector), typeof(DataTemplateSelector), typeof(TreeViewColumn), new FrameworkPropertyMetadata(null));
        public DataTemplateSelector HeaderTemplateSelector
        {
            get => (DataTemplateSelector)GetValue(HeaderTemplateSelectorProperty);
            set => SetValue(HeaderTemplateSelectorProperty, value);
        }

        public static readonly DependencyProperty HorizontalContentAlignmentProperty = DependencyProperty.Register(nameof(HorizontalContentAlignment), typeof(HorizontalAlignment), typeof(TreeViewColumn), new FrameworkPropertyMetadata(HorizontalAlignment.Left));
        public HorizontalAlignment HorizontalContentAlignment
        {
            get => (HorizontalAlignment)GetValue(HorizontalContentAlignmentProperty);
            set => SetValue(HorizontalContentAlignmentProperty, value);
        }

        new public static readonly DependencyProperty MaxWidthProperty = DependencyProperty.Register(nameof(MaxWidth), typeof(double), typeof(TreeViewColumn), new FrameworkPropertyMetadata(360.0));
        new public double MaxWidth
        {
            get => (double)GetValue(MaxWidthProperty);
            set => SetValue(MaxWidthProperty, value);
        }

        new public static readonly DependencyProperty MinWidthProperty = DependencyProperty.Register(nameof(MinWidth), typeof(double), typeof(TreeViewColumn), new FrameworkPropertyMetadata(32.0));
        new public double MinWidth
        {
            get => (double)GetValue(MinWidthProperty);
            set => SetValue(MinWidthProperty, value);
        }

        public static readonly DependencyProperty SortDirectionProperty = DependencyProperty.Register(nameof(SortDirection), typeof(ListSortDirection?), typeof(TreeViewColumn), new FrameworkPropertyMetadata(null));
        public ListSortDirection? SortDirection
        {
            get => (ListSortDirection?)GetValue(SortDirectionProperty);
            set => SetValue(SortDirectionProperty, value);
        }

        public static readonly DependencyProperty SortNameProperty = DependencyProperty.Register(nameof(SortName), typeof(object), typeof(TreeViewColumn), new FrameworkPropertyMetadata(null));
        public object SortName
        {
            get => GetValue(SortNameProperty);
            set => SetValue(SortNameProperty, value);
        }

        public static readonly DependencyProperty StringFormatProperty = DependencyProperty.Register(nameof(StringFormat), typeof(string), typeof(TreeViewColumn), new FrameworkPropertyMetadata(null));
        public string StringFormat
        {
            get => (string)GetValue(StringFormatProperty);
            set => SetValue(StringFormatProperty, value);
        }

        public static readonly DependencyProperty TemplateProperty = DependencyProperty.Register(nameof(Template), typeof(DataTemplate), typeof(TreeViewColumn), new FrameworkPropertyMetadata(null));
        public DataTemplate Template
        {
            get => (DataTemplate)GetValue(TemplateProperty);
            set => SetValue(TemplateProperty, value);
        }

        public static readonly DependencyProperty TemplateSelectorProperty = DependencyProperty.Register(nameof(TemplateSelector), typeof(DataTemplateSelector), typeof(TreeViewColumn), new FrameworkPropertyMetadata(null));
        public DataTemplateSelector TemplateSelector
        {
            get => (DataTemplateSelector)GetValue(TemplateSelectorProperty);
            set => SetValue(TemplateSelectorProperty, value);
        }

        public static readonly DependencyProperty VerticalContentAlignmentProperty = DependencyProperty.Register(nameof(VerticalContentAlignment), typeof(VerticalAlignment), typeof(TreeViewColumn), new FrameworkPropertyMetadata(VerticalAlignment.Center));
        public VerticalAlignment VerticalContentAlignment
        {
            get => (VerticalAlignment)GetValue(VerticalContentAlignmentProperty);
            set => SetValue(VerticalContentAlignmentProperty, value);
        }

        new public static readonly DependencyProperty WidthProperty = DependencyProperty.Register(nameof(Width), typeof(GridLength), typeof(TreeViewColumn), new FrameworkPropertyMetadata(new GridLength(64, GridUnitType.Pixel)));
        new public GridLength Width
        {
            get => (GridLength)GetValue(WidthProperty);
            set => SetValue(WidthProperty, value);
        }

        new public static readonly DependencyProperty VisibilityProperty = DependencyProperty.Register(nameof(Visibility), typeof(Visibility), typeof(TreeViewColumn), new FrameworkPropertyMetadata(Visibility.Visible, OnVisibilityChanged));
        new public Visibility Visibility
        {
            get => (Visibility)GetValue(VisibilityProperty);
            set => SetValue(VisibilityProperty, value);
        }
        static void OnVisibilityChanged(DependencyObject i, DependencyPropertyChangedEventArgs e) => i.As<TreeViewColumn>().OnVisibilityChanged(new Value<Visibility>(e));

        public TreeViewColumn() : base() => this.Bind(VisibilityProperty, new PropertyPath("(0)", XDependency.IsVisibleProperty), this, BindingMode.OneWay, Converters.BooleanToVisibilityConverter.Default);

        protected virtual void OnVisibilityChanged(Value<Visibility> input) => VisibilityChanged?.Invoke(this, EventArgs.Empty);
    }
}