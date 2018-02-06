using System.Windows;
using System.Windows.Controls;

namespace Imagin.Controls.Common.Linq
{
    /// <summary>
    /// 
    /// </summary>
    public static class GridExtensions
    {
        #region Columns

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty ColumnsProperty = DependencyProperty.RegisterAttached("Columns", typeof(int), typeof(GridExtensions), new PropertyMetadata(0, OnColumnsChanged));
        /// <summary>
        /// 
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static int GetColumns(FrameworkElement d)
        {
            return (int)d.GetValue(ColumnsProperty);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="d"></param>
        /// <param name="value"></param>
        public static void SetColumns(FrameworkElement d, int value)
        {
            d.SetValue(ColumnsProperty, value);
        }
        static void OnColumnsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var grid = d as Grid;
            var columns = (int)e.NewValue;

            grid.ColumnDefinitions.Clear();
            for (var i = 0; i < columns; i++)
            {
                grid.ColumnDefinitions.Add(new ColumnDefinition()
                {
                    Width = new GridLength(1.0, GridUnitType.Star)
                });
            }
        }

        #endregion

        #region Rows

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty RowsProperty = DependencyProperty.RegisterAttached("Rows", typeof(int), typeof(GridExtensions), new PropertyMetadata(0, OnRowsChanged));
        /// <summary>
        /// 
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static int GetRows(FrameworkElement d)
        {
            return (int)d.GetValue(RowsProperty);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="d"></param>
        /// <param name="value"></param>
        public static void SetRows(FrameworkElement d, int value)
        {
            d.SetValue(RowsProperty, value);
        }
        static void OnRowsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var grid = d as Grid;
            var rows = (int)e.NewValue;

            grid.RowDefinitions.Clear();
            for (var i = 0; i < rows; i++)
            {
                grid.RowDefinitions.Add(new RowDefinition()
                {
                    Height = new GridLength(1.0, GridUnitType.Star)
                });
            }
        }

        #endregion
    }
}
