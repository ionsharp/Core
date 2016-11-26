using Imagin.Common.Events;
using Imagin.Controls.Common.Extensions;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Imagin.Controls.Common
{
    [TemplatePart(Name = "PART_TreeView", Type = typeof(TreeView))]
    public class TreeViewComboBox : ComboBox
    {
        #region Properties

        public event EventHandler<EventArgs<object>> SelectedItemChanged;

        bool SelectionChangeHandled
        {
            get; set;
        }

        bool SelectedItemChangeHandled
        {
            get; set;
        }

        TreeView PART_TreeView
        {
            get; set;
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

        #endregion

        #region TreeViewComboBox

        public TreeViewComboBox() : base()
        {
            this.DefaultStyleKey = typeof(TreeViewComboBox);

            this.SelectionChanged += OnSelectionChanged;
        }

        #endregion

        #region Methods

        protected virtual void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!this.SelectionChangeHandled && e.AddedItems.Count > 0)
            {
                this.SelectedItemChangeHandled = true;
                this.Select(this.PART_TreeView, e.AddedItems[0]);
                this.SelectedItemChangeHandled = false;
            }
        }

        protected virtual void OnSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (!this.SelectedItemChangeHandled && !e.Handled)
            {
                e.Handled = true;

                this.SelectionChangeHandled = true;
                SelectedValue = PART_TreeView.SelectedItem;
                this.SelectionChangeHandled = false;

                this.IsDropDownOpen = false;
            }

            if (this.SelectedItemChanged != null)
                this.SelectedItemChanged(this, new EventArgs<object>(e.NewValue));
        }

        public override void OnApplyTemplate()
        {
            base.ApplyTemplate();

            var TreeView = this.Template.FindName("PART_TreeView", this) as TreeView;
            if (TreeView != null)
            {
                this.PART_TreeView = TreeView;
                this.PART_TreeView.Resources = this.Resources;
                this.PART_TreeView.SelectedItemChanged += OnSelectedItemChanged;
            }
        }

        void Select(ItemsControl ItemsControl, object ToSelect)
        {
            if (ItemsControl != null)
            {
                foreach (var i in ItemsControl.Items)
                {
                    var j = (ItemsControl)ItemsControl.ItemContainerGenerator.ContainerFromItem(i);
                    if (ToSelect == i)
                    {
                        TreeViewItemExtensions.SetIsSelected(j as TreeViewItem, true);
                        break;
                    }
                    else Select(j, ToSelect);
                }
            }
        }

        #endregion
    }
}
