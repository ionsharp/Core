using System.Windows;
using System.Windows.Media;

namespace Imagin.Common.Controls
{
    public class FillElement : ChildElement
    {
        public static readonly DependencyProperty CropHeightProperty = DependencyProperty.Register(nameof(CropHeight), typeof(double), typeof(FillElement), new FrameworkPropertyMetadata(0d, FrameworkPropertyMetadataOptions.AffectsRender, null, OnCropHeightCoerced));
        public double CropHeight
        {
            get => (double)GetValue(CropHeightProperty);
            set => SetValue(CropHeightProperty, value);
        }
        static object OnCropHeightCoerced(DependencyObject sender, object input) => input is double i && i >= 0 ? i : 0;

        public static readonly DependencyProperty CropWidthProperty = DependencyProperty.Register(nameof(CropWidth), typeof(double), typeof(FillElement), new FrameworkPropertyMetadata(0d, FrameworkPropertyMetadataOptions.AffectsRender, null, OnCropWidthCoerced));
        public double CropWidth
        {
            get => (double)GetValue(CropWidthProperty);
            set => SetValue(CropWidthProperty, value);
        }
        static object OnCropWidthCoerced(DependencyObject sender, object input) => input is double i && i >= 0 ? i : 0;

        public static readonly DependencyProperty CropXProperty = DependencyProperty.Register(nameof(CropX), typeof(double), typeof(FillElement), new FrameworkPropertyMetadata(0d, FrameworkPropertyMetadataOptions.AffectsRender, null, OnCropXCoerced));
        public double CropX
        {
            get => (double)GetValue(CropXProperty);
            set => SetValue(CropXProperty, value);
        }
        static object OnCropXCoerced(DependencyObject sender, object input) => input is double i && i >= 0 ? i : 0;

        public static readonly DependencyProperty CropYProperty = DependencyProperty.Register(nameof(CropY), typeof(double), typeof(FillElement), new FrameworkPropertyMetadata(0d, FrameworkPropertyMetadataOptions.AffectsRender, null, OnCropYCoerced));
        public double CropY
        {
            get => (double)GetValue(CropYProperty);
            set => SetValue(CropYProperty, value);
        }
        static object OnCropYCoerced(DependencyObject sender, object input) => input is double i && i >= 0 ? i : 0;

        public static readonly DependencyProperty FillProperty = DependencyProperty.Register(nameof(Fill), typeof(Brush), typeof(FillElement), new FrameworkPropertyMetadata(Brushes.Black, FrameworkPropertyMetadataOptions.AffectsRender));
        public Brush Fill
        {
            get => (Brush)GetValue(FillProperty);
            set => SetValue(FillProperty, value);
        }
        
        public FillElement() : base() { }

        protected override void OnRender(DrawingContext context)
        {
            base.OnRender(context);
            var rect0 = new Rect(0, 0, ActualWidth, ActualHeight);
            var rect1 = new Rect(CropX, CropY, CropWidth, CropHeight);
            var result = new CombinedGeometry(GeometryCombineMode.Xor, new RectangleGeometry(rect0), new RectangleGeometry(rect1));
            Try.Invoke(() => context.DrawGeometry(Fill, null, result));
        }
    }
}