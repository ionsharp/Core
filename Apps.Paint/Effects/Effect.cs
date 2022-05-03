using Imagin.Common;
using Imagin.Common.Linq;
using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;

namespace Imagin.Apps.Paint.Effects
{
    #region (abstract) BaseEffect

    public abstract class BaseEffect : ShaderEffect 
    {
        protected const string BasePath = "/Imagin.Apps.Paint;component/Effects/";
    }

    #endregion

    public enum ImageEffectCategory { Blend, Blur, Color, Distort, Noise, Sharpen, Sketch, Stroke }

    #region (abstract) ImageEffect

    [Icon(App.ImagePath + "Fx.png")]
    public abstract class ImageEffect : BaseEffect
    {
        public string Category
            => GetType().GetAttribute<Common.CategoryAttribute>().Category;

        Guid? index;
        public Guid Index => (index = index ?? Guid.NewGuid()).Value;

        public static readonly DependencyProperty InputProperty = RegisterPixelShaderSamplerProperty(nameof(Input), typeof(ImageEffect), 0);
        public System.Windows.Media.Brush Input
        {
            get => (System.Windows.Media.Brush)GetValue(InputProperty);
            set => SetValue(InputProperty, value);
        }

        public static readonly DependencyProperty IsVisibleProperty = DependencyProperty.Register(nameof(IsVisible), typeof(bool), typeof(ImageEffect), new FrameworkPropertyMetadata(true));
        [Hidden]
        public bool IsVisible
        {
            get => (bool)GetValue(IsVisibleProperty);
            set => SetValue(IsVisibleProperty, value);
        }

        public string Name
            => GetType().GetAttribute<Common.DisplayNameAttribute>().DisplayName;

        public ImageEffect() : base()
        {
            PixelShader = new() { UriSource = GetResource(GetType().Name) };
            UpdateShaderValue(InputProperty);
        }

        protected Uri GetResource(string name) 
            => new($"/{App.DefaultName};component/Effects/Image/{GetType().GetAttribute<CategoryAttribute>().Category}/{name.Replace(nameof(Effect), "")}.ps", UriKind.Relative);

        public abstract ImageEffect Copy();

        public virtual Color Apply(Color input, double opacity = 1) => input;

        public void Apply(WriteableBitmap input) => input.ForEach((x, y, oldColor) => Apply(oldColor));

        public override string ToString() => Name;
    }

    #endregion

    #region (abstract) BlendImageEffect

    public abstract class BlendImageEffect : ImageEffect
    {
        public static readonly DependencyProperty ActualBlendModeProperty = DependencyProperty.Register(nameof(ActualBlendMode), typeof(BlendModes), typeof(BlendImageEffect), new FrameworkPropertyMetadata(BlendModes.Normal, OnActualBlendModeChanged));
        [DisplayName("Blend mode"), Visible]
        public BlendModes ActualBlendMode
        {
            get => (BlendModes)GetValue(ActualBlendModeProperty);
            set => SetValue(ActualBlendModeProperty, value);
        }
        protected static void OnActualBlendModeChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
            => sender.As<BlendImageEffect>().BlendMode = (int)(BlendModes)e.NewValue;

        public static readonly DependencyProperty BlendModeProperty = DependencyProperty.Register(nameof(BlendMode), typeof(double), typeof(BlendImageEffect), new FrameworkPropertyMetadata((double)(int)BlendModes.Normal, PixelShaderConstantCallback(0)));
        [Hidden]
        public double BlendMode
        {
            get => (double)GetValue(BlendModeProperty);
            set => SetValue(BlendModeProperty, value);
        }

        public static readonly DependencyProperty OpacityProperty = DependencyProperty.Register(nameof(Opacity), typeof(double), typeof(BlendImageEffect), new FrameworkPropertyMetadata(1d, PixelShaderConstantCallback(1)));
        [Format(RangeFormat.Both)]
        [Range(0.0, 1.0, 0.01)]
        [Visible]
        public double Opacity
        {
            get => (double)GetValue(OpacityProperty);
            set => SetValue(OpacityProperty, value);
        }

        public BlendImageEffect() : base()
        {
            UpdateShaderValue
                (BlendModeProperty);
            UpdateShaderValue
                (OpacityProperty);
        }
    }

    #endregion

    #region (abstract) TargetImageEffect

    public abstract class TargetImageEffect : ImageEffect
    {
        public enum Targets { Color, Threshold, Tone }

        public static readonly DependencyProperty TargetProperty = DependencyProperty.Register(nameof(Target), typeof(double), typeof(TargetImageEffect), new FrameworkPropertyMetadata(0d, PixelShaderConstantCallback(12)));
        [Hidden]
        public double Target
        {
            get => (double)GetValue(TargetProperty);
            set => SetValue(TargetProperty, value);
        }

        public static readonly DependencyProperty ActualTargetProperty = DependencyProperty.Register(nameof(ActualTarget), typeof(Targets), typeof(TargetImageEffect), new FrameworkPropertyMetadata(Targets.Color, OnActualTargetChanged));
        [Visible]
        public Targets ActualTarget
        {
            get => (Targets)GetValue(ActualTargetProperty);
            set => SetValue(ActualTargetProperty, value);
        }
        static void OnActualTargetChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e) => sender.As<TargetImageEffect>().Target = (int)(Targets)e.NewValue;

        //...

        public static readonly DependencyProperty HighlightAmountProperty = DependencyProperty.Register(nameof(HighlightAmount), typeof(double), typeof(BrightnessEffect), new FrameworkPropertyMetadata(0d, PixelShaderConstantCallback(6)));
        [Category(nameof(Targets.Tone))]
        [Format(RangeFormat.Both)]
        [Range(-1.0, 1.0, 0.01)]
        [Visible]
        public double HighlightAmount
        {
            get => (double)GetValue(HighlightAmountProperty);
            set => SetValue(HighlightAmountProperty, value);
        }

        public static readonly DependencyProperty HighlightRangeProperty = DependencyProperty.Register(nameof(HighlightRange), typeof(double), typeof(TargetImageEffect), new FrameworkPropertyMetadata(1d, PixelShaderConstantCallback(7)));
        [Category(nameof(Targets.Tone))]
        [Format(RangeFormat.Both)]
        [Range(0.0, 1.0, 0.01)]
        [Visible]
        public double HighlightRange
        {
            get => (double)GetValue(HighlightRangeProperty);
            set => SetValue(HighlightRangeProperty, value);
        }

        //...

        public static readonly DependencyProperty MidtoneAmountProperty = DependencyProperty.Register(nameof(MidtoneAmount), typeof(double), typeof(TargetImageEffect), new FrameworkPropertyMetadata(0d, PixelShaderConstantCallback(8)));
        [Category(nameof(Targets.Tone))]
        [Format(RangeFormat.Both)]
        [Range(-1.0, 1.0, 0.01)]
        [Visible]
        public double MidtoneAmount
        {
            get => (double)GetValue(MidtoneAmountProperty);
            set => SetValue(MidtoneAmountProperty, value);
        }

        public static readonly DependencyProperty MidtoneRangeProperty = DependencyProperty.Register(nameof(MidtoneRange), typeof(double), typeof(TargetImageEffect), new FrameworkPropertyMetadata(0.55, PixelShaderConstantCallback(9)));
        [Category(nameof(Targets.Tone))]
        [Format(RangeFormat.Both)]
        [Range(0.0, 1.0, 0.01)]
        [Visible]
        public double MidtoneRange
        {
            get => (double)GetValue(MidtoneRangeProperty);
            set => SetValue(MidtoneRangeProperty, value);
        }

        //...

        public static readonly DependencyProperty ShadowAmountProperty = DependencyProperty.Register(nameof(ShadowAmount), typeof(double), typeof(TargetImageEffect), new FrameworkPropertyMetadata(0d, PixelShaderConstantCallback(10)));
        [Category(nameof(Targets.Tone))]
        [Format(RangeFormat.Both)]
        [Range(-1.0, 1.0, 0.01)]
        [Visible]
        public double ShadowAmount
        {
            get => (double)GetValue(ShadowAmountProperty);
            set => SetValue(ShadowAmountProperty, value);
        }

        public static readonly DependencyProperty ShadowRangeProperty = DependencyProperty.Register(nameof(ShadowRange), typeof(double), typeof(TargetImageEffect), new FrameworkPropertyMetadata(0.45, PixelShaderConstantCallback(11)));
        [Category(nameof(Targets.Tone))]
        [Format(RangeFormat.Both)]
        [Range(0.0, 1.0, 0.01)]
        [Visible]
        public double ShadowRange
        {
            get => (double)GetValue(ShadowRangeProperty);
            set => SetValue(ShadowRangeProperty, value);
        }

        //...

        public static readonly DependencyProperty BlueProperty = DependencyProperty.Register(nameof(Blue), typeof(double), typeof(TargetImageEffect), new FrameworkPropertyMetadata(((double)(0D)), PixelShaderConstantCallback(2)));
        [Category(nameof(Targets.Color))]
        [Format(RangeFormat.Both)]
        [Range(-200, 300, 1)]
        [Visible]
        public double Blue
        {
            get => (double)GetValue(BlueProperty);
            set => SetValue(BlueProperty, value);
        }

        public static readonly DependencyProperty CyanProperty = DependencyProperty.Register(nameof(Cyan), typeof(double), typeof(TargetImageEffect), new FrameworkPropertyMetadata(((double)(0D)), PixelShaderConstantCallback(3)));
        [Category(nameof(Targets.Color))]
        [Format(RangeFormat.Both)]
        [Range(-200, 300, 1)]
        [Visible]
        public double Cyan
        {
            get => (double)GetValue(CyanProperty);
            set => SetValue(CyanProperty, value);
        }

        public static readonly DependencyProperty GreenProperty = DependencyProperty.Register(nameof(Green), typeof(double), typeof(TargetImageEffect), new FrameworkPropertyMetadata(((double)(0D)), PixelShaderConstantCallback(1)));
        [Category(nameof(Targets.Color))]
        [Format(RangeFormat.Both)]
        [Range(-200, 300, 1)]
        [Visible]
        public double Green
        {
            get => (double)GetValue(GreenProperty);
            set => SetValue(GreenProperty, value);
        }

        public static readonly DependencyProperty MagentaProperty = DependencyProperty.Register(nameof(Magenta), typeof(double), typeof(TargetImageEffect), new FrameworkPropertyMetadata(((double)(0D)), PixelShaderConstantCallback(5)));
        [Category(nameof(Targets.Color))]
        [Format(RangeFormat.Both)]
        [Range(-200, 300, 1)]
        [Visible]
        public double Magenta
        {
            get => (double)GetValue(MagentaProperty);
            set => SetValue(MagentaProperty, value);
        }

        public static readonly DependencyProperty RedProperty = DependencyProperty.Register(nameof(Red), typeof(double), typeof(TargetImageEffect), new FrameworkPropertyMetadata(((double)(0D)), PixelShaderConstantCallback(0)));
        [Category(nameof(Targets.Color))]
        [Format(RangeFormat.Both)]
        [Range(-200, 300, 1)]
        [Visible]
        public double Red
        {
            get => (double)GetValue(RedProperty);
            set => SetValue(RedProperty, value);
        }

        public static readonly DependencyProperty YellowProperty = DependencyProperty.Register(nameof(Yellow), typeof(double), typeof(TargetImageEffect), new FrameworkPropertyMetadata(((double)(0D)), PixelShaderConstantCallback(4)));
        [Category(nameof(Targets.Color))]
        [Format(RangeFormat.Both)]
        [Range(-200, 300, 1)]
        [Visible]
        public double Yellow
        {
            get => (double)GetValue(YellowProperty);
            set => SetValue(YellowProperty, value);
        }

        public static readonly DependencyProperty ThresholdProperty = DependencyProperty.Register(nameof(Threshold), typeof(double), typeof(TargetImageEffect), new FrameworkPropertyMetadata(((double)(0D)), PixelShaderConstantCallback(13)));
        [Category(nameof(Targets.Threshold))]
        [Format(RangeFormat.Both)]
        [Range(-100.0, 100.0, 1.0)]
        [Visible]
        public double Threshold
        {
            get => (double)GetValue(ThresholdProperty);
            set => SetValue(ThresholdProperty, value);
        }

        public static readonly DependencyProperty ThresholdAmountProperty = DependencyProperty.Register(nameof(ThresholdAmount), typeof(double), typeof(TargetImageEffect), new FrameworkPropertyMetadata(((double)(0D)), PixelShaderConstantCallback(14)));
        [Category(nameof(Targets.Threshold))]
        [Format(RangeFormat.Both)]
        [Range(-100.0, 100.0, 1.0)]
        [Visible]
        public double ThresholdAmount
        {
            get => (double)GetValue(ThresholdAmountProperty);
            set => SetValue(ThresholdAmountProperty, value);
        }

        public TargetImageEffect() : base()
        {
            UpdateShaderValue
                (HighlightAmountProperty);
            UpdateShaderValue
                (HighlightRangeProperty);

            UpdateShaderValue
                (MidtoneAmountProperty);
            UpdateShaderValue
                (MidtoneRangeProperty);

            UpdateShaderValue
                (ShadowAmountProperty);
            UpdateShaderValue
                (ShadowRangeProperty);

            UpdateShaderValue
                (RedProperty);
            UpdateShaderValue
                (GreenProperty);
            UpdateShaderValue
                (BlueProperty);
            UpdateShaderValue
                (CyanProperty);
            UpdateShaderValue
                (YellowProperty);
            UpdateShaderValue
                (MagentaProperty);

            UpdateShaderValue
                (TargetProperty);

            UpdateShaderValue
                (ThresholdProperty);
            UpdateShaderValue
                (ThresholdAmountProperty);
        }

        public TargetImageEffect(TargetImageEffect input)
        {
            HighlightAmount
                = input.HighlightAmount;
            HighlightRange
                = input.HighlightRange;

            MidtoneAmount
                = input.MidtoneAmount;
            MidtoneRange
                = input.MidtoneRange;

            ShadowAmount
                = input.ShadowAmount;
            ShadowRange
                = input.ShadowRange;

            Red
                = input.Red;
            Green
                = input.Green;
            Blue
                = input.Blue;
            Cyan
                = input.Cyan;
            Yellow
                = input.Yellow;
            Magenta
                = input.Magenta;
        }

        public TargetImageEffect(double amount, Targets target) : this()
        {
            switch (target)
            {
                case Targets.Color:
                    SetCurrentValue
                        (RedProperty, amount);
                    SetCurrentValue
                        (GreenProperty, amount);
                    SetCurrentValue
                        (BlueProperty, amount);
                    SetCurrentValue
                        (CyanProperty, amount);
                    SetCurrentValue
                        (YellowProperty, amount);
                    SetCurrentValue
                        (MagentaProperty, amount);
                    break;

                case Targets.Tone:
                    SetCurrentValue
                        (HighlightAmountProperty, amount);
                    SetCurrentValue
                        (MidtoneAmountProperty, amount);
                    SetCurrentValue
                        (ShadowAmountProperty, amount);
                    break;
            }
        }
    }

    #endregion
}