using Imagin.Common;
using Imagin.Common.Controls;
using System.Windows.Media;

namespace Imagin.Apps.Paint.Effects
{
    [Category(ImageEffectCategory.Color), DisplayName("Equalize")]
    [MemberVisibility(Property: MemberVisibility.Explicit)]
    public class EqualizeEffect : ImageEffect
    {
        public EqualizeEffect() : base() { }

        public override Color Apply(Color color, double opacity = 1) => color;

        /*
        public override void Apply(ColorMatrix oldColors, ColorMatrix newColors)
        {
            int height = oldColors.Rows.Int32(), width = oldColors.Columns.Int32();

            var rHistogram = oldColors.Histogram(RedGreenBlue.Red);
            var gHistogram = oldColors.Histogram(RedGreenBlue.Green);
            var bHistogram = oldColors.Histogram(RedGreenBlue.Blue);

            var histR = new float[256];
            var histG = new float[256];
            var histB = new float[256];

            histR[0] = (rHistogram[0] * rHistogram.Length) / (width * height).Single();
            histG[0] = (gHistogram[0] * gHistogram.Length) / (width * height).Single();
            histB[0] = (bHistogram[0] * bHistogram.Length) / (width * height).Single();

            long cumulativeR = rHistogram[0];
            long cumulativeG = gHistogram[0];
            long cumulativeB = bHistogram[0];

            for (var i = 1; i < histR.Length; i++)
            {
                cumulativeR += rHistogram[i];
                histR[i] = (cumulativeR * rHistogram.Length).Single() / (width * height).Single();

                cumulativeG += gHistogram[i];
                histG[i] = (cumulativeG * gHistogram.Length).Single() / (width * height).Single();

                cumulativeB += bHistogram[i];
                histB[i] = (cumulativeB * bHistogram.Length).Single() / (width * height).Single();
            }

            oldColors.Each((y, x, oldColor) =>
            {
                var intensityR = oldColor.R;
                var intensityG = oldColor.G;
                var intensityB = oldColor.B;

                var nValueR = (byte)histR[intensityR].Coerce(255);
                var nValueG = (byte)histG[intensityG].Coerce(255);
                var nValueB = (byte)histB[intensityB].Coerce(255);

                newColors.SetValue(y.UInt32(), x.UInt32(), Color.FromArgb(oldColor.A, nValueR, nValueG, nValueB));
                return oldColor;
            });
        }
        */

        public override ImageEffect Copy() => new EqualizeEffect();
    }
}