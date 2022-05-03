using Imagin.Common.Converters;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace Imagin.Common.Linq
{
    [Extends(typeof(Grid))]
    public static class XGrid
    {
        #region Properties

        #region AutoColumns

        public static readonly DependencyProperty AutoColumnsProperty = DependencyProperty.RegisterAttached("AutoColumns", typeof(bool), typeof(XGrid), new FrameworkPropertyMetadata(false, OnAutoColumnsChanged));
        public static bool GetAutoColumns(Grid i) => (bool)i.GetValue(AutoColumnsProperty);
        public static void SetAutoColumns(Grid i, bool input) => i.SetValue(AutoColumnsProperty, input);
        static void OnAutoColumnsChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is Grid grid)
            {
                if ((bool)e.NewValue)
                {
                    var j = 0;
                    foreach (UIElement i in grid.Children)
                    {
                        Grid.SetColumn(i, j);
                        j++;
                    }
                }
            }
        }

        #endregion

        #region AutoRows

        public static readonly DependencyProperty AutoRowsProperty = DependencyProperty.RegisterAttached("AutoRows", typeof(bool), typeof(XGrid), new FrameworkPropertyMetadata(false, OnAutoRowsChanged));
        public static bool GetAutoRows(Grid i) => (bool)i.GetValue(AutoRowsProperty);
        public static void SetAutoRows(Grid i, bool input) => i.SetValue(AutoRowsProperty, input);
        static void OnAutoRowsChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is Grid grid)
            {
                if ((bool)e.NewValue)
                {
                    var j = 0;
                    foreach (UIElement i in grid.Children)
                    {
                        Grid.SetRow(i, j);
                        j++;
                    }
                }
            }
        }

        #endregion

        #region Columns

        public static readonly DependencyProperty ColumnsProperty = DependencyProperty.RegisterAttached("Columns", typeof(GridLength[]), typeof(XGrid), new FrameworkPropertyMetadata(null, OnColumnsChanged));
        [TypeConverter(typeof(GridLengthArrayTypeConverter))]
        public static GridLength[] GetColumns(Grid i) => (GridLength[])i.GetValue(ColumnsProperty);
        public static void SetColumns(Grid i, GridLength[] input) => i.SetValue(ColumnsProperty, input);
        static void OnColumnsChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is Grid grid)
            {
                var columns = (GridLength[])e.NewValue;

                grid.ColumnDefinitions.Clear();
                if (e.NewValue != null)
                {
                    for (var i = 0; i < columns.Length; i++)
                    {
                        var columnDefinition = new ColumnDefinition
                        {
                            Width = columns[i]
                        };
                        grid.ColumnDefinitions.Add(columnDefinition);
                    }
                }
            }
        }

        #endregion

        #region Rows

        public static readonly DependencyProperty RowsProperty = DependencyProperty.RegisterAttached("Rows", typeof(GridLength[]), typeof(XGrid), new FrameworkPropertyMetadata(null, OnRowsChanged));
        [TypeConverter(typeof(GridLengthArrayTypeConverter))]
        public static GridLength[] GetRows(Grid i) => (GridLength[])i.GetValue(RowsProperty);
        public static void SetRows(Grid i, GridLength[] input) => i.SetValue(RowsProperty, input);
        static void OnRowsChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is Grid grid)
            {
                var rows = (GridLength[])e.NewValue;

                grid.RowDefinitions.Clear();
                if (e.NewValue != null)
                {
                    for (var i = 0; i < rows.Length; i++)
                    {
                        var rowDefinition = new RowDefinition
                        {
                            Height = rows[i]
                        };
                        grid.RowDefinitions.Add(rowDefinition);
                    }
                }
            }
        }

        #endregion

        #region Methods

        public static void SetColumnsBy(this Grid input, params GridUnitType[] columns) => columns.ForEach(i => input.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, i) }));

        public static void SetRowsBy(this Grid input, params GridUnitType[] rows) => rows.ForEach(i => input.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, i) }));

        #endregion

        #endregion

        #region XGrid

        static XGrid()
        {
            EventManager.RegisterClassHandler(typeof(Grid), Grid.SizeChangedEvent,
                new SizeChangedEventHandler(OnSizeChanged), true);
        }

        static void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (sender is Grid grid)
            {
                if (GetAutoColumns(grid))
                    OnAutoColumnsChanged(grid, new(AutoColumnsProperty, true, true));

                if (GetAutoRows(grid))
                    OnAutoRowsChanged(grid, new(AutoRowsProperty, true, true));
            }
        }

        #endregion
    }
}