using Imagin.Colour.Controls.Models;
using Imagin.Common.Converters;
using Imagin.Common.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Imagin.Colour.Controls
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class ComponentSlider : ComponentSliderBase
    {
        bool ColorChangeHandled = false;

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty AlphaProperty = DependencyProperty.Register(nameof(Alpha), typeof(byte), typeof(ComponentSlider), new FrameworkPropertyMetadata((byte)255, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnAlphaChanged));
        /// <summary>
        /// 
        /// </summary>
        public byte Alpha
        {
            get => (byte)GetValue(AlphaProperty);
            set => SetValue(AlphaProperty, value);
        }
        static void OnAlphaChanged(DependencyObject element, DependencyPropertyChangedEventArgs e)
            => element.To<ComponentSlider>().OnAlphaChanged((byte)e.NewValue);

        /// <summary>
        /// 
        /// </summary>
        public ComponentSlider() => Bitmap = new WriteableBitmap(32, 256, 96, 96, PixelFormats.Bgr24, default(BitmapPalette));

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        void OnAlphaChanged(byte value)
            => PART_Image.IfNotNull(i => i.Opacity = new ByteToDoubleConverter().Convert(value));

        /// <summary>
        /// 
        /// </summary>
        protected override void Draw() => Component.DrawZ(Bitmap, Color);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="OldValue"></param>
        /// <param name="NewValue"></param>
        protected override void OnColorChanged(Color OldValue, Color NewValue)
        {
            if (ColorChangeHandled)
            {
                ColorChangeHandled = false;
                return;
            }

            if (Component?.IsUniform == false)
            {
                ColorChangeHandled = false;
                base.OnColorChanged(OldValue, NewValue);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="OldValue"></param>
        /// <param name="NewValue"></param>
        protected override void OnComponentChanged(VisualComponent OldValue, VisualComponent NewValue)
        {
            if (NewValue != null)
            {
                SetCurrentValue(MaximumProperty, NewValue.Maximum);
                SetCurrentValue(MinimumProperty, NewValue.Minimum);

                base.OnComponentChanged(OldValue, NewValue);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        protected override void OnValueChanged(double value)
        {
            //ColorChangeHandled = true;
            //SetCurrentValue(ColorProperty, Component.ColorFrom(Color, Value.ToInt32()).WithAlpha(Alpha));
        }
    }
}