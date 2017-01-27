using Imagin.Common.Drawing;
using Imagin.Common.Extensions;
using System.Windows;
using System.Windows.Controls;

namespace Imagin.Controls.Extended
{
    /// <summary>
    /// 
    /// </summary>
    public class ColorSpaceView : ItemsControl
    {
        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty ColorSpaceProperty = DependencyProperty.Register("ColorSpace", typeof(ColorSpaceModel), typeof(ColorSpaceView), new FrameworkPropertyMetadata(default(ColorSpaceModel), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnColorSpaceChanged));
        /// <summary>
        /// 
        /// </summary>
        public ColorSpaceModel ColorSpace
        {
            get
            {
                return (ColorSpaceModel)GetValue(ColorSpaceProperty);
            }
            set
            {
                SetValue(ColorSpaceProperty, value);
            }
        }
        static void OnColorSpaceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            d.As<ColorSpaceView>().OnColorSpaceChanged((ColorSpaceModel)e.NewValue);
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty IlluminantProperty = DependencyProperty.Register("Illuminant", typeof(Illuminant), typeof(ColorSpaceView), new FrameworkPropertyMetadata(Illuminant.Default, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnIlluminantChanged));
        /// <summary>
        /// 
        /// </summary>
        public Illuminant Illuminant
        {
            get
            {
                return (Illuminant)GetValue(IlluminantProperty);
            }
            set
            {
                SetValue(IlluminantProperty, value);
            }
        }
        static void OnIlluminantChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            d.As<ColorSpaceView>().OnIlluminantChanged((Illuminant)e.NewValue);
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty ObserverProperty = DependencyProperty.Register("Observer", typeof(ObserverAngle), typeof(ColorSpaceView), new FrameworkPropertyMetadata(ObserverAngle.Two, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
        public ObserverAngle Observer
        {
            get
            {
                return (ObserverAngle)GetValue(ObserverProperty);
            }
            set
            {
                SetValue(ObserverProperty, value);
            }
        }
        static void OnObserverChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            d.As<ColorSpaceView>().OnObserverChanged((ObserverAngle)e.NewValue);
        }

        /// <summary>
        /// 
        /// </summary>
        public ColorSpaceView() : base()
        {
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        protected virtual void OnColorSpaceChanged(ColorSpaceModel Value)
        {
            if (Value != null)
            {
                Value.Illuminant = Illuminant;
                Value.Observer = Observer;
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        protected virtual void OnIlluminantChanged(Illuminant Value)
        {
            if (ColorSpace != null)
                ColorSpace.Illuminant = Value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        protected virtual void OnObserverChanged(ObserverAngle Value)
        {
            if (ColorSpace != null)
                ColorSpace.Observer = Value;
        }
    }
}
