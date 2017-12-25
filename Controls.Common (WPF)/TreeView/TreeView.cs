using Imagin.Common.Collections.Generic;
using Imagin.Common.Input;
using Imagin.Common.Linq;
using Imagin.Controls.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Imagin.Controls.Common
{
    /// <summary>
    /// An extended version of <see cref="TreeView"/>.
    /// </summary>
    /// <remarks>
    /// Multi-selection borrowed from https://github.com/cmyksvoll/MultiSelectTreeView.
    /// </remarks>
    public class TreeView : System.Windows.Controls.TreeView
    {
        #region Properties

        static TreeViewItem SelectedItemOnMouseUp;

        bool SelectedIndexChangeHandled = false;

        bool SelectedItemChangeHandled = false;

        bool SelectedObjectChangeHandled = false;

        /// <summary>
        /// Occurs when one or more items are selected or unselected.
        /// </summary>
        public event EventHandler<EventArgs<IList<object>>> SelectedItemsChanged;

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty CanResizeColumnsProperty = DependencyProperty.Register("CanResizeColumns", typeof(bool), typeof(TreeView), new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
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

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty ColumnHeaderContextMenuProperty = DependencyProperty.Register("ColumnHeaderContextMenu", typeof(ContextMenu), typeof(TreeView), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
        public ContextMenu ColumnHeaderContextMenu
        {
            get
            {
                return (ContextMenu)GetValue(ColumnHeaderContextMenuProperty);
            }
            set
            {
                SetValue(ColumnHeaderContextMenuProperty, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty ColumnHeaderHeightProperty = DependencyProperty.Register("ColumnHeaderHeight", typeof(double), typeof(TreeView), new FrameworkPropertyMetadata(double.NaN, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
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

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty ColumnHeaderStyleProperty = DependencyProperty.Register("ColumnHeaderStyle", typeof(Style), typeof(TreeView), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
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

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty ColumnHeaderStyleSelectorProperty = DependencyProperty.Register("ColumnHeaderStyleSelector", typeof(StyleSelector), typeof(TreeView), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
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

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty ColumnHeaderTemplateProperty = DependencyProperty.Register("ColumnHeaderTemplate", typeof(DataTemplate), typeof(TreeView), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
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

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty ColumnHeaderTemplateSelectorProperty = DependencyProperty.Register("ColumnHeaderTemplateSelector", typeof(DataTemplateSelector), typeof(TreeView), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
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

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty ColumnHeaderStringFormatProperty = DependencyProperty.Register("ColumnHeaderStringFormat", typeof(string), typeof(TreeView), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
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

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty ColumnHeaderVisibilityProperty = DependencyProperty.Register("ColumnHeaderVisibility", typeof(Visibility), typeof(TreeView), new FrameworkPropertyMetadata(Visibility.Collapsed, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
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

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty ColumnsProperty = DependencyProperty.Register("Columns", typeof(TreeViewColumnCollection), typeof(TreeView), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
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

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty SelectedIndexProperty = DependencyProperty.Register("SelectedIndex", typeof(int[]), typeof(TreeView), new FrameworkPropertyMetadata(default(int[]), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnSelectedIndexChanged));
        /// <summary>
        /// 
        /// </summary>
        public int[] SelectedIndex
        {
            get
            {
                return (int[])GetValue(SelectedIndexProperty);
            }
            set
            {
                SetValue(SelectedIndexProperty, value);
            }
        }
        static void OnSelectedIndexChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            d.As<TreeView>().OnSelectedIndexChanged((int[])e.NewValue);
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty SelectedItemsProperty = DependencyProperty.Register("SelectedItems", typeof(TrackableCollection<object>), typeof(TreeView), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// Get or set list of selected items.
        /// </summary>
        public TrackableCollection<object> SelectedItems
        {
            get
            {
                return (TrackableCollection<object>)GetValue(SelectedItemsProperty);
            }
            set
            {
                SetValue(SelectedItemsProperty, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty SelectedObjectProperty = DependencyProperty.Register("SelectedObject", typeof(object), typeof(TreeView), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnSelectedObjectChanged));
        /// <summary>
        /// Get or set selected object.
        /// </summary>
        public object SelectedObject
        {
            get
            {
                return GetValue(SelectedObjectProperty);
            }
            set
            {
                SetValue(SelectedObjectProperty, value);
            }
        }
        static void OnSelectedObjectChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            d.As<TreeView>().OnSelectedObjectChanged(e.NewValue);
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty SelectedVisualProperty = DependencyProperty.Register("SelectedVisual", typeof(TreeViewItem), typeof(TreeView), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// Get or set visual associated with selected object.
        /// </summary>
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

        #endregion

        #region TreeView

        /// <summary>
        /// 
        /// </summary>
        public TreeView() : base()
        {
            DefaultStyleKey = typeof(TreeView);

            Columns = new TreeViewColumnCollection();
            SelectedIndex = new int[1] { -1 };
            SelectedItems = new TrackableCollection<object>();

            GotFocus += OnGotFocus;

            SelectedItemChanged += OnSelectedItemChanged;
            SelectedItems.ItemsChanged += OnSelectedItemsChanged;

            this.Bind(TreeViewExtensions.SelectedItemsProperty, this, "SelectedItems");
        }

        #endregion

        #region Methods

        static IEnumerable<int> GetSelectedIndex(TreeViewItem Item)
        {
            var Source = Item.To<ItemsControl>();
            var Parent = default(ItemsControl);

            while (true)
            {
                Parent = Source.As<DependencyObject>().GetParent<ItemsControl>();

                if (Parent != null)
                {
                    yield return Parent.Items.IndexOf(Source.DataContext);
                    Source = Parent;
                }

                if (Parent == null || Parent is TreeView)
                    break;
            }

            yield break;
        }

        static void OnGotFocus(object sender, RoutedEventArgs e)
        {
            SelectedItemOnMouseUp = null;
            if (!e.OriginalSource.Is<TreeView>())
            {
                var Item = (e.OriginalSource as DependencyObject).GetVisualParent<TreeViewItem>();
                if (Item != null && Item.Is<TreeViewItem>())
                {
                    if (Mouse.LeftButton == MouseButtonState.Pressed && TreeViewItemExtensions.GetIsSelected(Item) && Keyboard.Modifiers != ModifierKeys.Control)
                    {
                        SelectedItemOnMouseUp = Item;
                    }
                    else if (sender is TreeView && TreeViewExtensions.GetSelectionMode(sender as TreeView) == TreeViewSelectionMode.Single)
                    {
                        TreeViewExtensions.SelectItem(sender as TreeView, Item);
                    }
                    else (sender as TreeView).SelectItems(Item);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseLeftButtonDown(e);

            var Item = (e.OriginalSource as DependencyObject).GetVisualParent<TreeViewItem>();
            if (TreeViewExtensions.GetSelectionMode(this) == TreeViewSelectionMode.Extended && Item != null && Item.IsFocused)
                OnGotFocus(this, e);
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void OnPreviewMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseLeftButtonUp(e);

            var Item = (e.OriginalSource as DependencyObject).GetVisualParent<TreeViewItem>();
            if (TreeViewExtensions.GetSelectionMode(this) == TreeViewSelectionMode.Extended && Item == SelectedItemOnMouseUp)
                this.SelectItems(Item);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        protected virtual void OnSelectedIndexChanged(int[] Value)
        {
            if (!SelectedIndexChangeHandled)
            {
                if (Items.Count > 0)
                {
                    var Result = default(object);
                    this.Enumerate((i, j) => Result = i, Value);
                    SelectedObject = Result;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void OnSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (!SelectedItemChangeHandled)
            {
                SelectedObjectChangeHandled = true;
                SelectedObject = e.NewValue;
                SelectedObjectChangeHandled = false;

                SelectedVisual = null;

                this.Enumerate((i, j) =>
                {
                    if (i == e.NewValue)
                    {
                        SelectedVisual = j as TreeViewItem;
                        return null;
                    }
                    return true;
                });

                if (SelectedVisual != null)
                {
                    SelectedIndexChangeHandled = true;
                    SelectedIndex = GetSelectedIndex(SelectedVisual).Reverse<int>().ToArray<int>();
                    SelectedIndexChangeHandled = false;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void OnSelectedItemsChanged(object sender, EventArgs e)
        {
            var Collection = new List<object>();
            SelectedItems.ForEach<object>(i => Collection.Add(i));
            SelectedItemsChanged?.Invoke(this, new EventArgs<IList<object>>(Collection));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        protected virtual void OnSelectedObjectChanged(object Value)
        {
            if (!SelectedObjectChangeHandled)
            {
                SelectedItemChangeHandled = true;
                this.Select(Value);
                SelectedItemChangeHandled = false;
            }
        }

        ICommand collapseAllCommand;
        /// <summary>
        /// 
        /// </summary>
        public ICommand CollapseAllCommand
        {
            get
            {
                collapseAllCommand = collapseAllCommand ?? new RelayCommand(() => this.CollapseAll(), () => true);
                return collapseAllCommand;
            }
        }

        #endregion
    }
}