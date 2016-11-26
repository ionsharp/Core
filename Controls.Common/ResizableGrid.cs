using System;
using System.Collections;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Imagin.Controls.Common
{
    public class ResizableGrid : Grid
    {
        #region Properties

        public static readonly DependencyProperty ShowSplitterProperty = DependencyProperty.Register("ShowSplitter", typeof(bool), typeof(ResizableGrid), new PropertyMetadata(true, OnShowSplitterChanged));
        public bool ShowSplitter
        {
            get
            {
                return (bool)GetValue(ShowSplitterProperty);
            }
            set
            {
                SetValue(ShowSplitterProperty, value);
            }
        }
        static void OnShowSplitterChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
        }

        public static readonly DependencyProperty SplitterWidthProperty = DependencyProperty.Register("SplitterWidth", typeof(double), typeof(ResizableGrid), new PropertyMetadata(3.0, OnSplitterWidthChanged));
        public double SplitterWidth
        {
            get
            {
                return (double)GetValue(SplitterWidthProperty);
            }
            set
            {
                SetValue(SplitterWidthProperty, value);
            }
        }
        static void OnSplitterWidthChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            foreach (var i in ((ResizableGrid)d).Children.OfType<GridSplitter>())
                i.Width = (double)e.NewValue;
        }

        public static readonly DependencyProperty ItemTemplateProperty = DependencyProperty.Register("ItemTemplate", typeof(DataTemplate), typeof(ResizableGrid), new PropertyMetadata(default(DataTemplate), OnItemTemplateChanged));
        public DataTemplate ItemTemplate
        {
            get
            {
                return (DataTemplate)GetValue(ItemTemplateProperty);
            }
            set
            {
                SetValue(ItemTemplateProperty, value);
            }
        }
        static void OnItemTemplateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            foreach (var child in ((ResizableGrid)d).Children.OfType<ContentPresenter>())
                child.ContentTemplate = (DataTemplate)e.NewValue;
        }

        public static readonly DependencyProperty ItemContainerStyleProperty = DependencyProperty.Register("ItemContainerStyle", typeof(Style), typeof(ResizableGrid), new PropertyMetadata(default(Style), OnContainerStyleChanged));
        public Style ItemContainerStyle
        {
            get
            {
                return (Style)GetValue(ItemContainerStyleProperty);
            }
            set
            {
                SetValue(ItemContainerStyleProperty, value);
            }
        }
        static void OnContainerStyleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            foreach (var Child in ((ResizableGrid)d).Children.OfType<ContentPresenter>())
                Child.Style = (Style)e.NewValue;
        }

        public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register("ItemsSource", typeof(IEnumerable), typeof(ResizableGrid), new PropertyMetadata(null, OnItemsSourceChanged));
        public IEnumerable ItemsSource
        {
            get
            {
                return (IEnumerable)GetValue(ItemsSourceProperty);
            }
            set
            {
                SetValue(ItemsSourceProperty, value);
            }
        }
        static void OnItemsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ResizableGrid = (ResizableGrid)d;

            ResizableGrid.Children.Clear();
            ResizableGrid.ColumnDefinitions.Clear();

            var Items = e.NewValue as IEnumerable;
            if (Items == null) return;

            var Children = Items.Cast<object>().Select(ResizableGrid.GenerateContainer).ToArray();
            if (Children.Count() == 0) return;

            Application.Current.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Background, new Action(() =>
            {
                for (int i = 0; ; i++)
                {
                    var Child = Children[i];
                    var Column = ResizableGrid.GetColumnDefinition(Child, i);

                    Child.ClipToBounds = true;

                    ResizableGrid.ColumnDefinitions.Add(Column);
                    ResizableGrid.Children.Add(Child);

                    SetColumn(Child, i);

                    if (i == Children.Length - 1) break;

                    var Splitter = new GridSplitter
                    {
                        Width = ResizableGrid.SplitterWidth,
                        ResizeBehavior = GridResizeBehavior.CurrentAndNext,
                        VerticalAlignment = VerticalAlignment.Stretch,
                        HorizontalAlignment = HorizontalAlignment.Right
                    };
                    if (!ResizableGrid.ShowSplitter)
                        Splitter.Background = Brushes.Transparent;

                    SetColumn(Splitter, i);
                    ResizableGrid.Children.Add(Splitter);
                }
            }));
        }

        public static readonly DependencyProperty ColumnWidthProperty = DependencyProperty.RegisterAttached("ColumnWidth", typeof(GridLength), typeof(ResizableGrid), new PropertyMetadata(new GridLength(1, GridUnitType.Star), OnColumnWidthChanged));
        public static void SetColumnWidth(DependencyObject Object, GridLength Value)
        {
            Object.SetValue(ColumnWidthProperty, Value);
        }
        public static GridLength GetColumnWidth(DependencyObject Object)
        {
            return (GridLength)Object.GetValue(ColumnWidthProperty);
        }
        static void OnColumnWidthChanged(DependencyObject Object, DependencyPropertyChangedEventArgs e)
        {
            UpdateColumnDefinition(Object, Column => Column.Width = (GridLength)e.NewValue);
        }

        public static readonly DependencyProperty MinColumnWidthProperty = DependencyProperty.RegisterAttached("MinColumnWidth", typeof(double), typeof(ResizableGrid), new PropertyMetadata(100d, OnMinColumnWidthChanged));
        public static void SetMinColumnWidth(DependencyObject Object, double Value)
        {
            Object.SetValue(MinColumnWidthProperty, Value);
        }
        public static double GetMinColumnWidth(DependencyObject Object)
        {
            return (double)Object.GetValue(MinColumnWidthProperty);
        }
        static void OnMinColumnWidthChanged(DependencyObject Object, DependencyPropertyChangedEventArgs e)
        {
            UpdateColumnDefinition(Object, Column => Column.MinWidth = (double)e.NewValue);
        }

        public static readonly DependencyProperty MaxColumnWidthProperty = DependencyProperty.RegisterAttached("MaxColumnWidth", typeof(double), typeof(ResizableGrid), new PropertyMetadata(double.MaxValue, OnMaxColumnWidthChanged));
        public static void SetMaxColumnWidth(DependencyObject Object, double Value)
        {
            Object.SetValue(MaxColumnWidthProperty, Value);
        }
        public static double GetMaxColumnWidth(DependencyObject Object)
        {
            return (double)Object.GetValue(MaxColumnWidthProperty);
        }
        static void OnMaxColumnWidthChanged(DependencyObject Object, DependencyPropertyChangedEventArgs e)
        {
            UpdateColumnDefinition(Object, column => column.MaxWidth = (double)e.NewValue);
        }

        #endregion

        #region Methods

        ContentPresenter GenerateContainer(object Item)
        {
            return new ContentPresenter
            {
                Content = Item,
                ContentTemplate = ItemTemplate
            };
        }

        static void UpdateColumnDefinition(DependencyObject Child, Action<ColumnDefinition> UpdateAction)
        {
            var Grid = VisualTreeHelper.GetParent(Child) as ResizableGrid;
            if (Grid != null)
            {
                var Column = GetColumn((UIElement)Child);
                if (Column < Grid.ColumnDefinitions.Count)
                    Grid.Dispatcher.BeginInvoke(new Action(() => UpdateAction(Grid.ColumnDefinitions[Column])));
            }
        }

        protected virtual ColumnDefinition GetColumnDefinition(ContentPresenter Child, int Index)
        {
            return new ColumnDefinition
            {
                MaxWidth = GetMaxColumnWidth(Child),
                MinWidth = GetMinColumnWidth(Child),
                Width = GetColumnWidth(Child),
            };
        }

        #endregion
    }
}
