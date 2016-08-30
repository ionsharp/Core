using Imagin.Common;
using Imagin.Common.Collections;
using Imagin.Common.Events;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace Imagin.Controls.Common
{
    public class TreeViewComboBox : ComboBox
    {
        #region Properties

        bool Switch = false;

        public event EventHandler<ObjectEventArgs> SelectedItemChanged;

        TreeView TreeView
        {
            get; set;
        }

        public static DependencyProperty ItemsProperty = DependencyProperty.Register("Items", typeof(ConcurrentObservableCollection<NamedObject>), typeof(TreeViewComboBox), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public new ConcurrentObservableCollection<NamedObject> Items
        {
            get
            {
                return (ConcurrentObservableCollection<NamedObject>)GetValue(ItemsProperty);
            }
            set
            {
                SetValue(ItemsProperty, value);
            }
        }

        public new static DependencyProperty SelectedIndexProperty = DependencyProperty.Register("SelectedIndex", typeof(int), typeof(TreeViewComboBox), new FrameworkPropertyMetadata(-1, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnSelectedIndexChanged));
        public new int SelectedIndex
        {
            get
            {
                return (int)GetValue(SelectedIndexProperty);
            }
            set
            {
                SetValue(SelectedIndexProperty, value);
            }
        }
        private static void OnSelectedIndexChanged(DependencyObject Object, DependencyPropertyChangedEventArgs e)
        {
        }

        public new static DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(TreeViewComboBox), new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public new string Text
        {
            get
            {
                return (string)GetValue(TextProperty);
            }
            set
            {
                SetValue(TextProperty, value);
            }
        }

        public new static DependencyProperty SelectedItemProperty = DependencyProperty.Register("SelectedItem", typeof(object), typeof(TreeViewComboBox), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnSelectedItemChanged));
        public new object SelectedItem
        {
            get
            {
                return (NamedObject)GetValue(SelectedItemProperty);
            }
            set
            {
                SetValue(SelectedItemProperty, value);
            }
        }
        private static void OnSelectedItemChanged(DependencyObject Object, DependencyPropertyChangedEventArgs e)
        {
            TreeViewComboBox TreeViewComboBox = Object as TreeViewComboBox;
            if (TreeViewComboBox.SelectedItem is NamedObject)
                TreeViewComboBox.Text = (TreeViewComboBox.SelectedItem as NamedObject).Name;
            if (TreeViewComboBox.Switch)
            {
                TreeViewComboBox.Switch = false;
                return;
            }
            TreeViewComboBox.Select(TreeViewComboBox.SelectedItem);
        }

        #endregion

        #region TreeViewComboBox

        public TreeViewComboBox() : base()
        {
            this.DefaultStyleKey = typeof(TreeViewComboBox);
            this.Loaded += TreeViewComboBox_Loaded;
        }

        private void TreeViewComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            //Item is usually set before the template has a chance to load.
            //Because of this, initial set will always fail and so
            //we attempt again once everything loads.
            this.Select(this.SelectedItem);
        }

        #endregion

        #region Methods

        #region Overrides

        public override void OnApplyTemplate()
        {
            base.ApplyTemplate();

            TreeView TreeView = this.Template.FindName("PART_TreeView", this) as TreeView;
            if (TreeView != null)
            {
                this.TreeView = TreeView;
                this.TreeView.Resources = this.Resources;
                this.TreeView.SelectedItemChanged += TreeView_SelectedItemChanged;
            }

            Popup Popup = this.Template.FindName("PART_Popup", this) as Popup;
            if (Popup != null)
                Popup.LostFocus += Popup_LostFocus;
        }

        #endregion

        #region Private

        void Select(object Item)
        {
            if (this.TreeView == null || Item == null)
                return;
            TreeViewItem t = (TreeViewItem)this.TreeView.ItemContainerGenerator.ContainerFromItem(Item);
            if (t != null)
                t.IsSelected = true;
        }

        #endregion

        #region Events

        void Popup_LostFocus(object sender, RoutedEventArgs e)
        {
            this.IsDropDownOpen = false;
        }

        void TreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            object Item = (sender as TreeView).SelectedItem;
            this.Switch = true;
            this.SelectedItem = Item;
            if (this.SelectedItemChanged != null)
                this.SelectedItemChanged(this, new ObjectEventArgs(this.SelectedItem));
        }

        #endregion

        #endregion
    }
}
