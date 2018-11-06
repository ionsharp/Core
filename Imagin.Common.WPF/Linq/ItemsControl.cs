using Imagin.Common.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;

namespace Imagin.Common.Linq
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static bool GetAutoSizeColumns(ItemsControl d)
        {
            return (bool)d.GetValue(AutoSizeColumnsProperty);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="d"></param>
        /// <param name="value"></param>
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
            else if (sender is TreeView)
            {
                var t = sender as TreeView;
                var l = (bool)e.NewValue ? new GridLength(1.0, GridUnitType.Star) : new GridLength(1.0, GridUnitType.Auto);
                t.Columns.ForEach(i => i.Width = l);
            }
        }

        #endregion

        #region CanDragSelect

        /// <summary>
        /// Gets or sets value indicating whether ItemsControl should allow drag selecting items.
        /// </summary>
        public static readonly DependencyProperty CanDragSelectProperty = DependencyProperty.RegisterAttached("CanDragSelect", typeof(bool), typeof(ItemsControlExtensions), new PropertyMetadata(false, OnCanDragSelectChanged));
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool GetCanDragSelect(DependencyObject obj)
        {
            return (bool)obj.GetValue(CanDragSelectProperty);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="value"></param>
        public static void SetCanDragSelect(DependencyObject obj, bool value)
        {
            obj.SetValue(CanDragSelectProperty, value);
        }

        static void OnCanDragSelectChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var ItemsControl = sender as ItemsControl;
            if (ItemsControl != null)
            {
                if ((bool)e.NewValue)
                    ItemsControl.Loaded += RegisterCanDragSelect;
                else
                {
                    ItemsControl.Loaded -= RegisterCanDragSelect;

                    if (GetDragSelector(ItemsControl) != null)
                        GetDragSelector(ItemsControl).Deregister();

                    SetDragSelector(ItemsControl, null);
                }
            }
        }

        static void RegisterCanDragSelect(object sender, RoutedEventArgs e)
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static bool GetIsColumnMenuEnabled(ItemsControl d)
        {
            return (bool)d.GetValue(IsColumnMenuEnabledProperty);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="d"></param>
        /// <param name="value"></param>
        public static void SetIsColumnMenuEnabled(ItemsControl d, bool value)
        {
            d.SetValue(IsColumnMenuEnabledProperty, value);
        }

        static void IsColumnMenuEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is DataGrid || sender is TreeView)
            {
                var i = sender as ItemsControl;
                var h = i.GetHashCode();

                i.Loaded -= RegisterIsColumnMenuEnabled;

                if ((bool)e.NewValue)
                {
                    if (!i.IsLoaded)
                    {
                        i.Loaded += RegisterIsColumnMenuEnabled;
                    }
                    else RegisterIsColumnMenuEnabled(i, true);
                }
                else
                {
                    //TO DO: Remove menu
                    RegisterIsColumnMenuEnabled(i, false);
                }
            }
        }

        static void RegisterIsColumnMenuEnabled(ItemsControl Control, bool IsEnabled)
        {
            if (Control is DataGrid)
            {
                var d = Control as DataGrid;

                var t = new Style(typeof(DataGridColumnHeader));
                t.BasedOn = d.ColumnHeaderStyle;
                t.Setters.Add(new Setter(DataGridColumnHeader.ContextMenuProperty,
                    IsEnabled && d?.Columns.Count >= 0
                    ? GetContextMenu(d)
                    : null));

                d.ColumnHeaderStyle = t;
            }
            else if (Control is TreeView)
            {
                var t = Control as TreeView;
                t.ColumnHeaderContextMenu = IsEnabled ? GetContextMenu(t) : null;
            }
        }

        static void RegisterIsColumnMenuEnabled(object sender, RoutedEventArgs e)
        {
            var Control = sender as ItemsControl;
            RegisterIsColumnMenuEnabled(Control, GetIsColumnMenuEnabled(Control));
        }

        static MenuItem GetMenuItem(DependencyObject Column)
        {
            var Result = GetMenuItem();

            //Bind model boolean to menu item's check state
            BindingOperations.SetBinding(Result, MenuItem.IsCheckedProperty, new Binding()
            {
                Path = new PropertyPath("(0)", DependencyObjectExtensions.IsVisibleProperty),
                Mode = BindingMode.TwoWay,
                Source = Column
            });

            //Bind model boolean to column visibility
            BindingOperations.SetBinding(Column, DataGridColumn.VisibilityProperty, new Binding()
            {
                Path = new PropertyPath("IsChecked"),
                Mode = BindingMode.OneWay,
                Source = Result,
                Converter = new Imagin.Common.Converters.BooleanToVisibilityConverter()
            });

            return Result;
        }

        static ContextMenu GetContextMenu(TreeView TreeView)
        {
            var Result = new ContextMenu();
            foreach (var Column in TreeView.Columns)
            {
                if (Column is TreeViewTextColumn || Column is TreeViewTemplateColumn && (Column.Header != null && !Column.Header.ToString().IsEmpty()))
                {
                    if (Column is TreeViewTextColumn && (Column.As<TreeViewTextColumn>().MemberPath.IsNullOrEmpty()))
                        continue;

                    //Result.Items.Add(GetMenuItem(Column, () => Column.Header == null || Column.Header.ToString().IsEmpty() ? (Column as TreeViewTextColumn).MemberPath : Column.Header.ToString()));
                }
            }
            return Result;
        }

        static ContextMenu GetContextMenu(DataGrid DataGrid)
        {
            var Result = new ContextMenu();
            foreach (var Column in DataGrid.Columns)
            {
                if (Column is DataGridTextColumn || Column is DataGridTemplateColumn || Column is DataGridComboBoxColumn || Column is DataGridCheckBoxColumn)
                {
                    if (Column.Header != null && !Column.Header.ToString().IsNullOrEmpty() && !Column.SortMemberPath.IsNullOrEmpty())
                    {
                        var m = GetMenuItem(Column);

                        if (Column.Header != null)
                        {
                            m.SetBinding(MenuItem.HeaderProperty, new Binding()
                            {
                                Mode = BindingMode.OneWay,
                                Path = new PropertyPath("Header"),
                                Source = Column
                            });
                        }
                        else m.Header = Column.SortMemberPath;

                        Result.Items.Add(m);
                    }
                }
            }
            return Result;
        }

        static MenuItem GetMenuItem()
        {
            return new MenuItem()
            {
                IsCheckable = true,
                IsChecked = true,
                StaysOpenOnClick = true
            };
        }

        #endregion

        #region DragScrollOffset

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty DragScrollOffsetProperty = DependencyProperty.RegisterAttached("DragScrollOffset", typeof(double), typeof(ItemsControlExtensions), new PropertyMetadata(10.0));
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static double GetDragScrollOffset(DependencyObject obj)
        {
            return (double)obj.GetValue(DragScrollOffsetProperty);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="value"></param>
        public static void SetDragScrollOffset(DependencyObject obj, double value)
        {
            obj.SetValue(DragScrollOffsetProperty, value);
        }

        #endregion

        #region DragScrollTolerance

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty DragScrollToleranceProperty = DependencyProperty.RegisterAttached("DragScrollTolerance", typeof(double), typeof(ItemsControlExtensions), new PropertyMetadata(5.0));
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static double GetDragScrollTolerance(DependencyObject obj)
        {
            return (double)obj.GetValue(DragScrollToleranceProperty);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="value"></param>
        public static void SetDragScrollTolerance(DependencyObject obj, double value)
        {
            obj.SetValue(DragScrollToleranceProperty, value);
        }

        #endregion

        #region SelectNoneOnEmptySpaceClick

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty SelectNoneOnEmptySpaceClickProperty = DependencyProperty.RegisterAttached("SelectNoneOnEmptySpaceClick", typeof(bool), typeof(ItemsControlExtensions), new PropertyMetadata(false, OnSelectNoneOnEmptySpaceClickChanged));
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool GetSelectNoneOnEmptySpaceClick(DependencyObject obj)
        {
            return (bool)obj.GetValue(SelectNoneOnEmptySpaceClickProperty);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="value"></param>
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
        /// Collapse all items in ItemsControl (siblings of <see langword="Source"/>).
        /// </summary>
        /// <param name="Parent"></param>
        /// <param name="Source"></param>
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ToEvaluate"></param>
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
            else if (ToEvaluate is TreeView)
                ToEvaluate.As<TreeView>().SelectNone();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Control"></param>
        /// <param name="Item"></param>
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ToEvaluate"></param>
        /// <returns></returns>
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
