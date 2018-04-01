using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Imagin.Common
{
    /// <summary>
    ///  A flexible separator.
    /// </summary>
    public class Line : Control
    {
        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty ContentProperty = DependencyProperty.Register("Content", typeof(object), typeof(Line), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
        public object Content
        {
            get
            {
                return GetValue(ContentProperty);
            }
            set
            {
                SetValue(ContentProperty, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty ContentStringFormatProperty = DependencyProperty.Register("ContentStringFormat", typeof(string), typeof(Line), new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
        public string ContentStringFormat
        {
            get
            {
                return (string)GetValue(ContentStringFormatProperty);
            }
            set
            {
                SetValue(ContentStringFormatProperty, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty ContentTemplateProperty = DependencyProperty.Register("ContentTemplate", typeof(DataTemplate), typeof(Line), new FrameworkPropertyMetadata(default(DataTemplate), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
        public DataTemplate ContentTemplate
        {
            get
            {
                return (DataTemplate)GetValue(ContentTemplateProperty);
            }
            set
            {
                SetValue(ContentTemplateProperty, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty OrientationProperty = DependencyProperty.Register("Orientation", typeof(Orientation), typeof(Line), new FrameworkPropertyMetadata(Orientation.Horizontal, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
        public Orientation Orientation
        {
            get
            {
                return (Orientation)GetValue(OrientationProperty);
            }
            set
            {
                SetValue(OrientationProperty, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty StrokeProperty = DependencyProperty.Register("Stroke", typeof(Brush), typeof(Line), new FrameworkPropertyMetadata(Brushes.Black, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
        public Brush Stroke
        {
            get
            {
                return (Brush)GetValue(StrokeProperty);
            }
            set
            {
                SetValue(StrokeProperty, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty StrokeThicknessProperty = DependencyProperty.Register("StrokeThickness", typeof(double), typeof(Line), new FrameworkPropertyMetadata(1d, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
        public double StrokeThickness
        {
            get
            {
                return (double)GetValue(StrokeThicknessProperty);
            }
            set
            {
                SetValue(StrokeThicknessProperty, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public Line() : base()
        {
            DefaultStyleKey = typeof(Line);
        }
    }
}
