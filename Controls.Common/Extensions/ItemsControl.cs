using Imagin.Common.Extensions;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;

namespace Imagin.Controls.Common.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class ItemsControlExtensions
    {
        #region Properties

        #region AutoSizeColumns

        /// <summary>
        /// Applies GridUnit.Star GridLength to all columns.
        /// </summary>
        public static readonly DependencyProperty AutoSizeColumnsProperty = DependencyProperty.RegisterAttached("AutoSizeColumns", typeof(bool), typeof(ItemsControlExtensions), new PropertyMetadata(false, OnAutoSizeColumnsChanged));
        public static bool GetAutoSizeColumns(ItemsControl d)
        {
            return (bool)d.GetValue(AutoSizeColumnsProperty);
        }
        public static void SetAutoSizeColumns(ItemsControl d, bool value)
        {
            d.SetValue(AutoSizeColumnsProperty, value);
        }
        static void OnAutoSizeColumnsChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is DataGrid)
            {
                var d = sender as DataGrid;
                var l = (bool)e.NewValue ? new DataGridLength(1.0, DataGridLengthUnitType.Star) : new DataGridLength(1.0, DataGridLengthUnitType.Auto);
                d.Columns.ForEach(i => i.Width = l);
            }
            else if (sender is TreeViewExt)
            {
                var t = sender as TreeViewExt;
                var l = (bool)e.NewValue ? new GridLength(1.0, GridUnitType.Star) : new GridLength(1.0, GridUnitType.Auto);
                t.Columns.ForEach(i => i.Width = l);
            }
        }

        #endregion

        #region DragSelector

        internal static readonly DependencyProperty DragSelectorProperty = DependencyProperty.RegisterAttached("DragSelector", typeof(DragSelector), typeof(ItemsControlExtensions), new PropertyMetadata(null));
        internal static DragSelector GetDragSelector(ItemsControl d)
        {
            return (DragSelector)d.GetValue(DragSelectorProperty);
        }
        internal static void SetDragSelector(ItemsControl d, DragSelector value)
        {
            d.SetValue(DragSelectorProperty, value);
        }

        #endregion

        #region IsColumnMenuEnabled

        /// <summary>
        /// Determines whether or not to add a ContextMenu to the column header for toggling column visibility.
        /// </summary>
        public static readonly DependencyProperty IsColumnMenuEnabledProperty = DependencyProperty.RegisterAttached("IsColumnMenuEnabled", typeof(bool), typeof(ItemsControlExtensions), new PropertyMetadata(false, IsColumnMenuEnabledChanged));
        public static bool GetIsColumnMenuEnabled(ItemsControl d)
        {
            return (bool)d.GetValue(IsColumnMenuEnabledProperty);
        }
        public static void SetIsColumnMenuEnabled(ItemsControl d, bool value)
        {
            d.SetValue(IsColumnMenuEnabledProperty, value);
        }

        static void IsColumnMenuEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is DataGrid || sender is TreeViewExt)
            {
                var ItemsControl = sender as ItemsControl;
                if ((bool)e.NewValue)
                    ItemsControl.Loaded += RegisterIsColumnMenuEnabled;
                else
                {
                    //TO DO: Remove menu
                    ItemsControl.Loaded -= RegisterIsColumnMenuEnabled;
                }
            }
        }

        static void RegisterIsColumnMenuEnabled(object sender, RoutedEventArgs e)
        {
            if (sender is DataGrid)
            {
                var DataGrid = sender as DataGrid;
                if (DataGrid.Columns != null || DataGrid.Columns.Count >= 0)
                    DataGrid.Style = GetStyle(DataGrid);
            }
            else if (sender is TreeViewExt)
            {
                var TreeView = sender as TreeViewExt;
                TreeView.ColumnHeaderContextMenu = GetContextMenu(TreeView); 
            }
        }

        static MenuItem GetMenuItem(DependencyObject Column, Func<string> GetHeader)
        {
            var Result = GetMenuItem(GetHeader());

            //Bind model boolean to menu item's check state
            BindingOperations.SetBinding(Result, MenuItem.IsCheckedProperty, new Binding()
            {
                Path = new PropertyPath("(0)", ControlExtensions.IsVisibleProperty),
                Mode = BindingMode.TwoWay,
                Source = Column
            });

            //Bind model boolean to column visibility
            BindingOperations.SetBinding(Column, DataGridColumn.VisibilityProperty, new Binding()
            {
                Path = new PropertyPath("IsChecked"),
                Mode = BindingMode.OneWay,
                Source = Result,
                Converter = new BooleanToVisibilityConverter()
            });

            return Result;
        }

        static ContextMenu GetContextMenu(TreeViewExt TreeViewExt)
        {
            var Result = new ContextMenu();
            foreach (var Column in TreeViewExt.Columns)
            {
                if (Column is TreeViewTextColumn || Column is TreeViewTemplateColumn && (Column.Header != null && !Column.Header.ToString().IsEmpty()))
                {
                    if (Column is TreeViewTextColumn && (Column.As<TreeViewTextColumn>().MemberPath.IsNullOrEmpty())) continue;
                    Result.Items.Add(GetMenuItem(Column, () =>
                    {
                        return Column.Header == null || Column.Header.ToString().IsEmpty() ? (Column as TreeViewTextColumn).MemberPath : Column.Header.ToString();
                    }));
                }
            }
            return Result;
        }

        static ContextMenu GetContextMenu(DataGrid DataGrid)
        {
            var Result = new ContextMenu();
            foreach (var Column in DataGrid.Columns)
            {
                if (Column is DataGridTextColumn || Column is DataGridTemplateColumn)
                {
                    if (Column.Header != null && !Column.Header.ToString().IsNullOrEmpty() && !Column.SortMemberPath.IsNullOrEmpty())
                    {
                        Result.Items.Add(GetMenuItem(Column, () =>
                        {
                            return Column.Header.ToString().IsNullOrEmpty() ? Column.SortMemberPath : Column.Header.ToString();
                        }));
                    }
                }
            }
            return Result;
        }

        static Style GetColumnHeaderStyle(DataGrid DataGrid)
        {
            var Result = new Style(typeof(DataGridColumnHeader));
            if (DataGrid.ColumnHeaderStyle != null)
                Result.BasedOn = (Style)DataGrid.ColumnHeaderStyle;

            Result.Setters.Add(new Setter(DataGridColumnHeader.ContextMenuProperty, GetContextMenu(DataGrid)));

            return Result;
        }

        static MenuItem GetMenuItem(string Header)
        {
            return new MenuItem()
            {
                Header = Header,
                IsCheckable = true,
                IsChecked = true,
                StaysOpenOnClick = true
            };
        }

        static Style GetStyle(DataGrid DataGrid)
        {
            var OldStyle = default(Style);

            try
            {
                OldStyle = (Style)DataGrid.FindResource(typeof(DataGrid));
            }
            catch
            {
                OldStyle = null;
            }

            var Result = new Style(typeof(DataGrid));

            if (OldStyle != null)
                Result.BasedOn = OldStyle;

            Result.Setters.Add(new Setter(DataGrid.ColumnHeaderStyleProperty, GetColumnHeaderStyle(DataGrid)));

            return Result;
        }

        #endregion

        #region IsDragSelectionEnabled

        /// <summary>
        /// Gets or sets value indicating whether ItemsControl should allow drag selecting items.
        /// </summary>
        public static readonly DependencyProperty IsDragSelectionEnabledProperty = DependencyProperty.RegisterAttached("IsDragSelectionEnabled", typeof(bool), typeof(ItemsControlExtensions), new PropertyMetadata(false, OnIsDragSelectionEnabledChanged));
        public static bool GetIsDragSelectionEnabled(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsDragSelectionEnabledProperty);
        }
        public static void SetIsDragSelectionEnabled(DependencyObject obj, bool value)
        {
            obj.SetValue(IsDragSelectionEnabledProperty, value);
        }

        static void OnIsDragSelectionEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var ItemsControl = sender as ItemsControl;
            if (ItemsControl != null)
            {
                if ((bool)e.NewValue)
                    ItemsControl.Loaded += RegisterIsDragSelectionEnabled;
                else
                {
                    ItemsControl.Loaded -= RegisterIsDragSelectionEnabled;

                    if (GetDragSelector(ItemsControl) != null)
                        GetDragSelector(ItemsControl).Deregister();

                    SetDragSelector(ItemsControl, null);
                }
            }
        }

        static void RegisterIsDragSelectionEnabled(object sender, RoutedEventArgs e)
        {
            var ItemsControl = sender as ItemsControl;
            if (GetDragSelector(ItemsControl) == null)
            {
                ItemsControl.ApplyTemplate();

                var Manager = DragSelector.New(ItemsControl);
                Manager.Register();

                SetDragSelector(ItemsControl, Manager);
            }
        }

        #endregion

        #region DragScrollOffset

        public static readonly DependencyProperty DragScrollOffsetProperty = DependencyProperty.RegisterAttached("DragScrollOffset", typeof(double), typeof(ItemsControlExtensions), new PropertyMetadata(10.0));
        public static double GetDragScrollOffset(DependencyObject obj)
        {
            return (double)obj.GetValue(DragScrollOffsetProperty);
        }
        public static void SetDragScrollOffset(DependencyObject obj, double value)
        {
            obj.SetValue(DragScrollOffsetProperty, value);
        }

        #endregion

        #region DragScrollTolerance

        public static readonly DependencyProperty DragScrollToleranceProperty = DependencyProperty.RegisterAttached("DragScrollTolerance", typeof(double), typeof(ItemsControlExtensions), new PropertyMetadata(5.0));
        public static double GetDragScrollTolerance(DependencyObject obj)
        {
            return (double)obj.GetValue(DragScrollToleranceProperty);
        }
        public static void SetDragScrollTolerance(DependencyObject obj, double value)
        {
            obj.SetValue(DragScrollToleranceProperty, value);
        }

        #endregion

        #region SelectNoneOnEmptySpaceClick

        public static readonly DependencyProperty SelectNoneOnEmptySpaceClickProperty = DependencyProperty.RegisterAttached("SelectNoneOnEmptySpaceClick", typeof(bool), typeof(ItemsControlExtensions), new PropertyMetadata(false, OnSelectNoneOnEmptySpaceClickChanged));
        public static bool GetSelectNoneOnEmptySpaceClick(DependencyObject obj)
        {
            return (bool)obj.GetValue(SelectNoneOnEmptySpaceClickProperty);
        }
        public static void SetSelectNoneOnEmptySpaceClick(DependencyObject obj, bool value)
        {
            obj.SetValue(SelectNoneOnEmptySpaceClickProperty, value);
        }

        static void OnSelectNoneOnEmptySpaceClickChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var ItemsControl = sender as ItemsControl;
            if (ItemsControl != null && (bool)e.NewValue)
            {
                if ((bool)e.NewValue)
                    ItemsControl.PreviewMouseDown += RegisterSelectNoneOnEmptySpaceClick;
                else ItemsControl.PreviewMouseDown -= RegisterSelectNoneOnEmptySpaceClick;
            }
        }

        static void RegisterSelectNoneOnEmptySpaceClick(object sender, MouseButtonEventArgs e)
        {
            var ItemsControl = sender as ItemsControl;
            if (ItemsControlExtensions.GetSelectNoneOnEmptySpaceClick(ItemsControl) && e.LeftButton == MouseButtonState.Pressed)
            {
                if ((ItemsControl is TreeView && !e.OriginalSource.Is<TreeViewItem>())
                 || (ItemsControl is DataGrid && !e.OriginalSource.Is<DataGridRow>())
                 || (ItemsControl is ListBox && !e.OriginalSource.Is<ListBoxItem>()))
                    ItemsControl.TryClearSelection();
            }
        }

        #endregion

        #endregion

        #region Methods

        /// <summary>
        /// Collapse all items in ItemsControl (siblings of <param name="Source"/>).
        /// </summary>
        internal static void CollapseSiblings(this ItemsControl Parent, TreeViewItem Source)
        {
            if (Parent == null || Parent.Items == null) return;
            foreach (var i in Parent.Items)
            {
                var c = Parent.ItemContainerGenerator.ContainerFromItem(i);
                if (c == null) continue;
                var Child = c.As<TreeViewItem>();
                if (Child != null && !Child.Equals(Source))
                    Child.IsExpanded = false;
            }
        }

        public static void ClearSelection(this ItemsControl ToEvaluate)
        {
            if (ToEvaluate is ListBox)
                (ToEvaluate as ListBox).SelectedItems.Clear();
            else if (ToEvaluate is DataGrid)
                (ToEvaluate as DataGrid).SelectedItems.Clear();
            else if (ToEvaluate is TreeView)
            {
                var TreeView = ToEvaluate.As<TreeView>();
                var Item = TreeView.ItemContainerGenerator.ContainerFromItem(TreeView.SelectedItem);
                if (Item != null)
                    Item.As<TreeViewItem>().IsSelected = false;
            }
            else if (ToEvaluate is TreeViewExt)
                ToEvaluate.As<TreeViewExt>().SelectNone();
        }

        public static void Select(this ItemsControl Control, object Item)
        {
            if (Control != null)
            {
                foreach (var i in Control.Items)
                {
                    var j = (TreeViewItem)Control.ItemContainerGenerator.ContainerFromItem(i);

                    if (Item == i)
                    {
                        if (j is TreeViewItem)
                        {
                            TreeViewItemExtensions.SetIsSelected(j as TreeViewItem, true);
                        }
                    }
                    else 
                    {
                        TreeViewItemExtensions.SetIsSelected(j as TreeViewItem, false);
                        Select(j, Item);
                    }
                }
            }
        }

        public static bool TryClearSelection(this ItemsControl ToEvaluate)
        {
            try
            {
                ToEvaluate.ClearSelection();
                return true;
            }
            catch
            {
                return false;
            }
        }

        #endregion
    }
}