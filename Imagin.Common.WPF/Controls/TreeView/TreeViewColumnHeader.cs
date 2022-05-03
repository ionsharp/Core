using Imagin.Common.Linq;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace Imagin.Common.Controls
{
    public class TreeViewColumnHeader : ButtonBase
    {
        TreeViewColumnHeaderPresenter panel;

        TreeView treeView;

        //...

        public static readonly DependencyProperty ColumnWidthProperty = DependencyProperty.Register(nameof(ColumnWidth), typeof(GridLength), typeof(TreeViewColumnHeader), new FrameworkPropertyMetadata(default(GridLength)));
        public GridLength ColumnWidth
        {
            get => (GridLength)GetValue(ColumnWidthProperty);
            set => SetValue(ColumnWidthProperty, value);
        }

        public static readonly DependencyProperty GripperStyleProperty = DependencyProperty.Register(nameof(GripperStyle), typeof(Style), typeof(TreeViewColumnHeader), new FrameworkPropertyMetadata(null));
        public Style GripperStyle
        {
            get => (Style)GetValue(GripperStyleProperty);
            set => SetValue(GripperStyleProperty, value);
        }

        public static readonly DependencyProperty SortDirectionProperty = DependencyProperty.Register(nameof(SortDirection), typeof(ListSortDirection?), typeof(TreeViewColumnHeader), new FrameworkPropertyMetadata(null));
        public ListSortDirection? SortDirection
        {
            get => (ListSortDirection?)GetValue(SortDirectionProperty);
            set => SetValue(SortDirectionProperty, value);
        }

        public static readonly DependencyProperty SortNameProperty = DependencyProperty.Register(nameof(SortName), typeof(object), typeof(TreeViewColumnHeader), new FrameworkPropertyMetadata(null));
        public object SortName
        {
            get => GetValue(SortNameProperty);
            set => SetValue(SortNameProperty, value);
        }

        //...

        public TreeViewColumnHeader() : base()
        {
            this.RegisterHandler(i =>
            {
                SetCurrentValue(ColumnWidthProperty, new GridLength(ColumnWidth.Value, GridUnitType.Pixel));

                panel
                    = i.FindParent<TreeViewColumnHeaderPresenter>();
                treeView
                    = i.FindParent<TreeView>();

            }, null);
        }

        //...

        protected override void OnClick()
        {
            base.OnClick();
            var result = SortDirection == null || SortDirection == ListSortDirection.Descending ? ListSortDirection.Ascending : ListSortDirection.Descending;

            if (panel?.ItemsSource is TreeViewColumnCollection columns)
            {
                foreach (var i in columns)
                    i.SetCurrentValue(TreeViewColumn.SortDirectionProperty, null);
            }

            SetCurrentValue(SortDirectionProperty, result);
            if (treeView != null)
            {
                XItemsControl.SetSortDirection(treeView, SortDirection.Value);
                XItemsControl.SetSortName(treeView, SortName);
            }
        }
    }
}