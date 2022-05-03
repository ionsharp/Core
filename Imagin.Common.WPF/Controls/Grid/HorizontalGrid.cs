using Imagin.Common.Linq;
using System.Collections;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Imagin.Common.Controls
{
    public class HorizontalGrid : Grid
    {
        public static readonly DependencyProperty ItemContainerStyleProperty = DependencyProperty.Register(nameof(ItemContainerStyle), typeof(Style), typeof(HorizontalGrid), new FrameworkPropertyMetadata(null));
        public Style ItemContainerStyle
        {
            get => (Style)GetValue(ItemContainerStyleProperty);
            set => SetValue(ItemContainerStyleProperty, value);
        }

        public static readonly DependencyProperty ItemTemplateProperty = DependencyProperty.Register(nameof(ItemTemplate), typeof(DataTemplate), typeof(HorizontalGrid), new FrameworkPropertyMetadata(null));
        public DataTemplate ItemTemplate
        {
            get => (DataTemplate)GetValue(ItemTemplateProperty);
            set => SetValue(ItemTemplateProperty, value);
        }

        public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register(nameof(ItemsSource), typeof(IEnumerable), typeof(HorizontalGrid), new FrameworkPropertyMetadata(null, OnItemsSourceChanged));
        public IEnumerable ItemsSource
        {
            get => (IEnumerable)GetValue(ItemsSourceProperty);
            set => SetValue(ItemsSourceProperty, value);
        }
        static void OnItemsSourceChanged(DependencyObject i, DependencyPropertyChangedEventArgs e) => i.As<HorizontalGrid>().OnItemsSourceChanged(new Value<IEnumerable>(e));

        public static readonly DependencyProperty SplitterStyleProperty = DependencyProperty.Register(nameof(SplitterStyle), typeof(Style), typeof(HorizontalGrid), new FrameworkPropertyMetadata(null));
        public Style SplitterStyle
        {
            get => (Style)GetValue(SplitterStyleProperty);
            set => SetValue(SplitterStyleProperty, value);
        }

        public static readonly DependencyProperty SplitterVisibilityProperty = DependencyProperty.Register(nameof(SplitterVisibility), typeof(bool), typeof(HorizontalGrid), new FrameworkPropertyMetadata(true));
        public bool SplitterVisibility
        {
            get => (bool)GetValue(SplitterVisibilityProperty);
            set => SetValue(SplitterVisibilityProperty, value);
        }

        //...

        protected virtual void Clear()
        {
            ColumnDefinitions.Clear();
            Children.Clear();
        }

        //...

        protected virtual ContentPresenter OnContainerCreated(object input)
        {
            ContentPresenter result = new()
            {
                Content = input,
                ClipToBounds = true
            };
            result.Bind
                (ContentPresenter.ContentTemplateProperty,
                nameof(ItemTemplate), this);
            result.Bind
                (ContentPresenter.StyleProperty, 
                nameof(ItemContainerStyle), this);
            return result;
        }

        protected virtual ColumnDefinition OnDefinitionCreated(ContentPresenter child, int index) => new();

        protected virtual GridSplitter OnSplitterCreated()
        {
            GridSplitter result = new()
            {
                HorizontalAlignment 
                    = HorizontalAlignment.Right,
                ResizeBehavior 
                    = GridResizeBehavior.CurrentAndNext,
                VerticalAlignment 
                    = VerticalAlignment.Stretch
            };
            result.Bind
                (GridSplitter.StyleProperty, 
                nameof(SplitterStyle), this);
            result.Bind
                (GridSplitter.VisibilityProperty, 
                nameof(SplitterVisibility), 
                this, BindingMode.OneWay, 
                Converters.BooleanToVisibilityConverter.Default);
            return result;
        }

        //...

        protected virtual void OnItemsSourceChanged(Value<IEnumerable> input)
        {
            Clear();
            if (input.New != null)
            {
                var children = input.New.Cast<object>().Select(OnContainerCreated).ToArray();
                if (children.Count() > 0)
                {
                    for (int i = 0; ; i++)
                    {
                        var child = children[i];

                        SetColumn(child, i);
                        Children.Add(child);

                        ColumnDefinitions.Add(OnDefinitionCreated(child, i));

                        if (i == children.Length - 1)
                            break;

                        var gridSplitter = OnSplitterCreated();

                        SetColumn(gridSplitter, i);
                        Children.Add(gridSplitter);
                    }
                }
            }
        }
    }
}