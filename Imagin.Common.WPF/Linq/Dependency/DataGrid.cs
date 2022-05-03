using Imagin.Common.Controls;
using Imagin.Common.Linq;
using Imagin.Common.Text;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace Imagin.Common.Linq
{
    [Extends(typeof(DataGrid))]
    public static class XDataGrid
    {
        #region CanSelectColumns

        public static readonly DependencyProperty CanSelectColumnsProperty = DependencyProperty.RegisterAttached("CanSelectColumns", typeof(bool), typeof(XDataGrid), new FrameworkPropertyMetadata(false, OnCanSelectColumnsChanged));
        public static bool GetCanSelectColumns(DataGrid i) => (bool)i.GetValue(CanSelectColumnsProperty);
        public static void SetCanSelectColumns(DataGrid i, bool input) => i.SetValue(CanSelectColumnsProperty, input);
        static void OnCanSelectColumnsChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is DataGrid dataGrid)
                dataGrid.RegisterHandlerAttached((bool)e.NewValue, CanSelectColumnsProperty, i => i.PreviewMouseDown += CanSelectColumns_PreviewMouseDown, i => i.PreviewMouseDown -= CanSelectColumns_PreviewMouseDown);
        }

        static void CanSelectColumns_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is DataGrid grid)
            {
                if (e.OriginalSource?.FindParent<DataGridColumnHeader>() is DataGridColumnHeader header)
                {
                    grid.SelectedCells.Clear();
                    foreach (var item in grid.Items)
                        grid.SelectedCells.Add(new DataGridCellInfo(item, header.Column));
                }
            }
        }

        #endregion

        #region DisplayRowNumber

        public static readonly DependencyProperty DisplayRowNumberProperty = DependencyProperty.RegisterAttached("DisplayRowNumber", typeof(bool), typeof(XDataGrid), new FrameworkPropertyMetadata(false, OnDisplayRowNumberChanged));
        public static bool GetDisplayRowNumber(DataGrid i) => (bool)i.GetValue(DisplayRowNumberProperty);
        public static void SetDisplayRowNumber(DataGrid i, bool input) => i.SetValue(DisplayRowNumberProperty, input);
        static void OnDisplayRowNumberChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is DataGrid dataGrid)
            {
                dataGrid.RegisterHandlerAttached((bool)e.NewValue, DisplayRowNumberProperty, i =>
                {
                    i.LayoutUpdated
                        += DisplayRowNumber_OnLayoutUpdated;
                    i.LoadingRow
                        += DisplayRowNumber_OnLoadingRow;
                    i.SelectionChanged
                        += DisplayRowNumber_OnSelectionChanged;
                    i.SizeChanged
                        += DisplayRowNumber_OnSizeChanged;
                    i.UnloadingRow
                        += DisplayRowNumber_OnLoadingRow;

                    UpdateAllRowNumbers(i);
                }, i =>
                {
                    i.LayoutUpdated
                        -= DisplayRowNumber_OnLayoutUpdated;
                    i.LoadingRow
                        -= DisplayRowNumber_OnLoadingRow;
                    i.SelectionChanged
                        -= DisplayRowNumber_OnSelectionChanged;
                    i.SizeChanged
                        -= DisplayRowNumber_OnSizeChanged;
                    i.UnloadingRow
                        -= DisplayRowNumber_OnLoadingRow;

                    i.FindVisualChildren<DataGridRow>().ToList().ForEach(i => i.Header = null);
                });
            }
        }

        //...

        static void UpdateAllRowNumbers(DataGrid dataGrid) => dataGrid.FindVisualChildren<DataGridRow>().ForEach(i => UpdateRowNumber(dataGrid, i));

        static void UpdateRowNumber(DataGrid grid, DataGridRow dataGridRow)
        {
            var label = dataGridRow.Header as BulletElement ?? new();
            label.Bullet
                = GetDisplayRowNumberBullet(grid);
            label.Value
                = dataGridRow.GetIndex() + GetDisplayRowNumberOffset(grid);

            dataGridRow.Header = label;
        }

        //...

        static void DisplayRowNumber_OnSelectionChanged(object sender, EventArgs e) 
            => UpdateAllRowNumbers((DataGrid)sender);

        static void DisplayRowNumber_OnLayoutUpdated(object sender, EventArgs e) 
            => UpdateAllRowNumbers((DataGrid)sender);

        static void DisplayRowNumber_OnSizeChanged(object sender, SizeChangedEventArgs e) 
            => UpdateAllRowNumbers((DataGrid)sender);

        static void DisplayRowNumber_OnLoadingRow(object sender, DataGridRowEventArgs e) 
            => UpdateAllRowNumbers((DataGrid)sender);

        #endregion

        #region DisplayRowNumberBullet

        public static readonly DependencyProperty DisplayRowNumberBulletProperty = DependencyProperty.RegisterAttached("DisplayRowNumberBullet", typeof(Bullets), typeof(XDataGrid), new FrameworkPropertyMetadata(Bullets.NumberPeriod, OnDisplayRowNumberBulletChanged));
        public static Bullets GetDisplayRowNumberBullet(DependencyObject i) => (Bullets)i.GetValue(DisplayRowNumberBulletProperty);
        public static void SetDisplayRowNumberBullet(DependencyObject i, Bullets input) => i.SetValue(DisplayRowNumberBulletProperty, input);
        static void OnDisplayRowNumberBulletChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is DataGrid dataGrid)
            {
                if (GetDisplayRowNumber(dataGrid))
                    UpdateAllRowNumbers(dataGrid);
            }
        }

        #endregion

        #region DisplayRowNumberOffset

        public static readonly DependencyProperty DisplayRowNumberOffsetProperty = DependencyProperty.RegisterAttached("DisplayRowNumberOffset", typeof(int), typeof(XDataGrid), new FrameworkPropertyMetadata(0, OnDisplayRowNumberOffsetChanged));
        public static int GetDisplayRowNumberOffset(DependencyObject i) => (int)i.GetValue(DisplayRowNumberOffsetProperty);
        public static void SetDisplayRowNumberOffset(DependencyObject i, int input) => i.SetValue(DisplayRowNumberOffsetProperty, input);
        static void OnDisplayRowNumberOffsetChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is DataGrid dataGrid)
            {
                if (GetDisplayRowNumber(dataGrid))
                    UpdateAllRowNumbers(dataGrid);
            }
        }

        #endregion

        #region ScrollAddedIntoView

        public static readonly DependencyProperty ScrollAddedIntoViewProperty = DependencyProperty.RegisterAttached("ScrollAddedIntoView", typeof(bool), typeof(XDataGrid), new FrameworkPropertyMetadata(false, OnScrollAddedIntoViewChanged));
        public static bool GetScrollAddedIntoView(DataGrid i) => (bool)i.GetValue(ScrollAddedIntoViewProperty);
        public static void SetScrollAddedIntoView(DataGrid i, bool input) => i.SetValue(ScrollAddedIntoViewProperty, input);
        static void OnScrollAddedIntoViewChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is DataGrid dataGrid)
                dataGrid.RegisterHandlerAttached((bool)e.NewValue, ScrollAddedIntoViewProperty, i => i.LoadingRow += ScrollAddedIntoView_LoadingRow, i => i.LoadingRow -= ScrollAddedIntoView_LoadingRow);
        }

        static void ScrollAddedIntoView_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            if (sender is DataGrid dataGrid)
                dataGrid.ScrollIntoView(e.Row.Item);
        }

        #endregion
    }
}