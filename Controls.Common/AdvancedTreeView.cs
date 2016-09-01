using Imagin.Common.Extensions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Imagin.Common.Extensions;

namespace Imagin.Controls.Common
{
    public class AdvancedTreeView : TreeView
    {
        #region Properties

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

        public static DependencyProperty CollapseSiblingsProperty = DependencyProperty.Register("CollapseSiblings", typeof(bool), typeof(AdvancedTreeView), new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public bool CollapseSiblings
        {
            get
            {
                return (bool)GetValue(CollapseSiblingsProperty);
            }
            set
            {
                SetValue(CollapseSiblingsProperty, value);
            }
        }

        #endregion

        #region Methods

        public void CollapseAll()
        {
            this.ToggleAll(false);
        }

        #region Overrides

        /// <summary>
        /// Deselects TreeView when clicking on empty space.
        /// </summary>
        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);
            /*
            HitTestResult r = VisualTreeHelper.HitTest(this, e.GetPosition(this));
            if (r.VisualHit.GetType() != typeof(TreeViewItem))
            {
                if (this.SelectedVisual != null)
                    this.SelectedVisual.IsSelected = false;
            }
            */
        }

        /// <summary>
        /// Handles automatic expansion on click.
        /// </summary>
        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonUp(e);
            TreeViewItem Item = (e.OriginalSource as DependencyObject).VisualUpwardSearch();
            if (Item == null)
                return;
            if (ExpandOnClick)
                Item.IsExpanded = !Item.IsExpanded;
            if (Item.IsExpanded && CollapseSiblings)
                Item.CollapseSiblings();
        }

        /// <summary>
        /// Makes selection on right click.
        /// </summary>
        protected override void OnPreviewMouseRightButtonDown(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseRightButtonDown(e);
            TreeViewItem Item = (e.OriginalSource as DependencyObject).VisualUpwardSearch();
            if (Item == null)
                return;
            Item.IsSelected = true;
            e.Handled = true;
        }

        #endregion

        #endregion

        #region AdvancedTreeView

        public AdvancedTreeView() : base()
        {
            this.DefaultStyleKey = typeof(AdvancedTreeView);
            this.SelectedItemChanged += OnSelectedItemChanged;
        }

        void OnSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            this.SelectedVisual = this.ItemContainerGenerator.ContainerFromItem(this.SelectedItem).As<TreeViewItem>();
        }

        #endregion
    }
}
