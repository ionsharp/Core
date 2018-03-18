using System.ComponentModel;
using System.Windows;

namespace Imagin.Common
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class TreeViewColumn : DependencyObject
    {
        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty HeaderProperty = DependencyProperty.Register("Header", typeof(string), typeof(TreeViewColumn), new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
        public string Header
        {
            get
            {
                return (string)GetValue(HeaderProperty);
            }
            set
            {
                SetValue(HeaderProperty, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty WidthProperty = DependencyProperty.Register("Width", typeof(GridLength), typeof(TreeViewColumn), new FrameworkPropertyMetadata(default(GridLength), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
        public GridLength Width
        {
            get
            {
                return (GridLength)GetValue(WidthProperty);
            }
            set
            {
                SetValue(WidthProperty, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty MinWidthProperty = DependencyProperty.Register("MinWidth", typeof(double), typeof(TreeViewColumn), new FrameworkPropertyMetadata(75.0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
        public double MinWidth
        {
            get
            {
                return (double)GetValue(MinWidthProperty);
            }
            set
            {
                SetValue(MinWidthProperty, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty MaxWidthProperty = DependencyProperty.Register("MaxWidth", typeof(double), typeof(TreeViewColumn), new FrameworkPropertyMetadata(225.0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
        public double MaxWidth
        {
            get
            {
                return (double)GetValue(MaxWidthProperty);
            }
            set
            {
                SetValue(MaxWidthProperty, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty ContentMarginProperty = DependencyProperty.Register("ContentMargin", typeof(Thickness), typeof(TreeViewColumn), new FrameworkPropertyMetadata(default(Thickness), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
        public Thickness ContentMargin
        {
            get
            {
                return (Thickness)GetValue(ContentMarginProperty);
            }
            set
            {
                SetValue(ContentMarginProperty, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty HorizontalContentAlignmentProperty = DependencyProperty.Register("HorizontalContentAlignment", typeof(HorizontalAlignment), typeof(TreeViewColumn), new FrameworkPropertyMetadata(HorizontalAlignment.Left, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
        public HorizontalAlignment HorizontalContentAlignment
        {
            get
            {
                return (HorizontalAlignment)GetValue(HorizontalContentAlignmentProperty);
            }
            set
            {
                SetValue(HorizontalContentAlignmentProperty, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty SortDirectionProperty = DependencyProperty.Register("SortDirection", typeof(ListSortDirection?), typeof(TreeViewColumn), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
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

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty VerticalContentAlignmentProperty = DependencyProperty.Register("VerticalContentAlignment", typeof(VerticalAlignment), typeof(TreeViewColumn), new FrameworkPropertyMetadata(VerticalAlignment.Center, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
        public VerticalAlignment VerticalContentAlignment
        {
            get
            {
                return (VerticalAlignment)GetValue(VerticalContentAlignmentProperty);
            }
            set
            {
                SetValue(VerticalContentAlignmentProperty, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public TreeViewColumn() : base()
        {
        }
    }
}
