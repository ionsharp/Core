using System.Windows;

namespace Imagin.Common.Controls
{
    public class TreeViewThumbnailColumn : TreeViewBindingColumn
    {
        public static readonly DependencyProperty ThumbnailHeightProperty = DependencyProperty.Register(nameof(ThumbnailHeight), typeof(double), typeof(TreeViewThumbnailColumn), new FrameworkPropertyMetadata(16.0));
        public double ThumbnailHeight
        {
            get => (double)GetValue(ThumbnailHeightProperty);
            set => SetValue(ThumbnailHeightProperty, value);
        }

        public static readonly DependencyProperty ThumbnailWidthProperty = DependencyProperty.Register(nameof(ThumbnailWidth), typeof(double), typeof(TreeViewThumbnailColumn), new FrameworkPropertyMetadata(16.0));
        public double ThumbnailWidth
        {
            get => (double)GetValue(ThumbnailWidthProperty);
            set => SetValue(ThumbnailWidthProperty, value);
        }

        public TreeViewThumbnailColumn() : base() { }
    }
}