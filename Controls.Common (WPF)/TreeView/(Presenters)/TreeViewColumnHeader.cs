using System.ComponentModel;
using System.Windows;
using System.Windows.Controls.Primitives;
using Imagin.Common.Linq;

namespace Imagin.Controls.Common
{
    /// <summary>
    /// 
    /// </summary>
    public class TreeViewColumnHeader : ButtonBase
    {
        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty ColumnWidthProperty = DependencyProperty.Register("ColumnWidth", typeof(GridLength), typeof(TreeViewColumnHeader), new FrameworkPropertyMetadata(default(GridLength), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
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

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty GripperStyleProperty = DependencyProperty.Register("GripperStyle", typeof(Style), typeof(TreeViewColumnHeader), new FrameworkPropertyMetadata(default(Style), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
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

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty SortDirectionProperty = DependencyProperty.Register("SortDirection", typeof(ListSortDirection?), typeof(TreeViewColumnHeader), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
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

        /// <summary>
        /// 
        /// </summary>
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

        /// <summary>
        /// 
        /// </summary>
        public TreeViewColumnHeader() : base()
        {
            this.DefaultStyleKey = typeof(TreeViewColumnHeader);
            this.Loaded += OnLoaded;
        }
    }
}
