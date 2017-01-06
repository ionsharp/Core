using Imagin.Common;
using Imagin.Common.Input;
using Imagin.Controls.Common.Extensions;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Imagin.Controls.Common
{
    /// <summary>
    /// 
    /// </summary>
    [TemplatePart(Name = "PART_Content", Type = typeof(ContentControl))]
    [TemplatePart(Name = "PART_TreeView", Type = typeof(TreeView))]
    public class TreeViewComboBox : ComboBox
    {
        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler<EventArgs<object>> SelectedItemChanged;

        bool SelectionChangeHandled
        {
            get; set;
        }

        bool SelectedItemChangeHandled
        {
            get; set;
        }

        ContentControl PART_Content
        {
            get; set;
        }

        AdvancedTreeView PART_TreeView
        {
            get; set;
        }
        
        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty ContentTemplateProperty = DependencyProperty.Register("ContentTemplate", typeof(DataTemplate), typeof(TreeViewComboBox), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
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

        /// <summary>
        /// 
        /// </summary>
        public TreeViewComboBox() : base()
        {
            DefaultStyleKey = typeof(TreeViewComboBox);
            SelectionChanged += OnSelectionChanged;
        }

        #endregion

        #region Methods

        protected virtual void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!SelectionChangeHandled && e.AddedItems.Count > 0)
            {
                MessageBox.Show("OnSelectionChanged");
                SelectedItemChangeHandled = true;
                Select(PART_TreeView, e.AddedItems[0]);
                SelectedItemChangeHandled = false;
            }
        }

        protected virtual void OnSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (!SelectedItemChangeHandled && !e.Handled)
            {
                e.Handled = true;

                SelectionChangeHandled = true;
                SelectedItem = e.NewValue;
                SelectionChangeHandled = false;

                PART_Content.Content = e.NewValue;

                //IsDropDownOpen = false;
            }

            if (SelectedItemChanged != null)
                SelectedItemChanged(this, new EventArgs<object>(e.NewValue));
        }

        public override void OnApplyTemplate()
        {
            base.ApplyTemplate();

            PART_Content = Template.FindName("PART_Content", this) as ContentControl;

            PART_TreeView = Template.FindName("PART_TreeView", this) as AdvancedTreeView;
            if (PART_TreeView != null)
            {
                PART_TreeView.Resources = Resources;
                PART_TreeView.SelectedItemChanged += OnSelectedItemChanged;
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
