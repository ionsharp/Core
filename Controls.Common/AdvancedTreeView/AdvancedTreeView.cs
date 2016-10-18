using Imagin.Common.Events;
using Imagin.Common.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using Imagin.Common.Collections;

namespace Imagin.Controls.Common
{
    /// <summary>
    /// An advanced TreeView.
    /// </summary>
    /// <remarks>
    /// Multi selection behavior borrowed from https://github.com/cmyksvoll/MultiSelectTreeView.
    /// </remarks>
    public class AdvancedTreeView : TreeView, IDragSelector
    {
        #region Properties

        static TreeViewItem SelectedItemOnMouseUp;

        public event EventHandler<EventArgs<IList>> SelectedItemsChanged;

        #region Attached

        public static readonly DependencyProperty IsItemSelectedProperty = DependencyProperty.RegisterAttached("IsItemSelected", typeof(bool), typeof(AdvancedTreeView), new PropertyMetadata(false, OnIsItemSelectedPropertyChanged));
        public static bool GetIsItemSelected(TreeViewItem Item)
        {
            return (bool)Item.GetValue(IsItemSelectedProperty);
        }
        public static void SetIsItemSelected(TreeViewItem Item, bool Value)
        {
            if (Item != null)
                Item.SetValue(IsItemSelectedProperty, Value);
        }
        static void OnIsItemSelectedPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var TreeViewItem = d as TreeViewItem;
            var TreeView = FindTreeView(TreeViewItem);
            if (TreeView != null)
            {
                if (TreeViewItem != null)
                {
                    var SelectedItems = GetSelectedItems(TreeView);
                    if (SelectedItems != null)
                    {
                        if (GetIsItemSelected(TreeViewItem))
                            SelectedItems.Add(TreeViewItem.Header);
                        else SelectedItems.Remove(TreeViewItem.Header);
                    }
                }
                if (TreeView.Is<AdvancedTreeView>())
                {
                    var AdvancedTreeView = TreeView.As<AdvancedTreeView>();
                    if (AdvancedTreeView.SelectedItemsChanged != null)
                        AdvancedTreeView.SelectedItemsChanged(AdvancedTreeView, new EventArgs<IList>(GetSelectedItems(TreeView)));
                }
            }
        }

        public static readonly DependencyProperty SelectedItemsProperty = DependencyProperty.RegisterAttached("SelectedItems", typeof(IList), typeof(AdvancedTreeView));
        public static IList GetSelectedItems(TreeView TreeView)
        {
            return (IList)TreeView.GetValue(SelectedItemsProperty);
        }
        public static void SetSelectedItems(TreeView TreeView, IList Value)
        {
            TreeView.SetValue(SelectedItemsProperty, Value);
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

        #region Dependency

        public static DependencyProperty CanResizeColumnsProperty = DependencyProperty.Register("CanResizeColumns", typeof(bool), typeof(AdvancedTreeView), new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public bool CanResizeColumns
        {
            get
            {
                return (bool)GetValue(CanResizeColumnsProperty);
            }
            set
            {
                SetValue(CanResizeColumnsProperty, value);
            }
        }

        public static DependencyProperty ColumnHeaderHeightProperty = DependencyProperty.Register("ColumnHeaderHeight", typeof(double), typeof(AdvancedTreeView), new FrameworkPropertyMetadata(double.NaN, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public double ColumnHeaderHeight
        {
            get
            {
                return (double)GetValue(ColumnHeaderHeightProperty);
            }
            set
            {
                SetValue(ColumnHeaderHeightProperty, value);
            }
        }

        public static DependencyProperty ColumnHeaderStyleProperty = DependencyProperty.Register("ColumnHeaderStyle", typeof(Style), typeof(AdvancedTreeView), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public Style ColumnHeaderStyle
        {
            get
            {
                return (Style)GetValue(ColumnHeaderStyleProperty);
            }
            set
            {
                SetValue(ColumnHeaderStyleProperty, value);
            }
        }

        public static DependencyProperty ColumnHeaderStyleSelectorProperty = DependencyProperty.Register("ColumnHeaderStyleSelector", typeof(StyleSelector), typeof(AdvancedTreeView), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public StyleSelector ColumnHeaderStyleSelector
        {
            get
            {
                return (StyleSelector)GetValue(ColumnHeaderStyleSelectorProperty);
            }
            set
            {
                SetValue(ColumnHeaderStyleSelectorProperty, value);
            }
        }

        public static DependencyProperty ColumnHeaderTemplateProperty = DependencyProperty.Register("ColumnHeaderTemplate", typeof(DataTemplate), typeof(AdvancedTreeView), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public DataTemplate ColumnHeaderTemplate
        {
            get
            {
                return (DataTemplate)GetValue(ColumnHeaderTemplateProperty);
            }
            set
            {
                SetValue(ColumnHeaderTemplateProperty, value);
            }
        }

        public static DependencyProperty ColumnHeaderTemplateSelectorProperty = DependencyProperty.Register("ColumnHeaderTemplateSelector", typeof(DataTemplate), typeof(AdvancedTreeView), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public DataTemplateSelector ColumnHeaderTemplateSelector
        {
            get
            {
                return (DataTemplateSelector)GetValue(ColumnHeaderTemplateSelectorProperty);
            }
            set
            {
                SetValue(ColumnHeaderTemplateSelectorProperty, value);
            }
        }

        public static DependencyProperty ColumnHeaderStringFormatProperty = DependencyProperty.Register("ColumnHeaderStringFormat", typeof(string), typeof(AdvancedTreeView), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public string ColumnHeaderStringFormat
        {
            get
            {
                return (string)GetValue(ColumnHeaderStringFormatProperty);
            }
            set
            {
                SetValue(ColumnHeaderStringFormatProperty, value);
            }
        }

        public static DependencyProperty ColumnHeaderVisibilityProperty = DependencyProperty.Register("ColumnHeaderVisibility", typeof(Visibility), typeof(AdvancedTreeView), new FrameworkPropertyMetadata(Visibility.Collapsed, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public Visibility ColumnHeaderVisibility
        {
            get
            {
                return (Visibility)GetValue(ColumnHeaderVisibilityProperty);
            }
            set
            {
                SetValue(ColumnHeaderVisibilityProperty, value);
            }
        }

        public static DependencyProperty CollapseSiblingsOnClickProperty = DependencyProperty.Register("CollapseSiblingsOnClick", typeof(bool), typeof(AdvancedTreeView), new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public bool CollapseSiblingsOnClick
        {
            get
            {
                return (bool)GetValue(CollapseSiblingsOnClickProperty);
            }
            set
            {
                SetValue(CollapseSiblingsOnClickProperty, value);
            }
        }

        public static DependencyProperty ColumnsProperty = DependencyProperty.Register("Columns", typeof(TreeViewColumnCollection), typeof(AdvancedTreeView), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public TreeViewColumnCollection Columns
        {
            get
            {
                return (TreeViewColumnCollection)GetValue(ColumnsProperty);
            }
            set
            {
                SetValue(ColumnsProperty, value);
            }
        }

        public static DependencyProperty ColumnVisibilityProperty = DependencyProperty.Register("ColumnVisibility", typeof(Visibility), typeof(AdvancedTreeView), new FrameworkPropertyMetadata(Visibility.Collapsed, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public Visibility ColumnVisibility
        {
            get
            {
                return (Visibility)GetValue(ColumnVisibilityProperty);
            }
            set
            {
                SetValue(ColumnVisibilityProperty, value);
            }
        }

        public static DependencyProperty DragScrollOffsetProperty = DependencyProperty.Register("DragScrollOffset", typeof(double), typeof(AdvancedTreeView), new FrameworkPropertyMetadata(10.0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public double DragScrollOffset
        {
            get
            {
                return (double)GetValue(DragScrollOffsetProperty);
            }
            set
            {
                SetValue(DragScrollOffsetProperty, value);
            }
        }

        public static DependencyProperty DragScrollToleranceProperty = DependencyProperty.Register("DragScrollTolerance", typeof(double), typeof(AdvancedTreeView), new FrameworkPropertyMetadata(5.0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public double DragScrollTolerance
        {
            get
            {
                return (double)GetValue(DragScrollToleranceProperty);
            }
            set
            {
                SetValue(DragScrollToleranceProperty, value);
            }
        }

        public static DependencyProperty DragSelectionProperty = DependencyProperty.Register("DragSelection", typeof(Selection), typeof(AdvancedTreeView), new FrameworkPropertyMetadata(default(Selection), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public Selection DragSelection
        {
            get
            {
                return (Selection)GetValue(DragSelectionProperty);
            }
            set
            {
                SetValue(DragSelectionProperty, value);
            }
        }

        public static DependencyProperty DragSelectorProperty = DependencyProperty.Register("DragSelector", typeof(DragSelector), typeof(AdvancedTreeView), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public DragSelector DragSelector
        {
            get
            {
                return (DragSelector)GetValue(DragSelectorProperty);
            }
            set
            {
                SetValue(DragSelectorProperty, value);
            }
        }

        public static DependencyProperty ExpandOnClickProperty = DependencyProperty.Register("ExpandOnClick", typeof(bool), typeof(AdvancedTreeView), new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public bool ExpandOnClick
        {
            get
            {
                return (bool)GetValue(ExpandOnClickProperty);
            }
            set
            {
                SetValue(ExpandOnClickProperty, value);
            }
        }

        public static DependencyProperty IsDragSelectionEnabledProperty = DependencyProperty.Register("IsDragSelectionEnabled", typeof(bool), typeof(AdvancedTreeView), new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public bool IsDragSelectionEnabled
        {
            get
            {
                return (bool)GetValue(IsDragSelectionEnabledProperty);
            }
            set
            {
                SetValue(IsDragSelectionEnabledProperty, value);
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

        public static DependencyProperty SelectionModeProperty = DependencyProperty.Register("SelectionMode", typeof(DataGridSelectionMode), typeof(AdvancedTreeView), new FrameworkPropertyMetadata(DataGridSelectionMode.Extended, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnSelectionModeChanged));
        public DataGridSelectionMode SelectionMode
        {
            get
            {
                return (DataGridSelectionMode)GetValue(SelectionModeProperty);
            }
            set
            {
                SetValue(SelectionModeProperty, value);
            }
        }
        static void OnSelectionModeChanged(DependencyObject Object, DependencyPropertyChangedEventArgs e)
        {
            ((AdvancedTreeView)Object).OnSelectionModeChanged((DataGridSelectionMode)e.NewValue);
        }
        
        public static DependencyProperty SelectOnRightClickProperty = DependencyProperty.Register("SelectOnRightClick", typeof(bool), typeof(AdvancedTreeView), new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public bool SelectOnRightClick
        {
            get
            {
                return (bool)GetValue(SelectOnRightClickProperty);
            }
            set
            {
                SetValue(SelectOnRightClickProperty, value);
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

        #endregion

        #endregion

        #region AdvancedTreeView

        public AdvancedTreeView()
        {
            this.DefaultStyleKey = typeof(AdvancedTreeView);

            this.Columns = new TreeViewColumnCollection();

            this.GotFocus += OnGotFocus;

            this.SelectedItemChanged += (s, e) => this.SelectedVisual = this.ItemContainerGenerator.ContainerFromItem(this.SelectedItem).As<TreeViewItem>();
        }

        #endregion

        #region Methods

        #region Events

        static void OnGotFocus(object sender, RoutedEventArgs e)
        {
            SelectedItemOnMouseUp = null;
            if (!e.OriginalSource.Is<TreeView>())
            {
                var Item = FindTreeViewItem(e.OriginalSource as DependencyObject);
                if (Item != null && Item.Is<TreeViewItem>())
                {
                    if (Mouse.LeftButton == MouseButtonState.Pressed && GetIsItemSelected(Item) && Keyboard.Modifiers != ModifierKeys.Control)
                        SelectedItemOnMouseUp = Item;
                    else if (sender is AdvancedTreeView && sender.As<AdvancedTreeView>().SelectionMode == DataGridSelectionMode.Single)
                        SelectItem(sender as TreeView, Item);
                    else (sender as AdvancedTreeView).SelectItems(Item);
                }
            }
        }

        #endregion

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
            TreeViewItem Item = e.OriginalSource.As<DependencyObject>().GetVisualParent<TreeViewItem>();
            if (Item == null) return;
            if (ExpandOnClick)
                Item.IsExpanded = !Item.IsExpanded;
            if (CollapseSiblingsOnClick)
                Item.CollapseSiblings();
        }

        protected override void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseLeftButtonDown(e);

            var Item = FindTreeViewItem(e.OriginalSource as DependencyObject);
            if (this.SelectionMode == DataGridSelectionMode.Extended && Item != null && Item.IsFocused)
                OnGotFocus(this, e);
        }

        protected override void OnPreviewMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseLeftButtonUp(e);

            var Item = FindTreeViewItem(e.OriginalSource as DependencyObject);
            if (this.SelectionMode == DataGridSelectionMode.Extended && Item == SelectedItemOnMouseUp)
                this.SelectItems(Item);
        }

        protected override void OnPreviewMouseRightButtonDown(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseRightButtonDown(e);
            if (!this.SelectOnRightClick)
                return;
            var Item = e.OriginalSource.As<DependencyObject>().GetVisualParent<TreeViewItem>();
            if (Item == null) return;
            Item.IsSelected = true;
            e.Handled = true;
        }

        public override void OnApplyTemplate()
        {
            base.ApplyTemplate();
            this.DragSelector = new DragSelector(this);
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
            if (DependencyObject.Is<Visual>() || DependencyObject.Is<Visual3D>())
            {
                var Item = DependencyObject as TreeViewItem;
                if (Item != null) return Item;
                return FindTreeViewItem(VisualTreeHelper.GetParent(DependencyObject));
            }
            return null;
        }

        static void GetAllItems(ItemsControl Control, ICollection<TreeViewItem> AllItems)
        {
            if (Control != null)
            {
                for (int i = 0; i < Control.Items.Count; i++)
                {
                    var Item = Control.ItemContainerGenerator.ContainerFromIndex(i) as TreeViewItem;
                    if (Item != null)
                    {
                        AllItems.Add(Item);
                        GetAllItems(Item, AllItems);
                    }
                }
            }
        }

        static void SelectItem(TreeView TreeView, TreeViewItem Item)
        {
            SelectNone(TreeView);
            SetIsItemSelected(Item, true);
            SetStartItem(TreeView, Item);
        }

        void SelectItems(TreeViewItem Item)
        {
            if (Item == null) return;

            if ((Keyboard.Modifiers & (ModifierKeys.Control | ModifierKeys.Shift)) == (ModifierKeys.Control | ModifierKeys.Shift))
                SelectItemsContinuously(this, Item, true);
            else if (Keyboard.Modifiers == ModifierKeys.Control)
                SelectItemsRandomly(this, Item);
            else if (Keyboard.Modifiers == ModifierKeys.Shift)
                SelectItemsContinuously(this, Item);
            else SelectItem(this, Item);
        }

        static void SelectItemsContinuously(TreeView TreeView, TreeViewItem TreeViewItem, bool ShiftControl = false)
        {
            var StartItem = GetStartItem(TreeView);
            if (StartItem != null)
            {
                if (StartItem == TreeViewItem)
                {
                    SelectItem(TreeView, TreeViewItem);
                    return;
                }
                var AllItems = new List<TreeViewItem>();
                GetAllItems(TreeView, AllItems);

                bool IsBetween = false;
                foreach (var Item in AllItems)
                {
                    if (Item == TreeViewItem || Item == StartItem)
                    {
                        //Toggle to true if first element is found and back to false if last element is found
                        IsBetween = !IsBetween;
                        //Set boundary element
                        SetIsItemSelected(Item, true);
                        continue;
                    }
                    if (IsBetween)
                    {
                        SetIsItemSelected(Item, true);
                        continue;
                    }
                    if (!ShiftControl)
                        SetIsItemSelected(Item, false);
                }
            }
        }

        static void SelectItemsRandomly(TreeView TreeView, TreeViewItem TreeViewItem)
        {
            SetIsItemSelected(TreeViewItem, !GetIsItemSelected(TreeViewItem));
            if (GetStartItem(TreeView) == null || Keyboard.Modifiers == ModifierKeys.Control)
            {
                if (GetIsItemSelected(TreeViewItem))
                    SetStartItem(TreeView, TreeViewItem);
            }
            else
            {
                if (GetSelectedItems(TreeView).Count == 0)
                    SetStartItem(TreeView, null);
            }
        }

        static void SelectNone(ItemsControl Control)
        {
            if (Control != null)
            {
                for (int i = 0; i < Control.Items.Count; i++)
                {
                    var Item = Control.ItemContainerGenerator.ContainerFromIndex(i) as TreeViewItem;
                    if (Item != null)
                    {
                        SetIsItemSelected(Item, false);
                        SelectNone(Item);
                    }
                }
            }
        }

        #endregion

        #region Virtual

        protected virtual void OnSelectionModeChanged(DataGridSelectionMode Value)
        {
            if (Value == DataGridSelectionMode.Single)
            {
                var SelectedItems = GetSelectedItems(this);
                if (SelectedItems != null && SelectedItems.Count > 1)
                    SelectItem(this, this.ItemContainerGenerator.ContainerFromItem(SelectedItems[0]).As<TreeViewItem>());
            }
        }

        #endregion

        #endregion
    }
}