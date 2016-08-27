using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Imagin.Controls.Common
{
    public class ColorImage : UserControl
    {
        #region DependencyProperties

        public static DependencyProperty SourceProperty = DependencyProperty.Register("Source", typeof(ImageSource), typeof(ColorImage), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnSourceChanged));
        public ImageSource Source
        {
            get
            {
                return (ImageSource)GetValue(SourceProperty);
            }
            set
            {
                SetValue(SourceProperty, value);
            }
        }
        private static void OnSourceChanged(DependencyObject Object, DependencyPropertyChangedEventArgs e)
        {
            ColorImage ColorImage = Object as ColorImage;
            ColorImage.ImageBrush = new ImageBrush(ColorImage.Source);
        }

        public static DependencyProperty ImageBrushProperty = DependencyProperty.Register("ImageBrush", typeof(ImageBrush), typeof(ColorImage), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public ImageBrush ImageBrush
        {
            get
            {
                return (ImageBrush)GetValue(ImageBrushProperty);
            }
            set
            {
                SetValue(ImageBrushProperty, value);
            }
        }

        public static readonly DependencyProperty ImageColorProperty = DependencyProperty.Register("ImageColor", typeof(Brush), typeof(ColorImage), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public Brush ImageColor
        {
            get
            {
                return (Brush)GetValue(ImageColorProperty);
            }
            set
            {
                SetValue(ImageColorProperty, value);
            }
        }

        public static DependencyProperty ImageWidthProperty = DependencyProperty.Register("ImageWidth", typeof(double), typeof(ColorImage), new FrameworkPropertyMetadata(16.0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public double ImageWidth
        {
            get
            {
                return (double)GetValue(ImageWidthProperty);
            }
            set
            {
                SetValue(ImageWidthProperty, value);
            }
        }

        public static DependencyProperty ImageHeightProperty = DependencyProperty.Register("ImageHeight", typeof(double), typeof(ColorImage), new FrameworkPropertyMetadata(16.0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public double ImageHeight
        {
            get
            {
                return (double)GetValue(ImageHeightProperty);
            }
            set
            {
                SetValue(ImageHeightProperty, value);
            }
        }

        #endregion

        #region ColorImage

        public ColorImage()
        {
            this.DefaultStyleKey = typeof(ColorImage);
        }

        public override void OnApplyTemplate()
        {
            base.ApplyTemplate();
        }

        #endregion
    }
}
