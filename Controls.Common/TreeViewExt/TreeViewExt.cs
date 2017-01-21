using Imagin.Common;
using Imagin.Common.Collections.ObjectModel;
using Imagin.Common.Extensions;
using Imagin.Common.Input;
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
    public class TreeViewExt : TreeView
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
        public static DependencyProperty CanResizeColumnsProperty = DependencyProperty.Register("CanResizeColumns", typeof(bool), typeof(TreeViewExt), new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
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
        public static DependencyProperty ColumnHeaderContextMenuProperty = DependencyProperty.Register("ColumnHeaderContextMenu", typeof(ContextMenu), typeof(TreeViewExt), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
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
        public static DependencyProperty ColumnHeaderHeightProperty = DependencyProperty.Register("ColumnHeaderHeight", typeof(double), typeof(TreeViewExt), new FrameworkPropertyMetadata(double.NaN, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
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
        public static DependencyProperty ColumnHeaderStyleProperty = DependencyProperty.Register("ColumnHeaderStyle", typeof(Style), typeof(TreeViewExt), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
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
        public static DependencyProperty ColumnHeaderStyleSelectorProperty = DependencyProperty.Register("ColumnHeaderStyleSelector", typeof(StyleSelector), typeof(TreeViewExt), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
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
        public static DependencyProperty ColumnHeaderTemplateProperty = DependencyProperty.Register("ColumnHeaderTemplate", typeof(DataTemplate), typeof(TreeViewExt), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
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
        public static DependencyProperty ColumnHeaderTemplateSelectorProperty = DependencyProperty.Register("ColumnHeaderTemplateSelector", typeof(DataTemplateSelector), typeof(TreeViewExt), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
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
        public static DependencyProperty ColumnHeaderStringFormatProperty = DependencyProperty.Register("ColumnHeaderStringFormat", typeof(string), typeof(TreeViewExt), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
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
        public static DependencyProperty ColumnHeaderVisibilityProperty = DependencyProperty.Register("ColumnHeaderVisibility", typeof(Visibility), typeof(TreeViewExt), new FrameworkPropertyMetadata(Visibility.Collapsed, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
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
        public static DependencyProperty ColumnsProperty = DependencyProperty.Register("Columns", typeof(TreeViewColumnCollection), typeof(TreeViewExt), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
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
        public static DependencyProperty SelectedIndexProperty = DependencyProperty.Register("SelectedIndex", typeof(int[]), typeof(TreeViewExt), new FrameworkPropertyMetadata(default(int[]), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnSelectedIndexChanged));
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
            d.As<TreeViewExt>().OnSelectedIndexChanged((int[])e.NewValue);
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty SelectedItemsProperty = DependencyProperty.Register("SelectedItems", typeof(TrackableCollection<object>), typeof(TreeViewExt), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
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
        public static DependencyProperty SelectedObjectProperty = DependencyProperty.Register("SelectedObject", typeof(object), typeof(TreeViewExt), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnSelectedObjectChanged));
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
            d.As<TreeViewExt>().OnSelectedObjectChanged(e.NewValue);
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty SelectedVisualProperty = DependencyProperty.Register("SelectedVisual", typeof(TreeViewItem), typeof(TreeViewExt), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
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

        #region TreeViewExt

        /// <summary>
        /// 
        /// </summary>
        public TreeViewExt() : base()
        {
            DefaultStyleKey = typeof(TreeViewExt);

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

        static void OnGotFocus(object sender, RoutedEventArgs e)
        {
            SelectedItemOnMouseUp = null;
            if (!e.OriginalSource.Is<TreeView>())
            {
                var Item = (e.OriginalSource as DependencyObject).GetVisualParent<TreeViewItem>();
                if (Item != null && Item.Is<TreeViewItem>())
                {
                    if (Mouse.LeftButton == MouseButtonState.Pressed && TreeViewItemExtensions.GetIsSelected(Item) && Keyboard.Modifiers != ModifierKeys.Control)
                        SelectedItemOnMouseUp = Item;
                    else if (sender is TreeViewExt && TreeViewExtensions.GetSelectionMode(sender as TreeView) == TreeViewSelectionMode.Single)
                        TreeViewExtensions.SelectItem(sender as TreeView, Item);
                    else (sender as TreeViewExt).SelectItems(Item);
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
                    //TO-DO: Find object based on given index
                    //SelectedObject = Result;
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
                    var m = new List<int>();

                    var c = SelectedVisual.To<ItemsControl>();
                    var p = default(ItemsControl);

                    while (true)
                    {
                        p = c.As<DependencyObject>().GetParent<ItemsControl>();

                        if (p != null)
                        {
                            m.Add(p.Items.IndexOf(c.DataContext));
                            c = p;
                        }
                        if (p == null || p is TreeView)
                            break;
                    }

                    SelectedIndexChangeHandled = true;
                    SelectedIndex = m.Reverse<int>().ToArray<int>();
                    SelectedIndexChangeHandled = false;
                }
                else
                {
                    Console.WriteLine("SelectedVisual == null");
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void OnSelectedItemsChanged(object sender, EventArgs e)
        {
            var Collection = new List<object>();
            SelectedItems.ForEach(i => Collection.Add(i));
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
                collapseAllCommand = collapseAllCommand ?? new RelayCommand(x => this.CollapseAll(), true);
                return collapseAllCommand;
            }
        }

        #endregion
    }
}