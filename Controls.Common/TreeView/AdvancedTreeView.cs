using Imagin.Common.Collections.ObjectModel;
using Imagin.Common.Extensions;
using Imagin.Controls.Common.Extensions;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using Imagin.Common.Input;
using Imagin.Common.Events;
using System.Collections.Generic;

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

        static TreeViewItem SelectedItemOnMouseUp;

        public event EventHandler<EventArgs<IList<object>>> SelectedItemsChanged;

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
        
        public static DependencyProperty ColumnHeaderContextMenuProperty = DependencyProperty.Register("ColumnHeaderContextMenu", typeof(ContextMenu), typeof(AdvancedTreeView), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
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

        public static DependencyProperty SelectedItemsProperty = DependencyProperty.Register("SelectedItems", typeof(TrackableCollection<object>), typeof(AdvancedTreeView), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// Gets list of selected items.
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

        public static DependencyProperty SelectedVisualProperty = DependencyProperty.Register("SelectedVisual", typeof(TreeViewItem), typeof(AdvancedTreeView), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// Gets TreeViewItem associated with selected data object.
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

        #region AdvancedTreeView

        public AdvancedTreeView()
        {
            this.DefaultStyleKey = typeof(AdvancedTreeView);

            this.Columns = new TreeViewColumnCollection();

            this.GotFocus += OnGotFocus;

            this.SelectedItemChanged += (s, e) => this.SelectedVisual = this.ItemContainerGenerator.ContainerFromItem(this.SelectedItem).As<TreeViewItem>();

            this.SelectedItems = new TrackableCollection<object>();
            this.SelectedItems.ItemsChanged += OnSelectedItemsChanged;

            BindingOperations.SetBinding(this, TreeViewExtensions.SelectedItemsProperty, new Binding()
            {
                Mode = BindingMode.TwoWay,
                Path = new PropertyPath("SelectedItems"),
                Source = this
            });
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
                    else if (sender is AdvancedTreeView && TreeViewExtensions.GetSelectionMode(sender as TreeView) == TreeViewSelectionMode.Single)
                        TreeViewExtensions.SelectItem(sender as TreeView, Item);
                    else (sender as AdvancedTreeView).SelectItems(Item);
                }
            }
        }

        protected override void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseLeftButtonDown(e);

            var Item = (e.OriginalSource as DependencyObject).GetVisualParent<TreeViewItem>();
            if (TreeViewExtensions.GetSelectionMode(this) == TreeViewSelectionMode.Extended && Item != null && Item.IsFocused)
                OnGotFocus(this, e);
        }

        protected override void OnPreviewMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseLeftButtonUp(e);

            var Item = (e.OriginalSource as DependencyObject).GetVisualParent<TreeViewItem>();
            if (TreeViewExtensions.GetSelectionMode(this) == TreeViewSelectionMode.Extended && Item == SelectedItemOnMouseUp)
                this.SelectItems(Item);
        }

        protected virtual void OnSelectedItemsChanged(object sender, EventArgs e)
        {
            var Collection = new List<object>();
            foreach (var i in SelectedItems)
                Collection.Add(i);

            if (this.SelectedItemsChanged != null)
                this.SelectedItemsChanged(this, new EventArgs<IList<object>>(Collection));
        }

        #endregion
    }
}