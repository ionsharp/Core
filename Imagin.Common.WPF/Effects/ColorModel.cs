using Imagin.Common.Converters;
using Imagin.Common.Linq;
using Imagin.Common.Colors;
using System.Windows;
using System.Windows.Data;

namespace Imagin.Common.Effects
{
    public class ColorModelEffect : BaseEffect
    {
        public enum Modes { XY, Z }

        public override string FilePath => "ColorModel.ps";

        public static readonly DependencyProperty ComponentProperty = DependencyProperty.Register(nameof(Component), typeof(Components), typeof(ColorModelEffect), new FrameworkPropertyMetadata(Components.A));
        public Components Component
        {
            get => (Components)GetValue(ComponentProperty);
            set => SetValue(ComponentProperty, value);
        }

        public static readonly DependencyProperty ModelProperty = DependencyProperty.Register(nameof(Model), typeof(ColorModels), typeof(ColorModelEffect), new FrameworkPropertyMetadata(ColorModels.HSB));
        public ColorModels Model
        {
            get => (ColorModels)GetValue(ModelProperty);
            set => SetValue(ModelProperty, value);
        }

        internal static readonly DependencyProperty ActualComponentProperty = DependencyProperty.Register(nameof(ActualComponent), typeof(double), typeof(ColorModelEffect), new FrameworkPropertyMetadata(0.0, PixelShaderConstantCallback(1)));
        internal double ActualComponent
        {
            get => (double)GetValue(ActualComponentProperty);
            set => SetValue(ActualComponentProperty, value);
        }

        internal static readonly DependencyProperty ActualModelProperty = DependencyProperty.Register(nameof(ActualModel), typeof(double), typeof(ColorModelEffect), new FrameworkPropertyMetadata(0.0, PixelShaderConstantCallback(0)));
        internal double ActualModel
        {
            get => (double)GetValue(ActualModelProperty);
            set => SetValue(ActualModelProperty, value);
        }

        internal static readonly DependencyProperty ActualModeProperty = DependencyProperty.Register(nameof(ActualMode), typeof(double), typeof(ColorModelEffect), new FrameworkPropertyMetadata(0.0, PixelShaderConstantCallback(2)));
        internal double ActualMode
        {
            get => (double)GetValue(ActualModeProperty);
            set => SetValue(ActualModeProperty, value);
        }

        public static readonly DependencyProperty ModeProperty = DependencyProperty.Register(nameof(Mode), typeof(Modes), typeof(ColorModelEffect), new FrameworkPropertyMetadata(Modes.XY));
        public Modes Mode
        {
            get => (Modes)GetValue(ModeProperty);
            set => SetValue(ModeProperty, value);
        }

        public static readonly DependencyProperty XProperty = DependencyProperty.Register(nameof(X), typeof(double), typeof(ColorModelEffect), new FrameworkPropertyMetadata(0.0, PixelShaderConstantCallback(3)));
        public double X
        {
            get => (double)GetValue(XProperty);
            set => SetValue(XProperty, value);
        }

        public static readonly DependencyProperty YProperty = DependencyProperty.Register(nameof(Y), typeof(double), typeof(ColorModelEffect), new FrameworkPropertyMetadata(0.0, PixelShaderConstantCallback(4)));
        public double Y
        {
            get => (double)GetValue(YProperty);
            set => SetValue(YProperty, value);
        }

        public static readonly DependencyProperty ZProperty = DependencyProperty.Register(nameof(Z), typeof(double), typeof(ColorModelEffect), new FrameworkPropertyMetadata(0.0, PixelShaderConstantCallback(5)));
        public double Z
        {
            get => (double)GetValue(ZProperty);
            set => SetValue(ZProperty, value);
        }

        public ColorModelEffect() : base()
        {
            this.Bind(ActualComponentProperty,
                $"{nameof(Component)}", this, BindingMode.OneWay, new SimpleConverter<Components, double>(i => (int)i));
            this.Bind(ActualModelProperty,
                $"{nameof(Model)}", this, BindingMode.OneWay, new SimpleConverter<ColorModels, double>(i => i.GetAttribute<IndexAttribute>().Index));
            this.Bind(ActualModeProperty,
                $"{nameof(Mode)}", this, BindingMode.OneWay, new SimpleConverter<Modes, double>(i => (int)i));

            UpdateShaderValue(ActualComponentProperty);
            UpdateShaderValue(ActualModelProperty);
            UpdateShaderValue(ActualModeProperty);
            UpdateShaderValue(XProperty);
            UpdateShaderValue(YProperty);
            UpdateShaderValue(ZProperty);
        }
    }
}