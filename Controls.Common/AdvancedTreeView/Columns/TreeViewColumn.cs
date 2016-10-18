using System.Windows;

namespace Imagin.Controls.Common
{
    public abstract class TreeViewColumn : DependencyObject
    {
        public static DependencyProperty HeaderProperty = DependencyProperty.Register("Header", typeof(string), typeof(TreeViewColumn), new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
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

        public static DependencyProperty IsVisibleProperty = DependencyProperty.Register("IsVisible", typeof(bool), typeof(TreeViewColumn), new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public bool IsVisible
        {
            get
            {
                return (bool)GetValue(IsVisibleProperty);
            }
            set
            {
                SetValue(IsVisibleProperty, value);
            }
        }

        public static DependencyProperty WidthProperty = DependencyProperty.Register("Width", typeof(GridLength), typeof(TreeViewColumn), new FrameworkPropertyMetadata(default(GridLength), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
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

        public static DependencyProperty MinWidthProperty = DependencyProperty.Register("MinWidth", typeof(double), typeof(TreeViewColumn), new FrameworkPropertyMetadata(75.0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
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

        public static DependencyProperty MaxWidthProperty = DependencyProperty.Register("MaxWidth", typeof(double), typeof(TreeViewColumn), new FrameworkPropertyMetadata(225.0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
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

        public static DependencyProperty ContentMarginProperty = DependencyProperty.Register("ContentMargin", typeof(Thickness), typeof(TreeViewColumn), new FrameworkPropertyMetadata(default(Thickness), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
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

        public static DependencyProperty HorizontalContentAlignmentProperty = DependencyProperty.Register("HorizontalContentAlignment", typeof(HorizontalAlignment), typeof(TreeViewColumn), new FrameworkPropertyMetadata(HorizontalAlignment.Left, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
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

        public static DependencyProperty VerticalContentAlignmentProperty = DependencyProperty.Register("VerticalContentAlignment", typeof(VerticalAlignment), typeof(TreeViewColumn), new FrameworkPropertyMetadata(VerticalAlignment.Center, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
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

        public TreeViewColumn() : base()
        {
        }
    }
}
