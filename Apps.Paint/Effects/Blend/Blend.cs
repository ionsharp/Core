using Imagin.Common;
using Imagin.Common.Linq;
using System.Windows;

namespace Imagin.Apps.Paint.Effects
{
    public class BlendEffect : BaseEffect
    {
        public const string Uri = "Effects/Blend/Blend.ps";

        public static readonly DependencyProperty ActualBlendModeProperty = DependencyProperty.Register(nameof(ActualBlendMode), typeof(BlendModes), typeof(BlendEffect), new FrameworkPropertyMetadata(BlendModes.Normal, OnActualBlendModeChanged));
        public BlendModes ActualBlendMode
        {
            get => (BlendModes)GetValue(ActualBlendModeProperty);
            set => SetValue(ActualBlendModeProperty, value);
        }
        static void OnActualBlendModeChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
            => sender.As<BlendEffect>().BlendMode = (int)(BlendModes)e.NewValue;

        public static readonly DependencyProperty ActualMaskProperty = DependencyProperty.Register(nameof(ActualMask), typeof(LayerMasks), typeof(BlendEffect), new FrameworkPropertyMetadata(LayerMasks.None, OnActualMaskChanged));
        public LayerMasks ActualMask
        {
            get => (LayerMasks)GetValue(ActualMaskProperty);
            set => SetValue(ActualMaskProperty, value);
        }
        static void OnActualMaskChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
            => sender.As<BlendEffect>().Mask = (int)(LayerMasks)e.NewValue;

        public static readonly DependencyProperty AInputProperty = RegisterPixelShaderSamplerProperty(nameof(AInput), typeof(BlendEffect), 0);
        public System.Windows.Media.Brush AInput
        {
            get => (System.Windows.Media.Brush)GetValue(AInputProperty);
            set => SetValue(AInputProperty, value);
        }

        public static readonly DependencyProperty BInputProperty = RegisterPixelShaderSamplerProperty(nameof(BInput), typeof(BlendEffect), 1);
        public System.Windows.Media.Brush BInput
        {
            get => (System.Windows.Media.Brush)GetValue(BInputProperty);
            set => SetValue(BInputProperty, value);
        }

        public static readonly DependencyProperty CInputProperty = RegisterPixelShaderSamplerProperty(nameof(CInput), typeof(BlendEffect), 2);
        public System.Windows.Media.Brush CInput
        {
            get => (System.Windows.Media.Brush)GetValue(CInputProperty);
            set => SetValue(CInputProperty, value);
        }

        public static readonly DependencyProperty BlendModeProperty = DependencyProperty.Register(nameof(BlendMode), typeof(double), typeof(BlendEffect), new FrameworkPropertyMetadata((double)(int)BlendModes.Normal, PixelShaderConstantCallback(0)));
        public double BlendMode
        {
            get => (double)GetValue(BlendModeProperty);
            set => SetValue(BlendModeProperty, value);
        }

        public static readonly DependencyProperty MaskProperty = DependencyProperty.Register(nameof(Mask), typeof(double), typeof(BlendEffect), new FrameworkPropertyMetadata(0d, PixelShaderConstantCallback(1)));
        public double Mask
        {
            get => (double)GetValue(MaskProperty);
            set => SetValue(MaskProperty, value);
        }

        public BlendEffect() : base()
        {
            PixelShader = new() { UriSource = Resources.ProjectUri(Uri) };

            UpdateShaderValue(AInputProperty);
            UpdateShaderValue(BInputProperty);
            UpdateShaderValue(CInputProperty);
            //SetCurrentValue(CInputProperty, new ImageBrush() { ImageSource = BitmapFactory.New(1, 1, Colors.White) });

            UpdateShaderValue(BlendModeProperty);
            UpdateShaderValue(MaskProperty);
        }
    }
}