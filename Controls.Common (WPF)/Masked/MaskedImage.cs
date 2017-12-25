using Imagin.Common.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Imagin.Controls.Common
{
    /// <summary>
    /// 
    /// </summary>
    public class MaskedImage : UserControl
    {
        Rectangle PART_Image;

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty SourceProperty = DependencyProperty.Register("Source", typeof(ImageSource), typeof(MaskedImage), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnSourceChanged));
        /// <summary>
        /// 
        /// </summary>
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
        static void OnSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            d.As<MaskedImage>().OnSourceChanged((ImageSource)e.NewValue);
        }

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty SourceColorProperty = DependencyProperty.Register("SourceColor", typeof(Brush), typeof(MaskedImage), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
        public Brush SourceColor
        {
            get
            {
                return (Brush)GetValue(SourceColorProperty);
            }
            set
            {
                SetValue(SourceColorProperty, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty SourceHeightProperty = DependencyProperty.Register("SourceHeight", typeof(double), typeof(MaskedImage), new FrameworkPropertyMetadata(16.0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
        public double SourceHeight
        {
            get
            {
                return (double)GetValue(SourceHeightProperty);
            }
            set
            {
                SetValue(SourceHeightProperty, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty SourceWidthProperty = DependencyProperty.Register("SourceWidth", typeof(double), typeof(MaskedImage), new FrameworkPropertyMetadata(16.0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
        public double SourceWidth
        {
            get
            {
                return (double)GetValue(SourceWidthProperty);
            }
            set
            {
                SetValue(SourceWidthProperty, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public MaskedImage()
        {
            this.DefaultStyleKey = typeof(MaskedImage);
        }

        void RefreshImage()
        {
            if (PART_Image != null)
                PART_Image.OpacityMask = new ImageBrush(Source);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        protected virtual void OnSourceChanged(ImageSource Value)
        {
            RefreshImage();
        }

        /// <summary>
        /// 
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            PART_Image = Template.FindName("PART_Image", this) as Rectangle;
            RefreshImage();
        }
    }
}
