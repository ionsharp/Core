using System.Windows;
using System.Windows.Media;

namespace Imagin.Apps.Paint
{
    public class ToolPreview : FrameworkElement
    {
        public static readonly DependencyProperty ToolProperty = DependencyProperty.Register(nameof(Tool), typeof(Tool), typeof(ToolPreview), new FrameworkPropertyMetadata(default(Tool), FrameworkPropertyMetadataOptions.None, OnToolChanged));
        public Tool Tool
        {
            get => (Tool)GetValue(ToolProperty);
            set => SetValue(ToolProperty, value);
        }
        protected static void OnToolChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as ToolPreview).InvalidateVisual();

        public static readonly DependencyProperty ZoomProperty = DependencyProperty.Register(nameof(Zoom), typeof(double), typeof(ToolPreview), new FrameworkPropertyMetadata(1.0, FrameworkPropertyMetadataOptions.None, OnZoomChanged));
        public double Zoom
        {
            get => (double)GetValue(ZoomProperty);
            set => SetValue(ZoomProperty, value);
        }
        protected static void OnZoomChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as ToolPreview).InvalidateVisual();

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            Tool?.OnPreviewRendered(drawingContext, Zoom);
        }

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);
            InvalidateVisual();
        }
    }
}