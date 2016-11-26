using System.ComponentModel;
using System.Windows;
using System.Windows.Controls.Primitives;
using Imagin.Common.Extensions;

namespace Imagin.Controls.Common
{
    public class TreeViewColumnHeader : ButtonBase
    {
        public static DependencyProperty ColumnWidthProperty = DependencyProperty.Register("ColumnWidth", typeof(GridLength), typeof(TreeViewColumnHeader), new FrameworkPropertyMetadata(default(GridLength), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public GridLength ColumnWidth
        {
            get
            {
                return (GridLength)GetValue(ColumnWidthProperty);
            }
            set
            {
                SetValue(ColumnWidthProperty, value);
            }
        }

        public static DependencyProperty GripperStyleProperty = DependencyProperty.Register("GripperStyle", typeof(Style), typeof(TreeViewColumnHeader), new FrameworkPropertyMetadata(default(Style), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public Style GripperStyle
        {
            get
            {
                return (Style)GetValue(GripperStyleProperty);
            }
            set
            {
                SetValue(GripperStyleProperty, value);
            }
        }

        public static DependencyProperty SortDirectionProperty = DependencyProperty.Register("SortDirection", typeof(ListSortDirection?), typeof(TreeViewColumnHeader), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public ListSortDirection? SortDirection
        {
            get
            {
                return (ListSortDirection?)GetValue(SortDirectionProperty);
            }
            set
            {
                SetValue(SortDirectionProperty, value);
            }
        }

        void OnLoaded(object sender, RoutedEventArgs e)
        {
            this.ColumnWidth = new GridLength(ColumnWidth.Value, GridUnitType.Pixel);
        }

        protected override void OnClick()
        {
            base.OnClick();

            var Result = this.SortDirection == null || this.SortDirection == ListSortDirection.Descending ? ListSortDirection.Ascending : ListSortDirection.Descending;

            var Parent = this.GetParent<TreeViewColumnHeadersPresenter>();
            if (Parent != null)
            {
                foreach (var i in Parent.Columns)
                    i.SortDirection = null;
            }

            this.SortDirection = Result;
        }

        public TreeViewColumnHeader() : base()
        {
            this.DefaultStyleKey = typeof(TreeViewColumnHeader);
            this.Loaded += OnLoaded;
        }
    }
}
