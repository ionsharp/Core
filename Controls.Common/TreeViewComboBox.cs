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

        bool IgnoreSelectedItemChange = false;

        public event EventHandler<ObjectEventArgs> SelectedItemChanged;

        TreeView TreeView
        {
            get; set;
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

        public static DependencyProperty ContentTemplateProperty = DependencyProperty.Register("ContentTemplate", typeof(DataTemplate), typeof(TreeViewComboBox), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public DataTemplate ContentTemplate
        {
            get
            {
                return (DataTemplate)GetValue(ContentTemplateProperty);
            }
            set
            {
                SetValue(ContentTemplateProperty, value);
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
            if (TreeViewComboBox.IgnoreSelectedItemChange)
            {
                TreeViewComboBox.IgnoreSelectedItemChange = false;
                return;
            }
            TreeViewComboBox.Select(TreeViewComboBox.SelectedItem);
        }

        #endregion

        #region TreeViewComboBox

        public TreeViewComboBox() : base()
        {
            this.DefaultStyleKey = typeof(TreeViewComboBox);
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
                this.TreeView.SelectedItemChanged += OnSelectedItemChanged;
            }

            Popup Popup = this.Template.FindName("PART_Popup", this) as Popup;
            if (Popup != null)
                Popup.LostFocus += (s, e) => this.IsDropDownOpen = false;

            this.Select(this.SelectedItem);
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

        void OnSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (this.TreeView == null)
                return;
            this.IgnoreSelectedItemChange = true;
            this.SelectedItem = e.NewValue;
            if (this.SelectedItemChanged != null)
                this.SelectedItemChanged(this, new ObjectEventArgs(this.SelectedItem));
        }

        #endregion

        #endregion
    }
}
