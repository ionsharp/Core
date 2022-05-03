using Imagin.Common;
using Imagin.Common.Controls;
using Imagin.Common.Linq;
using System.Windows;
using System.Windows.Media;

namespace Imagin.Apps.Paint.Effects
{
    [Category(ImageEffectCategory.Noise), DisplayName("Noise")]
    [MemberVisibility(Property: MemberVisibility.Explicit)]
    public class NoiseEffect : ImageEffect
    {
        public enum Modes { Add, Subtract, AddOrSubtract }

        public static readonly DependencyProperty ActualModeProperty = DependencyProperty.Register(nameof(ActualMode), typeof(Modes), typeof(NoiseEffect), new UIPropertyMetadata(Modes.Add, OnActualModeChanged));
        [DisplayName("Mode"), Visible]
        public Modes ActualMode
        {
            get => (Modes)GetValue(ActualModeProperty);
            set => SetValue(ActualModeProperty, value);
        }
        static void OnActualModeChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e) => sender.As<NoiseEffect>().Mode = (double)(int)(Modes)e.NewValue;

        public static readonly DependencyProperty AmountProperty = DependencyProperty.Register(nameof(Amount), typeof(double), typeof(NoiseEffect), new UIPropertyMetadata(0d, PixelShaderConstantCallback(0)));
        [Format(RangeFormat.Slider)]
        [Range(0.0, 1.0, 0.01)]
        [Visible]
        public double Amount
        {
            get => (double)GetValue(AmountProperty);
            set => SetValue(AmountProperty, (double)value);
        }

        public static readonly DependencyProperty ModeProperty = DependencyProperty.Register(nameof(Mode), typeof(double), typeof(NoiseEffect), new UIPropertyMetadata(0d, PixelShaderConstantCallback(1)));
        public double Mode
        {
            get => (double)GetValue(ModeProperty);
            set => SetValue(ModeProperty, value);
        }

        public NoiseEffect() : base()
        {
            UpdateShaderValue(AmountProperty);
            UpdateShaderValue(ModeProperty);
        }

        public NoiseEffect(double amount) : this()
        {
            SetCurrentValue(AmountProperty, amount);
        }

        public override Color Apply(Color color, double opacity = 1)
        {
            var amount = (Amount * 255).Coerce(255).Int32();

            int or = color.R, og = color.G, ob = color.B;
            int ir = Random.Current.Next(-amount, amount + 1), ig = 0, ib = 0;
            ig = Random.Current.Next(-amount, amount + 1);
            ib = Random.Current.Next(-amount, amount + 1);

            byte
                nr = (or + ir).Coerce(255).Byte(),
                ng = (og + ig).Coerce(255).Byte(),
                nb = (ob + ib).Coerce(255).Byte();

            return Color.FromArgb(color.A, nr, ng, nb);
        }

        public override ImageEffect Copy()
        {
            return new NoiseEffect()
            {
                Amount = Amount,
                Mode = Mode,
            };
        }
    }
}