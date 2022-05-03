using Imagin.Common;
using Imagin.Common.Controls;
using System.Windows;
using System.Windows.Input;

namespace Imagin.Apps.Paint
{
    public partial class ImageViewer : Viewer
    {
        #region Properties

        public static readonly DependencyProperty DocumentProperty = DependencyProperty.Register(nameof(Document), typeof(ImageDocument), typeof(ImageViewer), new FrameworkPropertyMetadata(null));
        public ImageDocument Document
        {
            get => (ImageDocument)GetValue(DocumentProperty);
            set => SetValue(DocumentProperty, value);
        }

        public static readonly DependencyProperty ImageHeightProperty = DependencyProperty.Register(nameof(ImageHeight), typeof(double), typeof(ImageViewer), new FrameworkPropertyMetadata(0d));
        public double ImageHeight
        {
            get => (double)GetValue(ImageHeightProperty);
            set => SetValue(ImageHeightProperty, value);
        }

        public static readonly DependencyProperty ImageWidthProperty = DependencyProperty.Register(nameof(ImageWidth), typeof(double), typeof(ImageViewer), new FrameworkPropertyMetadata(0d));
        public double ImageWidth
        {
            get => (double)GetValue(ImageWidthProperty);
            set => SetValue(ImageWidthProperty, value);
        }

        public static readonly DependencyProperty LayersProperty = DependencyProperty.Register(nameof(Layers), typeof(LayerCollection), typeof(ImageViewer), new FrameworkPropertyMetadata(null));
        public LayerCollection Layers
        {
            get => (LayerCollection)GetValue(LayersProperty);
            set => SetValue(LayersProperty, value);
        }

        public static readonly DependencyProperty LinesVisibilityProperty = DependencyProperty.Register(nameof(LinesVisibility), typeof(Visibility), typeof(ImageViewer), new FrameworkPropertyMetadata(Visibility.Visible));
        public Visibility LinesVisibility
        {
            get => (Visibility)GetValue(LinesVisibilityProperty);
            set => SetValue(LinesVisibilityProperty, value);
        }

        public static readonly DependencyProperty LinesVisibilityThresholdProperty = DependencyProperty.Register(nameof(LinesVisibilityThreshold), typeof(double), typeof(ImageViewer), new FrameworkPropertyMetadata(5.0));
        public double LinesVisibilityThreshold
        {
            get => (double)GetValue(LinesVisibilityThresholdProperty);
            set => SetValue(LinesVisibilityThresholdProperty, value);
        }

        public static readonly DependencyProperty SelectionsProperty = DependencyProperty.Register(nameof(Selections), typeof(object), typeof(ImageViewer), new FrameworkPropertyMetadata(null));
        public object Selections
        {
            get => (object)GetValue(SelectionsProperty);
            set => SetValue(SelectionsProperty, value);
        }

        public static readonly DependencyProperty ToolProperty = DependencyProperty.Register(nameof(Tool), typeof(Tool), typeof(ImageViewer), new FrameworkPropertyMetadata(null));
        public Tool Tool
        {
            get => (Tool)GetValue(ToolProperty);
            set => SetValue(ToolProperty, value);
        }

        #endregion

        #region ImageViewer

        public ImageViewer() : base() => InitializeComponent();

        #endregion

        #region Methods

        void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (Tool == null)
            {
                Dialog.Show(nameof(Tool), "No tool selected!", DialogImage.Error, Buttons.Ok);
                return;
            }

            var position = e.GetPosition(Content as UIElement);
            Tool.MouseDownAbsolute = e.GetPosition(this);
            Tool.ImageViewer = this;
            switch (e.ClickCount)
            {
                case 1:
                    Mouse.Capture((IInputElement)sender);
                    Tool?.OnMouseDown(position);
                    break;
                case 2:
                    Tool?.OnMouseDoubleClick(position);
                    break;
            }
            PART_ToolPreview.InvalidateVisual();
        }

        void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (Tool == null)
                return;

            Tool.MouseMoveAbsolute = e.GetPosition(this);
            Tool.ImageViewer = this;

            Tool?.OnMouseMove(e.GetPosition(Content as UIElement));
            PART_ToolPreview.InvalidateVisual();
        }

        void OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (Tool == null)
                return;

            Tool.MouseUpAbsolute = e.GetPosition(this);
            Tool.ImageViewer = this;

            Tool?.OnMouseUp(e.GetPosition(Content as UIElement));
            ((IInputElement)sender).ReleaseMouseCapture();
            Tool.ImageViewer = null;
            PART_ToolPreview.InvalidateVisual();
        }

        #endregion
    }
}