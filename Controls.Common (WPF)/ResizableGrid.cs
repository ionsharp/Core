using System;
using System.Collections;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Imagin.Common.Linq;

namespace Imagin.Controls.Common
{
    public class ResizableGrid : Grid
    {
        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty ItemContainerStyleProperty = DependencyProperty.Register("ItemContainerStyle", typeof(Style), typeof(ResizableGrid), new PropertyMetadata(default(Style), OnItemContainerStyleChanged));
        /// <summary>
        /// 
        /// </summary>
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
        static void OnItemContainerStyleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            d.As<ResizableGrid>().OnItemContainerStyleChanged((Style)e.NewValue);
        }

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty ItemTemplateProperty = DependencyProperty.Register("ItemTemplate", typeof(DataTemplate), typeof(ResizableGrid), new PropertyMetadata(default(DataTemplate), OnItemTemplateChanged));
        /// <summary>
        /// 
        /// </summary>
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
            d.As<ResizableGrid>().OnItemTemplateChanged((DataTemplate)e.NewValue);
        }

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register("ItemsSource", typeof(IEnumerable), typeof(ResizableGrid), new PropertyMetadata(null, OnItemsSourceChanged));
        /// <summary>
        /// 
        /// </summary>
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
            d.As<ResizableGrid>().OnItemsSourceChange((IEnumerable)e.NewValue);
        }

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty ColumnWidthProperty = DependencyProperty.RegisterAttached("ColumnWidth", typeof(GridLength), typeof(ResizableGrid), new PropertyMetadata(new GridLength(1, GridUnitType.Star), OnColumnWidthChanged));
        /// <summary>
        /// 
        /// </summary>
        public static void SetColumnWidth(DependencyObject Object, GridLength Value)
        {
            Object.SetValue(ColumnWidthProperty, Value);
        }
        /// <summary>
        /// 
        /// </summary>
        public static GridLength GetColumnWidth(DependencyObject Object)
        {
            return (GridLength)Object.GetValue(ColumnWidthProperty);
        }
        static void OnColumnWidthChanged(DependencyObject Object, DependencyPropertyChangedEventArgs e)
        {
            UpdateColumnDefinition(Object, Column => Column.Width = (GridLength)e.NewValue);
        }

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty MinColumnWidthProperty = DependencyProperty.RegisterAttached("MinColumnWidth", typeof(double), typeof(ResizableGrid), new PropertyMetadata(100d, OnMinColumnWidthChanged));
        /// <summary>
        /// 
        /// </summary>
        public static void SetMinColumnWidth(DependencyObject Object, double Value)
        {
            Object.SetValue(MinColumnWidthProperty, Value);
        }
        /// <summary>
        /// 
        /// </summary>
        public static double GetMinColumnWidth(DependencyObject Object)
        {
            return (double)Object.GetValue(MinColumnWidthProperty);
        }
        static void OnMinColumnWidthChanged(DependencyObject Object, DependencyPropertyChangedEventArgs e)
        {
            UpdateColumnDefinition(Object, Column => Column.MinWidth = (double)e.NewValue);
        }

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty MaxColumnWidthProperty = DependencyProperty.RegisterAttached("MaxColumnWidth", typeof(double), typeof(ResizableGrid), new PropertyMetadata(double.MaxValue, OnMaxColumnWidthChanged));
        /// <summary>
        /// 
        /// </summary>
        public static void SetMaxColumnWidth(DependencyObject Object, double Value)
        {
            Object.SetValue(MaxColumnWidthProperty, Value);
        }
        /// <summary>
        /// 
        /// </summary>
        public static double GetMaxColumnWidth(DependencyObject Object)
        {
            return (double)Object.GetValue(MaxColumnWidthProperty);
        }
        static void OnMaxColumnWidthChanged(DependencyObject Object, DependencyPropertyChangedEventArgs e)
        {
            UpdateColumnDefinition(Object, column => column.MaxWidth = (double)e.NewValue);
        }

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty ShowSplitterProperty = DependencyProperty.Register("ShowSplitter", typeof(bool), typeof(ResizableGrid), new PropertyMetadata(true));
        /// <summary>
        /// 
        /// </summary>
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

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty SplitterWidthProperty = DependencyProperty.Register("SplitterWidth", typeof(double), typeof(ResizableGrid), new PropertyMetadata(3.0, OnSplitterWidthChanged));
        /// <summary>
        /// 
        /// </summary>
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
            d.As<ResizableGrid>().OnSplitterWidthChanged((double)e.NewValue);
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

        /// <summary>
        /// 
        /// </summary>
        protected virtual ColumnDefinition GetColumnDefinition(ContentPresenter Child, int Index)
        {
            return new ColumnDefinition
            {
                MaxWidth = GetMaxColumnWidth(Child),
                MinWidth = GetMinColumnWidth(Child),
                Width = GetColumnWidth(Child),
            };
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void OnItemContainerStyleChanged(Style Value)
        {
            Children.OfType<ContentPresenter>().ForEach(i => i.Style = Value);
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void OnItemsSourceChange(IEnumerable Value)
        {
            Children.Clear();
            ColumnDefinitions.Clear();

            if (Value != null)
            {
                var Columns = Value.Cast<object>().Select(GenerateContainer).ToArray();
                if (Columns.Count() > 0)
                {
                    Application.Current.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Background, new Action(() =>
                    {
                        for (int i = 0; ; i++)
                        {
                            var Child = Columns[i];
                            Child.ClipToBounds = true;
                            SetColumn(Child, i);
                            Children.Add(Child);

                            ColumnDefinitions.Add(GetColumnDefinition(Child, i));

                            if (i == Columns.Length - 1) break;

                            var s = new GridSplitter
                            {
                                HorizontalAlignment = HorizontalAlignment.Right,
                                ResizeBehavior = GridResizeBehavior.CurrentAndNext,
                                VerticalAlignment = VerticalAlignment.Stretch,
                                Visibility = ShowSplitter.ToVisibility(),
                                Width = SplitterWidth,
                            };

                            SetColumn(s, i);
                            Children.Add(s);
                        }
                    }));
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void OnItemTemplateChanged(DataTemplate Value)
        {
            Children.OfType<ContentPresenter>().ForEach(i => i.ContentTemplate = Value);
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void OnSplitterWidthChanged(double Value)
        {
            Children.OfType<GridSplitter>().ForEach(i => i.Width = Value);
        }

        #endregion
    }
}
