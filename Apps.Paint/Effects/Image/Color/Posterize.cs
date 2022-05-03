using Imagin.Common;
using Imagin.Common.Controls;
using Imagin.Common.Linq;
using System.Windows;
using System.Windows.Media;

namespace Imagin.Apps.Paint.Effects
{
    [Category(ImageEffectCategory.Color), DisplayName("Posterize")]
    [MemberVisibility(Property: MemberVisibility.Explicit)]
    public class PosterizeEffect : ImageEffect
    {
        readonly int defaultThreshold = 64;

        float NumAreas;

        float NumValues;

        public static readonly DependencyProperty ThresholdProperty = DependencyProperty.Register("Threshold", typeof(double), typeof(PosterizeEffect), new FrameworkPropertyMetadata(6.0, PixelShaderConstantCallback(0)));
        [Hidden(false)]
        [Range(3.0, 64.0, 1.0)]
        [Format(RangeFormat.Both)]
        public double Threshold
        {
            get => (double)GetValue(ThresholdProperty);
            set => SetValue(ThresholdProperty, value);
        }

        public PosterizeEffect() : base()
        {
            Threshold = defaultThreshold;
            Update();

            UpdateShaderValue(ThresholdProperty);
        }

        void Update()
        {
            NumAreas = 256f / Threshold.Single();
            NumValues = 255f / (Threshold.Single() - 1f);
        }

        public override Color Apply(Color color, double opacity = 1)
        {
            int currentRed = color.R, currentGreen = color.G, currentBlue = color.B;

            float redAreaFloat = currentRed.Single() / NumAreas;
            int redArea = redAreaFloat.Int32();
            if (redArea > redAreaFloat) redArea--;
            float newRedFloat = NumValues * redArea;
            int newRed = newRedFloat.Int32();
            if (newRed > newRedFloat) newRed--;

            float greenAreaFloat = currentGreen.Single() / NumAreas;
            int greenArea = greenAreaFloat.Int32();
            if (greenArea > greenAreaFloat) greenArea--;
            float newGreenFloat = NumValues * greenArea;
            int newGreen = newGreenFloat.Int32();
            if (newGreen > newGreenFloat) newGreen--;

            float blueAreaFloat = currentBlue.Single() / NumAreas;
            int blueArea = blueAreaFloat.Int32();
            if (blueArea > blueAreaFloat) blueArea--;
            float newBlueFloat = NumValues * blueArea;
            int newBlue = newBlueFloat.Int32();
            if (newBlue > newBlueFloat) newBlue--;

            return Color.FromArgb(color.A, newRed.Coerce(255).Byte(), newGreen.Coerce(255).Byte(), newBlue.Coerce(255).Byte());
        }

        public override ImageEffect Copy()
        {
            return new PosterizeEffect()
            {
                Threshold = Threshold
            };
        }
    }
}