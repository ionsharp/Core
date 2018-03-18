using Imagin.Colour.Controls.Models;
using Imagin.Common.Linq;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Imagin.Colour.Controls
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class AlphaSlider : ComponentSliderBase
    {
        /// <summary>
        /// 
        /// </summary>
        public AlphaSlider() : base()
        {
            SetCurrentValue(MaximumProperty, 255.0);
            SetCurrentValue(MinimumProperty, 0.0);

            Bitmap = new WriteableBitmap(32, 256, 96, 96, PixelFormats.Bgra32, default(BitmapPalette));
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void Draw() => Bitmap.ForEach(pixel => System.Drawing.Color.FromArgb((255 - pixel.Y).Coerce(255).ToByte(), Color.R, Color.G, Color.B));

        /// <summary>
        /// 
        /// </summary>
        /// <param name="OldValue"></param>
        /// <param name="NewValue"></param>
        protected override void OnColorChanged(Color OldValue, Color NewValue)
        {
            if (OldValue != NewValue)
                base.OnColorChanged(OldValue, NewValue);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="OldValue"></param>
        /// <param name="NewValue"></param>
        protected override void OnComponentChanged(VisualComponent OldValue, VisualComponent NewValue) { }
    }
}
