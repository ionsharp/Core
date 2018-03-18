using Imagin.Colour.Controls.Models;
using System.Windows;
using System.Windows.Controls;

namespace Imagin.Colour.Controls
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class ColorSpaceView : ItemsControl
    {
        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty ColorSpaceProperty = DependencyProperty.Register(nameof(ColorSpace), typeof(ColorModel), typeof(ColorSpaceView), new FrameworkPropertyMetadata(default(ColorModel), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
        public ColorModel ColorSpace
        {
            get => (ColorModel)GetValue(ColorSpaceProperty);
            set => SetValue(ColorSpaceProperty, value);
        }

        /// <summary>
        /// 
        /// </summary>
        public ColorSpaceView() : base() { }
    }
}