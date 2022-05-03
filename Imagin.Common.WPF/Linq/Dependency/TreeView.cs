using Imagin.Common.Collections;
using Imagin.Common.Collections.Generic;
using Imagin.Common.Controls;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Imagin.Common.Linq
{
    [Extends(typeof(TreeView))]
    public static class XTreeView
    {
        #region Properties

        #region CanResizeColumns

        public static readonly DependencyProperty CanResizeColumnsProperty = DependencyProperty.RegisterAttached("CanResizeColumns", typeof(bool), typeof(XTreeView), new FrameworkPropertyMetadata(true));
        public static bool GetCanResizeColumns(TreeView i) => (bool)i.GetValue(CanResizeColumnsProperty);
        public static void SetCanResizeColumns(TreeView i, bool input) => i.SetValue(CanResizeColumnsProperty, input);

        #endregion

        #region CollapseAllCommand

        public static readonly RoutedUICommand CollapseAllCommand = new(nameof(CollapseAllCommand), nameof(CollapseAllCommand), typeof(XItemsControl));
        static void OnCollapseAll(object sender, ExecutedRoutedEventArgs e)
        {
            if (sender is TreeView treeView)
                treeView.CollapseAll();
        }
        static void OnCanCollapseAll(object sender, CanExecuteRoutedEventArgs e) => e.CanExecute = true;

        #endregion

        #region CollapseSiblingsOnClick

        public static readonly DependencyProperty CollapseSiblingsOnClickProperty = DependencyProperty.RegisterAttached("CollapseSiblingsOnClick", typeof(bool), typeof(XTreeView), new FrameworkPropertyMetadata(false, OnCollapseSiblingsOnClickChanged));
        public static bool GetCollapseSiblingsOnClick(TreeView i) => (bool)i.GetValue(CollapseSiblingsOnClickProperty);
        public static void SetCollapseSiblingsOnClick(TreeView i, bool input) => i.SetValue(CollapseSiblingsOnClickProperty, input);
        static void OnCollapseSiblingsOnClickChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is TreeView treeView)
                treeView.RegisterHandlerAttached((bool)e.NewValue, CollapseSiblingsOnClickProperty, i => i.MouseLeftButtonUp += CollapseSiblingsOnClick_MouseLeftButtonUp, i => i.MouseLeftButtonUp -= CollapseSiblingsOnClick_MouseLeftButtonUp);
        }

        static void CollapseSiblingsOnClick_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
            => e.OriginalSource.FindParent<TreeViewItem>()?.CollapseSiblings();

        #endregion

        #region ColumnHeaderStyle

        public static readonly DependencyProperty ColumnHeaderStyleProperty = DependencyProperty.RegisterAttached("ColumnHeaderStyle", typeof(Style), typeof(XTreeView), new FrameworkPropertyMetadata(null));
        public static Style GetColumnHeaderStyle(TreeView i) => (Style)i.GetValue(ColumnHeaderStyleProperty);
        public static void SetColumnHeaderStyle(TreeView i, Style input) => i.SetValue(ColumnHeaderStyleProperty, input);

        #endregion

        #region ColumnHeaderStyleSelector

        public static readonly DependencyProperty ColumnHeaderStyleSelectorProperty = DependencyProperty.RegisterAttached("ColumnHeaderStyleSelector", typeof(StyleSelector), typeof(XTreeView), new FrameworkPropertyMetadata(null));
        public static StyleSelector GetColumnHeaderStyleSelector(TreeView i) => (StyleSelector)i.GetValue(ColumnHeaderStyleSelectorProperty);
        public static void SetColumnHeaderStyleSelector(TreeView i, StyleSelector input) => i.SetValue(ColumnHeaderStyleSelectorProperty, input);

        #endregion

        #region ColumnHeaderTemplate

        public static readonly DependencyProperty ColumnHeaderTemplateProperty = DependencyProperty.RegisterAttached("ColumnHeaderTemplate", typeof(DataTemplate), typeof(XTreeView), new FrameworkPropertyMetadata(null));
        public static DataTemplate GetColumnHeaderTemplate(TreeView i) => (DataTemplate)i.GetValue(ColumnHeaderTemplateProperty);
        public static void SetColumnHeaderTemplate(TreeView i, DataTemplate input) => i.SetValue(ColumnHeaderTemplateProperty, input);

        #endregion

        #region ColumnHeaderTemplateSelector

        public static readonly DependencyProperty ColumnHeaderTemplateSelectorProperty = DependencyProperty.RegisterAttached("ColumnHeaderTemplateSelector", typeof(DataTemplateSelector), typeof(XTreeView), new FrameworkPropertyMetadata(null));
        public static DataTemplateSelector GetColumnHeaderTemplateSelector(TreeView i) => (DataTemplateSelector)i.GetValue(ColumnHeaderTemplateSelectorProperty);
        public static void SetColumnHeaderTemplateSelector(TreeView i, DataTemplateSelector input) => i.SetValue(ColumnHeaderTemplateSelectorProperty, input);

        #endregion

        #region ColumnHeaderStringFormat

        public static readonly DependencyProperty ColumnHeaderStringFormatProperty = DependencyProperty.RegisterAttached("ColumnHeaderStringFormat", typeof(ContextMenu), typeof(XTreeView), new FrameworkPropertyMetadata(null));
        public static ContextMenu GetColumnHeaderStringFormat(TreeView i) => (ContextMenu)i.GetValue(ColumnHeaderStringFormatProperty);
        public static void SetColumnHeaderStringFormat(TreeView i, ContextMenu input) => i.SetValue(ColumnHeaderStringFormatProperty, input);

        #endregion

        #region Columns

        public static readonly DependencyProperty ColumnsProperty = DependencyProperty.RegisterAttached("Columns", typeof(TreeViewColumnCollection), typeof(XTreeView), new FrameworkPropertyMetadata(null));
        public static TreeViewColumnCollection GetColumns(TreeView i) => (TreeViewColumnCollection)i.GetValue(ColumnsProperty);
        public static void SetColumns(TreeView i, TreeViewColumnCollection input) => i.SetValue(ColumnsProperty, input);

        #endregion

        #region (private) HandleSelection

        static readonly DependencyProperty HandleSelectionProperty = DependencyProperty.RegisterAttached("HandleSelection", typeof(Handle), typeof(XTreeView), new FrameworkPropertyMetadata(null));
        static Handle GetHandleSelection(TreeView i) => i.GetValueOrSetDefault<Handle>(HandleSelectionProperty, () => false);

        #endregion

        #region Mode

        public static readonly DependencyProperty ModeProperty = DependencyProperty.RegisterAttached("Mode", typeof(TreeViewModes), typeof(XTreeView), new FrameworkPropertyMetadata(TreeViewModes.Default));
        public static TreeViewModes GetMode(TreeView i) => (TreeViewModes)i.GetValue(ModeProperty);
        public static void SetMode(TreeView i, TreeViewModes input) => i.SetValue(ModeProperty, input);

        #endregion

        #region SelectedIndex

        public static readonly DependencyProperty SelectedIndexProperty = DependencyProperty.RegisterAttached("SelectedIndex", typeof(int[]), typeof(XTreeView), new FrameworkPropertyMetadata(new int[1] { -1 }, OnSelectedIndexChanged));
        public static int[] GetSelectedIndex(TreeView i) => (int[])i.GetValue(SelectedIndexProperty);
        public static void SetSelectedIndex(TreeView i, int[] input) => i.SetValue(SelectedIndexProperty, input);
        static void OnSelectedIndexChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is TreeView view)
            {
                GetHandleSelection(view).SafeInvoke(() =>
                {
                    if (view.Items.Count > 0)
                    {
                        object result = null;
                        view.Enumerate((i, j) => result = i, (int[])e.NewValue);
                        SetSelectedItem(view, result);
                    }
                    view.SelectSingle(GetSelectedItem(view));
                });
            }
        }

        #endregion

        #region SelectedItem

        public static readonly DependencyProperty SelectedItemProperty = DependencyProperty.RegisterAttached("SelectedItem", typeof(object), typeof(XTreeView), new FrameworkPropertyMetadata(null, OnSelectedItemChanged));
        public static object GetSelectedItem(TreeView i) => i.GetValue(SelectedItemProperty);
        public static void SetSelectedItem(TreeView i, object input) => i.SetValue(SelectedItemProperty, input);
        static void OnSelectedItemChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is TreeView view)
            {
                GetHandleSelection(view).SafeInvoke(() =>
                {
                    if (view.GetContainer(e.NewValue) is TreeViewItem item)
                        SetSelectedIndex(view, item.GetIndex());

                    view.SelectSingle(e.NewValue);
                });
            }
        }

        #endregion

        #region SelectedItems

        static readonly Dictionary<ICollectionChanged, TreeView> SelectionCache = new();

        static readonly DependencyPropertyKey SelectedItemsKey = DependencyProperty.RegisterAttachedReadOnly("SelectedItems", typeof(ICollectionChanged), typeof(XTreeView), new FrameworkPropertyMetadata(null));
        public static readonly DependencyProperty SelectedItemsProperty = SelectedItemsKey.DependencyProperty;
        public static ICollectionChanged GetSelectedItems(TreeView i) => i.GetValueOrSetDefault<ICollectionChanged>(SelectedItemsKey, () => new ObservableCollection<object>());

        #endregion

        #region SelectionMode

        public static readonly DependencyProperty SelectionModeProperty = DependencyProperty.RegisterAttached("SelectionMode", typeof(Controls.SelectionMode), typeof(XTreeView), new FrameworkPropertyMetadata(Controls.SelectionMode.Single, OnSelectionModeChanged));
        public static Controls.SelectionMode GetSelectionMode(TreeView i) => (Controls.SelectionMode)i.GetValue(SelectionModeProperty);
        public static void SetSelectionMode(TreeView i, Controls.SelectionMode input) => i.SetValue(SelectionModeProperty, input);
        static void OnSelectionModeChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is TreeView treeView)
            {
                if ((Controls.SelectionMode)e.NewValue == Controls.SelectionMode.Single)
                {
                    if (GetSelectedItems(treeView).Count > 0)
                        treeView.SelectSingle(treeView.ItemContainerGenerator.ContainerFromItem(GetSelectedItems(treeView)[0]) as TreeViewItem);
                }
            }
        }

        #endregion

        #region (private) SelectionStart

        static readonly DependencyProperty SelectionStartProperty = DependencyProperty.RegisterAttached("SelectionStart", typeof(TreeViewItem), typeof(XTreeView));
        static TreeViewItem GetSelectionStart(TreeView i) => (TreeViewItem)i.GetValue(SelectionStartProperty);
        static void SetSelectionStart(TreeView i, TreeViewItem input) => i.SetValue(SelectionStartProperty, input);

        #endregion

        #region SelectOnRightClick

        public static readonly DependencyProperty SelectOnRightClickProperty = DependencyProperty.RegisterAttached("SelectOnRightClick", typeof(bool), typeof(XTreeView), new FrameworkPropertyMetadata(false, OnSelectOnRightClickChanged));
        public static bool GetSelectOnRightClick(TreeView i) => (bool)i.GetValue(SelectOnRightClickProperty);
        public static void SetSelectOnRightClick(TreeView i, bool input) => i?.SetValue(SelectOnRightClickProperty, input);
        static void OnSelectOnRightClickChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is TreeView treeView)
                treeView.RegisterHandlerAttached((bool)e.NewValue, SelectOnRightClickProperty, i => i.PreviewMouseRightButtonDown += SelectOnRightClick_PreviewMouseRightButtonDown, i => i.PreviewMouseRightButtonDown -= SelectOnRightClick_PreviewMouseRightButtonDown);
        }

        static void SelectOnRightClick_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is TreeView treeView)
            {
                var i = e.OriginalSource.FindParent<TreeViewItem>();
                if (i != null)
                {
                    treeView.SelectSingle(i);
                    e.Handled = true;
                }
            }
        }

        #endregion

        #endregion

        #region XTreeView

        static XTreeView()
        {
            EventManager.RegisterClassHandler(typeof(TreeView), TreeView.LoadedEvent,
                new RoutedEventHandler(OnLoaded), true);
            EventManager.RegisterClassHandler(typeof(TreeView), TreeView.PreviewMouseLeftButtonDownEvent,
                new MouseButtonEventHandler(OnPreviewMouseLeftButtonDown), true);
            EventManager.RegisterClassHandler(typeof(TreeView), TreeView.UnloadedEvent,
                new RoutedEventHandler(OnUnloaded), true);
        }

        static void OnLoaded(object sender, RoutedEventArgs e)
        {
            if (sender is TreeView view)
            {
                var selection = GetSelectedItems(view);
                SelectionCache.Add(selection, view);
                selection.CollectionChanged += OnSelectedItemsChanged;
            }
        }

        static void OnPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is TreeView view)
            {
                if (e.OriginalSource is DependencyObject i && i is not TreeView)
                {
                    if (i.FindVisualParent<TreeViewItem>() is TreeViewItem item)
                        view.Select(item);
                }
            }
        }

        static void OnSelectedItemsChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (sender is ICollectionChanged items)
            {
                if (items.Count > 0)
                {
                    if (SelectionCache[items] is TreeView view)
                    {
                        GetHandleSelection(view).SafeInvoke(() =>
                        {
                            var i = GetSelectedItems(view)[0];
                            SetSelectedItem(view, i);
                            if (view.GetContainer(i) is TreeViewItem item)
                                SetSelectedIndex(view, item.GetIndex());
                        });
                    }
                }
            }
        }

        static void OnUnloaded(object sender, RoutedEventArgs e)
        {
            if (sender is TreeView view)
            {
                var selection = GetSelectedItems(view);
                selection.CollectionChanged -= OnSelectedItemsChanged;
                SelectionCache.Remove(selection);
            }
        }

        #endregion

        #region Methods

        public static void Enumerate(this ItemsControl input, Action<object, ItemsControl> action, params int[] indices) => input.Enumerate<object>(action, indices);

        public static void Enumerate<T>(this ItemsControl input, Action<T, ItemsControl> action, params int[] indices)
        {
            if (input.Items != null && indices.Length > 0)
            {
                var m = indices[0];
                var n = 0;
                foreach (var i in input.Items)
                {
                    if (m == n)
                    {
                        if (i is T j)
                        {
                            var k = (ItemsControl)input.ItemContainerGenerator.ContainerFromItem(i);
                            action(j, k);

                            if (k != null)
                                k.Enumerate<T>(action, indices.Skip(1).ToArray());
                        }
                    }
                    n++;
                }
            }
        }

        //...

        static IEnumerable<TreeViewItem> GetAll(this ItemsControl input)
        {
            if (input != null)
            {
                for (int i = 0; i < input.Items.Count; i++)
                {
                    if (input.ItemContainerGenerator.ContainerFromIndex(i) is TreeViewItem item)
                    {
                        yield return item;
                        foreach (var j in GetAll(item as ItemsControl))
                            yield return j;
                    }
                }
            }
            yield break;
        }

        public static IEnumerable<TreeViewItem> GetAll(this TreeView input)
        {
            foreach (var i in GetAll(input as ItemsControl))
                yield return i;

            yield break;
        }

        public static IEnumerable<TreeViewItem> GetAll(this TreeViewItem input)
        {
            foreach (var i in GetAll(input as ItemsControl))
                yield return i;

            yield break;
        }

        //...

        public static int[] GetIndex(this TreeViewItem input)
        {
            var result = new List<int>();

            ItemsControl parent = input;
            while (parent != null && !(parent is TreeView))
            {
                var nextParent = parent.FindParent<ItemsControl>();
                if (nextParent != null)
                    result.Add(nextParent.Items.IndexOf(parent.DataContext));

                parent = nextParent;
            }
            result.Reverse();
            return result.ToArray();
        }

        public static TreeViewItem GetContainer(this TreeView input, object item) => XItemsControl.GetContainer(input, item) as TreeViewItem;

        public static ItemsControl GetParent(this TreeViewItem input) => input.FindParent<TreeViewItem>() as ItemsControl ?? input.FindParent<TreeView>();

        //...

        public static void Select(this TreeView input, TreeViewItem item)
        {
            if (item == null)
                return;

            var a = GetSelectionMode(input) != Controls.SelectionMode.Multiple;
            var b = !ModifierKeys.Control.Pressed() && !ModifierKeys.Shift.Pressed();
            
            if (a || b)
                input.SelectSingle(item);

            else if (ModifierKeys.Control.Pressed())
                SelectMultiple(input, item);

            else if (ModifierKeys.Shift.Pressed())
                SelectMultipleBetween(input, item);
        }

        //...

        static void SelectMultiple(this TreeView input, TreeViewItem item)
        {
            XTreeViewItem.SetIsSelected(item, !XTreeViewItem.GetIsSelected(item));
            if (GetSelectionStart(input) == null)
            {
                if (XTreeViewItem.GetIsSelected(item))
                    SetSelectionStart(input, item);
            }
            else if (GetSelectedItems(input).Count == 0)
                SetSelectionStart(input, null);
        }

        static void SelectMultipleBetween(this TreeView input, TreeViewItem item, bool modifier = false)
        {
            if (GetSelectionStart(input) != null)
            {
                if (GetSelectionStart(input) == item)
                {
                    input.SelectSingle(item);
                    return;
                }

                var allItems = new List<TreeViewItem>(input.GetAll());

                bool between = false;
                foreach (var i in allItems)
                {
                    if (i == item || i == GetSelectionStart(input))
                    {
                        between = !between;
                        XTreeViewItem.SetIsSelected(i, true);
                        continue;
                    }
                    if (between)
                    {
                        XTreeViewItem.SetIsSelected(i, true);
                        continue;
                    }
                    if (!modifier)
                        XTreeViewItem.SetIsSelected(i, false);
                }
            }
        }

        //...

        public static void SelectNone(this TreeView input)
        {
            if (GetSelectionMode(input) != Controls.SelectionMode.Single)
                input.GetAll().ForEach(i => XTreeViewItem.SetIsSelected(i, false));
        }

        //...

        public static void SelectSingle(this TreeView input, TreeViewItem item)
        {
            foreach (var i in input.GetAll())
            {
                var select = i == item
                    ? (GetSelectionMode(input) == Controls.SelectionMode.Single
                        ? true
                        : !XTreeViewItem.GetIsSelected(i))
                    : false;

                XTreeViewItem.SetIsSelected(i, select);
                if (select)
                    SetSelectionStart(input, i);
            }
        }

        public static void SelectSingle(this TreeView input, object item)
        {
            foreach (var i in input.GetAll())
            {
                var select = i.DataContext == item
                    ? (GetSelectionMode(input) == Controls.SelectionMode.Single
                        ? true
                        : !XTreeViewItem.GetIsSelected(i))
                    : false;

                XTreeViewItem.SetIsSelected(i, select);
                if (select)
                    SetSelectionStart(input, i);
            }
        }

        //...

        public static void CollapseAll(this TreeView input) => input.GetAll().ForEach(i => i.IsExpanded = false);

        public static void CollapseSiblings(this TreeViewItem input)
        {
            if (input.GetParent() is ItemsControl parent)
            {
                for (var i = parent.Items.Count - 1; i >= 0; i--)
                {
                    if (i >= parent.Items.Count)
                        break;

                    if (parent.ItemContainerGenerator.ContainerFromItem(parent.Items[i]) is TreeViewItem j)
                        j.IsExpanded = j.Equals(input);
                }
            }
        }

        public static void ExpandAll(this TreeView input) => input.GetAll().ForEach(i => i.IsExpanded = true);

        #endregion
    }
}