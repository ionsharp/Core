using Imagin.Common.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace Imagin.Controls.Common
{
    /// <summary>
    /// An advanced TreeView.
    /// </summary>
    /// <remarks>
    /// Multi selection behavior borrowed from https://github.com/cmyksvoll/MultiSelectTreeView.
    /// </remarks>
    public class AdvancedTreeView : TreeView
    {
        #region Properties

        private static TreeViewItem _selectTreeViewItemOnMouseUp;

        public static DependencyProperty CollapseItemSiblingsOnClickProperty = DependencyProperty.Register("CollapseItemSiblingsOnClick", typeof(bool), typeof(AdvancedTreeView), new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public bool CollapseItemSiblingsOnClick
        {
            get
            {
                return (bool)GetValue(CollapseItemSiblingsOnClickProperty);
            }
            set
            {
                SetValue(CollapseItemSiblingsOnClickProperty, value);
            }
        }

        public static DependencyProperty ExpandItemOnClickProperty = DependencyProperty.Register("ExpandItemOnClick", typeof(bool), typeof(AdvancedTreeView), new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public bool ExpandItemOnClick
        {
            get
            {
                return (bool)GetValue(ExpandItemOnClickProperty);
            }
            set
            {
                SetValue(ExpandItemOnClickProperty, value);
            }
        }

        public static DependencyProperty SelectedVisualProperty = DependencyProperty.Register("SelectedVisual", typeof(TreeViewItem), typeof(AdvancedTreeView), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public TreeViewItem SelectedVisual
        {
            get
            {
                return (TreeViewItem)GetValue(SelectedVisualProperty);
            }
            set
            {
                SetValue(SelectedVisualProperty, value);
            }
        }

        public static DependencyProperty SelectItemOnRightClickProperty = DependencyProperty.Register("SelectItemOnRightClick", typeof(bool), typeof(AdvancedTreeView), new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public bool SelectItemOnRightClick
        {
            get
            {
                return (bool)GetValue(SelectItemOnRightClickProperty);
            }
            set
            {
                SetValue(SelectItemOnRightClickProperty, value);
            }
        }

        public static DependencyProperty SelectNoneOnEmptySpaceClickProperty = DependencyProperty.Register("SelectNoneOnEmptySpaceClick", typeof(bool), typeof(AdvancedTreeView), new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public bool SelectNoneOnEmptySpaceClick
        {
            get
            {
                return (bool)GetValue(SelectNoneOnEmptySpaceClickProperty);
            }
            set
            {
                SetValue(SelectNoneOnEmptySpaceClickProperty, value);
            }
        }

        public static readonly DependencyProperty IsItemSelectedProperty = DependencyProperty.RegisterAttached("IsItemSelected", typeof(Boolean), typeof(AdvancedTreeView), new PropertyMetadata(false, OnIsItemSelectedPropertyChanged));
        public static bool GetIsItemSelected(TreeViewItem element)
        {
            return (bool)element.GetValue(IsItemSelectedProperty);
        }
        public static void SetIsItemSelected(TreeViewItem element, Boolean value)
        {
            if (element == null) return;
            element.SetValue(IsItemSelectedProperty, value);
        }
        static void OnIsItemSelectedPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var TreeViewItem = d as TreeViewItem;
            var TreeView = FindTreeView(TreeViewItem);
            if (TreeViewItem != null && TreeView != null)
            {
                var SelectedItems = GetSelectedItems(TreeView);
                if (SelectedItems != null)
                {
                    if (GetIsItemSelected(TreeViewItem))
                        SelectedItems.Add(TreeViewItem.Header);
                    else SelectedItems.Remove(TreeViewItem.Header);
                }
            }
        }

        public static readonly DependencyProperty SelectedItemsProperty = DependencyProperty.RegisterAttached("SelectedItems", typeof(IList), typeof(AdvancedTreeView));
        public static IList GetSelectedItems(TreeView element)
        {
            return (IList)element.GetValue(SelectedItemsProperty);
        }
        public static void SetSelectedItems(TreeView element, IList value)
        {
            element.SetValue(SelectedItemsProperty, value);
        }

        static readonly DependencyProperty StartItemProperty = DependencyProperty.RegisterAttached("StartItem", typeof(TreeViewItem), typeof(AdvancedTreeView));
        static TreeViewItem GetStartItem(TreeView element)
        {
            return (TreeViewItem)element.GetValue(StartItemProperty);
        }
        static void SetStartItem(TreeView element, TreeViewItem value)
        {
            element.SetValue(StartItemProperty, value);
        }

        #endregion

        #region AdvancedTreeView

        public AdvancedTreeView()
        {
            this.DefaultStyleKey = typeof(AdvancedTreeView);
            this.GotFocus += OnTreeViewItemGotFocus;
            this.PreviewMouseLeftButtonDown += OnTreeViewItemPreviewMouseDown;
            this.PreviewMouseLeftButtonUp += OnTreeViewItemPreviewMouseUp;
            this.SelectedItemChanged += OnSelectedItemChanged;
        }

        #endregion

        #region Methods

        #region Overrides

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);
            if (!this.SelectNoneOnEmptySpaceClick)
                return;
            if (!e.OriginalSource.Is<TreeViewItem>())
                this.SelectNone();
        }

        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonUp(e);
            TreeViewItem Item = e.OriginalSource.As<DependencyObject>().VisualUpwardSearch<TreeViewItem>();
            if (Item == null) return;
            if (ExpandItemOnClick)
                Item.IsExpanded = !Item.IsExpanded;
            if (CollapseItemSiblingsOnClick)
                Item.CollapseSiblings();
        }

        protected override void OnPreviewMouseRightButtonDown(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseRightButtonDown(e);
            if (!this.SelectItemOnRightClick)
                return;
            TreeViewItem Item = e.OriginalSource.As<DependencyObject>().VisualUpwardSearch<TreeViewItem>();
            if (Item == null) return;
            Item.IsSelected = true;
            e.Handled = true;
        }

        #endregion

        #region Public

        public void CollapseAll()
        {
            this.ToggleAll(false);
        }

        public bool CollapseSiblings()
        {
            if (this.SelectedVisual == null)
                return false;
            this.SelectedVisual.CollapseSiblings();
            return true;
        }

        public void SelectNone()
        {
            SelectNone(this);
        }

        #endregion

        #region Static

        static TreeView FindTreeView(DependencyObject DependencyObject)
        {
            if (DependencyObject == null) return null;
            var TreeView = DependencyObject.As<TreeView>();
            return TreeView ?? FindTreeView(VisualTreeHelper.GetParent(DependencyObject));
        }

        static TreeViewItem FindTreeViewItem(DependencyObject DependencyObject)
        {
            if (!DependencyObject.Is<Visual>() && !DependencyObject.Is<Visual3D>())
                return null;
            var Item = DependencyObject as TreeViewItem;
            if (Item != null) return Item;
            return FindTreeViewItem(VisualTreeHelper.GetParent(DependencyObject));
        }

        static void GetAllItems(TreeView TreeView, TreeViewItem TreeViewItem, ICollection<TreeViewItem> AllItems)
        {
            if (TreeView != null)
            {
                for (int i = 0; i < TreeView.Items.Count; i++)
                {
                    var Item = TreeView.ItemContainerGenerator.ContainerFromIndex(i) as TreeViewItem;
                    if (Item != null)
                    {
                        AllItems.Add(Item);
                        GetAllItems(null, Item, AllItems);
                    }
                }
            }
            else
            {
                for (int i = 0; i < TreeViewItem.Items.Count; i++)
                {
                    var Item = TreeViewItem.ItemContainerGenerator.ContainerFromIndex(i) as TreeViewItem;
                    if (Item != null)
                    {
                        AllItems.Add(Item);
                        GetAllItems(null, Item, AllItems);
                    }
                }
            }
        }

        static void SelectItem(TreeView TreeView, TreeViewItem Item)
        {
            SelectNone(TreeView, null);
            SetIsItemSelected(Item, true);
            SetStartItem(TreeView, Item);
        }

        static void SelectItems(TreeViewItem Item, TreeView TreeView)
        {
            if (Item == null || TreeView == null)
                return;
            if ((Keyboard.Modifiers & (ModifierKeys.Control | ModifierKeys.Shift)) == (ModifierKeys.Control | ModifierKeys.Shift))
                SelectItemsContinuously(TreeView, Item, true);
            else if (Keyboard.Modifiers == ModifierKeys.Control)
                SelectItemsRandomly(TreeView, Item);
            else if (Keyboard.Modifiers == ModifierKeys.Shift)
                SelectItemsContinuously(TreeView, Item);
            else SelectItem(TreeView, Item);
        }

        static void SelectItemsContinuously(TreeView TreeView, TreeViewItem TreeViewItem, bool ShiftControl = false)
        {
            TreeViewItem StartItem = GetStartItem(TreeView);
            if (StartItem != null)
            {
                if (StartItem == TreeViewItem)
                {
                    SelectItem(TreeView, TreeViewItem);
                    return;
                }
                ICollection<TreeViewItem> AllItems = new List<TreeViewItem>();
                GetAllItems(TreeView, null, AllItems);
                //DeSelectAllItems(treeView, null);
                bool isBetween = false;
                foreach (var Item in AllItems)
                {
                    if (Item == TreeViewItem || Item == StartItem)
                    {
                        //Toggle to true if first element is found and back to false if last element is found
                        isBetween = !isBetween;
                        //Set boundary element
                        SetIsItemSelected(Item, true);
                        continue;
                    }
                    if (isBetween)
                    {
                        SetIsItemSelected(Item, true);
                        continue;
                    }
                    if (!ShiftControl)
                        SetIsItemSelected(Item, false);
                }
            }
        }

        static void SelectItemsRandomly(TreeView treeView, TreeViewItem treeViewItem)
        {
            SetIsItemSelected(treeViewItem, !GetIsItemSelected(treeViewItem));
            if (GetStartItem(treeView) == null || Keyboard.Modifiers == ModifierKeys.Control)
            {
                if (GetIsItemSelected(treeViewItem))
                    SetStartItem(treeView, treeViewItem);
            }
            else
            {
                if (GetSelectedItems(treeView).Count == 0)
                    SetStartItem(treeView, null);
            }
        }

        static void SelectNone(TreeView TreeView, TreeViewItem TreeViewItem = null)
        {
            if (TreeView != null)
            {
                for (int i = 0; i < TreeView.Items.Count; i++)
                {
                    var Item = TreeView.ItemContainerGenerator.ContainerFromIndex(i) as TreeViewItem;
                    if (Item != null)
                    {
                        SetIsItemSelected(Item, false);
                        SelectNone(null, Item);
                    }
                }
            }
            else if (TreeViewItem != null)
            {
                for (int i = 0; i < TreeViewItem.Items.Count; i++)
                {
                    var Item = TreeViewItem.ItemContainerGenerator.ContainerFromIndex(i) as TreeViewItem;
                    if (Item != null)
                    {
                        SetIsItemSelected(Item, false);
                        SelectNone(null, Item);
                    }
                }
            }
        }

        #endregion

        #region Events

        void OnSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            this.SelectedVisual = this.ItemContainerGenerator.ContainerFromItem(this.SelectedItem).As<TreeViewItem>();
        }

        static void OnTreeViewItemGotFocus(object sender, RoutedEventArgs e)
        {
            _selectTreeViewItemOnMouseUp = null;
            if (e.OriginalSource is TreeView) return;
            var Item = FindTreeViewItem(e.OriginalSource as DependencyObject);
            if (Mouse.LeftButton == MouseButtonState.Pressed && GetIsItemSelected(Item) && Keyboard.Modifiers != ModifierKeys.Control)
                _selectTreeViewItemOnMouseUp = Item;
            else SelectItems(Item, sender as TreeView);
        }

        static void OnTreeViewItemPreviewMouseDown(object sender, MouseEventArgs e)
        {
            var Item = FindTreeViewItem(e.OriginalSource as DependencyObject);
            if (Item != null && Item.IsFocused)
                OnTreeViewItemGotFocus(sender, e);
        }

        static void OnTreeViewItemPreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            var Item = FindTreeViewItem(e.OriginalSource as DependencyObject);
            if (Item == _selectTreeViewItemOnMouseUp)
                SelectItems(Item, sender as TreeView);
        }

        #endregion

        #endregion
    }
}