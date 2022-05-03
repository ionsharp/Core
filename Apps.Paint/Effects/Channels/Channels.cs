using System;
using System.Windows;
using System.Windows.Media.Effects;

namespace Imagin.Apps.Paint.Effects
{
    public class ChannelsEffect : BaseEffect
    {
        public static readonly DependencyProperty InputProperty = RegisterPixelShaderSamplerProperty(nameof(Input), typeof(ChannelsEffect), 0);
        public System.Windows.Media.Brush Input
        {
            get => (System.Windows.Media.Brush)GetValue(InputProperty);
            set => SetValue(InputProperty, value);
        }

        public static readonly DependencyProperty RProperty = DependencyProperty.Register(nameof(R), typeof(double), typeof(ChannelsEffect), new FrameworkPropertyMetadata(1.0, PixelShaderConstantCallback(0)));
        public double R
        {
            get => (double)GetValue(RProperty);
            set => SetValue(RProperty, value);
        }

        public static readonly DependencyProperty GProperty = DependencyProperty.Register(nameof(G), typeof(double), typeof(ChannelsEffect), new FrameworkPropertyMetadata(1.0, PixelShaderConstantCallback(1)));
        public double G
        {
            get => (double)GetValue(GProperty);
            set => SetValue(GProperty, value);
        }

        public static readonly DependencyProperty BProperty = DependencyProperty.Register(nameof(B), typeof(double), typeof(ChannelsEffect), new FrameworkPropertyMetadata(1.0, PixelShaderConstantCallback(2)));
        public double B
        {
            get => (double)GetValue(BProperty);
            set => SetValue(BProperty, value);
        }

        public static readonly DependencyProperty RedProperty = DependencyProperty.Register(nameof(Red), typeof(bool), typeof(ChannelsEffect), new FrameworkPropertyMetadata(true, OnRedChanged));
        public bool Red
        {
            get => (bool)GetValue(RedProperty);
            set => SetValue(RedProperty, value);
        }
        static void OnRedChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
            => (sender as ChannelsEffect).SetCurrentValue(RProperty, (bool)e.NewValue ? 1.0 : 0.0);

        public static readonly DependencyProperty GreenProperty = DependencyProperty.Register(nameof(Green), typeof(bool), typeof(ChannelsEffect), new FrameworkPropertyMetadata(true, OnGreenChanged));
        public bool Green
        {
            get => (bool)GetValue(GreenProperty);
            set => SetValue(GreenProperty, value);
        }
        static void OnGreenChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
            => (sender as ChannelsEffect).SetCurrentValue(GProperty, (bool)e.NewValue ? 1.0 : 0.0);

        public static readonly DependencyProperty BlueProperty = DependencyProperty.Register(nameof(Blue), typeof(bool), typeof(ChannelsEffect), new FrameworkPropertyMetadata(true, OnBlueChanged));
        public bool Blue
        {
            get => (bool)GetValue(BlueProperty);
            set => SetValue(BlueProperty, value);
        }
        static void OnBlueChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
            => (sender as ChannelsEffect).SetCurrentValue(BProperty, (bool)e.NewValue ? 1.0 : 0.0);

        public ChannelsEffect()
        {
            PixelShader = new PixelShader { UriSource = new Uri($"{BasePath}Channels/Channels.ps", UriKind.Relative) };

            UpdateShaderValue(InputProperty);
            UpdateShaderValue(RProperty);
            UpdateShaderValue(GProperty);
            UpdateShaderValue(BProperty);
        }
    }
}