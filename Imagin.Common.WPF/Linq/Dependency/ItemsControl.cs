using Imagin.Common.Analytics;
using Imagin.Common.Collections.Generic;
using Imagin.Common.Controls;
using Imagin.Common.Converters;
using Imagin.Common.Input;
using Imagin.Common.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Threading;

namespace Imagin.Common.Linq
{
    [Extends(typeof(ItemsControl))]
    public static class XItemsControl
    {
        public static readonly ResourceKey<GroupItem> DefaultGroupStyleKey = new();

        public static readonly ResourceKey<GroupItem> MenuGroupStyleKey = new();
        
        //...

        public static readonly ResourceKey EmptyHorizontalTemplateKey = new();

        public static readonly ResourceKey EmptyVerticalTemplateKey = new();

        #region Properties

        #region AddCommand

        public static readonly RoutedUICommand AddCommand = new("AddCommand", "AddCommand", typeof(XItemsControl));
        static void OnAddCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (sender is ItemsControl control)
            {
                if (control.ItemsSource is IList list)
                {
                    Type result = default;
                    foreach (var i in list.GetType().Inheritance())
                    {
                        var j = i.GetGenericArguments();
                        if (j.Length == 1)
                        {
                            result = j[0];
                            break;
                        }
                    }

                    if (result != null)
                    {
                        object instance = null;
                        Try.Invoke(() => instance = result.Create<object>());
                        if (instance != null)
                            list.Add(instance);
                    }
                }
            }
        }
        static void OnAddCommandCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (sender is DataGrid dataGrid)
                e.CanExecute = dataGrid.CanUserAddRows;

            e.CanExecute = true;
        }

        #endregion

        #region AutoSizeColumns

        /// <summary>
        /// Applies GridUnit.Star GridLength to all columns.
        /// </summary>
        public static readonly DependencyProperty AutoSizeColumnsProperty = DependencyProperty.RegisterAttached("AutoSizeColumns", typeof(bool), typeof(XItemsControl), new FrameworkPropertyMetadata(false, OnAutoSizeColumnsChanged));
        public static bool GetAutoSizeColumns(ItemsControl i) => (bool)i.GetValue(AutoSizeColumnsProperty);
        public static void SetAutoSizeColumns(ItemsControl i, bool input) => i.SetValue(AutoSizeColumnsProperty, input);
        static void OnAutoSizeColumnsChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is not DataGrid && sender is not ListView && sender is not TreeView)
                throw new NotSupportedException();

            if (sender is DataGrid dataGrid)
            {
                var l = (bool)e.NewValue ? new DataGridLength(1.0, DataGridLengthUnitType.Star) : new DataGridLength(1.0, DataGridLengthUnitType.Auto);
                dataGrid.Columns.ForEach(i => i.Width = l);
            }
            if (sender is ListView listView)
            {
                listView.RegisterHandlerAttached(true, AutoSizeColumnsProperty, i =>
                {
                    UpdateColumnWidth(listView);
                    listView.SizeChanged 
                        += AutoSizeColumns_SizeChanged;
                }, i =>
                {
                    listView.SizeChanged 
                        -= AutoSizeColumns_SizeChanged;
                });
            }
            else if (sender is TreeView treeView)
            {
                var l = (bool)e.NewValue ? new GridLength(1.0, GridUnitType.Star) : new GridLength(1.0, GridUnitType.Auto);
                XTreeView.GetColumns(treeView)?.ForEach(i => i.Width = l);
            }
        }

        static void AutoSizeColumns_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (sender is ListView listView)
            {
                if (listView.IsLoaded)
                    UpdateColumnWidth(listView);
            }
        }

        //...

        static void UpdateColumnWidth(ListView listView)
        {
            //Pull the stretch columns fromt the tag property.
            List<GridViewColumn> columns = (listView.Tag as List<GridViewColumn>);
            double specifiedWidth = 0;

            if (listView.View is GridView gridView)
            {
                if (columns == null)
                {
                    //Instance if its our first run.
                    columns = new List<GridViewColumn>();
                    // Get all columns with no width having been set.
                    foreach (GridViewColumn column in gridView.Columns)
                    {
                        if (!(column.Width >= 0))
                        {
                            columns.Add(column);
                        }
                        else
                        {
                            specifiedWidth += column.ActualWidth;
                        }
                    }
                }
                else
                {
                    // Get all columns with no width having been set.
                    foreach (GridViewColumn column in gridView.Columns)
                    {
                        if (!columns.Contains(column))
                        {
                            specifiedWidth += column.ActualWidth;
                        }
                    }
                }

                // Allocate remaining space equally.
                foreach (GridViewColumn column in columns)
                {
                    double newWidth = (listView.ActualWidth - specifiedWidth) / columns.Count;
                    if (newWidth >= 10)
                    {
                        column.Width = newWidth - 10;
                    }
                }

                //Store the columns in the TAG property for later use.
                listView.Tag = columns;
            }
        }

        #endregion

        #region CanDragSelect

        /// <summary>
        /// Gets or sets value indicating whether ItemsControl should allow drag selection.
        /// </summary>
        public static readonly DependencyProperty CanDragSelectProperty = DependencyProperty.RegisterAttached("CanDragSelect", typeof(bool), typeof(XItemsControl), new FrameworkPropertyMetadata(false));
        public static bool GetCanDragSelect(DependencyObject i) => (bool)i.GetValue(CanDragSelectProperty);
        public static void SetCanDragSelect(DependencyObject i, bool input) => i.SetValue(CanDragSelectProperty, input);

        #endregion

        #region ColumnLength

        /// <summary>
        /// This is incomplete...
        /// </summary>
        public static readonly DependencyProperty ColumnLengthProperty = DependencyProperty.RegisterAttached("ColumnLength", typeof(ControlLengthList), typeof(XItemsControl), new FrameworkPropertyMetadata(null, OnColumnLengthChanged));
        public static ControlLengthList GetColumnLength(ItemsControl i) => (ControlLengthList)i.GetValue(ColumnLengthProperty);
        public static void SetColumnLength(ItemsControl i, ControlLengthList input) => i.SetValue(ColumnLengthProperty, input);
        static void OnColumnLengthChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is ItemsControl control)
                ApplyColumnLength(control);
        }

        #endregion

        #region ColumnMenu

        public static readonly DependencyProperty ColumnMenuProperty = DependencyProperty.RegisterAttached("ColumnMenu", typeof(ContextMenu), typeof(XItemsControl), new FrameworkPropertyMetadata(null));
        public static ContextMenu GetColumnMenu(ItemsControl i) => (ContextMenu)i.GetValue(ColumnMenuProperty);
        public static void SetColumnMenu(ItemsControl i, ContextMenu input) => i.SetValue(ColumnMenuProperty, input);

        #endregion

        #region ColumnMenuDefaultHeader

        public static readonly DependencyProperty ColumnMenuDefaultHeaderProperty = DependencyProperty.RegisterAttached("ColumnMenuDefaultHeader", typeof(string), typeof(XItemsControl), new FrameworkPropertyMetadata("Column ({0})"));
        public static string GetColumnMenuDefaultHeader(ItemsControl i) => (string)i.GetValue(ColumnMenuDefaultHeaderProperty);
        public static void SetColumnMenuDefaultHeader(ItemsControl i, string input) => i.SetValue(ColumnMenuDefaultHeaderProperty, input);

        #endregion

        #region (private) ColumnMenuItemIndex

        static readonly DependencyProperty ColumnMenuItemIndexProperty = DependencyProperty.RegisterAttached("ColumnMenuItemIndex", typeof(int), typeof(XItemsControl), new FrameworkPropertyMetadata(-1));
        static int GetColumnMenuItemIndex(MenuItem i) => (int)i.GetValue(ColumnMenuItemIndexProperty);
        static void SetColumnMenuItemIndex(MenuItem i, int input) => i.SetValue(ColumnMenuItemIndexProperty, input);

        #endregion

        #region (private) ColumnMenuItemParent

        static readonly DependencyProperty ColumnMenuItemParentProperty = DependencyProperty.RegisterAttached("ColumnMenuItemParent", typeof(ItemsControl), typeof(XItemsControl), new FrameworkPropertyMetadata(null));
        static ItemsControl GetColumnMenuItemParent(MenuItem i) => (ItemsControl)i.GetValue(ColumnMenuItemParentProperty);
        static void SetColumnMenuItemParent(MenuItem i, ItemsControl input) => i.SetValue(ColumnMenuItemParentProperty, input);

        #endregion

        #region ColumnVisibility

        public static readonly DependencyProperty ColumnVisibilityProperty = DependencyProperty.RegisterAttached("ColumnVisibility", typeof(BooleanList), typeof(XItemsControl), new FrameworkPropertyMetadata(null, OnColumnVisibilityChanged));
        public static BooleanList GetColumnVisibility(ItemsControl i) => (BooleanList)i.GetValue(ColumnVisibilityProperty);
        public static void SetColumnVisibility(ItemsControl i, BooleanList input) => i.SetValue(ColumnVisibilityProperty, input);
        static void OnColumnVisibilityChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is ItemsControl control)
                ApplyColumnVisibility(control);
        }

        #endregion

        #region (readonly) FrameworkElement > ContainerIndex

        static readonly DependencyPropertyKey ContainerIndexKey = DependencyProperty.RegisterAttachedReadOnly("ContainerIndex", typeof(int), typeof(XItemsControl), new FrameworkPropertyMetadata(-1));
        public static readonly DependencyProperty ContainerIndexProperty = ContainerIndexKey.DependencyProperty;
        public static int GetContainerIndex(FrameworkElement i) => (int)i.GetValue(ContainerIndexProperty);
        static void SetContainerIndex(FrameworkElement i, int input) => i.SetValue(ContainerIndexKey, input);

        #endregion

        #region ContainerIndexOrigin

        public static readonly DependencyProperty ContainerIndexOriginProperty = DependencyProperty.RegisterAttached("ContainerIndexOrigin", typeof(int), typeof(XItemsControl), new FrameworkPropertyMetadata(0));
        public static int GetContainerIndexOrigin(ItemsControl i) => (int)i.GetValue(ContainerIndexOriginProperty);
        public static void SetContainerIndexOrigin(ItemsControl i, int input) => i.SetValue(ContainerIndexOriginProperty, input);

        #endregion

        #region ContainerTracking

        public static readonly DependencyProperty ContainerTrackingProperty = DependencyProperty.RegisterAttached("ContainerTracking", typeof(bool), typeof(XItemsControl), new FrameworkPropertyMetadata(false));
        public static bool GetContainerTracking(ItemsControl i) => (bool)i.GetValue(ContainerTrackingProperty);
        public static void SetContainerTracking(ItemsControl i, bool input) => i.SetValue(ContainerTrackingProperty, input);

        #endregion

        #region (private) Count

        static readonly DependencyProperty CountProperty = DependencyProperty.RegisterAttached("Count", typeof(int), typeof(XItemsControl), new FrameworkPropertyMetadata(0, OnCountChanged));
        static void OnCountChanged(object sender, DependencyPropertyChangedEventArgs e) => sender.If<ItemsControl>(i => OnCountChanged(i, (Value<int>)e));

        #endregion

        #region DragScrollOffset

        public static readonly DependencyProperty DragScrollOffsetProperty = DependencyProperty.RegisterAttached("DragScrollOffset", typeof(double), typeof(XItemsControl), new FrameworkPropertyMetadata(8.0));
        public static double GetDragScrollOffset(DependencyObject i) => (double)i.GetValue(DragScrollOffsetProperty);
        public static void SetDragScrollOffset(DependencyObject i, double input) => i.SetValue(DragScrollOffsetProperty, input);

        #endregion

        #region DragScrollOffsetMaximum

        public static readonly DependencyProperty DragScrollOffsetMaximumProperty = DependencyProperty.RegisterAttached("DragScrollOffsetMaximum", typeof(double), typeof(XItemsControl), new FrameworkPropertyMetadata(32.0));
        public static double GetDragScrollOffsetMaximum(DependencyObject i) => (double)i.GetValue(DragScrollOffsetMaximumProperty);
        public static void SetDragScrollOffsetMaximum(DependencyObject i, double input) => i.SetValue(DragScrollOffsetMaximumProperty, input);

        #endregion

        #region DragScrollTolerance

        public static readonly DependencyProperty DragScrollToleranceProperty = DependencyProperty.RegisterAttached("DragScrollTolerance", typeof(double), typeof(XItemsControl), new FrameworkPropertyMetadata(5.0));
        public static double GetDragScrollTolerance(DependencyObject i) => (double)i.GetValue(DragScrollToleranceProperty);
        public static void SetDragScrollTolerance(DependencyObject i, double input) => i.SetValue(DragScrollToleranceProperty, input);

        #endregion

        #region EmptyTemplate

        public static readonly DependencyProperty EmptyTemplateProperty = DependencyProperty.RegisterAttached("EmptyTemplate", typeof(DataTemplate), typeof(XItemsControl), new FrameworkPropertyMetadata(null));
        public static DataTemplate GetEmptyTemplate(ItemsControl i) => (DataTemplate)i.GetValue(EmptyTemplateProperty);
        public static void SetEmptyTemplate(ItemsControl i, DataTemplate input) => i.SetValue(EmptyTemplateProperty, input);

        #endregion

        #region EmptyTemplateVisibility

        public static readonly DependencyProperty EmptyTemplateVisibilityProperty = DependencyProperty.RegisterAttached("EmptyTemplateVisibility", typeof(Visibility), typeof(XItemsControl), new FrameworkPropertyMetadata(Visibility.Collapsed));
        public static Visibility GetEmptyTemplateVisibility(ItemsControl i) => (Visibility)i.GetValue(EmptyTemplateVisibilityProperty);
        public static void SetEmptyTemplateVisibility(ItemsControl i, Visibility input) => i.SetValue(EmptyTemplateVisibilityProperty, input);

        #endregion

        #region EnableColumnMenu

        /// <summary>
        /// Gets or sets whether or not to add a <see cref="ContextMenu"/> to the column header for toggling column visibility.
        /// </summary>
        public static readonly DependencyProperty EnableColumnMenuProperty = DependencyProperty.RegisterAttached("EnableColumnMenu", typeof(bool), typeof(XItemsControl), new FrameworkPropertyMetadata(false, OnEnableColumnMenuChanged));
        public static bool GetEnableColumnMenu(ItemsControl i) => (bool)i.GetValue(EnableColumnMenuProperty);
        public static void SetEnableColumnMenu(ItemsControl i, bool input) => i.SetValue(EnableColumnMenuProperty, input);
        static void OnEnableColumnMenuChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is not DataGrid && sender is not ListView && sender is not TreeView)
                throw new NotSupportedException();

            if (sender is ItemsControl control)
                control.RegisterHandlerAttached((bool)e.NewValue, EnableColumnMenuProperty, EnableColumnMenu_Loaded, EnableColumnMenu_Unloaded);
        }

        //...

        static void UnsubscribeColumnMenu(ContextMenu input)
        {
            if (input != null)
            {
                foreach (MenuItem i in input.Items)
                    UnsubscribeColumnMenuItem(i);
            }
        }

        static void UnsubscribeColumnMenuItem(MenuItem item)
        {
            item.Checked
               -= EnableColumnMenu_MenuItem_Checked;
            item.Unchecked
                -= EnableColumnMenu_MenuItem_Unchecked;

            SetColumnMenuItemIndex(item, -1);
            SetColumnMenuItemParent(item, null);
        }

        //...

        static void EnableColumnMenu_ViewChanged(object sender, EventArgs e)
        {
            if (sender is ListView listView)
            {
                UnsubscribeColumnMenu(GetColumnMenu(listView));
                if (CreateMenu(listView) is ContextMenu result)
                {
                    listView.FindVisualChildOfType<GridViewHeaderRowPresenter>().If(i => i != null, i => i.ContextMenu = result);

                    SetColumnMenu(listView, result);
                    ApplyColumnVisibility(listView);
                }
            }
        }

        //...

        static void EnableColumnMenu_Loaded(FrameworkElement sender)
        {
            if (sender is ItemsControl control)
            {
                ContextMenu result = default;
                if (control is DataGrid dataGrid)
                {
                    result = CreateMenu(dataGrid);
                    dataGrid.FindVisualChildOfType<DataGridColumnHeadersPresenter>().If(i => i != null, i => i.ContextMenu = result);
                }

                if (control is ListView listView)
                {
                    //listView.RemoveChanged
                    //(ListView.ViewProperty, EnableColumnMenu_ViewChanged);
                    //listView.AddChanged
                    //(ListView.ViewProperty, EnableColumnMenu_ViewChanged);

                    result = CreateMenu(listView);
                    listView.FindVisualChildOfType<GridViewHeaderRowPresenter>().If(i => i != null, i => i.ContextMenu = result);
                }

                if (control is TreeView treeView)
                {
                    result = CreateMenu(treeView);
                    treeView.FindVisualChildOfType<TreeViewColumnHeaderPresenter>().If(i => i != null, i => i.ContextMenu = result);
                }

                if (result != null)
                {
                    SetColumnMenu(control, result);
                    ApplyColumnVisibility(control);
                }
            }
        }

        static void EnableColumnMenu_Unloaded(FrameworkElement sender)
        {
            if (sender is ItemsControl control)
            {
                if (GetColumnMenu(control) is ContextMenu contextMenu)
                    contextMenu.Items.Cast<MenuItem>().ForEach(i => UnsubscribeColumnMenuItem(i));

                if (control is ListView listView)
                {
                    //listView.RemoveChanged(ListView.ViewProperty, EnableColumnMenu_ViewChanged);
                    GetHiddenColumns(listView).Clear();
                }
            }
        }

        //...

        static ContextMenu CreateMenu(DataGrid dataGrid)
        {
            return CreateMenu<DataGridColumn>
            (
                dataGrid,
                dataGrid.Columns,
                i => $"{i.Header}",
                i => $"{i.SortMemberPath}"
            );
        }

        static ContextMenu CreateMenu(ListView listView)
        {
            if (listView.View is GridView gridView)
            {
                return CreateMenu<GridViewColumn>
                (
                    listView,
                    gridView.Columns,
                    i => $"{i.Header}",
                    i => $"{i.Header}",
                    (i, j) => XGridViewColumn.SetColumnIndex(j, i)
                );
            }
            return default;
        }

        static ContextMenu CreateMenu(TreeView treeView)
        {
            return CreateMenu<TreeViewColumn>
            (
                treeView,
                XTreeView.GetColumns(treeView),
                i => $"{i.Header}",
                i => $"{i.SortName}"
            );
        }

        //...

        static ContextMenu CreateMenu<T>(ItemsControl control, IList columns, Func<T, string> header, Func<T, string> sortName, Action<int, T> each = null) where T : DependencyObject
        {
            ContextMenu result = default;
            if (columns?.Count > 0)
            {
                result = new();

                var index = 0;
                foreach (var i in columns)
                {
                    if (i is T j)
                    {
                        var item = CreateMenuItem(control, index, j, header(j), sortName(j));
                        result.Items.Add(item);

                        each?.Invoke(index, j);
                        index++;
                    }
                }
            }
            return result;
        }

        //...

        static MenuItem CreateMenuItem(ItemsControl control, int index, DependencyObject column, string header, string sortName)
        {
            var result = new MenuItem()
            {
                IsCheckable = true,
                IsChecked = true,
                StaysOpenOnClick = true
            };

            SetColumnMenuItemIndex(result, index);
            SetColumnMenuItemParent(result, control);

            result.Checked
                += EnableColumnMenu_MenuItem_Checked;
            result.Unchecked
                += EnableColumnMenu_MenuItem_Unchecked;

            var defaultHeader = GetColumnMenuDefaultHeader(control).F(index);

            if (control is DataGrid || control is TreeView)
            {
                result.Bind
                    (MenuItem.IsCheckedProperty,
                    new PropertyPath("(0)", XDependency.IsVisibleProperty),
                    column,
                    BindingMode.TwoWay);
            }

            result.Bind
                (HeaderedItemsControl.HeaderProperty,
                new PropertyPath(nameof(MenuItem.Header)),
                column,
                BindingMode.OneWay,
                new ComplexConverter<object, string>(i =>
                {
                    if (i.ActualValue is string j)
                        return j;

                    return defaultHeader;
                }),
                column);

            if (control is DataGrid)
            {
                column.Bind
                    (DataGridColumn.VisibilityProperty,
                    "IsChecked", result,
                    BindingMode.OneWay,
                    Converters.BooleanToVisibilityConverter.Default);
            }
            return result;
        }

        //...

        /// <summary>
        /// This is incomplete...
        /// </summary>
        static void ApplyColumnLength(ItemsControl control)
        {
            if (GetColumnMenu(control) is ContextMenu menu)
            {
                if (GetColumnLength(control) is ControlLengthList list)
                {
                    var a = menu.Items.Count;
                    var b = list.Count;

                    if (a != b)
                    {
                        list.Clear();
                        for (var i = 0; i < a; i++)
                            list.Add(new(1, ControlLengthUnit.Star));
                    }

                    for (var i = 0; i < list.Count; i++)
                    {
                        if (i > menu.Items.Count - 1)
                            break;

                        if (control is DataGrid dataGrid)
                            dataGrid.Columns[i].SetCurrentValue(DataGridColumn.WidthProperty, list[i]);

                        if (control is ListView listView)
                        {
                            if (listView.View is GridView gridView)
                                gridView.Columns[i].SetCurrentValue(GridViewColumn.WidthProperty, list[i]);
                        }

                        if (control is TreeView treeView)
                            XTreeView.GetColumns(treeView)[i].SetCurrentValue(TreeViewColumn.WidthProperty, list[i]);
                    }
                }
            }
        }

        static void ApplyColumnVisibility(ItemsControl control)
        {
            if (GetColumnMenu(control) is ContextMenu menu)
            {
                if (GetColumnVisibility(control) is BooleanList list)
                {
                    var a = menu.Items.Count;
                    var b = list.Count;

                    if (a != b)
                    {
                        list.Clear();
                        for (var i = 0; i < a; i++)
                            list.Add(true);
                    }

                    for (var i = 0; i < list.Count; i++)
                    {
                        if (i > menu.Items.Count - 1)
                            break;

                        var j = menu.Items[i];
                        var item = j is MenuItem menuItem
                            ? menuItem
                            : GetColumnMenu(control).ItemContainerGenerator.ContainerFromItem(j) as MenuItem;

                        item.SetCurrentValue(MenuItem.IsCheckedProperty, list[i]);
                    }
                }
            }
        }

        //...

        static void EnableColumnMenu_MenuItem_Checked(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem item)
            {
                var control = GetColumnMenuItemParent(item);
                if (GetColumnVisibility(control) is BooleanList list)
                    list[GetColumnMenuItemIndex(item)] = true;

                if (control is ListView listView)
                {
                    if (listView.View is GridView gridView)
                    {
                        var index
                            = GetColumnMenuItemIndex(item);
                        var gridViewColumn
                            = GetHiddenColumns(listView).FirstOrDefault(i => XGridViewColumn.GetColumnIndex(i) == index);

                        var k = 0;
                        foreach (var i in gridView.Columns)
                        {
                            if (index <= XGridViewColumn.GetColumnIndex(i))
                                break;

                            k++;
                        }
                        gridView.Columns.Insert(k, gridViewColumn);
                    }
                }
            }
        }

        static void EnableColumnMenu_MenuItem_Unchecked(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem item)
            {
                var control = GetColumnMenuItemParent(item);
                if (GetColumnVisibility(control) is BooleanList list)
                    list[GetColumnMenuItemIndex(item)] = false;

                if (control is ListView listView)
                {
                    if (listView.View is GridView gridView)
                    {
                        var index = GetColumnMenuItemIndex(item);

                        var gridViewColumn = gridView.Columns.First(i => XGridViewColumn.GetColumnIndex(i) == index);
                        GetHiddenColumns(listView).Add(gridViewColumn);
                        gridView.Columns.Remove(gridViewColumn);
                    }
                }
            }
        }

        #endregion

        #region Extend

        public static readonly DependencyProperty ExtendProperty = DependencyProperty.RegisterAttached("Extend", typeof(bool), typeof(XItemsControl), new FrameworkPropertyMetadata(false, OnExtendChanged));
        public static bool GetExtend(ItemsControl i) => (bool)i.GetValue(ExtendProperty);
        public static void SetExtend(ItemsControl i, bool input) => i.SetValue(ExtendProperty, input);
        static void OnExtendChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is ItemsControl control)
                control.RegisterHandlerAttached((bool)e.NewValue, ExtendProperty, OnLoaded, OnUnloaded);
        }

        #endregion

        #region Group/Sort

        static bool CanGroup(ItemsControl control) 
            => GetGroupName(control) is string groupName && groupName != GetGroupNameDisable(control);

        static void GroupSort(ItemsControl control)
        {
            if (GetGroupName(control) == null && GetSortComparer(control) == null && GetSortName(control) == null)
                return;

            if (control.ItemsSource is ListCollectionView a)
            {
                a.GroupDescriptions.Clear(); a.SortDescriptions.Clear();

                var styles = control.GroupStyle;
                if (CanGroup(control))
                {
                    var groupName = GetGroupName(control);
                    PropertyGroupDescription description = new() { Converter = GetGroupConverterSelector(control)?.Select(groupName) ?? GetGroupConverter(control) };
                    if (description.Converter == null)
                        description.PropertyName = groupName;

                    a.GroupDescriptions.Add(description);
                    if (GetSortName(control) != null)
                        a.SortDescriptions.Add(new SortDescription(groupName, GetGroupDirection(control)));

                    if (GetGroupStyle(control) is GroupStyle style)
                    {
                        styles.Clear();
                        styles.Add(style);
                    }
                }

                if (GetSortComparer(control) is IComparer comparer)
                {
                    a.CustomSort = comparer;
                    a.Refresh();
                }

                else if (GetSortName(control)?.ToString().Empty() == false)
                    a.SortDescriptions.Add(new SortDescription($"{GetSortName(control)}", GetSortDirection(control)));
            }
            else if (control.ItemsSource is IList b)
            {
                var result = new ListCollectionView(b);
                control.SetCurrentValue(ItemsControl.ItemsSourceProperty, result);
            }
        }

        //...

        static void OnGroupSortChanged(object sender, DependencyPropertyChangedEventArgs e) => GroupSort(sender as ItemsControl);

        #endregion

        #region GroupContainerStyle

        public static readonly DependencyProperty GroupContainerStyleProperty = DependencyProperty.RegisterAttached("GroupContainerStyle", typeof(Style), typeof(XItemsControl), new FrameworkPropertyMetadata(null));
        public static Style GetGroupContainerStyle(ItemsControl i) => (Style)i.GetValue(GroupContainerStyleProperty);
        public static void SetGroupContainerStyle(ItemsControl i, Style input) => i.SetValue(GroupContainerStyleProperty, input);

        #endregion

        #region GroupConverter

        public static readonly DependencyProperty GroupConverterProperty = DependencyProperty.RegisterAttached("GroupConverter", typeof(IValueConverter), typeof(XItemsControl), new FrameworkPropertyMetadata(null, OnGroupSortChanged));
        public static IValueConverter GetGroupConverter(ItemsControl i) => (IValueConverter)i.GetValue(GroupConverterProperty);
        public static void SetGroupConverter(ItemsControl i, IValueConverter input) => i.SetValue(GroupConverterProperty, input);

        #endregion

        #region GroupConverterSelector

        public static readonly DependencyProperty GroupConverterSelectorProperty = DependencyProperty.RegisterAttached("GroupConverterSelector", typeof(ConverterSelector), typeof(XItemsControl), new FrameworkPropertyMetadata(null, OnGroupSortChanged));
        public static ConverterSelector GetGroupConverterSelector(ItemsControl i) => (ConverterSelector)i.GetValue(GroupConverterSelectorProperty);
        public static void SetGroupConverterSelector(ItemsControl i, ConverterSelector input) => i.SetValue(GroupConverterSelectorProperty, input);

        #endregion

        #region GroupDirection

        public static readonly DependencyProperty GroupDirectionProperty = DependencyProperty.RegisterAttached("GroupDirection", typeof(ListSortDirection), typeof(XItemsControl), new FrameworkPropertyMetadata(ListSortDirection.Ascending, OnGroupSortChanged));
        public static ListSortDirection GetGroupDirection(ItemsControl i) => (ListSortDirection)i.GetValue(GroupDirectionProperty);
        public static void SetGroupDirection(ItemsControl i, ListSortDirection input) => i.SetValue(GroupDirectionProperty, input);

        #endregion

        #region GroupName

        public static readonly DependencyProperty GroupNameProperty = DependencyProperty.RegisterAttached("GroupName", typeof(string), typeof(XItemsControl), new FrameworkPropertyMetadata(null, OnGroupSortChanged, OnGroupNameCoerced));
        public static string GetGroupName(ItemsControl i) => (string)i.GetValue(GroupNameProperty);
        public static void SetGroupName(ItemsControl i, string input) => i.SetValue(GroupNameProperty, input);
        static object OnGroupNameCoerced(DependencyObject sender, object input) => input?.ToString().Trim() is string result ? (result.Empty() ? null : result) : null;
        
        #endregion

        #region GroupNameDisable

        public static readonly DependencyProperty GroupNameDisableProperty = DependencyProperty.RegisterAttached("GroupNameDisable", typeof(string), typeof(XItemsControl), new FrameworkPropertyMetadata("None"));
        public static string GetGroupNameDisable(ItemsControl i) => (string)i.GetValue(GroupNameDisableProperty);
        public static void SetGroupNameDisable(ItemsControl i, string input) => i.SetValue(GroupNameDisableProperty, input);

        #endregion

        #region GroupsItself

        public static readonly DependencyProperty GroupsItselfProperty = DependencyProperty.RegisterAttached("GroupsItself", typeof(bool), typeof(XItemsControl), new FrameworkPropertyMetadata(false, OnGroupsItselfChanged));
        public static bool GetGroupsItself(ItemsControl i) => (bool)i.GetValue(GroupsItselfProperty);
        public static void SetGroupsItself(ItemsControl i, bool input) => i.SetValue(GroupsItselfProperty, input);
        static void OnGroupsItselfChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is ItemsControl control)
                UpdateGroupStyle(control);
        }

        #endregion
        
        #region GroupStyle

        public static readonly DependencyProperty GroupStyleProperty = DependencyProperty.RegisterAttached("GroupStyle", typeof(GroupStyle), typeof(XItemsControl), new FrameworkPropertyMetadata(null, OnGroupStyleChanged));
        public static GroupStyle GetGroupStyle(ItemsControl i) => (GroupStyle)i.GetValue(GroupStyleProperty);
        public static void SetGroupStyle(ItemsControl i, GroupStyle input) => i.SetValue(GroupStyleProperty, input);
        static void OnGroupStyleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is ItemsControl control)
                UpdateGroupStyle(control);
        }

        static void UpdateGroupStyle(ItemsControl control)
        {
            var styles = control.GroupStyle;
            styles.Clear();

            if (GetGroupsItself(control) || (control.ItemsSource is ListCollectionView view && view.GroupDescriptions.Count > 0))
            {
                if (GetGroupStyle(control) is GroupStyle style)
                    styles.Add(style);
            }
        }

        #endregion

        #region (internal) HiddenColumns

        internal static readonly DependencyProperty HiddenColumnsProperty = DependencyProperty.RegisterAttached("HiddenColumns", typeof(List<GridViewColumn>), typeof(XItemsControl), new FrameworkPropertyMetadata(null));
        internal static List<GridViewColumn> GetHiddenColumns(ItemsControl i) => i.GetValueOrSetDefault<List<GridViewColumn>>(HiddenColumnsProperty, () => new());
        internal static void SetHiddenColumns(ItemsControl i, List<GridViewColumn> input) => i.SetValue(HiddenColumnsProperty, input);

        #endregion

        #region (ReadOnly) IsEmpty

        static readonly DependencyPropertyKey IsEmptyKey = DependencyProperty.RegisterAttachedReadOnly("IsEmpty", typeof(bool), typeof(XItemsControl), new FrameworkPropertyMetadata(true, null, new CoerceValueCallback(OnIsEmptyCoerced)));
        public static readonly DependencyProperty IsEmptyProperty = IsEmptyKey.DependencyProperty;
        public static bool GetIsEmpty(ItemsControl i) => (bool)i.GetValue(IsEmptyProperty);
        static object OnIsEmptyCoerced(DependencyObject sender, object input)
        {
            if (sender is ItemsControl i)
                return i.Items.Count == 0;

            return default;
        }

        #endregion

        #region (readonly) FrameworkElement > IsLastContainer

        static readonly DependencyPropertyKey IsLastContainerKey = DependencyProperty.RegisterAttachedReadOnly("IsLastContainer", typeof(bool), typeof(XItemsControl), new FrameworkPropertyMetadata(false));
        public static readonly DependencyProperty IsLastContainerProperty = IsLastContainerKey.DependencyProperty;
        public static bool GetIsLastContainer(FrameworkElement i) => (bool)i.GetValue(IsLastContainerProperty);
        static void SetIsLastContainer(FrameworkElement i, bool input) => i.SetValue(IsLastContainerKey, input);

        #endregion

        #region KeySelect

        public static readonly DependencyProperty KeySelectProperty = DependencyProperty.RegisterAttached("KeySelect", typeof(bool), typeof(XItemsControl), new FrameworkPropertyMetadata(false, OnKeySelectChanged));
        public static bool GetKeySelect(ItemsControl i) => (bool)i.GetValue(KeySelectProperty);
        public static void SetKeySelect(ItemsControl i, bool input) => i.SetValue(KeySelectProperty, input);
        static void OnKeySelectChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is not DataGrid && sender is not ListBox && sender is not TreeView)
                throw new NotSupportedException();

            if (sender is ItemsControl control)
            {
                control.RegisterHandlerAttached((bool)e.NewValue, KeySelectProperty, k =>
                {
                    control.KeyDown
                        += KeySelect_KeyDown;
                }, k =>
                {
                    control.KeyDown
                        -= KeySelect_KeyDown;

                    GetKeySelectTimer(control).Stop();
                    GetKeySelectTimer(control).Tick -= KeySelect_Tick;

                    KeySelectTimers.Remove(GetKeySelectTimer(control));
                    SetKeySelectTimer(control, null);
                });
            }
        }

        static void KeySelect_KeyDown(object sender, KeyEventArgs e)
        {
            var listBox = sender as ListBox;

            char? character = e.Key.Character();
            if (character == null)
            {
                SetKeySelectQuery(listBox, null);
                return;
            }

            if (GetKeySelectQuery(listBox) == null)
                SetKeySelectQuery(listBox, string.Empty);

            SetKeySelectQuery(listBox, $"{GetKeySelectQuery(listBox)}{character}");

            var timer = GetKeySelectTimer(listBox);
            timer.Stop();
            timer.Start();

            object item = null;
            IList items = listBox.ItemsSource as IList;

            var comparer = GetKeySelectComparer(listBox);
            if (items != null)
            {
                foreach (var i in items)
                {
                    if (comparer.Compare(i.GetPropertyValue(GetKeySelectProperty(listBox)), GetKeySelectQuery(listBox)))
                    {
                        item = i;
                        break;
                    }
                }
            }

            if (item == null)
            {
                SetKeySelectQuery(listBox, null);
                return;
            }

            foreach (var i in items)
            {
                if (i != item)
                {
                    if (listBox.ItemContainerGenerator.ContainerFromItem(i) is ListViewItem j)
                        j.IsSelected = false;
                }
            }

            listBox.ScrollIntoView(item);
            if (listBox.ItemContainerGenerator.ContainerFromItem(item) is ListViewItem k)
                k.IsSelected = true;
        }

        static void KeySelect_Tick(object sender, EventArgs e)
        {
            if (sender is DispatcherTimer i)
            {
                i.Stop();
                SetKeySelectQuery(KeySelectTimers[i], null);
            }
        }

        #endregion

        #region KeySelectComparer

        public static readonly DependencyProperty KeySelectComparerProperty = DependencyProperty.RegisterAttached("KeySelectComparer", typeof(IKeySelectComparer), typeof(XItemsControl), new FrameworkPropertyMetadata(null));
        public static IKeySelectComparer GetKeySelectComparer(ItemsControl i) => i.GetValueOrSetDefault<IKeySelectComparer>(KeySelectComparerProperty, () => new DefaultKeySelectComparer());
        public static void SetKeySelectComparer(ItemsControl i, IKeySelectComparer input) => i.SetValue(KeySelectComparerProperty, input);

        #endregion

        #region KeySelectProperty

        public static readonly DependencyProperty KeySelectPropertyProperty = DependencyProperty.RegisterAttached("KeySelectProperty", typeof(string), typeof(XItemsControl), new FrameworkPropertyMetadata(null));
        public static string GetKeySelectProperty(ItemsControl i) => (string)i.GetValue(KeySelectPropertyProperty);
        public static void SetKeySelectProperty(ItemsControl i, string input) => i.SetValue(KeySelectPropertyProperty, input);

        #endregion

        #region (private) KeySelectQuery

        static readonly DependencyProperty KeySelectQueryProperty = DependencyProperty.RegisterAttached("KeySelectQuery", typeof(string), typeof(XItemsControl), new FrameworkPropertyMetadata(string.Empty));
        static string GetKeySelectQuery(ItemsControl i) => (string)i.GetValue(KeySelectQueryProperty);
        static void SetKeySelectQuery(ItemsControl i, string input) => i.SetValue(KeySelectQueryProperty, input);

        #endregion

        #region (private) KeySelectTimer

        static readonly Dictionary<DispatcherTimer, ItemsControl> KeySelectTimers = new();

        static readonly DependencyProperty KeySelectTimerProperty = DependencyProperty.RegisterAttached("KeySelectTimer", typeof(DispatcherTimer), typeof(XItemsControl), new FrameworkPropertyMetadata(null));
        static DispatcherTimer GetKeySelectTimer(ItemsControl i) => i.GetValueOrSetDefault(KeySelectTimerProperty, () =>
        {
            var result = new DispatcherTimer() { Interval = 2.Seconds() };
            result.Tick += KeySelect_Tick;
            KeySelectTimers.Add(result, i);
            return result;
        });
        static void SetKeySelectTimer(ItemsControl i, DispatcherTimer input) => i.SetValue(KeySelectTimerProperty, input);

        #endregion

        #region SelectNoneOnEmptySpaceClick

        public static readonly DependencyProperty SelectNoneOnEmptySpaceClickProperty = DependencyProperty.RegisterAttached("SelectNoneOnEmptySpaceClick", typeof(bool), typeof(XItemsControl), new FrameworkPropertyMetadata(false, OnSelectNoneOnEmptySpaceClickChanged));
        public static bool GetSelectNoneOnEmptySpaceClick(DependencyObject i) => (bool)i.GetValue(SelectNoneOnEmptySpaceClickProperty);
        public static void SetSelectNoneOnEmptySpaceClick(DependencyObject i, bool input) => i.SetValue(SelectNoneOnEmptySpaceClickProperty, input);
        static void OnSelectNoneOnEmptySpaceClickChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is ItemsControl control)
                control.RegisterHandlerAttached((bool)e.NewValue, SelectNoneOnEmptySpaceClickProperty, i => i.PreviewMouseDown += SelectNoneOnEmptySpaceClick_PreviewMouseDown, i => i.PreviewMouseDown -= SelectNoneOnEmptySpaceClick_PreviewMouseDown);
        }

        static void SelectNoneOnEmptySpaceClick_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is ItemsControl control)
            {
                if (e.LeftButton == MouseButtonState.Pressed)
                {
                    if (control is DataGrid)
                    {
                        if (e.OriginalSource.FindParent<DataGridRow>() is not null)
                            return;
                    }
                    if (control is ListBox)
                    {
                        if (e.OriginalSource.FindParent<ListBoxItem>() is not null)
                            return;
                    }
                    if (control is TreeView)
                    {
                        if (e.OriginalSource.FindParent<TreeViewItem>() is not null)
                            return;

                        if (control is TreeViewBox)
                        {
                            if (!e.OriginalSource.HasParent<Popup>())
                                return;
                        }
                    }
                    if (e.OriginalSource.FindParent<ScrollBar>() is null)
                    {
                        //This prevents everything from unselecting when drag selecting with modifier key pressed
                        if (GetCanDragSelect(control))
                        {
                            if (ModifierKeys.Control.Pressed() || ModifierKeys.Shift.Pressed())
                                return;
                        }
                        control.TryClearSelection();
                    }
                }
            }
        }

        #endregion

        #region SortComparer

        public static readonly DependencyProperty SortComparerProperty = DependencyProperty.RegisterAttached("SortComparer", typeof(IComparer), typeof(XItemsControl), new FrameworkPropertyMetadata(null, OnGroupSortChanged));
        public static IComparer GetSortComparer(ItemsControl i) => (IComparer)i.GetValue(SortComparerProperty);
        public static void SetSortComparer(ItemsControl i, IComparer input) => i.SetValue(SortComparerProperty, input);

        #endregion

        #region SortName

        public static readonly DependencyProperty SortNameProperty = DependencyProperty.RegisterAttached("SortName", typeof(object), typeof(XItemsControl), new FrameworkPropertyMetadata(null, OnGroupSortChanged, OnSortNameCoerced));
        public static object GetSortName(ItemsControl i) => i.GetValue(SortNameProperty);
        public static void SetSortName(ItemsControl i, object input) => i.SetValue(SortNameProperty, input);
        static object OnSortNameCoerced(DependencyObject sender, object input) => input?.ToString().Trim() is string result ? (result.Empty() ? null : result) : null;

        #endregion

        #region SortDirection

        public static readonly DependencyProperty SortDirectionProperty = DependencyProperty.RegisterAttached("SortDirection", typeof(ListSortDirection), typeof(XItemsControl), new FrameworkPropertyMetadata(ListSortDirection.Ascending, OnGroupSortChanged));
        public static ListSortDirection GetSortDirection(ItemsControl i) => (ListSortDirection)i.GetValue(SortDirectionProperty);
        public static void SetSortDirection(ItemsControl i, ListSortDirection input) => i.SetValue(SortDirectionProperty, input);

        #endregion

        #region (private) Source

        static readonly DependencyProperty SourceProperty = DependencyProperty.RegisterAttached("Source", typeof(object), typeof(XItemsControl), new FrameworkPropertyMetadata(null, OnSourceChanged));
        static void OnSourceChanged(object sender, DependencyPropertyChangedEventArgs e) => sender.If<ItemsControl>(i => OnSourceChanged(i, (Value<object>)e));

        #endregion

        #region Spacing

        /// <summary>
        /// Applies <see cref="Thickness"/> to all children except those that define <see cref="XElement.MarginProperty"/>.
        /// </summary>
        public static readonly DependencyProperty SpacingProperty = DependencyProperty.RegisterAttached("Spacing", typeof(Thickness?), typeof(XItemsControl), new FrameworkPropertyMetadata(null, OnSpacingChanged));
        public static void SetSpacing(ItemsControl i, Thickness? input) => i.SetValue(SpacingProperty, input);
        public static Thickness? GetSpacing(ItemsControl i) => (Thickness?)i.GetValue(SpacingProperty);
        static void OnSpacingChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is ItemsControl control)
                UpdateSpacing(control);
        }

        #endregion

        #region SpacingExcept

        public static readonly DependencyProperty SpacingExceptProperty = DependencyProperty.RegisterAttached("SpacingExcept", typeof(SpacingExceptions), typeof(XItemsControl), new FrameworkPropertyMetadata(SpacingExceptions.None, OnSpacingChanged));
        public static void SetSpacingExcept(ItemsControl i, SpacingExceptions input) => i.SetValue(SpacingExceptProperty, input);
        public static SpacingExceptions GetSpacingExcept(ItemsControl i) => (SpacingExceptions)i.GetValue(SpacingExceptProperty);

        #endregion

        #region (enum) SpacingExceptions

        [Flags]
        public enum SpacingExceptions { None = 0, First = 1, Last = 2 }

        #endregion

        #endregion

        #region XItemsControl

        static void OnCountChanged(ItemsControl control, Value<int> input)
        {
            lock (IsEmptyProperty)
                control.InvalidateProperty(IsEmptyProperty);

            if (GetContainerTracking(control))
                UpdateContainerTracking(control);

            if (GetSpacing(control) != default)
                UpdateSpacing(control);
        }

        static void OnSourceChanged(ItemsControl control, Value<object> input)
        {
            lock (IsEmptyProperty)
                control.InvalidateProperty(IsEmptyProperty);

            GroupSort(control);
        }

        //...

        static void OnLoaded(ItemsControl control)
        {
            lock (IsEmptyProperty)
                control.InvalidateProperty(IsEmptyProperty);

            GroupSort(control);
            UpdateSpacing(control);

            Subscribe(control);
        }

        static void OnUnloaded(ItemsControl control)
        {
            Unsubscribe(control);
        }

        //...

        static void Subscribe(ItemsControl control)
        {
            Unsubscribe(control);
            control.Bind(CountProperty, $"{nameof(ItemsControl.ItemsSource)}.{nameof(IList.Count)}", control);
            control.Bind(SourceProperty, nameof(ItemsControl.ItemsSource), control);
        }

        static void Unsubscribe(ItemsControl control)
        {
            control.Unbind(CountProperty);
            control.Unbind(SourceProperty);
        }

        #endregion

        #region Methods

        static void UpdateContainerTracking(ItemsControl control)
        {
            var origin = GetContainerIndexOrigin(control);

            var lastIndex = control.Items.Count - 1;
            for (var i = lastIndex; i >= 0; i--)
            {
                var container = control.GetContainer(i);
                if (container != null)
                {
                    SetContainerIndex(container, i + origin);
                    SetIsLastContainer(container, i == lastIndex);
                }
            }
        }

        static void UpdateSpacing(ItemsControl control)
        {
            if (control.ItemContainerGenerator.Status != GeneratorStatus.ContainersGenerated || GetSpacing(control) == null)
                return;

            var spacing
                = GetSpacing(control);
            var except
                = GetSpacingExcept(control);

            var first
                = except.HasFlag(SpacingExceptions.First);
            var last
                = except.HasFlag(SpacingExceptions.Last);

            for (int i = 0, count = control.Items.Count; i < count; i++)
            {
                if (control.ItemContainerGenerator.ContainerFromItem(control.Items[i]) is FrameworkElement j)
                {
                    if (XElement.GetOverrideMargin(j) == null)
                    {
                        j.Margin
                            = (i == 0 && first) || (i == (count - 1) && last)
                            ? new Thickness(0)
                            : j.Margin = spacing.Value;
                    }
                }
            }
        }

        //...

        public static void ClearSelection(this ItemsControl control)
        {
            if (control is Selector selector)
                selector.ClearSelection();

            else if (control is TreeView view)
                view.SelectNone();
        }

        public static FrameworkElement GetContainer(this ItemsControl control, int index)
            => control.ItemContainerGenerator.ContainerFromIndex(index) as FrameworkElement;

        public static FrameworkElement GetContainer(this ItemsControl control, object item)
            => control.ItemContainerGenerator.ContainerFromItem(item) as FrameworkElement;

        public static object GetItem(this ItemsControl control, DependencyObject container)
            => control.ItemContainerGenerator.ItemFromContainer(container);

        public static Result TryClearSelection(this ItemsControl control) 
            => Try.Invoke(() => control.ClearSelection());

        #endregion
    }
}