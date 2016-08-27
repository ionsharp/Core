using Imagin.Common;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Imagin.Controls.Common
{
    public class ImageButton : Button
    {
        #region DependencyProperties
        
        public static DependencyProperty TextPlacementProperty = DependencyProperty.Register("TextPlacement", typeof(Side), typeof(ImageButton), new FrameworkPropertyMetadata(Side.Right, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public Side TextPlacement
        {
            get
            {
                return (Side)GetValue(TextPlacementProperty);
            }
            set
            {
                SetValue(TextPlacementProperty, value);
            }
        }

        public static DependencyProperty SourceProperty = DependencyProperty.Register("Source", typeof(ImageSource), typeof(ImageButton), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnSourceChanged));
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
            ImageButton Button = Object as ImageButton;
            Button.ImageBrush = new ImageBrush(Button.Source);
        }

        public static DependencyProperty ImageBrushProperty = DependencyProperty.Register("ImageBrush", typeof(ImageBrush), typeof(ImageButton), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
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

        public static readonly DependencyProperty ImageColorProperty = DependencyProperty.Register("ImageColor", typeof(Brush), typeof(ImageButton), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
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

        public static DependencyProperty ImageWidthProperty = DependencyProperty.Register("ImageWidth", typeof(double), typeof(ImageButton), new FrameworkPropertyMetadata(16.0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
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

        public static DependencyProperty ImageHeightProperty = DependencyProperty.Register("ImageHeight", typeof(double), typeof(ImageButton), new FrameworkPropertyMetadata(16.0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
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

        public static DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(object), typeof(ImageButton), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public object Text
        {
            get
            {
                return (object)GetValue(TextProperty);
            }
            set
            {
                SetValue(TextProperty, value);
            }
        }

        public static DependencyProperty TextMarginProperty = DependencyProperty.Register("TextMargin", typeof(Thickness), typeof(ImageButton), new FrameworkPropertyMetadata(default(Thickness), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public Thickness TextMargin
        {
            get
            {
                return (Thickness)GetValue(TextMarginProperty);
            }
            set
            {
                SetValue(TextMarginProperty, value);
            }
        }
        
        #endregion

        #region ImageButton

        public ImageButton()
        {
            this.TextMargin = new Thickness(5, 0, 0, 0);
            this.DefaultStyleKey = typeof(ImageButton);
        }

        public override void OnApplyTemplate()
        {
            base.ApplyTemplate();
        }

        #endregion
    }
}
